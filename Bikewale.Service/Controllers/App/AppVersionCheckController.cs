using Bikewale.DTO.App;
using Bikewale.Interfaces.App;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.App;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   To check whether APP Versions is supported and Latest
    /// Created On  :   07 Dec 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class AppVersionCheckController : CompressionApiController//ApiController
    {
        private readonly IAppVersionCache _AppVersion = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AppVersion"></param>
        public AppVersionCheckController(IAppVersionCache AppVersion)
        {
            _AppVersion = AppVersion;
        }

        /// <summary>
        /// Checks the App Version is supported and latest
        /// </summary>
        /// <returns>AppVersion DTO</returns>
        [ResponseType(typeof(Bikewale.DTO.App.AppVersion))]
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
                    if (dto.IsLatest && dto.IsSupported)
                    {
                        dto.Code = AppVersionMessageCode.LatestVersion;
                        dto.Message = "Latest version";
                    }
                    else if (dto.IsSupported && (!dto.IsLatest))
                    {
                        dto.Code = AppVersionMessageCode.OlderVersionStillSupported;
                        dto.Message = "New version is available";
                    }
                    else if ((!dto.IsLatest) && (!dto.IsSupported))
                    {
                        dto.Code = AppVersionMessageCode.OldVersion;
                        dto.Message = "Your application is not supported please download latest version";
                    }
                    dto.IsTrackDayVisible = Bikewale.Utility.BWConfiguration.Instance.IsAppTrackDayVisible;
                    return Ok(dto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.App.AppVersionCheckerController");
               
                return InternalServerError();
            }
        }

    }
}
