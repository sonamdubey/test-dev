using Bikewale.Entities.AppDeepLinking;
using Bikewale.Interfaces.AppDeepLinking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.AppDeepLinking;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// Description : API to handle Deeplinking Requests.
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class DeepLinkingController : CompressionApiController//ApiController
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.AndroidApp.Get");
                return InternalServerError();
            }
        }
    }
}