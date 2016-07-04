using Bikewale.DAL.AutoBiz;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = cityId;
                    objParam.DealerId = dealerId > 0 ? Convert.ToUInt32(dealerId) : default(UInt32);
                    objParam.VersionId = versionID;
                    dealerQuotation = objPriceQuote.GetDealerPriceQuoteByPackage(objParam);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "DealerPriceQuoteDetail.GetDealerQuotation");
                objErr.SendMail();
            }
            return dealerQuotation;
        }
    }
}