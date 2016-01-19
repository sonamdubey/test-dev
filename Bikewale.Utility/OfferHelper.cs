using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class OfferHelper
    {
        public static bool HasFreeInsurance(string dealerId, string modelId, string categoryName, UInt32 categoryValue, ref UInt32 insuranceValue)
        {
            bool retVal = false;
            string[] modelIds = null;
            string[] dealers = null;
            NameValueCollection nvc = null;
            try
            {
                nvc = ConfigurationManager.GetSection("dealerInsurance") as NameValueCollection;
                if (nvc != null && nvc.HasKeys())
                {
                    dealers = nvc.AllKeys;
                }
                if (dealers.Contains(dealerId))
                {
                    //Model Id is not used for insurance offer
                    //modelIds = nvc[dealerId].Split(',');
                    //if (modelIds != null && modelIds.Length > 0)
                    //{
                    //    if (modelIds.Contains(modelId))
                    //    {
                    //        if (categoryName.ToUpper().Contains("INSURANCE"))
                    //        {
                    //            retVal = true;
                    //        }
                    //    }
                    //}
                    if (categoryName.ToUpper().Contains("INSURANCE"))
                    {
                        retVal = true;
                    }
                }
            }
            catch (Exception)
            {

            }
            if (retVal)
            {
                insuranceValue = categoryValue;
            }
            else
            {
                if (insuranceValue == 0)
                    insuranceValue = 0;
            }
            return retVal;
        }
        public static bool HasBumperDealerOffer(string dealerId, string modelId)
        {
            bool retVal = false;
            string[] dealers = null;
            NameValueCollection nvc = null;
            try
            {
                nvc = ConfigurationManager.GetSection("BumperDealerOffer") as NameValueCollection;
                if (nvc != null && nvc.HasKeys())
                {
                    dealers = nvc.AllKeys;
                }
                if (dealers != null && dealers.Contains(dealerId))
                {
                    retVal = true;
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        /// <summary>
        /// Created By      :    Sangram Nandkhile
        /// Summary         :    To return list of Items which needs to be deducted from the total Price
        /// </summary>
        /// <param name="offers"></param>
        public static List<PQ_Price> ReturnDiscountPriceList(List<OfferEntity> offers, List<PQ_Price> priceList )
        {
            try
            {
                if (offers == null || priceList == null) return null;
                List<PQ_Price> discountedPriceList = new List<PQ_Price>();
                foreach (var offer in offers)
                {
                    if (offer.IsPriceImpact)
                    {
                        string displayText = ContainsAny(offer.OfferText.ToLower());
                        if (displayText != string.Empty)
                        {
                            var priceItem = new PQ_Price();
                            priceItem.CategoryName = displayText;
                            uint calcOfferVal = 0;
                            if (offer.OfferValue == 0)
                            {
                                try
                                {
                                    var selected = priceList.Where(p => p.CategoryName.ToLower().Contains(displayText.ToLower()));
                                    if (selected != null && selected.Count() > 0)
                                    {
                                        calcOfferVal = selected.First().Price;
                                        priceItem.Price = calcOfferVal;
                                    }
                                }
                                catch { }
                            }
                            else
                            {
                                priceItem.Price = offer.OfferValue;
                            }
                            discountedPriceList.Add(priceItem);
                        }
                    }
                }
                return discountedPriceList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Check if string has bumper offer categories
        /// </summary>
        /// <param name="offerText">Individual offer to be checked</param>
        /// <returns></returns>
        public static string ContainsAny(string offerText)
        {
            string displayText = string.Empty;
            try
            {
                NameValueCollection keyValCollection = ConfigurationManager.GetSection("offerCategory") as NameValueCollection;
                if (keyValCollection != null)
                {
                    foreach (var keyp in keyValCollection.AllKeys)
                    {
                        if (offerText.Contains(keyp))
                        {
                            displayText = keyValCollection.GetValues(keyp).First();
                            break;
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
            displayText = displayText != string.Empty ? displayText : displayText;
            return displayText;
        }
    }
}
