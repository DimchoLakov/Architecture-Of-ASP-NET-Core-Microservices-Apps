using MyOnlineShop.WebMVC.Data.Models.Customers;
using System;
using System.Collections.Generic;

namespace MyOnlineShop.WebMVC.Data.Models.Orders
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
        public Address DeliveryAddress { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
