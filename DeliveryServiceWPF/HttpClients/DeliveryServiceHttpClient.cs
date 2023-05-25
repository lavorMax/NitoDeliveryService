using Microsoft.Extensions.Configuration;
using NitoDeliveryService.Shared.View.Models.DeliveryServicePortal;
using NitoDeliveryService.Shared.View.Models.PlaceManagementPortal;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

        public Task CreateOrder(OrderDTO order)
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
        }

        public Task<int> CreateUser(UserDto user)
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
        }

        public Task FinishOrder(int orderId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<OrderDTO>> GetAllOrders()
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
        }

        public Task<List<PlaceDTO>> GetAllPlaces(string address)
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
        }

        public Task<OrderDTO> GetOrder(int orderId)
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
        }

        public Task<PlaceDTO> GetPlace(int placeId, int clientId)
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
        }

        public async Task<UserDto> GetUser(int userId)
        {
            SetupAuthHeader();
            throw new System.NotImplementedException();
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
