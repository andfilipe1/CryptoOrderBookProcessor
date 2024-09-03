using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoOrderBookProcessor.Domain.Entities
{
    public class OrderBook
    {
        public required string Instrument { get; set; }
        public List<Order> Bids { get; set; } = new();
        public List<Order> Asks { get; set; } = new();
    }
}
