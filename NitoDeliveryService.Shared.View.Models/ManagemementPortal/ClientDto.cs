using System.Collections.Generic;
using System.ComponentModel;

namespace NitoDeliveryService.Shared.View.Models.ManagementPortal
{
    public class ClientDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ResponsiblePhone { get; set; }

        public IEnumerable<SlotDto> Slots { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Title}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
