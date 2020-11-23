using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Firebase.Authentication.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using FirebaseAdmin;
using IdentityService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            FirebaseApp.Create();
            services.AddHttpClient<MainService>(c => { c.BaseAddress = new Uri("https://json.aspsms.com/"); });
            services.AddGrpc();
            services.AddGrpcReflection();
            services.AddStackExchangeRedisCache(options => { options.Configuration = "localhost:6379"; });
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    options.Authority = "https://securetoken.google.com/mcsv-firebase-service";
                    options.Audience = "mcsv-firebase-service";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/mcsv-firebase-service",
                        ValidateAudience = true,
                        ValidAudience = "mcsv-firebase-service",
                        ValidateLifetime = true,
                    };
                });
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //grpcurl -H 'authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJIM3B1czNKNmZyVnJmNG9LY29LQWlJWmh0QUwyIiwiY2xhaW1zIjp7InVzZXJpZCI6IkgzcHVzM0o2ZnJWcmY0b0tjb0tBaUlaaHRBTDIifSwiaXNzIjoiZmlyZWJhc2UtYWRtaW5zZGstd3A5cGtAbWNzdi1maXJlYmFzZS1zZXJ2aWNlLmlhbS5nc2VydmljZWFjY291bnQuY29tIiwic3ViIjoiZmlyZWJhc2UtYWRtaW5zZGstd3A5cGtAbWNzdi1maXJlYmFzZS1zZXJ2aWNlLmlhbS5nc2VydmljZWFjY291bnQuY29tIiwiYXVkIjoiaHR0cHM6Ly9pZGVudGl0eXRvb2xraXQuZ29vZ2xlYXBpcy5jb20vZ29vZ2xlLmlkZW50aXR5LmlkZW50aXR5dG9vbGtpdC52MS5JZGVudGl0eVRvb2xraXQiLCJleHAiOjE2MDU0NDkzNDMsImlhdCI6MTYwNTQ0NTc0M30.VD6RxZ4V2aZP88QBVLU7KeOsHrlkYjsByRaTPg_a8TlHE1FAvFqcSuYPUaM3YZTavzmMdeOxBj-nNy554wQAFAOXVi4rpN0NgecOu0ksCnlttNdht-FIVY8pqgP8w5LkmlfYO6NWKbuizBgNyn3XFXhJ6mqGHLjAJeBviojHYNDcAA9DiQcevXIVQswUtZAs-R2JV48FS6Wpm0q2ux6y4_zn7hZnNAErvaktmR_q2fxhwff_yumhIZmsFCThR1jSQ6BcjknkkLhfcwCLIhwdcOk8RFY_JPnEilrALCk1oMZA84x8et4_CY11yqtxwwiE1fkxpG9u8cFyeBOegpSkQQ' -insecure localhost:5001 services.Identity/VerifyToken
            app.UseRouting();
            app.UseGrpcWeb();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MainService>();
                endpoints.MapGrpcReflectionService();
                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}