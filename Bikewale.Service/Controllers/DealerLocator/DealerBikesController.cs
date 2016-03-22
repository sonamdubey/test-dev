using Bikewale.DTO.DealerLocator;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.DealerLocator;
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
        private readonly Bikewale.Interfaces.DealerLocator.IDealer _dealer = null;
        private readonly IDealerCacheRepository _cache = null;

        public DealerBikesController(Bikewale.Interfaces.DealerLocator.IDealer dealer, IDealerCacheRepository cache)
        {
            _dealer = dealer;
            _cache = cache;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 22 March 2016
        /// Description : To get Detail of Bikes for specific Dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IHttpActionResult Get(UInt16 dealerId)
        {
            try
            {
                DealerBikesEntity dealerBikes = _cache.GetDealerBikes(dealerId);
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Controllers.DealerLocatorGet");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        

    }
}