using Newtonsoft.Json;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.HttpClients
{
    public class Auth0ApiClient : IAuth0ApiClient
    {
        private readonly HttpClient _httpClient;

        public Auth0ApiClient(Auth0PlaceManagementOptions options)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Audience)
            };
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.ClientId}:{options.ClientSecret}")));
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }

        public async Task<Auth0CredentialsResponse> CreateUser(string email, int clientId, int slotId)
        {
            var password = GenerateRandomPassword();
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                email,
                password,
                connection = "Username-Password-Authentication",
                app_metadata = new
                {
                    clientId,
                    slotId
                }
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/v2/users", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }

            return new Auth0CredentialsResponse()
            {
                auth0login = email,
                auth0password = password
            };
        }

        public async Task<Auth0CredentialsResponse> GetPassword(string username)
        {
            var response = await _httpClient.GetAsync($"/api/v2/users?q=username%3A%22{Uri.EscapeDataString(username)}%22&search_engine=v3");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<dynamic>(responseContent);
            if (users.Length == 0)
            {
                throw new Exception($"No user found with username: {username}");
            }

            var user = users[0];
            var userId = user.user_id;
            var passwordResponse = await _httpClient.GetAsync($"/api/v2/users/{Uri.EscapeDataString(userId)}/password-rotation/enrollment-status");

            if (!passwordResponse.IsSuccessStatusCode)
            {
                var errorResponse = await passwordResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get user password. Status code: {passwordResponse.StatusCode}. Error response: {errorResponse}");
            }

            var passwordContent = await passwordResponse.Content.ReadAsStringAsync();
            var password = JsonConvert.DeserializeObject<dynamic>(passwordContent);
            return new Auth0CredentialsResponse()
            {
                auth0login = username,
                auth0password = password.password
            };
        }

        public async Task DeleteUser(string userId)
        {
            var response = await _httpClient.DeleteAsync($"/api/v2/users/{Uri.EscapeDataString(userId)}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to delete user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            var password = new StringBuilder();
            for (int i = 0; i < 12; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            return password.ToString();
        }
    }
}
