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
		[ResponseType(typeof(IEnumerable<CityFinance>)), Route("api/pwa/cities/model/{modelId}")]
		public IHttpActionResult GetModelCities(uint modelId)
		{	
			try
			{
				uint popularCityCount = 6;
				IEnumerable<CityFinance> objDTOCityList = null;
				IEnumerable<CityEntityBase> objCityList = _cityCache.GetModelPriceCities(modelId, popularCityCount);
				if (objCityList != null)
				{
					objDTOCityList = new List<CityFinance>();
					objDTOCityList = CityListMapper.MapCity(objCityList);
					return Ok(objDTOCityList);
				}
				else
				{
					return NotFound();
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
	}

}
