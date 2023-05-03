using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Interfaces
{
    public interface IPaymentConfigurationService
    {
        Task CreateNewConfiguration(PaymentConfigurationDTO configuration);
        Task RemoveConfiguration(int configurationId);
    }
}
