using Bikewale.DTO;
using Bikewale.Interfaces.App;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
namespace Bikewale.Service.Controllers
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 05-May-2017
    /// Summary: Controller which holds actions for splash screen API
    /// </summary>
    public class SplashScreenController : CompressionApiController
    {
        private readonly ISplashScreen _IsplashScreen = null;

        public SplashScreenController(ISplashScreen isplashScreen)
        {
            _IsplashScreen = isplashScreen;
        }

        public IHttpActionResult Get()
        {
            SplashScreen objDTOSplash = null;
            try
            {
                // If android, IOS client sanitize the article content 
                string platformId = string.Empty;

                if (Request.Headers.Contains("platformId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                }

                if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                {
                    var objSplash = _IsplashScreen.GetAppSplashScreen();
                    if (_IsplashScreen != null && objSplash != null)
                    {
                        objDTOSplash = new SplashScreen();
                        objDTOSplash = MakeListMapper.Convert(objSplash);
                        return Ok(objDTOSplash);
                    }
                    else
                        return new StatusCodeResult(HttpStatusCode.NoContent, this);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controller.SplashScreenController");
                return InternalServerError();
            }
        }

    }
}