using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarValuation;


namespace Carwale.Interfaces.Classified.CarValuation
{
    public interface IValuationBL
    {
        ValuationReport GetValuationReport(short year, int car, int city, int? kms, UsedCarOwnerTypes owner = UsedCarOwnerTypes.NA, bool isSellingIndex = true);
    }
}
