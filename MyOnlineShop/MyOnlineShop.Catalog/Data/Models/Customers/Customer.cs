using System.Collections.Generic;

namespace MyOnlineShop.Catalog.Data.Models.Customers
{
    public class Customer
    {
        public Customer()
        {
            this.Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
