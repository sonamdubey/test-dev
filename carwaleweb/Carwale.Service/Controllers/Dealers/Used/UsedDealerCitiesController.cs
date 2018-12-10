using Carwale.Entity.Dealers;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Service.Filters;
using System;
using System.Web.Http;

namespace Carwale.Service.Controllers.Dealers.Used
{
    public class UsedDealerCitiesController: ApiController
    {
        private readonly IUsedDealerCitiesBL _citiesBL;
        private readonly IGetUsedCarDealerStatus _getUsedCarDealerStatus;
        public UsedDealerCitiesController(IUsedDealerCitiesBL citiesBL, IGetUsedCarDealerStatus getUsedCarDealerStatus)
        {
            _citiesBL = citiesBL;
            _getUsedCarDealerStatus = getUsedCarDealerStatus;
        }

        //Extra Route atribute to give temporary support for old APIs in same action method
        [Route("api/useddealers/{dealerId}/cities"), Route("api/dealers/{dealerId}/cities"), HandleException, ApiAuthorization]
        public IHttpActionResult Get(int dealerId)
        {
            return Ok(_citiesBL.GetCities(dealerId));
        }

        [Route("api/useddealers/{dealerId}/cities"), Route("api/dealers/{dealerId}/cities"), HandleException, ValidateModel("multicityDealerCities"), LogApi, ApiAuthorization]
        public IHttpActionResult Put(int dealerId, [FromBody]MulticityDealerCities multicityDealerCities)
        {
            if (dealerId <= 0)
            {
                ModelState.AddModelError("Input", "Dealer Id must be greater than 0.");
            }
            else
            {
                string dealerStatus = _getUsedCarDealerStatus.GetDealerStatus(dealerId);
                if (dealerStatus != "ok")
                {
                    ModelState.AddModelError("Input", String.Format("Dealer status : {0}", dealerStatus));
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_citiesBL.SaveCities(dealerId, multicityDealerCities.CityIds))
            {
                return BadRequest("One or more cities of the list are invalid.");
            }
            return Ok("Successfully updated dealer cities.");
        }

        [Route("api/useddealers/{dealerId}/cities"), Route("api/dealers/{dealerId}/cities"), HandleException, LogApi, ApiAuthorization, ActionName("Cities")]
        public IHttpActionResult Delete(int dealerId)
        {
            //Don't check for ValidateDealer as it won't allow to delete deactivated dealer's cities
            _citiesBL.SaveCities(dealerId, null);
            return Ok("Successfully deleted dealer cities.");
        }
    }
}
