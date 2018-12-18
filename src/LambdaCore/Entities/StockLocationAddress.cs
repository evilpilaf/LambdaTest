namespace LambdaCore.Entities
{
    public readonly struct StockLocationAddress
    {
        public int StockLocationId { get; }
        public int StockLocationAddressId { get; }
        public string DisplayName { get; }

        public StockLocationAddress(int stockLocationId, int stockLocationAddressId, string displayName)
        {
            StockLocationId = stockLocationId;
            StockLocationAddressId = stockLocationAddressId;
            DisplayName = displayName;
        }
    }
}
