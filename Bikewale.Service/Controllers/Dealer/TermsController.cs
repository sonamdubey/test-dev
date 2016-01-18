using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Dealer
{
    /// <summary>
    /// summary
    /// </summary>
    public class TermsController : ApiController
    {
        [ResponseType(typeof(string))]
        public IHttpActionResult Get(int? offerId, string offerMaskingName = null)
        {
            try
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json", _apiUrl = string.Empty, imagePath = string.Empty, bikeName = string.Empty;
                _apiUrl = String.Format("api/DealerPriceQuote/GetOfferTerms/?offerMaskingName={0}&offerId={1}", offerMaskingName, offerId);

                OfferHtmlEntity offerText = new OfferHtmlEntity();
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    offerText = objClient.GetApiResponseSync<OfferHtmlEntity>(APIHost.AB, _requestType, _apiUrl, offerText);
                }

                if(offerText != null)
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.AppNotifications.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
