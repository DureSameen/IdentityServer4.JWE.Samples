using Clients;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleApi.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        public IdentityController()
        {
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var response = await RequestTokenAsync();
          

           var text= await CallServiceAsync(response.AccessToken);

            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }


        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "api1",
                ClientSecret = "secret2",
                GrantType = OidcConstants.GrantTypes.ClientCredentials,
                Parameters =
                {
                    { "scope", "api2" }

                }

            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task<string> CallServiceAsync(string token)
        {
            var baseAddress = Constants.SampleApi2;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");


            return response;
        }
    }
}