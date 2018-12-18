using LambdaCore.Adapters;
using LambdaCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LambdaCore
{
    public sealed class UseCase
    {
        private readonly IGetStockLocationsQuery _getStockLocationsQuery;

        public UseCase(IGetStockLocationsQuery getStockLocationsQuery)
        {
            _getStockLocationsQuery = getStockLocationsQuery;
        }

        public Task<IEnumerable<StockLocationAddress>> Execute()
        {
            return _getStockLocationsQuery.GetStockLocations();
        }
    }
}
