using Carwale.Entity.Classified.CarValuation;

namespace Carwale.Interfaces.Classified.CarValuation
{
    public interface ICVFinalValueCalculator
    {
        void GetFinalValues(ValuationRequest valuationRequest);

        double GetPoorValueIndividual();
        double GetFairValueIndividual();
        double GetGoodValueIndividual();
        double GetExcellentValueIndividual();

        double GetExcellentSaleValueDealer();
        double GetGoodSaleValueDealer();
        double GetFairSaleValueDealer();

        double ExcellentPurchaseValueDealer();
        double GoodPurchaseValueDealer();
        double FairPurchaseValueDealer();
        double PoorPurchaseValueDealer();
    }
}
