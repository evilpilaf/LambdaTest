using LambdaCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LambdaCore.Adapters
{
    public interface IGetStockLocationsQuery
    {
        Task<IEnumerable<StockLocationAddress>> GetStockLocations();
    }
}
