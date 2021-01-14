namespace LiveRepository.App.Entities
{
    public class DeliveryItem
    {
        public DeliveryItem() { }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int DeliveryId { get; set; }
    }
}
