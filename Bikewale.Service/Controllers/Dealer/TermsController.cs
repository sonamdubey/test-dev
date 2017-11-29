using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Dealer
{
    /// <summary>
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class TermsController : CompressionApiController//ApiController
    {
        [ResponseType(typeof(string))]
        public IHttpActionResult Get(int? offerId, string offerMaskingName = null)
        {
            try
            {
                OfferHtmlEntity offerText = new OfferHtmlEntity();
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objCategoryNames = container.Resolve<Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    offerText = objCategoryNames.GetOfferTerms(offerMaskingName, offerId);
                }

                if (offerText != null)
                {
                    return Ok(offerText.Html);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.AppNotifications.Post");
               
                return InternalServerError();
            }
        }
    }
}
