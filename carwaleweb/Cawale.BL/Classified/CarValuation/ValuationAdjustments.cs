using Carwale.Entity.Classified.CarValuation;
using System;

namespace Carwale.BL.Classified.CarValuation
{
    public static class ValuationAdjustments
    {
        public static ValuationBaseValue ApplyCityAdjustment(this ValuationBaseValue baseValues, int cityId)
        {
            if (cityId == 12 || cityId == 198 || cityId == 143 || cityId == 128)
            {
                if (baseValues.BaseValue % 4000 > 0)
                {
                    baseValues.BaseValue = baseValues.BaseValue - baseValues.BaseValue % 4000 + 4000;
                }
            }
            return baseValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYearValue"></param>
        /// <param name="nextYearValue"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static ValuationBaseValue MakeMonthAdjustment(this ValuationBaseValue baseValues, double currentYearGoodValue, ValuationRequest valuationRequest)
        {
            double margin = 0;
            double maxIncrement = 0;
            double[] monthMultiple = new double[12];

            // assign multiples
            monthMultiple[0] = 0;
            monthMultiple[1] = 0.03;
            monthMultiple[2] = 0.07;
            monthMultiple[3] = 0.13;
            monthMultiple[4] = 0.2;
            monthMultiple[5] = 0.28;
            monthMultiple[6] = 0.37;
            monthMultiple[7] = 0.47;
            monthMultiple[8] = 0.58;
            monthMultiple[9] = 0.7;
            monthMultiple[10] = 0.83;
            monthMultiple[11] = 1;

            // in case car is of current year or next year data is not available.
            // Ten percent of current value would be considered.
            //if (currentYearGoodValue <= 0)
            //    margin = currentYearGoodValue * 0.1;
            if (baseValues.NextYearBaseValue <= 0)
                margin = currentYearGoodValue * 0.1;
            else // if next year data is available.
                margin = baseValues.NextYearBaseValue - currentYearGoodValue;

            // this is the maximum value a car can have for any given month, 
            // most probably it will match value of December
            // 45% as of now.
            maxIncrement = margin * 0.45;

            // return month adjustment
            baseValues.BaseValue += maxIncrement * monthMultiple[valuationRequest.ManufactureMonth - 1];

            return baseValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carValue"></param>
        /// <param name="kms"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static ValuationBaseValue ApplyMileageAdjustment(this ValuationBaseValue baseValues, ValuationRequest valuationRequest)
        {
            int yearDiff = (DateTime.Today.Year - valuationRequest.ManufactureYear) > 19 ? 19 : DateTime.Today.Year - valuationRequest.ManufactureYear;
            double kmDeviation = 0;
            double segmentDeviation = 0;
            double adjustment = 0;
            int kms = valuationRequest.KmsTraveled;

            double[,] yearConst = new double[20, 3]{
					{1.000,1,5000}, // 2009
					{0.900,10000,15000}, // 2008
					{0.810,20000,30000}, // 2007
					{0.730,30000,40000}, // 2006
					{0.650,35000,50000}, // 2005
					{0.580,45000,55000}, // 2004
					{0.510,45000,60000}, // 2003
					{0.450,50000,65000}, // 2002
					{0.400,55000,70000}, // 2001
					{0.370,60000,75000}, // 2000
					{0.340,65000,80000}, // 1999
					{0.320,65000,80000}, // 1998
					{0.300,70000,90000}, // 1997
					{0.295,70000,90000}, // 1996
					{0.290,70000,90000}, // 1995
					{0.285,75000,100000}, // 1994
					{0.280,75000,100000}, // 1993
					{0.275,75000,100000}, // 1992
					{0.270,75000,100000}, // 1991
					{0.265,75000,100000}}; // 1990

            double[,] segmentConst = new double[7, 3]{
					{0,200000,0.4},
					{200001,450000,0.7},
					{450001,600000,1.1},
					{600001,900000,1.4},
					{900001,1500000,2},
					{1500001,2500000,3.2},
					{2500001,10000000,5.5}};

            // normalize kms first.
            if (kms % 5000 > 0)
            {
                kms = kms - kms % 5000 + 5000;
            }

            if (yearConst[yearDiff, 1] >= kms)
            {
                kmDeviation = kms - yearConst[yearDiff, 1];
            }
            else if (yearConst[yearDiff, 2] < kms)
            {
                kmDeviation = kms - yearConst[yearDiff, 2];
            }
            else
            {
                kmDeviation = 0;
            }
            

            for (int i = 0; i < 7; i++)
            {
                if (baseValues.BaseValue > segmentConst[i, 0] && baseValues.BaseValue <= segmentConst[i, 1])
                {                    
                    segmentDeviation = segmentConst[i, 2];
                    break;
                }
            }

            adjustment = kmDeviation * segmentDeviation * yearConst[yearDiff, 0];

            double unsignedAdjustment = adjustment >= 0 ? adjustment : -adjustment;

            // adjusted amount should not be greater than 20% of vehicle value.
            if (unsignedAdjustment > baseValues.BaseValue * 0.2)
                adjustment = baseValues.BaseValue * 0.2;

            baseValues.BaseValue -= adjustment;

            return baseValues;
        }

        // Value = ( currentMonth - baseMonth ) * 0.8%.
        // Means add 0.75% of value as every month passes.
        // Don't forget to change the base value 
        // as soon as you update the valuation guide.
        public static ValuationBaseValue RegularMonthlyDepreciation(this ValuationBaseValue baseValues, double currentYearGoodValue)
        {
            double depreciation = 0;

            // don't apply any depreciation if the value is below 50,000.
            if (currentYearGoodValue > 50000)
            {
                int currentMonth = DateTime.Now.Month; // Current Month
                int baseMonth = 7; // Guide was last updated in July.

                depreciation = currentYearGoodValue * (currentMonth - baseMonth) * 0.8 / 100;
            }

            baseValues.BaseValue -= depreciation;

            return baseValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseValues"></param>
        /// <returns></returns>
        public static ValuationBaseValue MakeMultipleOfFiveThousand(this ValuationBaseValue baseValues)
        {
            if (baseValues.BaseValue % 100 > 0)
                baseValues.BaseValue = baseValues.BaseValue - baseValues.BaseValue % 100;
            
            return baseValues;
        }
    }
}
