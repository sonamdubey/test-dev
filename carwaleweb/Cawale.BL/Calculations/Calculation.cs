using Carwale.Notifications;
using Carwale.Utility;
using System;

namespace Carwale.BL.Calculation
{
    public static class Calculation
    {
        public static string CalculateEmi(int exShowRoomPrice)
        {
            int emi = 0, loanAmount = 0;
            double rateOfInterest = 0;
            string stremi = "";
            try
            {
                loanAmount = Convert.ToInt32(0.85 * exShowRoomPrice);
                rateOfInterest = ((CWConfiguration.INTEREST_RATE / 12) / 100);
                emi = Convert.ToInt32((loanAmount * rateOfInterest * Math.Pow((1 + rateOfInterest), CWConfiguration.TENURE)) / (Math.Pow((1 + rateOfInterest), CWConfiguration.TENURE) - 1));
                stremi = Format.FormatNumericCommaSep(emi.ToString());
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Calculation.CalculateEmi()");
                objErr.LogException();
            }
            return stremi;
        }
    }//class
}//namespace
