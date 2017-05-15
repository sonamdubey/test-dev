using Bikewale.DTO;
using Bikewale.Interfaces.App;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
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
                var objSplash = _IsplashScreen.GetAppSplashScreen();
                if (_IsplashScreen != null && objSplash != null)
                {
                    objDTOSplash = new SplashScreen();
                    objDTOSplash = MakeListMapper.Convert(objSplash);
                    return Ok(objDTOSplash);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controller.SplashScreenController");
                return InternalServerError();
            }
        }

    }
}