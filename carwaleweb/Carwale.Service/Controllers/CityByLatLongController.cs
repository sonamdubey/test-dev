using Carwale.BL.GeoLocation;
using AEPLCore.Cache;
using Carwale.Cache.Geolocation;
using Carwale.DAL.Geolocation;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using Carwale.Interfaces;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers
{
    public class CityByLatLongController : ApiController
    {
        /// <summary>
        /// citybylatlong api 
        /// written by Natesh kumar on 5/11/14
        /// Modified By : Shalini Nair on 14/05/2015
        /// </summary>
        [HttpGet]
        [LatLongApiValidator]
        public HttpResponseMessage CityDetailsByLatLong([FromUri]LatLongURI queryString)
        {
            var response = new HttpResponseMessage();

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPQGeoLocationBL,PQGeoLocationBL>();

                    IPQGeoLocationBL repository = container.Resolve<IPQGeoLocationBL>();

                    City cityDetails = repository.GetCityDetailsByLatLong(queryString);

                    //if no results are found 
                    if (cityDetails.CityId == 0 || string.IsNullOrEmpty(cityDetails.CityName))
                    {
                       return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No results found");
                    }

                    response.Content = new StringContent(JsonConvert.SerializeObject(cityDetails));                   
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CityByLatLongController.CityDetailsByLatLong()");
                objErr.LogException();
                
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }

            return response;
        }
    }
}
