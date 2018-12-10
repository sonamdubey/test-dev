using Carwale.Entity.Dealers;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Service.Filters;
using System;
using System.Web.Http;

namespace Carwale.Service.Controllers.Dealers.Used
{
    [RoutePrefix("api/useddealers")]
    public class UsedDealerRatingsController : ApiController
    {
        private readonly IUsedDealerRatingsBL _ratingsBL;
        private readonly IGetUsedCarDealerStatus _getUsedCarDealerStatus;
        public UsedDealerRatingsController(IUsedDealerRatingsBL ratingsBL, IGetUsedCarDealerStatus getUsedCarDealerStatus)
        {
            _ratingsBL = ratingsBL;
            _getUsedCarDealerStatus = getUsedCarDealerStatus;
        }

        [Route("{dealerId}/rating"), HandleException, ApiAuthorization]
        public IHttpActionResult Get(int dealerId)
        {
            return Ok(_ratingsBL.GetRating(dealerId));
        }

        [Route("{dealerId}/rating"), HandleException, LogApi, ValidateModel("dealerRating"), ApiAuthorization]
        public IHttpActionResult Put(int dealerId, [FromBody]UsedCarDealersRating dealerRating)
        {
            if (dealerId <= 0)
            {
                ModelState.AddModelError("Input", "Dealer Id must be greater than 0.");
            }
            else
            {
                string dealerStatus = _getUsedCarDealerStatus.GetDealerStatus(dealerId);
                if (!(dealerStatus == "ok" || dealerStatus == "package has expired"))
                {
                    ModelState.AddModelError("Input", String.Format("Dealer status : {0}", dealerStatus));
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _ratingsBL.SaveRating(dealerId, dealerRating.RatingText);
            return Ok();
        }

        [Route("{dealerId}/rating"), HandleException, LogApi, ApiAuthorization]
        public IHttpActionResult Delete(int dealerId)
        {
            _ratingsBL.SaveRating(dealerId, null);
            return Ok();
        }

    }
}
