using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NitoDeliveryService.Shared.BL;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PlaceManagementPortalWPF.HttpClients
{
    public class PlaceManagementPortal : IPlaceManagementPortal
    {
        private readonly string _placeManagementPortalUrl;
        private readonly HttpClient _httpClient;
        private string _token;

        public PlaceManagementPortal(IConfiguration config)
        {
            _placeManagementPortalUrl = config.GetValue<string>("PlaceManagementPortalUrl");

            _httpClient = new HttpClient();
        }

        public void Close(int orderId)
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var url = _placeManagementPortalUrl + $"/order/close/{orderId}";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        public void CreateDish(DishDTO dish)
        {
            var data = JsonConvert.SerializeObject(dish);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _placeManagementPortalUrl + $"/dish/create";
            var response = _httpClient.PostAsync(url, content).Result;
        }

        public void CreatePaymentConfiguration(PaymentConfigDTO configuration)
        {
            var data = JsonConvert.SerializeObject(configuration);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _placeManagementPortalUrl + $"/PaymentConfiguration/create";
            var response = _httpClient.PostAsync(url, content).Result;
        }

        public void DeleteDish(int dishId)
        {
            var url = _placeManagementPortalUrl + $"/dish/remove/{dishId}";
            var response = _httpClient.DeleteAsync(url).Result;
        }

        public void DeletePaymentCofiguration(int configurationId)
        {
            var url = _placeManagementPortalUrl + $"/PaymentConfiguration/remove/{configurationId}";
            var response = _httpClient.DeleteAsync(url).Result;
        }

        public void Deliver(int orderId)
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var url = _placeManagementPortalUrl + $"/order/deliver/{orderId}";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        public IEnumerable<OrderDTO> GetAllOrders(int placeId)
        {
            var url = _placeManagementPortalUrl + "/order/getactive";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return new List<OrderDTO>();
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseContent);
        }

        public OrderDTO GetOrder(int orderId)
        {
            var url = _placeManagementPortalUrl + $"/order/get/{orderId}";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<OrderDTO>(responseContent);
        }

        public PlaceDTO GetPlaceByToken()
        {
            var url = _placeManagementPortalUrl + "/place/get";
            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<PlaceDTO>(responseContent);
        }

        public void Prepare(int orderId)
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var url = _placeManagementPortalUrl + $"/order/prepare/{orderId}";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        public void SetupToken(string token)
        {
            _token = token;
            SetupAuthHeader();
        }

        public void UpdatePlace(PlaceDTO place)
        {
            var data = JsonConvert.SerializeObject(place);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = _placeManagementPortalUrl + $"/place/update";
            var response = _httpClient.PutAsync(url, content).Result;
        }

        private void SetupAuthHeader()
        {
            var authHeader = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }
    }
}
