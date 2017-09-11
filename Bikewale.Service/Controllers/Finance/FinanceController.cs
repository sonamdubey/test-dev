using Bikewale.DTO.Finance;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Finance;
using System;
using System.Web.Http;

namespace Bikewale.Service.Controllers
{
    public class FinanceController : ApiController
    {
        private readonly IFinanceRepository _objRepository = null;
        private readonly ICapitalFirst _objICapitalFirst = null;
        public FinanceController(IFinanceRepository objRepository, ICapitalFirst objICapitalFirst)
        {
            _objRepository = objRepository;
            _objICapitalFirst = objICapitalFirst;

        }
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/savepersonaldetails/")]
        public IHttpActionResult SavePersonalDetails(PersonalDetails objDetails)
        {
            try
            {
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
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/saveemployedetails/")]
        public IHttpActionResult SaveEmployeDetails(EmployeDetails objDetails)
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

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Finance Lead API for Capital First
        /// This API will be called by CarTrade to push Capital First Loan Voucher details and Capital First Agent Details
        /// </summary>
        /// <param name="ctLeadId"></param>
        /// <param name="voucher">Capital First Voucher details</param>
        /// <returns></returns>
        [HttpPost, Route("api/finance/capitalfirst/{ctLeadId}/")]
        public IHttpActionResult SaveCapitalFirstVoucherDetails(string ctLeadId, [FromBody]CapitalFirstVoucherDTO voucher)
        {
            if (!String.IsNullOrEmpty(ctLeadId) && voucher != null)
            {
                try
                {
                    CapitalFirstVoucherEntityBase entity = FinanceMapper.Convert(voucher);
                    string message = _objICapitalFirst.SaveVoucherDetails(ctLeadId, entity);
                    return Ok(message);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, String.Format("Bikewale.Service.Controllers.FinanceController.SaveCapitalFirstVoucherDetails({0},{1})", ctLeadId, Newtonsoft.Json.JsonConvert.SerializeObject(voucher)));
                    return InternalServerError(new Exception("Server error has occured."));
                }
            }
            else
            {
                return BadRequest("Invalid Request");
            }
        }
    }
}
