using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBookMicroservice.Domain.Entities
{
    public class Order
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
