using AutoMapper;
using Carwale.DTOs;
using Carwale.DTOs.Deals;
using Carwale.Entity.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Deals
{
    public abstract class DealsBLBase
    {
        private readonly IDealsCache _dealsCache;
        
        public DealsBLBase(IDealsCache dealsCache)
        {
            _dealsCache = dealsCache;
        }

        public virtual List<City> GetAdvantageCities(int modelId = 0, int versionId = 0, int makeId = 0)
        {
            List<Carwale.Entity.Geolocation.City> cities = _dealsCache.GetAdvantageCities(modelId, versionId, makeId) ?? new List<Carwale.Entity.Geolocation.City>();
            return Mapper.Map<List<Carwale.Entity.Geolocation.City>, List<City>>(cities);
        }

        public virtual BookingReasons GetReasonsText(DealsStock deals, string makeName, string modelName)
        {
            BookingReasons reasonsSlug = new BookingReasons();
            reasonsSlug.Reasons = new List<ReasonsText>();
            try
            {
                if (deals != null)
                {
                    if (deals.Savings > 0)
                    {
                        reasonsSlug.Reasons.Add(new ReasonsText(string.Format("\u20B9 {0} cash discount", Format.FormatNumericCommaSep(deals.Savings.ToString())), "Exclusively for CarWale users"));
                    }
                    else if (!string.IsNullOrWhiteSpace(deals.Offers))
                    {
                        if (deals.OfferValue > 0)
                            reasonsSlug.Reasons.Add(new ReasonsText(string.Format("Attractive offers worth \u20B9 {0} ", Format.FormatNumericCommaSep(deals.OfferValue.ToString())), string.Format("Offers only from {0} authorized dealerships", makeName)));
                        else
                            reasonsSlug.Reasons.Add(new ReasonsText("Attractive offers ", string.Format("Offers only from {0} authorized dealerships", makeName)));
                    }
                    reasonsSlug.Reasons.Add(new ReasonsText("Authorized dealers", string.Format("Offers only from {0} authorized dealerships", makeName)));
                     string deliveryType = deals.DeliveryTimeline == 0 ? "Immediate" : "Priority";
                     string deliveryDays = deals.DeliveryTimeline > 0 ? string.Format(" within {0}", GetTimeMessageFromDays(deals.DeliveryTimeline)) : string.Empty;
                     reasonsSlug.Reasons.Add(new ReasonsText(string.Format("{0} Delivery", deliveryType), string.Format("This car is available with {0} delivery{1}.", deliveryType.ToLower(), deliveryDays)));
                    reasonsSlug.Heading = reasonsSlug.Reasons.Count().ToString() + " Reasons to book this " + Format.FilterModelName(modelName) + " right now!";
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBL.GetReasonsText()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return reasonsSlug;
        }

        //public virtual List<KeyValuePair<string, string>> FillPriceBreakUp(DealsPriceBreakupEntity dealsPriceBreakUp)
        //{
        //    List<KeyValuePair<string, string>> dealsPriceBreakUpList = new List<KeyValuePair<string, string>>();
        //    if (dealsPriceBreakUp != null)
        //    {
        //        if (dealsPriceBreakUp.ExShowroom > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Ex-Showroom Price", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.ExShowroom.ToString())));
        //        if (dealsPriceBreakUp.RTO > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("RTO", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.RTO.ToString())));
        //        if (dealsPriceBreakUp.Insurance > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Insurance", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Insurance.ToString())));
        //        if (dealsPriceBreakUp.Accesories > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Accessories", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Accesories.ToString())));
        //        if (dealsPriceBreakUp.CustomerCare > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Customer Care Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.CustomerCare.ToString())));
        //        if (dealsPriceBreakUp.Incidental > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Incidental Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Incidental.ToString())));
        //        if (dealsPriceBreakUp.Depot > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Depot Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Depot.ToString())));
        //        if (dealsPriceBreakUp.HandlingLogistics > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Handling/Logistics Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.HandlingLogistics.ToString())));
        //        if (dealsPriceBreakUp.TCS > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Tax Collected at Source(TCS)", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.TCS.ToString())));
        //        if (dealsPriceBreakUp.LBT > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("LBT", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.LBT.ToString())));
        //        if (dealsPriceBreakUp.Facilitation > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Facilitation Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Facilitation.ToString())));
        //        if (dealsPriceBreakUp.Delivery > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Delivery Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Delivery.ToString())));
        //        if (dealsPriceBreakUp.Service > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Service Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Service.ToString())));
        //        if (dealsPriceBreakUp.Registration > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Registration Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Registration.ToString())));
        //        if (dealsPriceBreakUp.Other > 0)
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Other Charges", Carwale.Utility.Format.FormatNumericCommaSep(dealsPriceBreakUp.Other.ToString())));
        //        if (!string.IsNullOrWhiteSpace(dealsPriceBreakUp.AdditionalComments))
        //            dealsPriceBreakUpList.Add(new KeyValuePair<string, string>("Additional Comments", dealsPriceBreakUp.AdditionalComments));
        //    }
        //    return dealsPriceBreakUpList;
        //}

         public static string GetTimeMessageFromDays(int days)
         {
             string message;
             switch (days)
             {
                 case 14:
                     message = "2 weeks";
                     break;
                 case 30:
                     message = "1 month";
                     break;
                 case 60:
                     message = "2 months";
                     break;
                 case 90:
                     message = "3 months";
                     break;
                 default:
                     message = string.Format("{0} days", days);
                     break;
             }
             return message;
         }
    }
}
