using AutoMapper;
using Bikewale.DAL.Location;
using Bikewale.DTO.City;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.City;
using Bikewale.Utility;
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


        private readonly ICityCacheRepository _cityCache = null;
        public CityListController(ICityCacheRepository cityCache)
        {
            _cityCache = cityCache;
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
            try
            {
                CityList objDTOCityList = GetAllCities(requestType);
                if (objDTOCityList != null)
                {
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

        /// <summary>
        /// Created by  : Pratibha Verma on 11 May 2018
        /// Description : Returns top 6 cities by popularity and others by alphabatical order  
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns>City List</returns>
        [ResponseType(typeof(CityList)), Route("api/pwa/cities/")]
        public IHttpActionResult GetCities(EnumBikeType requestType)
        {
            try
            {
                CityList objDTOCityList = GetAllCities(requestType);
                if (objDTOCityList != null)
                {
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

        /// <summary>
        /// Created By  : Pratibha Verma on 17 May 2018
        /// Description : return all cities where model price is available
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CityFinance>)), Route("api/pwa/cities/model/{modelId:int}/")]
        public IHttpActionResult GetModelCities(uint modelId)
        {
            try
            {
                IEnumerable<CityFinance> objDTOCityList = null;
                if (modelId > 0)
                {
                    IEnumerable<CityEntityBase> objCityList = _cityCache.GetModelPriceCities(modelId, BWConstants.FinancePopularCityCount);
                    if (objCityList != null)
                    {
                        objDTOCityList = CityListMapper.MapCity(objCityList);
                        return Ok(objDTOCityList);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.City.CityListController.GetModelCities");
                return InternalServerError();
            }
        }

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

        private CityList GetAllCities(EnumBikeType requestType)
        {
            CityList objDTOCityList = null;
            try
            {
                IEnumerable<CityEntityBase> objCityList = null;
                objCityList = _cityCache.GetAllCities(requestType);

                if (objCityList != null)
                {
                    objDTOCityList = new CityList();
                    objDTOCityList.City = CityListMapper.Convert(objCityList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.City.CityListController.GetAllCities()");
            }

            return objDTOCityList;
        }
        #region Cities based on state Name
        /// <summary>
        /// Author  :   Kartik Rathod on 8 jun 2018
        /// Desc    :   Get cities based on state name 
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns>CityList</returns>
        [ResponseType(typeof(CityList)), Route("api/cities/")]
        public IHttpActionResult GetCitiesByStateName(string stateName)
        {
            ICollection<CityEntityBase> objCityEntityList = null;
            ICityRepository _citysRepository;
            try
            {
                if (!string.IsNullOrEmpty(stateName))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICityRepository, CityRepository>();
                        _citysRepository = container.Resolve<ICityRepository>();

                        objCityEntityList = _citysRepository.GetCitiesByStateName(stateName);

                        if (objCityEntityList != null && objCityEntityList.Count > 0)
                        {
                            CityList objCityList = new CityList();
                        
                            objCityList.City = Mapper.Map<ICollection<CityEntityBase>, List<CityBase>>(objCityEntityList);

                            objCityEntityList.Clear();
                            objCityEntityList = null;

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
