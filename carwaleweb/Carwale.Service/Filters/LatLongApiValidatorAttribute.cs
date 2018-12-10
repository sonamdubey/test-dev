using Carwale.Utility;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    public class LatLongApiValidatorAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// for parameters to be passed in querysting for citybylatlong api validator
        /// written by Natesh kumar on 5/11/14
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var querystringPrm = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value); // mapping value of parameter corresponding to keys
            bool isValidRequest = Validate(querystringPrm["latitude"], querystringPrm["longitude"]);
            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }
        }
        private static bool Validate(string latitude, string longitude)
        {
            return (ValidateLatitude(latitude) && ValidateLongitude(longitude));
        }
        private static bool ValidateLatitude(string latitude)
        {
            double lat;
            if (Double.TryParse(latitude, out lat) && RegExValidations.IsValidLatitude(lat))
            {
                return true;
            }
            return false;
        }
        private static bool ValidateLongitude(string longitude)
        {
            double lng;
            if (Double.TryParse(longitude, out lng) && RegExValidations.IsValidLongitude(lng))
            {
                return true;
            }
            return false;
        }
    }
}
