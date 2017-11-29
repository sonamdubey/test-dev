using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Dealer
{
    /// <summary>
    /// Crated by Sangram N. on 03-Feb-2015
    /// </summary>
    public class OffersController : ApiController
    {
         private readonly IOffer _offer = null;
         /// <summary>
         /// constructor 
         /// </summary>
         /// <param name="Offer">Object</param>
         public OffersController(IOffer Offer)
         {
             _offer = Offer;
         }

        /// <summary>
        /// Fetch offers based on DealerId and ModelId
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<Offer>))]
        public IHttpActionResult Get(uint dealerId, uint modelId)
        {
            List<Offer> offers = null;
            try
            {
                offers = _offer.GetOffersByDealerId(dealerId, modelId);
                if (offers != null && offers.Count > 0)
                {
                    return Ok(offers);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Dealers.OffersController.Get");
               
                return InternalServerError();
            }
        }
    }
}
