using System.ComponentModel.DataAnnotations;

namespace NiteDeliveryService.Shared.DAL
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
