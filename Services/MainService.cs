using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        public HttpClient _httpClient;
        public Random _random = new Random();
        public IDistributedCache _distributedCache;
        public Subject<UserCreatedPhoneNumberAndId> _userCreatedPhoneNumber = new Subject<UserCreatedPhoneNumberAndId>();

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
                var otp = _random.Next(000000, 10000000);
                // store the random OTP to be retrieved in cache by other methods with an expiration of 60s
                await _distributedCache.SetStringAsync(request.PhoneNumber, otp.ToString(),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(480))
                    });
                string[] recipients = {request.PhoneNumber};
                // var aspBody = new AspSmsBody("MZGUJ47WZTHX", "bt88KmAdok3Fa7ye7Siqa6pNfzoRECfFT65QsCFt", "Knot", recipients , $"YOUR OTP FROM KNOT IS {otp} DO NOT SHARE THIS WITH ANYONE!");
                // // format the sms JSON
                // var smsJson = new StringContent(JsonConvert.SerializeObject(aspBody, new JsonSerializerSettings
                // {
                //     ContractResolver = new CamelCasePropertyNamesContractResolver()
                // }));
                // // send the SMS
                // await _httpClient.PostAsync("/SendTextSMS", smsJson);
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
                _userCreatedPhoneNumber.OnNext(new UserCreatedPhoneNumberAndId
                {
                    PhoneNumber = request.PhoneNumber,
                    UserId = newUser.Uid
                });
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
                    Token = jwt
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
            // subscribe to the user created event.
            _userCreatedPhoneNumber.Subscribe(async x =>
            {
                await responseStream.WriteAsync(new UserSignUpEvent
                {
                    Userid = x.UserId,
                    PhoneNumber = x.PhoneNumber
                });
            });
        }
    }
}