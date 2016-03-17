using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 March 2016
    /// Description : Implement IDealerPriceQuote interface.
    /// </summary>
    public class DealerPriceQuoteDetail : IDealerPriceQuoteDetail
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 15 March 2016
        /// Description : call API to get reponse for DealerPriceQuote Page.
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="versionID">e.g. 806</param>
        /// <param name="dealerId">e.g. 12527</param>
        /// <returns>DetailedDealerQuotationEntity entity</returns>
        public DetailedDealerQuotationEntity GetDealerQuotation(UInt32 cityId, UInt32 versionID, UInt32 dealerId)
        {
            DetailedDealerQuotationEntity dealerQuotation = null;
            try
            {
                string _abHostUrl = string.Empty;//ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json", _apiUrl = string.Empty;
                _apiUrl = String.Format("/api/v2/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, versionID, dealerId);

                dealerQuotation = new DetailedDealerQuotationEntity();
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    dealerQuotation = objClient.GetApiResponseSync<DetailedDealerQuotationEntity>(APIHost.AB, _requestType, _apiUrl, dealerQuotation);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dealerQuotation;
        }
    }
}
