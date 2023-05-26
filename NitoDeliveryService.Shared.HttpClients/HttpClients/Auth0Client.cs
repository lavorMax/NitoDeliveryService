using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NitoDeliveryService.Shared.HttpClients
{
    public class Auth0Client : IAuthClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly Auth0Options _options;

        public Auth0Client(Auth0Options options)
        {
            _options = options;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://{options.Domain}")
            };
            
        }

        public async Task<string> Authenticate(string login, string password)
        {
            var requestData = new { username = login, 
                password, 
                grant_type = "http://auth0.com/oauth/grant-type/password-realm", 
                client_id=_options.ClientId, 
                client_secret=_options.ClientSecret, 
                audience = _options.Audience, 
                realm = _options.Realm
            };

            var response = await _httpClient.PostAsJsonAsync("/oauth/token", requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var token = JObject.Parse(responseData)["access_token"].ToString();
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
