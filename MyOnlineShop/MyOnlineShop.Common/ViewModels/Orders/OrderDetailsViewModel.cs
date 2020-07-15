using System.Collections.Generic;
using System.Linq;

namespace MyOnlineShop.Common.ViewModels.Orders
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            this.OrderItemDetailsViewModels = new List<OrderItemDetailsViewModel>();
        }

        public string Date { get; set; }

        public decimal DeliveryCost { get; set; }

        public decimal Total => this.DeliveryCost + this.OrderItemDetailsViewModels
                                        .Select(x => x.ProductPrice * x.Quantity)
                                        .DefaultIfEmpty(0)
                                        .Sum();

        public ICollection<OrderItemDetailsViewModel> OrderItemDetailsViewModels { get; set; }
    }
}
