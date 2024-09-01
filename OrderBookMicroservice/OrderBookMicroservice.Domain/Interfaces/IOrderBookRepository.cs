using OrderBookMicroservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBookMicroservice.Domain.Interfaces
{
    public interface IOrderBookRepository
    {
        Task SaveOrderBookAsync(OrderBook orderBook, CancellationToken cancellationToken);
    }
}
