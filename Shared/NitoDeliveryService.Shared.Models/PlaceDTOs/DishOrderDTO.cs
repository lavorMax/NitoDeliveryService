namespace NitoDeliveryService.Shared.Models.PlaceDTOs
{
    public class DishOrderDTO
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int Number { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }
    }
}
