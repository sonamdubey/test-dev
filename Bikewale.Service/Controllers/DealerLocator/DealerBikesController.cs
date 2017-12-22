using System;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.DTO.DealerLocator;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.DealerLocator;
using Bikewale.Service.Utilities;

namespace Bikewale.Service.Controllers.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 22 march 2016
    /// Description : for getting dealer detail and bike detail w.r.t dealer.
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class DealerBikesController : CompressionApiController//ApiController
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
                    Bikewale.DTO.DealerLocator.DealerBikes bikes;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.DealerLocator.DealerBikesController.Get");
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 20 May 2016
        /// Description :   Returns the Bike list for APP
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [ResponseType(typeof(DTO.DealerLocator.v2.DealerBikes)), Route("api/v2/DealerBikes/")]
        public IHttpActionResult GetV2(uint dealerId, uint campaignId)
        {
            try
            {
                if (dealerId > 0 && campaignId > 0)
                {
                    DealerBikesEntity dealerBikes = _cache.GetDealerDetailsAndBikes(dealerId, campaignId);
                    Bikewale.DTO.DealerLocator.v2.DealerBikes bikes = null;
                    if (dealerBikes != null)
                    {
                        bikes = new DTO.DealerLocator.v2.DealerBikes();
                        bikes.Bikes = DealerBikesEntityMapper.ConvertV2(dealerBikes.Models);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.DealerLocator.DealerBikesController.GetV2");
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 19th Dec 2017
        /// Summary : Get list of bikes for given dealer with it's on-road price
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(DealerBikeModels)), Route("api/dealer/{dealerId}/make/{makeId}/models/")]
        public IHttpActionResult GetDealerBikes(uint dealerId, uint makeId)
        {
            try
            {
                if(dealerId > 0 && makeId > 0)
                {
                    DealerBikeModelsEntity dealerDetails = _cache.GetBikesByDealerAndMake(dealerId, makeId);
                    if(dealerDetails != null)
                    {
                        DealerBikeModels dealerBikes = DealerBikesEntityMapper.Convert(dealerDetails);
                        return Ok(dealerBikes);
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
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Service.DealerLocator.DealerBikesController, dealerId: {0} and makeId: {1}", dealerId, makeId));
                return InternalServerError();
            }
        }

    }
}