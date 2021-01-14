using System.Collections.Generic;

namespace LiveRepository.App.Entities
{
    public class Delivery
    {
        public int Id { get; set; }
        public string MotorcycleCourier { get; set; }
        public List<DeliveryItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Completed { get; set; }
    }
}
