using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using System;
using System.Web.Http;

namespace Bikewale.Service.Controllers
{
    public class FinanceController : ApiController
    {
        private readonly IFinanceRepository _objRepository = null;
        public FinanceController(IFinanceRepository objRepository)
        {
            _objRepository = objRepository;

        }
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/savepersonaldetails/")]
        public IHttpActionResult SavePersonalDetails([FromBody] PersonalDetails objDetails)
        {
            try {
                _objRepository.SavePersonalDetails(objDetails);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.SavePersonalDetails");

                return InternalServerError();
            }


        }


        /// <summary>
        /// Created By :- Subodh Jain on 11 september 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/saveemployedetails/")]
        public IHttpActionResult SaveEmployeDetails([FromBody] EmployeDetails objDetails)
        {
            try
            {
                _objRepository.SaveEmployeDetails(objDetails);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.SaveEmployeDetails");

                return InternalServerError();
            }


        }
    }
}
