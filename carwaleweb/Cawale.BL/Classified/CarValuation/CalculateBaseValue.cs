using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified.CarValuation;
using System;

namespace Carwale.BL.Classified.CarValuation
{
    public class CalculateBaseValue : ICVBaseValueCalculator
    {
        private readonly IValuationRepository _valuationRepo;
        private ValuationRequest _valuationRequest;
        protected ValuationBaseValue _baseValue;

        public CalculateBaseValue(IValuationRepository valuationRepo)
        {
            _valuationRepo = valuationRepo;
        }

        /// <summary>
        /// Gets the Base price for the car
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public void GetBaseValue(ValuationRequest valuationRequest)
        {
            _valuationRequest = valuationRequest;            
            _baseValue = _valuationRepo.GetValuationBaseValue(_valuationRequest.VersionId, _valuationRequest.CityID, _valuationRequest.ManufactureYear);

            // Current year good value
            double goodValue = _baseValue.BaseValue +  (_baseValue.BaseValue * _baseValue.Deviation) / 100;          
            _baseValue.NextYearBaseValue = _baseValue.NextYearBaseValue + (_baseValue.NextYearBaseValue * _baseValue.Deviation) / 100;

            _baseValue.BaseValue = goodValue;
            _baseValue = _baseValue.ApplyCityAdjustment(_valuationRequest.CityID);
            _baseValue = _baseValue.RegularMonthlyDepreciation(goodValue);
            _baseValue = _baseValue.MakeMonthAdjustment(goodValue, _valuationRequest);
            _baseValue = _baseValue.ApplyMileageAdjustment(_valuationRequest);
            _baseValue = _baseValue.MakeMultipleOfFiveThousand();
        }
        
        /// <summary>
        /// Get the Excellent price for the car
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public double GetExcellentValue()
        {
            double goodValue = Convert.ToDouble(_baseValue.BaseValue);
            
            // Now calculate the year difference, the newer the car
            // the lower the excellent value would be. 
            // If year difference is more than 5, 
            // Excellent value will not be affected.
            int yearDifference = DateTime.Today.Year - _valuationRequest.ManufactureYear;

            if (goodValue <= 50000)
            {
                goodValue = goodValue + goodValue * (20 + yearDifference * 0.25) / 100;
            }
            else if (yearDifference <= 8)
            {
                goodValue = goodValue + goodValue * (3 + yearDifference * 0.25) / 100;
            }
            else
            {
                goodValue = goodValue + goodValue * (1 + yearDifference * 0.5) / 100;
            }

            // Normalize the excellent value 
            // in a way that its is 500's multiples.			
            if (goodValue % 500 > 0) goodValue = goodValue - goodValue % 500 + 500;

            return goodValue;
        }

        /// <summary>
        /// Get the Good price for the car
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public double GetGoodValue()
        {
            return _baseValue.BaseValue;
        }

        /// <summary>
        /// Get the Fair price for the car
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public double GetFairValue()
        {
            double goodValue = Convert.ToDouble(_baseValue.BaseValue);

            if (goodValue <= 100000)
            {
                goodValue -= (goodValue * 16 / 100);
            }
            else
            {
                goodValue -= (goodValue * 12 / 100) <= 16000 ? 16000 : (goodValue * 12 / 100); // 12% or 16000 whichever is higher.
            }

            if (goodValue % 500 > 0)
                goodValue -= goodValue % 500;// - 5000;

            return goodValue;
        }

        /// <summary>
        /// Not in use for now .Get the Excellent price for the car
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public double GetPoorValue()
        {
            double goodValue = Convert.ToDouble(_baseValue.BaseValue);

            if (goodValue <= 100000)
                goodValue -= (goodValue * 10 / 100);
            if (goodValue <= 200000)
                goodValue -= (goodValue * 25 / 100);
            else if (goodValue <= 400000)
                goodValue -= (goodValue * 22 / 100);
            else if (goodValue <= 600000)
                goodValue -= (goodValue * 17 / 100);
            else
                goodValue -= (goodValue * 15 / 100);

            if (goodValue % 500 > 0)
                goodValue -= goodValue % 500;// + 5000;

            return goodValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodValue"></param>
        /// <returns></returns>
        public double GetDealerMargin()
        {
            double reduction = 0;

            double gv = _baseValue.BaseValue;

            if (gv <= 25000)
                reduction = gv; // Dealer Value would be Zero. That means CarWale can't provide dealer value for this car.
            else if (gv <= 50000)
                reduction = (gv * 30 / 100);
            else if (gv <= 100000)
                reduction = (gv * 25 / 100) > 15000 ? (gv * 25 / 100) : 15000; // 25% or 15000 whichever is higher
            else if (gv <= 200000)
                reduction = (gv * 15 / 100) > 25000 ? (gv * 15 / 100) : 25000; // 15% or 25000 whichever is higher
            else if (gv <= 400000)
                reduction = (gv * 13 / 100) > 30000 ? (gv * 13 / 100) : 30000; // 13% or 30000 whichever is higher
            else
                reduction = (gv * 12 / 100) > 52000 ? (gv * 12 / 100) : 52000; // 12% or 52000 whichever is higher

            // Now calculate the year difference.
            // Only two years would be considered i.e. current and previous one.
            int yearDifference = DateTime.Today.Year - _valuationRequest.ManufactureYear;

            // For cars older than 2 years, dealer margin would remain same
            //if ( yearDifference <= 1 )
            //{
            //	reduction = reduction * (60 + (yearDifference * 20))/100;
            //}

            if (reduction % 500 > 0) reduction = reduction - reduction % 500 + 500;

            return reduction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public double AddPercentAmount(double amount, double percent)
        {
            double incrementedAmount = 0;
            double amountActual = 0;

            amountActual = Convert.ToInt64(amount);

            incrementedAmount = amountActual + amountActual * percent;

            if (incrementedAmount % 500 > 0) incrementedAmount = incrementedAmount - incrementedAmount % 500 + 500;

            return incrementedAmount;
        }       		
    }
}
