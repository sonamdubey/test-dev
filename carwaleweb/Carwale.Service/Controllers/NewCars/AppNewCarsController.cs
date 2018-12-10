using Carwale.DTOs.NewCars;
using Carwale.Interfaces.Home;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Linq;

namespace Carwale.Service.Controllers
{
    public class AppNewCarsController : ApiController
    {
        private readonly IUnityContainer _container;

        public AppNewCarsController(IUnityContainer container)
        {
            _container = container;
        }

        [HttpGet, AuthenticateBasic,Route("api/newcars/")]
        public IHttpActionResult Get()
        {
            NewCarHome _newCarHomeDto = null;
            try
            {
                Carwale.Interfaces.NewCars.IServiceAdapter _appNewcarAdapter = _container.Resolve<Carwale.Interfaces.NewCars.IServiceAdapter>("AppAdapterNewCars");
                _newCarHomeDto = _appNewcarAdapter.Get<NewCarHome>();

                return ResponseMessage(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonConvert.SerializeObject(_newCarHomeDto), Encoding.UTF8, "application/json") });
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AppNewCarsController.Get(): old");
                objErr.LogException();
                return ResponseMessage(new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("Something went wrong on the server", Encoding.UTF8, "application/json") });
            }
        }

        [Route("api/v2/newcars/{cityId:int:min(-1)}")]
        public IHttpActionResult Get(int cityId)
        {
            NewCarHomeV2 _newCarHomeDTO = null;
            try
            {
                _container.RegisterInstance(cityId);
                IServiceAdapter _appNewcarAdapter = _container.Resolve<IServiceAdapter>("AppAdapterNewCarsV2");
                _newCarHomeDTO = _appNewcarAdapter.Get<NewCarHomeV2>(cityId.ToString());

                int sourceId=0;
                int appVersionId=0;
                IEnumerable<string> headerValues;
                if (Request.Headers.TryGetValues("SourceId", out headerValues))
                {
                    int.TryParse(headerValues.First(), out sourceId);
                }
                if (Request.Headers.TryGetValues("AppVersionId", out headerValues))
                {
                    int.TryParse(headerValues.First(), out appVersionId);
                }
                if (_newCarHomeDTO != null && _newCarHomeDTO.Makes.IsNotNullOrEmpty() && sourceId == 83 && appVersionId <= 30)
                {
                    _newCarHomeDTO.Makes =_newCarHomeDTO.Makes.Where(x => x.MakeId != 70).ToList();
                }
                if (_newCarHomeDTO != null)
                    return Ok(_newCarHomeDTO);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AppNewCarsController.Get()");
                objErr.LogException();
            }
            return InternalServerError();
        }


        [HttpGet,Route("api/v3/newcars/{cityId:int:min(-1)}")]
        public IHttpActionResult GetResult(int cityId)
        {
            NewCarHomeV3 _newCarHomeDTO = null;
            try
            {            
                Interfaces.NewCars.IServiceAdapterV2 _appNewcarAdapter = _container.Resolve<Interfaces.NewCars.IServiceAdapterV2>("AppAdapterNewCarsV3");
                _newCarHomeDTO = _appNewcarAdapter.Get<NewCarHomeV3,int>(cityId);

                if (_newCarHomeDTO != null)
                {
                    return Ok(_newCarHomeDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return InternalServerError();
        }
    }
}
