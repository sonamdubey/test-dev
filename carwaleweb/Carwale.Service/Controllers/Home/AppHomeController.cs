using Carwale.DAL;
using Carwale.DTOs;
using Carwale.Interfaces.Home;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Carwale.Interfaces.NewCars;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Geolocation;
using AEPLCore.Logging;

namespace Carwale.Service.Controllers
{
    public class AppHomeController : ApiController
    {
        private readonly IUnityContainer _container;
		private static Logger Logger = LoggerFactory.GetLogger();
		public AppHomeController(IUnityContainer container)
        {
            _container = container;
        }

		[AuthenticateBasic]
		[HttpGet, Route("api/v3/home/{cityId}")]
		public IHttpActionResult GetV1(int cityId)
		{
			try
			{
				CarDataAdapterInputs homeInput = new CarDataAdapterInputs
				{
					CustLocation = new Location
					{
						CityId = cityId
					}
				};
				CarHomeV3 _homeDto = null;
				IServiceAdapterV2 _appHomeAdapter = _container.Resolve<IServiceAdapterV2>("AppAdapterHomeV3");
				_homeDto = _appHomeAdapter.Get<CarHomeV3, CarDataAdapterInputs>(homeInput);

				if (_homeDto == null)
				{
					return NotFound();
				}
				return Ok(_homeDto);

			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
				return InternalServerError();
			}

		}

		[AuthenticateBasic]
        [HttpGet, Route("api/v2/home/{cityId:int:min(-1)}")]
        public IHttpActionResult Get(int cityId)
        {
            try
            {
                CarHomeV2 _homeDto = new CarHomeV2();

                bool isSupported = false, isLatest = false;
                AppFunctions.CheckVersionStatus(Convert.ToInt32(Request.Headers.GetValues("appVersion").First()), Convert.ToInt32(Request.Headers.GetValues("SourceId").First()), out isSupported, out isLatest);
                if (isSupported)
                {
                    _container.RegisterInstance<int>(cityId);

					Interfaces.Home.IServiceAdapter _appHomeAdapter = _container.Resolve<Interfaces.Home.IServiceAdapter>("AppAdapterHomeV2");
                    _homeDto = _appHomeAdapter.Get<CarHomeV2>(cityId.ToString());

                    _homeDto.Response = new AppResponseV2();

                    if (!isLatest)
                    {
                        _homeDto.Response.Code = "3";
                        _homeDto.Response.Message = "New version is available";
                    }
                    else
                    {
                        _homeDto.Response.Message = "Ok";
                        _homeDto.Response.Code = "1";
                    }
                }
                else
                {
                    _homeDto.Response = new AppResponseV2();
                    _homeDto.Response.Code = "2";
                    _homeDto.Response.Message = "Your application is not supported please download latest version";
                }

                if (_homeDto != null)
                    return Ok(_homeDto);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.AppHomeController.Get()");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [HttpGet, Route("api/home/"), AuthenticateBasic]
        public IHttpActionResult Get(string cityId = "-1")
        {
            try
            {
                //check app version
                bool isSupported = false, isLatest = false;
                AppFunctions.CheckVersionStatus(Convert.ToInt32(Request.Headers.GetValues("appVersion").First()), Convert.ToInt32(Request.Headers.GetValues("SourceId").First()), out isSupported, out isLatest);

                CarHome _homeDto = new CarHome();

                if (isSupported)
                {
					Interfaces.Home.IServiceAdapter _appHomeAdapter = _container.Resolve<Interfaces.Home.IServiceAdapter>("AppAdapterHome");
                    _homeDto = _appHomeAdapter.Get<CarHome>(cityId);

                    _homeDto.Response = new AppResponse();

                    if (!isLatest)
                    {
                        _homeDto.Response.Code = "3";
                        _homeDto.Response.Message = "New version is available";
                    }
                    else
                    {
                        _homeDto.Response.Message = "Ok";
                        _homeDto.Response.Code = "1";
                    }
                }
                else
                {
                    _homeDto.Response = new AppResponse();
                    _homeDto.Response.Code = "2";
                    _homeDto.Response.Message = "Your application is not supported please download latest version";
                }

                return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(_homeDto), Encoding.UTF8, "application/json") });
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.AppHomeController.Get(string cityId = " + cityId + ")");
                objErr.LogException();
                return InternalServerError(new Exception("Something went wrong on the server"));
            }
        }
    }
}
