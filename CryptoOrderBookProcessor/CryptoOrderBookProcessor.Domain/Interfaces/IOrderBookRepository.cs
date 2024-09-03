using CryptoOrderBookProcessor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoOrderBookProcessor.Domain.Interfaces
{
    public interface IOrderBookRepository
    {
        Task SaveOrderBookAsync(OrderBook orderBook, CancellationToken cancellationToken);
    }
}
