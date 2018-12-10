using AutoMapper;
using Carwale.Entity.Dealers;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.Service.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Carwale.Service.Controllers.Dealers
{
    [RoutePrefix("api/dealers")]
    public class CwDealersController : ApiController
    {
        private readonly IDealers _dealer;
        private readonly IDealerCache _dealerCache;
        private readonly ICarModelCacheRepository _carModelCache;
        private readonly INewCarDealers _newCarDealers;
        private readonly IGetUsedCarDealerStatus _getUsedCarDealerStatus;


        public CwDealersController(IDealers dealer, IDealerCache dealerCache, INewCarDealers newCarDealers, ICarModelCacheRepository carModelCache, IGetUsedCarDealerStatus getUsedCarDealerStatus)
        {
            _dealer = dealer;
            _dealerCache = dealerCache;
            _newCarDealers = newCarDealers;
            _carModelCache = carModelCache;
            _getUsedCarDealerStatus = getUsedCarDealerStatus;
        }

        [HttpGet]
        public IHttpActionResult DealerDetailsOnDealerId(int dealerId)
        {
            if (dealerId < 1)
                return BadRequest("Invalid Parameter");

            var content = _dealer.GetDealerDetailsOnDealerId(dealerId);

            return Ok(Mapper.Map<Carwale.Entity.Dealers.DealerDetails, Carwale.DTO.Dealers.DealerDetails>(content));
        }

        [HttpGet]
        public IHttpActionResult DealerDetailsOnMakeState(int makeId, int stateId, int count = -1)
        {
            if (stateId < 1 || makeId < 1)
                return BadRequest("Invalid Parameter");

            var content = _dealer.DealerDetailsOnMakeState(makeId, stateId, count);

            return Ok(Mapper.Map<List<Carwale.Entity.Dealers.DealerDetails>, List<Carwale.DTO.Dealers.DealerDetails>>(content));
        }

        /// <summary>
        /// Gets the dealers in nearest city from the specified cityId AND within the given dealerIds
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="campaignIds"></param>
        /// <returns></returns>
        [Route("nearest")]
        [HttpGet]
        public IHttpActionResult NearestDealerDetails(int cityId, string campaignIds)
        {
            if (string.IsNullOrWhiteSpace(campaignIds))
                return BadRequest("Invalid Parameter");
            //var content = _dealerCache.GetNearbyDealers(cityId, campaignIds);

            //return Ok(Mapper.Map<List<Carwale.Entity.Dealers.NewCarDealer>, List<Carwale.DTO.Dealers.NewCarDealer>>(content));
            return Ok();
        }

        [HttpGet, Route("outlets")]
        public IHttpActionResult GetDealersByMakeModel(int cityId, int makeId = 0, int modelId = 0)
        {
            if (cityId < 1 || (makeId < 1 && modelId < 1))
                return BadRequest("Bad Parameters");
            int make = 0;
            if (modelId > 0)
            {
                try
                {
                    make = _carModelCache.GetModelDetailsById(modelId).MakeId;
                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetCTDealers");
                    objErr.LogException();
                    return BadRequest("Bad Parameters");
                }
            }
            makeId = makeId > 0 ? makeId : make;
            if (make > 0 && makeId != make)
                return BadRequest("Bad Parameters");

            try
            {
                var dealers = _newCarDealers.GetDealersByMakeModel(makeId, modelId, cityId);
                if (dealers == null || dealers.Count < 1)
                    return StatusCode(HttpStatusCode.NoContent);
                return Ok(dealers);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetCTDealers");
                objErr.LogException();
                return InternalServerError();
            }

        }
    }
}
