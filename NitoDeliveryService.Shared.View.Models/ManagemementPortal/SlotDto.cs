using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.ManagementPortal
{
    public class SlotDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public string Name { get; set; }

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
