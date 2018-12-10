namespace Carwale.Entity.Stock
{
    public enum BoostPackResponseStatus
    {
        ServerError = 0,
        Success = 1,
        InvalidPackageId = 2,
        StockNotLive = 3,
        DuplicatePackage = 4
    }
}
