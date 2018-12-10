using Carwale.Entity.CMS.URIs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;



namespace Carwale.Service.Filters.CMS
{
    public class CMSImageApiValidator : ActionFilterAttribute
    {
        /// <summary>
        /// validation of querystring b4 api execution
        /// written by Natesh Kumar on 26/9/14
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var querystringPrm = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value); // mapping value of parameter corresponding to keys
            string categoryIdList = string.Empty;
            bool isValidRequest = true;

            if (!RegExValidations.IsNumeric(querystringPrm["applicationid"]))
            {
                isValidRequest = false;
            }
            else
            {
                ushort appid = Convert.ToUInt16(querystringPrm["applicationid"]);
                if (appid != 1 && appid != 2)
                {
                    isValidRequest = false;
                }
            }

            if (!RegExValidations.IsNumeric(querystringPrm["modelid"]))
            {
                isValidRequest = false;
            }
            else
            {
                uint modelId = Convert.ToUInt32(querystringPrm["modelid"]);
                if (modelId == 0)
                {
                    isValidRequest = false;
                }
            }
            if (querystringPrm.ContainsKey("categoryidlist"))
            {
                categoryIdList = querystringPrm["categoryidlist"];
            }

            if (!string.IsNullOrEmpty(categoryIdList) && !RegExValidations.ValidateCommaSeperatedNumbers("categoryIdList"))
            {
                isValidRequest = false;
            }

            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }

        }
    }
}
