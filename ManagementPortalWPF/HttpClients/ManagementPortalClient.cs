using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NitoDeliveryService.Shared.Models.DTOs;
using NitoDeliveryService.Shared.View.Models.ManagementPortal;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ManagementPortalWPF.HttpClients
{
    public class ManagementPortalClient : IManagementPortalClient
    {
        private readonly string _managementPortalUrl;
        private readonly HttpClient _httpClient;
        public string Token { get; set; }

        public ManagementPortalClient(IConfiguration config)
        {
            _managementPortalUrl = config.GetValue<string>("ManagementPortalUrl");

            _httpClient = new HttpClient();
        }

        public void CreateCustomer(ClientDto client)
        {
            SetupAuthHeader();

            var data = JsonConvert.SerializeObject(client);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _managementPortalUrl + $"/client/create";
            var response = _httpClient.PostAsync(url, content).Result;
        }

        public ClientDto Get(int clientId)
        {
            SetupAuthHeader();

            var url = _managementPortalUrl + $"/client/get/{clientId}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<ClientDto>(responseContent);
        }

        public IEnumerable<ClientDto> GetAll()
        {
            SetupAuthHeader();

            var url = _managementPortalUrl + "/client/getall";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return new List<ClientDto>();
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<IEnumerable<ClientDto>>(responseContent);
        }

        public void RemoveCustomer(int clientId)
        {
            SetupAuthHeader();

            var url = _managementPortalUrl + $"/client/remove/{clientId}";
            var response = _httpClient.DeleteAsync(url).Result;
        }

        public void CreateSlot(int number, int clientId)
        {
            SetupAuthHeader();

            var content = new StringContent("", Encoding.UTF8, "application/json");

            var url = _managementPortalUrl + $"/slot/create/{clientId}/{number}";
            var response = _httpClient.PostAsync(url, content).Result;
        }

        public void DeleteSlot(int slotId)
        {
            SetupAuthHeader();

            var url = _managementPortalUrl + $"/slot/remove/{slotId}";
            var response = _httpClient.DeleteAsync(url).Result;
        }

        public void InitSlot(InitializeSlotRequest slotRequest)
        {
            SetupAuthHeader();

            var data = JsonConvert.SerializeObject(slotRequest);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _managementPortalUrl + $"/slot/initialize";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        public void DeinitSlot(int slotId)
        {
            SetupAuthHeader();

            var content = new StringContent("", Encoding.UTF8, "application/json");

            var url = _managementPortalUrl + $"/slot/deinitialize/{slotId}";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        public Auth0CredentialsResponse GetCreds(int slotId)
        {
            SetupAuthHeader();

            var url = _managementPortalUrl + $"/slot/credentials/{slotId}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<Auth0CredentialsResponse>(responseContent);
        }

        private void SetupAuthHeader()
        {
            var authHeader = new AuthenticationHeaderValue("Bearer", Token);
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }
    }
}