using Carwale.Entity.Stock;

namespace Carwale.Interfaces.Stock
{
    public interface IStockPackagesRepository
    {
        StockBoostPackageDetails GetBoostPackage(int inquiryId);
        BoostPackResponseStatus UpdateBoostPackage(int inquiryId, bool isDealer, int? boostPackageId);
    }
}
