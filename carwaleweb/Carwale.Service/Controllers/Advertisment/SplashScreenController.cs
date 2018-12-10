using AutoMapper;
using Carwale.DTOs.Advertisment;
using Carwale.Entity.Advertizings.Apps;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Advertizings.App;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Advertisment
{
    public class SplashScreenController : ApiController
    {
        private readonly IAppSplashCache _appSplashCache;
        private readonly IAppSplashScreenBL _appSplashScreenBL;
        
        public SplashScreenController(IAppSplashCache appSplashCache, IAppSplashScreenBL appSplashScreenBL)
        {
            _appSplashCache = appSplashCache;
            _appSplashScreenBL = appSplashScreenBL;
        }

        
        [HttpGet, AuthenticateBasic, Route("api/v1/customsplash")]
        public IHttpActionResult GetRandomSplashScreen(int applicationId = 1)
        {
            try
            {
                if (Request.Headers.Contains("sourceid"))
                {
                    Platform platform;
                    Enum.TryParse<Platform>(Request.Headers.GetValues("sourceid").First(), out platform);
                    int platformId = Convert.ToInt32(platform);
                    var splashResult=_appSplashScreenBL.GetRandomSplashScreen(platformId, applicationId);
                    return Ok(Mapper.Map<SplashScreenBanner, CustomSplashDTO>(splashResult));
                }
                else
                {
                    return BadRequest("platformid is missing");
                }                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SplashScreenController.GetRandomSplashScreen()");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [EnableCors(origins: "https://www.bikewale.com,https://staging.bikewale.com,http://localhost:9096", headers: "*", methods: "GET")]
        [HttpGet, AuthenticateBasic, Route("api/customsplash")]
        public IHttpActionResult GetSplashScreen(int applicationId = 1)
        {
            try
            {
                if (Request.Headers.Contains("sourceid"))
                {
                    Platform platform;
                    Enum.TryParse<Platform>(Request.Headers.GetValues("sourceid").First(), out platform);
                    int platformId = Convert.ToInt32(platform);
                    return Ok(_appSplashScreenBL.GetSplashScreenByPriority(platformId, applicationId));                    
                }
                else
                {
                    return BadRequest("platformid is missing");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SplashScreenController.GetSplashScreen()");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [HttpGet, AuthenticateBasic, Route("api/v2/customsplash")]
        public IHttpActionResult GetRandomSplashScreenV2(int applicationId = 1)
        {
            try
            {
                Platform platform;
                Enum.TryParse<Platform>(Request.Headers.GetValues("sourceid").First(), out platform);
                int platformId = Convert.ToInt32(platform);
                var splashResult = _appSplashScreenBL.GetRandomSplashScreen(platformId, applicationId);
                if (splashResult != null && !splashResult.IsDefault && !string.IsNullOrWhiteSpace(splashResult.Splashurl))
                {
                    return Ok(Mapper.Map<SplashScreenBanner, SplashScreenBannerDto>(splashResult));
                }
                return Content(HttpStatusCode.NoContent, "Splash Screen");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
