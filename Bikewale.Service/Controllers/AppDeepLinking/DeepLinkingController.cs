using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Interfaces.AppDeepLinking;
using System.Web.Http.Description;
using Bikewale.Notifications;
using Bikewale.Interfaces.App;
using Bikewale.DTO.AppDeepLinking;
using Bikewale.Entities.AppDeepLinking;
using Bikewale.Service.AutoMappers.AppDeepLinking;

namespace Bikewale.Service.Controllers.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// Description : API to handle Deeplinking Requests.
    /// </summary>
    public class DeepLinkingController : ApiController
    {
        private readonly IDeepLinking _deeplink = null;
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : Controller to response user HTTP Request.
        /// </summary>
        /// <param name="deeplink"></param>
        public DeepLinkingController(IDeepLinking deeplink)
        {
            _deeplink = deeplink;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : Input : bikewale.com's URL, Output: DeepLinkingEntity.
        /// </summary>
        [ResponseType(typeof(Bikewale.DTO.AppDeepLinking.DeepLinking))]
        public IHttpActionResult Get(string url)
        {
            try 
            {
                if (Request.Headers.Contains("version_code") && Request.Headers.Contains("platformId") && !string.IsNullOrEmpty(url))
                {
                    DeepLinkingEntity deepLinkResponse = null;
                    deepLinkResponse = _deeplink.GetParameters(url);
                    if (deepLinkResponse != null)
                    {
                        return Ok(DeepLinkingMapper.Convert(deepLinkResponse));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.AndroidApp.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }       
    }
}