using Bikewale.DTO.DealerLocator;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.DealerLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bikewale.Service.Controllers.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 22 march 2016
    /// Description : for getting dealer detail and bike detail w.r.t dealer.
    /// </summary>
    public class DealerBikesController : ApiController
    {
        private readonly IDealer _dealer = null;
        private readonly IDealerCacheRepository _cache = null;

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 22 march 2016
        /// Description : for Resolving IDealer and IDealerCacheRepository.
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="cache"></param>
        public DealerBikesController(IDealer dealer, IDealerCacheRepository cache)
        {
            _dealer = dealer;
            _cache = cache;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 22 March 2016
        /// Description : To get Detail of Bikes for specific Dealer.
        /// Modified By : Sushil Kumar on 26th March 2016
        /// Description : Added campaignId as a parameter to fetch dealer info based on camapign Id
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public IHttpActionResult Get(UInt16 dealerId, uint campaignId)
        {
            try
            {
                if (dealerId > 0 && campaignId > 0)
                {
                    DealerBikesEntity dealerBikes = _cache.GetDealerDetailsAndBikes(dealerId, campaignId);
                    DealerBikes bikes;
                    if (dealerBikes != null)
                    {
                        bikes = DealerBikesEntityMapper.Convert(dealerBikes);
                        return Ok(bikes);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Controllers.DealerLocator.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }


    }
}