using AutoMapper;
using Carwale.BL.Insurance;
using Carwale.DTOs;
using Carwale.DTOs.Insurance;
using Carwale.Entity.Enum;
using Carwale.Entity.Insurance;
using Carwale.Interfaces.Insurance;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Insurance
{
    [EnableCors(origins: "http://www.bikewale.com,https://www.bikewale.com,https://staging.bikewale.com,http://localhost:9011,http://staging.bikewale.com,http://webserver:9011", headers: "*", methods: "*")]
    public class ClientInsuranceController : ApiController
    {
        protected JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
        protected Clients requestSource = 0;
        protected Application application = (Application)1;
        protected Platform platform = Platform.CarwaleDesktop;
        protected int AppVersion = 0;
        private readonly IUnityContainer _unityContainer;
        protected readonly string maskingNumber = ConfigurationManager.AppSettings["InsuranceAPIMaskingNumber"];
                
        public ClientInsuranceController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }
        /// <summary>
        /// Api to submit Insurance Lead
        /// </summary>
        /// <param name="request"></param>
        /// <param name="insuranceLead"></param>
        /// <returns></returns>
        [Route("api/insurance/quote/")]
        [HttpPost]
        public IHttpActionResult SubmitLead([FromBody] InsuranceLead inputs)
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                    inputs.clientId = requestSource;
                }
                else
                {
                    return BadRequest("clientid is missing");
                }
                if (
                    (requestSource != Clients.Coverfox && requestSource != Clients.RoyalSundaram && requestSource != Clients.CW)
                    &&
                    (string.IsNullOrWhiteSpace(inputs.Name) || string.IsNullOrWhiteSpace(inputs.CityName) || !(RegExValidations.IsValidEmail(inputs.Email) || RegExValidations.IsNumeric(inputs.Mobile) || RegExValidations.IsPositiveNumber(Convert.ToString(inputs.CityId)) || RegExValidations.IsPositiveNumber(Convert.ToString(inputs.VersionId)) || RegExValidations.IsValidDate(inputs.CarPurchaseDate)))
                )
                {
                    return BadRequest("invalid Parameters");
                }


                //ApplicationId
                if (Request.Headers.Contains("platformid") && Request.Headers.GetValues("platformid").First()=="2")
                {
                    Enum.TryParse<Application>(Request.Headers.GetValues("platformid").First(), out application);
                    inputs.Application = application;
                }
                else if (Request.Headers.Contains("application"))
                {
                    Enum.TryParse<Application>(Request.Headers.GetValues("application").First(), out application);
                    inputs.Application = application;
                }
                else
                {
                    inputs.Application = application;
                }
                //PlatformId
                if (Request.Headers.Contains("sourceId"))
                {
                    Enum.TryParse<Platform>(Request.Headers.GetValues("sourceId").First(), out platform);
                    inputs.Platform = platform;
                }
                else {
                    inputs.Platform = platform;
                }
                IInsurance _iInsurance;

                if (inputs.clientId == Clients.Coverfox) 
                {
                    _iInsurance = _unityContainer.Resolve<IInsurance>("Coverfox");
                }
                else if (inputs.clientId == Clients.CW)
                {
                    _iInsurance = _unityContainer.Resolve<IInsurance>("CW");
                }
                else if (inputs.clientId == Clients.RoyalSundaram)
                {
                    _iInsurance = _unityContainer.Resolve<IInsurance>("RoyalSundaram");
                }   
                else if (inputs.clientId == Clients.Chola)
                {
                    _iInsurance = _unityContainer.Resolve<IInsurance>("chola");
                }
                else
                {
                    _iInsurance = _unityContainer.Resolve<IInsurance>("PolicyBoss");
                }
                    
                var data = _iInsurance.SubmitLead(inputs);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientInsurance.SubmitLead()"+(inputs==null?"NULL":JsonConvert.SerializeObject(inputs)));
                objErr.LogException();
            }
            return InternalServerError();
        }

        [Route("api/v2/insurance/quote/")]
        [HttpPost]
        public IHttpActionResult SubmitLeadV2([FromBody] InsuranceLead inputs)
        {
            try
            {
                if (inputs == null) return BadRequest("inputs cannot be null");
                if (Request.Headers.Contains("sourceId"))
                {
                    Enum.TryParse<Platform>(Request.Headers.GetValues("sourceId").First(), out platform);
                    inputs.Platform = platform;
                }

                if (Request.Headers.Contains("AppVersion"))
                {
                    int.TryParse(Request.Headers.GetValues("AppVersion").First(), out AppVersion);
                }

                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                    inputs.clientId = requestSource;
                }

                //DECIDE CLIENT N SEND LEAD
                InsuranceAdapter Adapter = new InsuranceAdapter(_unityContainer, inputs.clientId);
                var data = Adapter.Get(inputs, AppVersion);
                //
                
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientInsurance.SubmitLead()" + (inputs == null ? "NULL" : JsonConvert.SerializeObject(inputs)));
                objErr.LogException();
            }
            return InternalServerError();
        }

        [Route("api/insurance/screen/")]
        [AuthenticateBasic]
        public IHttpActionResult GetInsuranceScreen()
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                }
                else
                {
                    return BadRequest("clientid is missing");
                }
                if (Request.Headers.Contains("sourceId"))
                {
                    Enum.TryParse<Platform>(Request.Headers.GetValues("sourceId").First(), out platform);
                }
                if (Request.Headers.Contains("AppVersion"))
                {
                    int.TryParse(Request.Headers.GetValues("AppVersion").First(), out AppVersion);
                }
                               
                IInsurance _iInsurance = _unityContainer.Resolve<IInsurance>("RoyalSundaram");

                if (platform == Platform.CarwaleiOS && AppVersion >= 20)
                {
                    var V2Dto = new InsuranceScreenV2();
                    V2Dto.Makes = _iInsurance.GetMakes(application);
                    V2Dto.Cities = Mapper.Map<List<InsuranceCity>, List<InsuranceCityDTO>>(_iInsurance.GetCities(application));
                    V2Dto.MaskingNumber = maskingNumber == null ? string.Empty : maskingNumber;
                    return Json(V2Dto, jsonSerializerSettings);
                }
                else
                {
                    InsuranceScreen dto = new InsuranceScreen();
                    dto.Makes = _iInsurance.GetMakes(application);
                    dto.Cities = _iInsurance.GetCities(application);
                    dto.MaskingNumber = maskingNumber == null ? string.Empty : maskingNumber;
                    return Json(dto, jsonSerializerSettings);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientInsurance.GetMakes()");
                objErr.LogException();
            }
            return InternalServerError();
        }


        /// <summary>
        /// Get Respective Mapped Makes
        /// </summary>
        /// <returns></returns>
        [Route("api/insurance/makes/")]
        public IHttpActionResult GetMakes()
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                }
                else
                {
                    return BadRequest("clientid is missing");
                }
                if (Request.Headers.Contains("platformid"))
                {
                    Enum.TryParse<Application>(Request.Headers.GetValues("platformid").First(), out application);
                }

                InsuranceAdapter insuranceAdapter = new InsuranceAdapter(_unityContainer, requestSource);
                var data = insuranceAdapter.GetMakes(application);

                return Json(data, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientInsurance.GetMakes()");
                objErr.LogException();
            }
            return InternalServerError();
        }

        /// <summary>
        /// Get Respective Mapped Models
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Route("api/insurance/models/{makeid:int:min(1)}/")]
        public IHttpActionResult GetModels(int makeId)
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                }
                else
                {
                    return BadRequest("clientid is missing");
                }
                if (Request.Headers.Contains("platformid"))
                {
                    Enum.TryParse<Application>(Request.Headers.GetValues("platformid").First(), out application);
                }

                InsuranceAdapter insuranceAdapter = new InsuranceAdapter(_unityContainer, requestSource);
                var data = insuranceAdapter.GetModels(makeId, application);
                return Json(data, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PolicyBoss.GetModels()");
                objErr.LogException();
            }

            return InternalServerError();
        }

        /// <summary>
        /// Get Respective Mapped Versions
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        
        [Route("api/insurance/versions/{modelid:int:min(1)}")]
        public IHttpActionResult GetVersions(int modelId)
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                }
                else
                {
                    return BadRequest("clientid is missing");
                }
                if (Request.Headers.Contains("platformid"))
                {
                    Enum.TryParse<Application>(Request.Headers.GetValues("platformid").First(), out application);
                }

                InsuranceAdapter insuranceAdapter = new InsuranceAdapter(_unityContainer, requestSource);
                var data = insuranceAdapter.GetVersions(modelId, application);
                return Json(data, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientInsurance.GetVariants()"+modelId);
                objErr.LogException();
            }
            return InternalServerError();
        }

        /// <summary>
        /// Get Respective Mapped RTOCities
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Route("api/insurance/cities")]
        public IHttpActionResult GetCities()
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestSource);
                }
                else
                {
                    return BadRequest("clientid is missing");
                }

                InsuranceAdapter insuranceAdapter = new InsuranceAdapter(_unityContainer, requestSource);
                var data = insuranceAdapter.GetCities(application);

                return Json(data, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientInsurance.GetCities()");
                objErr.LogException();
            }
            return InternalServerError();
        }
    }
}
