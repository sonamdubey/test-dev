using Carwale.DAL.Insurance;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Insurance;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Insurance
{
    public class Common:ICommon
    {
        private readonly IPQCacheRepository _pqCachedRepo;
        private readonly IValuationRepository _valuationRepo;
        private readonly ICompareCarsCacheRepository _compareCarCacheRepo;

        public Common(IPQCacheRepository pqCachedRepo, IValuationRepository valuationRepo, ICompareCarsCacheRepository compareCarCacheRepo)
        {
            _pqCachedRepo = pqCachedRepo;
            _valuationRepo = valuationRepo;
            _compareCarCacheRepo = compareCarCacheRepo;
        }

        // This function will calculate insurance charges
        // for given city and new car version-id.
        public double GetInsurancePremium(string carVersionId, string strCityId,int year)
        {
            int versionId,cityId;
            Int32.TryParse(carVersionId, out versionId);
            Int32.TryParse(strCityId, out cityId);

            InsuranceRepository DAL = new InsuranceRepository();

            double cc = 0, premium = 0, discount;
            discount = DAL.GetInsuranceDiscount(versionId, cityId, year);

            double price = 0;

            var ccData = _compareCarCacheRepo.GetVersionData(versionId).Item1;
            if (ccData.ContainsKey("14")) //fetch displacement value having key = 14
            {
                var temp = (Carwale.Entity.CompareCars.ValueData)ccData["14"];
                Double.TryParse(temp.Value, out cc);
            }

            List<PQItem> pqList = _pqCachedRepo.GetPQ(cityId, versionId).PriceQuoteList;

            var pqItem = from pq in pqList
                         where pq.Key == "Ex-Showroom Price"
                         select pq;

            if (pqItem.Count() > 0)
            {
                price = pqItem.ToList()[0].Value;
            }

            if (price == 0)
            {
                var basePrice = _valuationRepo.GetValuationBaseValue(versionId, cityId, year);
                if (basePrice != null)
                    price = basePrice.BaseValue;
            }

            if (price > 0)
            {
                try
                {
                    double rate = 0; // rate to be applied on IDV.
                    double idvCar = 0;
                    double liabilities = 0, liaPremium = 0, liaPADriverOwner = 0, liaPaidDriver = 0;
                    string zone = "B";

                    //#string[] zoneA = new string[] { "ahmedabad", "bangalore", "chennai", "hyderabad", "kolkata", "mumbai", "new delhi", "pune" };
                    int[] zoneA = new int[] { 128, 2, 176, 105, 198, 1, 10, 12 };

                    // normalize cc. Three categories. 1000, 1500 and above 1500.
                    if (cc <= 1000) cc = 1; // category 1
                    else if (cc <= 1500) cc = 2; // category 2
                    else cc = 3; // category 3

                    // check if the city comes in zone A?

                    if (zoneA.Contains(cityId)) zone = "A";

                    //Fetch Insurance discount if exists
                    //sql = " SELECT IsNull(Discount, 0) AS Discount "
                    //    + " FROM Con_InsuranceDiscount AS CD, CarVersions AS CV"
                    //    + " WHERE CV.Id = " + carVersionId + " AND CD.ModelID = CV.CarModelId AND CD.CityId = " + cityId;
                    
                    //Ends here

                    rate = double.Parse(ConfigurationManager.AppSettings[zone + ":" + cc]);

                    rate = rate * (1 - discount / 100.0);	//deduct the discount

                    // calculate IDV 					
                    idvCar = price * 95 / 100.0;// even a new car will have a depreciation of 5%.

                    // calculate od.
                    premium = idvCar * rate / 100.0;

                    // Third-party Liability.					
                    liaPremium = double.Parse(ConfigurationManager.AppSettings["L:" + cc]);

                    liaPADriverOwner = double.Parse(ConfigurationManager.AppSettings["PADriverOwnerLiability"]); // Owner-Driver mandatory liability.

                    // Paid-Driver liability is optional however, 
                    // we r making it mandatory for compulsary
                    // considering its amount 25/-
                    // Removed. Only liaPADriverOwner is being considered.
                    liaPaidDriver = double.Parse(ConfigurationManager.AppSettings["PaidDriverLiability"]);

                    // compute all the liabilities.				
                    liabilities = liaPremium + liaPADriverOwner + liaPaidDriver;

                    // Get the Premium.
                    premium += liabilities;

                    // Add service tax.
                    premium += premium * 15.0 / 100.0;

                    // round premium
                    premium = Math.Floor(premium);
                }
                catch (Exception err)
                {
                    //HttpContext.Current.Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, "BL.Insurance.Common.GetInsurancePremium() carVersionid=" + carVersionId??"NULL" + ";cityid=" + cityId??"NULL" + ";price=" + price?? "NULL");
                    objErr.SendMail();
                } // catch Exception	
            }

            return premium;
        }   // End of GetInsurancePremium method

    }
}
