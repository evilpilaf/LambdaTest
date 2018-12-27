using LambdaCore.Adapters;
using LambdaCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace LambdaCore
{
    public sealed class UseCase
    {
        private readonly IGetStockLocationsQuery _getStockLocationsQuery;
        private readonly ILogger<UseCase> _logger;

        public UseCase(IGetStockLocationsQuery getStockLocationsQuery, ILogger<UseCase> logger)
        {
            _getStockLocationsQuery = getStockLocationsQuery;
            _logger = logger;
            _logger.LogDebug("UseCase constructed");
        }

        public Task<IEnumerable<StockLocationAddress>> Execute()
        {
            return _getStockLocationsQuery.GetStockLocations();
        }
    }
}
