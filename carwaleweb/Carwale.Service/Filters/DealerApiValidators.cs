using Carwale.Entity.CMS.URIs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;



namespace Carwale.Service.Filters.Dealer
{
    public static class CampaignValidation
    {
        public static string ValidatePlatform(NameValueCollection querystringPrm)
        {
            if (string.IsNullOrWhiteSpace(querystringPrm["platformid"]))
            {
                string message = "Bad Request, platformid cannot be null";
                return message;
            }
            return string.Empty;
        }

        public static string ValidateHeaderPlatform(NameValueCollection headerstringPrm)
        {
            if (string.IsNullOrWhiteSpace(headerstringPrm["sourceid"]))
            {
                string message = "Bad Request, platformid cannot be null";
                return message;
            }
            return string.Empty;
        }

        public static string ValidateModelCity(NameValueCollection querystringPrm)
        {
            if (string.IsNullOrWhiteSpace(querystringPrm["cityid"]) || !RegExValidations.IsNumeric(querystringPrm["cityid"]) ||
                string.IsNullOrWhiteSpace(querystringPrm["modelid"]) || !RegExValidations.IsPositiveNumber(querystringPrm["modelid"]))
            {
                string message = "Bad Request, invalid cityid or modelid";
                return message;
            }
            return string.Empty;
        }

        public static string ValidateZone(NameValueCollection querystringPrm)
        {
            if (!(string.IsNullOrWhiteSpace(querystringPrm["zoneid"]) || RegExValidations.IsNumeric(querystringPrm["zoneid"])))
            {
                string message = "Bad Request, invalid zoneid";
                return message;
            }
            return string.Empty;
        }

        public static string ValidateArea(NameValueCollection querystringPrm)
        {
            if (!(string.IsNullOrWhiteSpace(querystringPrm["areaid"]) || RegExValidations.IsNumeric(querystringPrm["areaid"])))
            {
                string message = "Bad Request, invalid areaid";
                return message;
            }
            return string.Empty;
        }

        public static bool IsValidCampaign(NameValueCollection querystringPrm)
        {
            if (string.IsNullOrWhiteSpace(querystringPrm["campaignid"]) || !RegExValidations.IsPositiveNumber(querystringPrm["campaignid"]))
            {
                return false;
            }
            return true;
        }
    }

    public class DealerCampaignValidation : ActionFilterAttribute
    {
        /// <summary>
        /// validation of querystring b4 api execution
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            NameValueCollection querystringPrm = HttpContext.Current.Request.QueryString;

            string message = CampaignValidation.ValidatePlatform(querystringPrm);
            if (message == string.Empty && !CampaignValidation.IsValidCampaign(querystringPrm))
            {
                message = CampaignValidation.ValidateModelCity(querystringPrm);
                if (message == string.Empty)
                {
                    message = CampaignValidation.ValidateZone(querystringPrm);
                }
            }

            if (message != string.Empty)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }
        }
    }

    public class DealerAdValidation : ActionFilterAttribute
    {
        /// <summary>
        /// validation of querystring b4 api execution
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            NameValueCollection querystringPrm = HttpContext.Current.Request.QueryString;
            NameValueCollection requestHeaders = HttpContext.Current.Request.Headers;

            string message = CampaignValidation.ValidateHeaderPlatform(requestHeaders);
            if(message == string.Empty && !CampaignValidation.IsValidCampaign(querystringPrm))
            {
                message = CampaignValidation.ValidateModelCity(querystringPrm);
                if (message == string.Empty)
                {
                    message = CampaignValidation.ValidateArea(querystringPrm);
                    if(message == string.Empty)
                    {
                        message = CampaignValidation.ValidateZone(querystringPrm);
                    }
                }
            }

            if (message != string.Empty)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
        }
    }
}
