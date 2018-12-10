using Carwale.DAL.Security;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
    public class UsedMakesAndCitiesController : ApiController
    {
        private static readonly bool _skipValidation = Convert.ToBoolean(ConfigurationManager.AppSettings["SkipValidation"]);
        private readonly ISecurityRepository<bool> _securityRepo;
        private readonly ICommonOperationsCacheRepository _commonOperationCache;

        public UsedMakesAndCitiesController(ICommonOperationsCacheRepository commonOperationCache, ISecurityRepository<bool> securityRepo)
        {
            _securityRepo = securityRepo;
            _commonOperationCache = commonOperationCache;
        }

        [Route("api/UsedMakesAndCities/")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            if (_skipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && _securityRepo.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                UsedMakesAndCities makesAndCities = FetchMakesAndCities();
                if(makesAndCities != null)
                {
                    return Request.CreateResponse<UsedMakesAndCities>(HttpStatusCode.OK, makesAndCities);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }

        private UsedMakesAndCities FetchMakesAndCities()
        {
            UsedMakesAndCities makesAndCities = null;
            List<CarMakeEntityBase> makes = _commonOperationCache.GetLiveListingMakes().ToList();
            List<DealerCityEntity> cities = _commonOperationCache.GetLiveListingCities().ToList();

            if (makes != null && cities != null)
            {
                makesAndCities = new UsedMakesAndCities();
                makesAndCities.makes = new List<Data>();
                makesAndCities.cities = new List<Data>();

                makesAndCities.cities.Add(new Data { Label = "Mumbai", Value = "1" });
                makesAndCities.cities.Add(new Data { Label = "New Delhi", Value = "10" });
                makesAndCities.cities.Add(new Data { Label = "Bangalore", Value = "2" });
                makesAndCities.cities.Add(new Data { Label = "Chennai", Value = "176" });
                makesAndCities.cities.Add(new Data { Label = "Kolkata", Value = "198" });
                makesAndCities.cities.Add(new Data { Label = "Ahmedabad", Value = "128" });
                makesAndCities.cities.Add(new Data { Label = "Pune", Value = "12" });

                foreach (var make in makes)
                {
                    makesAndCities.makes.Add(new Data { Label = make.MakeName, Value = make.MakeId.ToString() });
                }
                foreach (var city in cities)
                {
                    makesAndCities.cities.Add(new Data { Label = city.CityName, Value = city.CityId.ToString() });
                }
            }
            return makesAndCities;
        }
    }
}
