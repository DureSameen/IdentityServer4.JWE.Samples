using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebSockets.Internal;
using Microsoft.Extensions.Logging;
using Clients;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace SampleApi
{
  
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
       
        public Startup(ILogger<Startup> logger)
        {
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
           services
                .AddMvcCore()
                .AddJsonFormatters()
                .AddAuthorization();
            services.AddDistributedMemoryCache();

            
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
              
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = Clients.Constants.Authority;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";
                    options.CertificateThumbprint = "5d2855097da042a134c9fa6eb61ae38c6b09ee95";
                    options.RequireSignedTokens = false;
                    options.ValidateIssuer = false;
                    options.JwtBearerEvents = new JwtBearerEvents
                    {
                        OnMessageReceived = e =>
                        {
                            _logger.LogTrace("JWT: message received");
                            return Task.CompletedTask;
                        },

                        OnTokenValidated = e =>
                        {
                            _logger.LogTrace("JWT: token validated");
                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = e =>
                        {
                            _logger.LogTrace("JWT: authentication failed");
                            return Task.CompletedTask;
                        },

                        OnChallenge = e =>
                        {
                            _logger.LogTrace("JWT: challenge");
                            return Task.CompletedTask;
                        }
                    };
                });
       
        }

        public void Configure(IApplicationBuilder app)
        {
            
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}