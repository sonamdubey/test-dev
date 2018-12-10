using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified.CarValuation;

namespace Carwale.BL.Classified.CarValuation
{
    public class CalculateFinalValue : ICVFinalValueCalculator
    {       
        private readonly IValuationRepository _valuationRepo;
        private readonly ICVBaseValueCalculator _baseValueCalc;

        double _goodValue, _fairValue, _excellentValue, _poorValue, _dealerMargin;

        public CalculateFinalValue(IValuationRepository valuationRepo, ICVBaseValueCalculator baseValueCalc)
        {
            _valuationRepo = valuationRepo;
            _baseValueCalc = baseValueCalc;
        }

        /// <summary>
        /// Get the Final price for the car
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public void GetFinalValues(ValuationRequest valuationRequest)
        {
            _baseValueCalc.GetBaseValue(valuationRequest);
            _goodValue = _baseValueCalc.GetGoodValue();
            _excellentValue = _baseValueCalc.GetExcellentValue();
            _fairValue = _baseValueCalc.GetFairValue();
            _poorValue = _baseValueCalc.GetPoorValue();
            _dealerMargin = _baseValueCalc.GetDealerMargin();
        }

        public double GetPoorValueIndividual()
        {
            return _poorValue;
        }

        public double GetFairValueIndividual()
        {
            return _fairValue;
        }

        public double GetGoodValueIndividual()
        {
            return _goodValue;
        }

        public double GetExcellentValueIndividual()
        {
            return _excellentValue;
        }

        public double GetExcellentSaleValueDealer()
        {
            return _baseValueCalc.AddPercentAmount(_excellentValue, 0.04);
        }

        public double GetGoodSaleValueDealer()
        {
            return _baseValueCalc.AddPercentAmount(_goodValue, 0.02);
        }

        public double GetFairSaleValueDealer()
        {
            return _baseValueCalc.AddPercentAmount(_fairValue, 0.01);
        }

        public double ExcellentPurchaseValueDealer()
        {
            return (_excellentValue - _dealerMargin * 0.5) > 0 ? (_excellentValue - _dealerMargin * 0.5) : 0;
        }

        public double GoodPurchaseValueDealer()
        {
            return (_goodValue - _dealerMargin * 0.7) > 0 ? (_goodValue - _dealerMargin * 0.7) : 0;
        }

        public double FairPurchaseValueDealer()
        {
            return (_fairValue - _dealerMargin) > 0 ? (_fairValue - _dealerMargin) : 0;
        }

        public double PoorPurchaseValueDealer()
        {
            return (_poorValue - _dealerMargin) > 0 ? (_poorValue - _dealerMargin) : 0;
        }
    }
    
}
