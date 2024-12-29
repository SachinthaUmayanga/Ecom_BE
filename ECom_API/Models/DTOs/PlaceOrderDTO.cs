namespace ECom_API.Models.DTOs
{
    public class PlaceOrderDTO
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
