using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DeliveryServiceWPF.HttpClients
{
    public class DeliveryServiceHttpClient : IDeliveryServiceHttpClient
    {
        private readonly string _deliverySerivePortalUrl;
        private readonly HttpClient _httpClient;
        private string _token;

        public DeliveryServiceHttpClient(IConfiguration config)
        {
            _deliverySerivePortalUrl = config.GetValue<string>("DeliveryServiceUrl");

            _httpClient = new HttpClient();
        }

        public void CreateOrder(OrderDTO order)
        {
            var data = JsonConvert.SerializeObject(order);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _deliverySerivePortalUrl + $"/order/create";
            var response = _httpClient.PostAsync(url, content).Result;
        }

        public int CreateUser(UserDto user)
        {
            var data = JsonConvert.SerializeObject(user);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _deliverySerivePortalUrl + $"/user/create";
            var response = _httpClient.PostAsync(url, content).Result;

            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<int>(responseContent);
        }

        public void FinishOrder(int orderId)
        {
            var data = JsonConvert.SerializeObject(OrderStatuses.Finished);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _deliverySerivePortalUrl + $"/order/status/{orderId}";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        public List<OrderDTO> GetAllOrders(int userId)
        {
            var url = _deliverySerivePortalUrl + $"/order/getall/{userId}/true";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<OrderDTO>>(responseContent);
        }

        public List<PlaceViewDTO> GetAllPlaces(string address)
        {
            var url = _deliverySerivePortalUrl + $"/PlaceView/getall/{address}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return new List<PlaceViewDTO>();
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<PlaceViewDTO>>(responseContent);
        }

        public OrderDTO GetOrder(int orderId)
        {
            var url = _deliverySerivePortalUrl + $"/order/get/{orderId}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<OrderDTO>(responseContent);
        }

        public PlaceDTO GetPlace(int placeId, int clientId)
        {
            var url = _deliverySerivePortalUrl + $"/PlaceView/get/{placeId}/{clientId}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<PlaceDTO>(responseContent);
        }

        public UserDto GetUser(string login)
        {
            var url = _deliverySerivePortalUrl + $"/user/get/{login}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<UserDto>(responseContent);
        }

        public void SetupToken(string token)
        {
            _token = token;
            SetupAuthHeader();
        }

        private void SetupAuthHeader()
        {
            var authHeader = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }
    }
}
