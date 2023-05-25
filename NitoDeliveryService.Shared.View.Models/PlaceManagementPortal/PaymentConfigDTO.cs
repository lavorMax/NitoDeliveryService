using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.PlaceManagementPortal
{
    public class PaymentConfigDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public decimal Price { get; set; }
        public int MaxRange { get; set; }

        public override string ToString()
        {
            return $"{MaxRange} - {Price}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
