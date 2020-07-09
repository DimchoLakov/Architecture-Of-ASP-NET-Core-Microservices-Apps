using System;
using System.Collections.Generic;

namespace MyOnlineShop.Ordering.Data.Models.Orders
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public decimal DeliveryCost { get; set; }

        public DateTime Date { get; set; }

        public int DeliveryAddressId { get; set; }

        // Customer
        public string UserId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
