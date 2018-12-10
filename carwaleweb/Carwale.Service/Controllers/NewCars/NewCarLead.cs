using Carwale.BL.Dealers;
using Carwale.BL.ThirdParty.Leads;
using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.Cache.Dealers;
using Carwale.DAL.CarData;
using Carwale.DAL.Dealers;
using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.Entity.ThirdParty.Leads;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.NewCars
{
    public class NewCarLeadController : ApiController
    {
        private readonly IRequestManager<ThirdPartyInquiryDetails> _reqThirdParty;
        private readonly ILead _reqlead;
        public NewCarLeadController(IRequestManager<ThirdPartyInquiryDetails> reqThirdParty, ILead reqlead)
        {
            _reqThirdParty = reqThirdParty;
            _reqlead = reqlead;
        }

        [HttpPost]
        [Route("api/newcar/lead/")]
        public IHttpActionResult SaveLead()
        {
            return Ok();
        }

        [HttpPost]
        [AuthenticateBasic]
        [Route("api/newcar/lead/thirdparty/")]
        public IHttpActionResult SaveLeadThirdParty([FromBody] ThirdPartyInquiryDetails InquiryDetails)
        {
            try
            {
                IEnumerable<string> headerValues = Request.Headers.GetValues("SourceId");
                InquiryDetails.PartnerSourceId = headerValues.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarLead.SaveLeadThirdParty1()");
                objErr.LogException();
            }

            if ((InquiryDetails.ModelId < 1 && InquiryDetails.VersionId < 1) || InquiryDetails.CityId < 1)
            {
                InquiryDetails.StatusId = -1;
                _reqThirdParty.ProcessRequest<int>(InquiryDetails);
                return BadRequest("Invalid Parameters(model/version/city)");
            }
            if (!RegExValidations.IsValidMobile(InquiryDetails.Mobile))
            {
                InquiryDetails.StatusId = -2;
                _reqThirdParty.ProcessRequest<int>(InquiryDetails);
                return BadRequest("Invalid Parameter(mobile)");
            }

            try
            {
                var _pqDealerAdLeadId = _reqThirdParty.ProcessRequest<ulong>(InquiryDetails);
                if (_pqDealerAdLeadId < 1)
                    return BadRequest("Rejected");
                else
                    return Ok("Accepted");
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarLead.SaveLeadThirdParty2()");
                objErr.LogException();
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/newcar/leads/repushlead")]
        public IHttpActionResult RepushLead([FromBody] List<string> LeadIds)
        {
            if (_reqlead.RepushLead(LeadIds))
                return Ok();
            else
                return BadRequest("Rejected");
        }
    }
}
