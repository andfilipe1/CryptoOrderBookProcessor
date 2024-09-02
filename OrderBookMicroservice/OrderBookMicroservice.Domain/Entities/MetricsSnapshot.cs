using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBookMicroservice.Domain.Entities
{
    public class MetricsSnapshot
    {
        public int BtcPricesCount { get; set; }
        public int BtcQuantitiesCount { get; set; }
        public int EthPricesCount { get; set; }
        public int EthQuantitiesCount { get; set; }
    }
}
