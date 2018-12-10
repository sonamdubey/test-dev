using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using Carwale.Utility.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers
{
    public class CarMakesDataController : ApiController
    {
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly ICarMakesRepository _carMakesRepo;
        private readonly JsonSerializerSettings _serializerSettings;

        public CarMakesDataController(ICarMakesCacheRepository carMakesCacheRepo, ICarMakesRepository carMakesRepo)
        {
            _carMakesCacheRepo = carMakesCacheRepo;
            _carMakesRepo = carMakesRepo;
            _serializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Modified By:rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver, send status code 400 (Bad request),404(if makes not found) and 200(makes are available)
        /// </summary>
        /// <returns></returns>
        //[AthunticateBasic]
        public IHttpActionResult GetValuationMakes(int year)
        {
            var carMakesList = new ValuationMakesDTO()
            {
                Makes = _carMakesRepo.GetValuationMakes(year)
            };

            if (carMakesList.Makes.Count == 0)
                return NotFound();

            return Ok(carMakesList);
        }

        /// <summary>
        /// Populates the make list based on type passed
        /// Written By : Supriya on 2/6/2014
        /// Modified By:rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver,404(if makes not found) and 200(makes are available)
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8081,https://cwoprst.carwale.com,https://operations.carwale.com", headers: "*", methods: "GET"), Route("api/makes/"), Route("api/carmakesdata/GetCarMakes/"), Route("webapi/carmakesdata/GetCarMakes/")]
        public IHttpActionResult GetCarMakes(string type = "all", int? year = null, Modules? module = null, bool? isPopular = null, int filter = 0)
        {
            try
            {
                ThreeSixtyViewCategory threesixtytype;
                if (Enum.TryParse<ThreeSixtyViewCategory>(HttpContext.Current.Request.QueryString["threeSixtyType"], true, out threesixtytype))
                    filter = (int)threesixtytype;

                List<CarMakeEntityBase> carMakes = _carMakesCacheRepo.GetCarMakesByType(type, module, isPopular, filter);

                if (carMakes.Count == 0)
                    return NotFound();
                if (module != null)
                {
                    if (isPopular == true)
                        return Ok(new { popularMakes = carMakes });
                    else if (isPopular == false)
                        return Ok(new { otherMakes = carMakes });

                    return Ok(new { popularMakes = carMakes.Where(x => x.IsPopular == true), otherMakes = carMakes.Where(x => x.IsPopular != true) });
                }

                return Ok(carMakes);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }

        }


        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8081", headers: "*", methods: "GET"),Route("api/v1/makes/")]
        public IHttpActionResult GetCarMakesV1(string type = "all", int? year = null, Modules? module = null, bool? isPopular = null,int filter = 0)
        {
            try
            {
                ThreeSixtyViewCategory threesixtytype;
                if (Enum.TryParse<ThreeSixtyViewCategory>(HttpContext.Current.Request.QueryString["threeSixtyType"],true, out threesixtytype))
                    filter = (int)threesixtytype;

                List<CarMakeEntityBase> carMakes = _carMakesCacheRepo.GetCarMakesByType(type, module, isPopular,filter);

                if (carMakes.Count == 0)
                    return NotFound();
                if (module != null)
                {
                    if (isPopular == true)
                        return Ok(new { popularMakes = carMakes });
                    else if (isPopular == false)
                        return Ok(new { otherMakes = carMakes });

                    return Ok(new { popularMakes = carMakes.Where(x => x.IsPopular == true), otherMakes = carMakes.Where(x => x.IsPopular != true) });
                }

                return Ok(Mapper.Map<List<CarMakeEntityBase>, List<CarMakesDTO>>(carMakes));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }

        }
        /// <summary>
        ///Gets Car Make Description based on the makeId passed 
        /// Written By : Shalini on 15/07/14
        /// modified by: Rakesh Yadav on 27 Aug 2015, enhance the way we use Dependency Injection using UnityBootstrapper and UnityResolver
        /// </summary>
        /// <returns></returns>

        public IHttpActionResult GetCarMakeDescription(int makeId = -1)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            return Ok(_carMakesCacheRepo.GetCarMakeDescription(makeId));
        }

        /// <summary>
        /// Author:Ajay Singh on 03-07-2017
        /// Description:Basic Data required for CW-CT Make table mapping.Takes MakeId as input and returns the required data.
        /// </summary>        
        [Route("api/make/{id:int:min(1)}/")]
        public IHttpActionResult GetCarMakeDetails(int id)
        {
            CarMakesDTOV1 obj = new CarMakesDTOV1();
            try
            {
                obj = AutoMapper.Mapper.Map<CarMakeEntityBase, CarMakesDTOV1>(_carMakesCacheRepo.GetCarMakeDetails(id));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Ok(obj);
        }


        //Gets CarMakes based on IDs passed, if no ids passed then it will return all makes 
        [Route("api/v2/makes")]
        [HttpGet, EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult GetMakes(string ids = null, string fields = null, Modules module = Modules.Default, bool? isPopular = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    int temp = 0;
                    ids = string.Join(",", ids.Split(',').Where(s => int.TryParse(s, out temp)).Distinct().Select(x => temp).OrderBy(x => x)); //convert ids to comma separated sorted ids
                }
                var makeDetails = _carMakesCacheRepo.GetMakes(ids, module);
                if (makeDetails != null && makeDetails.Any())
                {
                    _serializerSettings.ContractResolver = new CustomPropertiesContractResolver(fields);
                    _serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    return Json(makeDetails, _serializerSettings);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}

