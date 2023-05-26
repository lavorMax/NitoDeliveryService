using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.DeliveryServicePortal
{
    public class PlaceViewDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

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
