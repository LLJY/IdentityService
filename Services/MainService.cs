using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Grpc.Core;
using IdentityService.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IdentityService.Services
{
    public class UserCreatedPhoneNumberAndId
    {
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
    }
    /**
     * Main service
     */
    public class MainService : Identity.IdentityBase
    {
        private readonly HttpClient _httpClient;
        private readonly Random _random = new Random();
        private readonly IDistributedCache _distributedCache;
        // controller is instantiated for every call, keep this static so we can have a call per service
        private static readonly BlockingCollection<IServerStreamWriter<UserSignUpEvent>> UserStreams = new ();
            public MainService(HttpClient httpClient, IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _distributedCache = distributedCache;
        }

        public override async Task<OTPResponse> RequestOTP(OTPRequest request, ServerCallContext context)
        {
            try
            {
                // ensure that OTP is not in the cache
                await _distributedCache.RemoveAsync(request.PhoneNumber);
                // generate the random otp
                var otp = _random.Next(99999, 1000000);
                // store the random OTP to be retrieved in cache by other methods with an expiration of 60s
                await _distributedCache.SetStringAsync(request.PhoneNumber, otp.ToString(),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(480))
                    });
                string[] recipients = {request.PhoneNumber};
                var aspBody = new AspSmsBody("MZGUJ47WZTHX", "bt88KmAdok3Fa7ye7Siqa6pNfzoRECfFT65QsCFt", "Knot", recipients , $"YOUR OTP FROM KNOT IS {otp} DO NOT SHARE THIS WITH ANYONE!");
                // format the sms JSON
                var smsJson = new StringContent(JsonConvert.SerializeObject(aspBody, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
                // send the SMS
                await _httpClient.PostAsync("/SendTextSMS", smsJson);
                Console.WriteLine(otp);
                return new OTPResponse
                {
                    ErrorMessage = "",
                    IsSuccessful = true,
                    TimeLeft = 60
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        [Authorize]
        public override async Task<VerifyTokenResponse> VerifyToken(VerifyTokenRequest request, ServerCallContext context)
        {
            return new VerifyTokenResponse
            {
                Success = true
            };
        }

        public override async Task<VerifyOTPResponse> VerifyOTP(VerifyOTPRequest request, ServerCallContext context)
        {
            string jwt = "";
            var realOtp = await _distributedCache.GetStringAsync(request.PhoneNumber);
            if(realOtp == null)
            {
                return new VerifyOTPResponse
                {
                    IsSuccessful = false,
                    NumTries = 1,
                };
            }
            // check if the user exists in firebase auth, then mark it as sign up
            UserRecord user = null;
            try
            {
                user = await FirebaseAuth.DefaultInstance.GetUserByPhoneNumberAsync(request.PhoneNumber);
            }
            catch (Exception ex)
            {
                
            }
            if (realOtp != request.Otp)
            {
                return new VerifyOTPResponse
                {
                    IsSuccessful = false,
                    NumTries = 1,
                    
                };
            }
            // if user is null, it is a sign up, not a login
            if (user == null)
            {
                // create a new user
                var newUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
                {
                    PhoneNumber = request.PhoneNumber,
                    Disabled = false
                });
                // update the subject with data
                foreach (var responseStream in UserStreams)
                {
                    await responseStream.WriteAsync(new UserSignUpEvent
                    {
                        Userid = newUser.Uid,
                        PhoneNumber = request.PhoneNumber
                    });
                }
                jwt = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(newUser.Uid,
                    new Dictionary<string, object>
                    {
                        {"uid", newUser.Uid}
                    });
                Console.WriteLine($"JWT: {jwt}");
                return new VerifyOTPResponse
                {
                    IsSuccessful = true,
                    NumTries = 1,
                    // create a new token
                    Token = jwt,
                    IsSignUp = true
                };
            }
            // create new firebase token since the existing user is logging in
            jwt = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid,
                new Dictionary<string, object>
                {
                    {"uid", user.Uid}
                });
            Console.WriteLine($"JWT: {jwt}");
            return new VerifyOTPResponse
            {
                // create a custom token with the userid for our other services to consume
                Token = jwt,
                IsSuccessful = true,
                NumTries = 0,
                IsSignUp = false
            };
            
        }

        public override async Task UserSignUpListener(UserSignUpListenerRequest request,
            IServerStreamWriter<UserSignUpEvent> responseStream,
            ServerCallContext context)
        {
            // add to the list of streams waiting for responses
            UserStreams.Add(responseStream);
            try
            {
                Thread.Sleep(-1);
            }
            // task can be abruptly cancelled, if there is any exception, remove the call from the stream and throw
            catch (Exception e)
            {
                UserStreams.TryTake(out responseStream);
            }
        }
    }
}