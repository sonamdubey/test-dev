using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using Carwale.Entity.Enum;
using Carwale.DTOs.Dealer;
using Carwale.Utility;

namespace Carwale.Service.Filters.Campaigns
{
    public class LeadSubmissionValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            DealerInquiryDetailsDTO inquiryDetails = (DealerInquiryDetailsDTO)actionContext.ActionArguments["dealerInquiryDetails"];
           
            bool isValidRequest = true;
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(inquiryDetails.Name))
                isValidRequest = false;

            if (!(!string.IsNullOrWhiteSpace(inquiryDetails.Mobile) && RegExValidations.IsValidMobile(inquiryDetails.Mobile.ToString())))
                isValidRequest = false;

            if (Convert.ToInt16(inquiryDetails.ModelId) <= 0 || Convert.ToInt16(inquiryDetails.CityId) <= 0 || Convert.ToInt16(inquiryDetails.DealerId) <= 0)
                isValidRequest = false;

            if(Convert.ToInt16(inquiryDetails.ApplicationId) <= 0 || Convert.ToInt16(inquiryDetails.PlatformSourceId) <=0 || Convert.ToInt16(inquiryDetails.InquirySourceId) <=0 || Convert.ToInt16(inquiryDetails.LeadClickSource) <=0)
                isValidRequest = false;


            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Parameters");
            }
        }
    }
}
