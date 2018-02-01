using Bikewale.Entities.BikeBooking;
using System;


namespace Bikewale.Utility
{
    public static class EMICalculation
    {
        /// <summary>
        /// Created BY : Sushil Kumar on 14th March 2015
        /// Summary : To set EMI details for the dealer if no EMI Details available for the dealer
        /// </summary>
        public static EMI SetDefaultEMIDetails(uint bikePrice)
        {
            EMI _objEMI = null;
            if (bikePrice > 0)
            {
                try
                {
                    _objEMI = new EMI();
                    _objEMI.MaxDownPayment = Convert.ToSingle(40 * bikePrice / 100);
                    _objEMI.MinDownPayment = Convert.ToSingle(10 * bikePrice / 100);
                    _objEMI.MaxTenure = 48;
                    _objEMI.MinTenure = 12;
                    _objEMI.MaxRateOfInterest = 15;
                    _objEMI.MinRateOfInterest = 10;
                    _objEMI.ProcessingFee = 0; //2000

                    _objEMI.Tenure = Convert.ToUInt16((_objEMI.MaxTenure - _objEMI.MinTenure) / 2 + _objEMI.MinTenure);
                    _objEMI.RateOfInterest = (_objEMI.MaxRateOfInterest - _objEMI.MinRateOfInterest) / 2 + _objEMI.MinRateOfInterest;
                    _objEMI.MinLoanToValue = Convert.ToUInt32(Math.Round(bikePrice * 0.7, MidpointRounding.AwayFromZero));
                    _objEMI.MaxLoanToValue = bikePrice;
                    _objEMI.EMIAmount = Convert.ToUInt32(Math.Round((_objEMI.MinLoanToValue * _objEMI.RateOfInterest / 1200) / (1 - Math.Pow((1 + (_objEMI.RateOfInterest / 1200)), (-1.0 * _objEMI.Tenure)))));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _objEMI;
        }
    }
}
