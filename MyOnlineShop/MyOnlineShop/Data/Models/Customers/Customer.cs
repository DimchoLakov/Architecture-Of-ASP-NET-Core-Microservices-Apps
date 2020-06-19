using Microsoft.AspNetCore.Identity;
using MyOnlineShop.Data.Models.Orders;
using System.Collections.Generic;

namespace MyOnlineShop.Data.Models.Customers
{
    public class Customer : IdentityUser
    {
        public Customer()
        {
            this.Addresses = new HashSet<Address>();
            this.Orders = new HashSet<Order>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
