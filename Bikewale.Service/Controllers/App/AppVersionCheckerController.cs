using Bikewale.Entities.App;
using Bikewale.DTO.App;
using Bikewale.Interfaces.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Service.AutoMappers.App;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   To check whether APP Versions is supported and Latest
    /// Created On  :   07 Dec 2015
    /// </summary>
    public class AppVersionCheckerController : ApiController
    {
        private readonly IAppVersion _AppVersion = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AppVersion"></param>
        public AppVersionCheckerController(IAppVersion AppVersion)
        {
            _AppVersion = AppVersion;
        }

        /// <summary>
        /// Checks the App Version is supported and latest
        /// </summary>
        /// <returns>AppVersion DTO</returns>
        public IHttpActionResult Get()
        {
            Bikewale.DTO.App.AppVersion dto = null;
            Bikewale.Entities.App.AppVersion entity = null;
            uint appVersionId = 0, sourceId = 0;
            try
            {
                if (Request.Headers.Contains("version_code") && Request.Headers.Contains("platformId"))
                {
                    sourceId = Convert.ToUInt32(Request.Headers.GetValues("platformId").First().ToString());
                    appVersionId = Convert.ToUInt32(Request.Headers.GetValues("version_code").First().ToString());
                    entity = _AppVersion.CheckVersionStatus(appVersionId, sourceId);
                }

                if (entity != null)
                {
                    dto = AppVersionMapper.Convert(entity);
                    return Ok(dto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.App.AppVersionCheckerController");
                objErr.SendMail();
                return InternalServerError();
            }
        }

    }
}
