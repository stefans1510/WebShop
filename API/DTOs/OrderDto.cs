namespace API.DTOs
{
    public class OrderDto
    {
        public string CartId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}