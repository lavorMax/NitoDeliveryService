using System.Collections.Generic;
using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.PlaceManagementPortal
{
    public class PlaceDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public IEnumerable<DishDTO> Dishes { get; set; }
        public IEnumerable<PaymentConfigDTO> PaymentConfigurations { get; set; }


        public override string ToString()
        {
            return $"{Id} - {Name}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
