using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.PlaceManagementPortal
{
    public class DishOrderDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int Number { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }

        public override string ToString()
        {
            return $"{DishName} - {Number}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
