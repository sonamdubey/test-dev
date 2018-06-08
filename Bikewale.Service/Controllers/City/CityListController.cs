using AutoMapper;
using Bikewale.DAL.Location;
using Bikewale.DTO.City;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.City;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.City
{
    /// <summary>
    /// To Get List of Cities
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class CityListController : ApiController
    {


        private readonly ICity _city = null;
        public CityListController(ICity city)
        {
            _city = city;
        }

        #region All Cities
        /// <summary>
        /// To get Cities Based on request Type for DropDowns only
        /// </summary>
        /// <param name="requestType">Used/All (Irrespective of Pricequotes)</param>
        /// <returns>Cities List</returns>
        [ResponseType(typeof(CityList))]
        public IHttpActionResult Get(EnumBikeType requestType)
        {
            List<CityEntityBase> objCityList = null;
            CityList objDTOCityList = null;
            try
            {
                objCityList = _city.GetAllCities(requestType);

                if (objCityList != null && objCityList.Count > 0)
                {
                    objDTOCityList = new CityList();
                    objDTOCityList.City = CityListMapper.Convert(objCityList);

                    objCityList.Clear();
                    objCityList = null;

                    return Ok(objDTOCityList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.City.CityListController");
               
                return InternalServerError();
            }
        }   // Get Cities 
        #endregion

        #region State's Cities
        /// <summary>
        /// To get Cities Based on state and request Type
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns>State's Cities</returns>
        [ResponseType(typeof(CityList))]
        public HttpResponseMessage Get(string stateId, EnumBikeType requestType)
        {
            List<CityEntityBase> objCityList = null;
            CityList objDTOCityList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    ICity citysRepository = null;

                    container.RegisterType<ICity, CityRepository>();
                    citysRepository = container.Resolve<ICity>();

                    objCityList = citysRepository.GetCities(stateId, requestType);

                    if (objCityList != null && objCityList.Count > 0)
                    {
                        // Auto map the properties
                        objDTOCityList = new CityList();

                        Mapper.CreateMap<CityEntityBase, CityBase>();
                        objDTOCityList.City = Mapper.Map<List<CityEntityBase>, List<CityBase>>(objCityList);

                        objCityList.Clear();
                        objCityList = null;

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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.City.CityListController");
               
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get  
        #endregion

        #region Cities based on state Name
        /// <summary>
        /// Author  :   Kartik Rathod on 8 jun 2018
        /// Desc    :   Get cities based on state name 
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns>CityBase List</returns>
        [Route("api/citylist/getcities/")]
        public IHttpActionResult GetCitiesByStateName(string stateName)
        {
            IEnumerable<CityBase> objCityList = null;
            ICity _citysRepository;
            try
            {
                if (!string.IsNullOrEmpty(stateName))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICity, CityRepository>();
                        _citysRepository = container.Resolve<ICity>();

                        objCityList = _citysRepository.GetCitiesByStateName(stateName);

                        if (objCityList != null)
                        {
                            return Ok(objCityList);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.City.GetCitiesByStateName");
                return InternalServerError();
            }
        }   // Get  
        #endregion

    }

}
