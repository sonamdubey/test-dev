using Carwale.Entity.Classified.CarValuation;

namespace Carwale.Interfaces.Classified.CarValuation
{
    public interface ICVBaseValueCalculator
    {
        void GetBaseValue(ValuationRequest _valuationRequest);
        double GetExcellentValue();
        double GetFairValue();
        double GetGoodValue();
        double GetPoorValue();
        double GetDealerMargin();
        double AddPercentAmount(double amount, double percent);
    }
}
