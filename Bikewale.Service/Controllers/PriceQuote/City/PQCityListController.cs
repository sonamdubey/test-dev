using Bikewale.DAL.Location;
using Bikewale.DTO.PriceQuote.City;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.City;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.City
{
    /// <summary>
    /// Price Quote City Controller
    /// Author : Sumit Kate
    /// Created On : 20 Aug 2015
    /// </summary>
    public class PQCityListController : ApiController
    {
        /// <summary>
        /// Returns the Price Quote City List by Model Id
        /// </summary>
        /// <param name="modelId">Model Id. Should be Positive Integer</param>
        /// <returns>List of Cities</returns>
        [ResponseType(typeof(PQCityList))]
        public HttpResponseMessage Get(uint modelId)
        {
            List<CityEntityBase> objCityList = null;
            PQCityList objDTOCityList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    ICity cityRepository = container.Resolve<ICity>();

                    objCityList = cityRepository.GetPriceQuoteCities(modelId);

                    if (objCityList != null && objCityList.Count > 0)
                    {
                        // Auto map the properties
                        objDTOCityList = new PQCityList();
                        objDTOCityList.Cities = PQCityEntityToDTO.ConvertCityList(objCityList);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOCityList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.City.PQCityListController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
