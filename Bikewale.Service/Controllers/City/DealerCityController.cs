using Bikewale.DTO.City;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.City;
using Bikewale.Service.AutoMappers.PriceQuote.City;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.City
{
    /// <summary>
    /// Dealer City Controller
    /// Author  :   Sumit Kate
    /// Created :   04 Sept 2015
    /// </summary>
    public class DealerCityController : CompressionApiController//ApiController
    {
        private readonly IDealer _objDealer = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealer"></param>
        public DealerCityController(IDealer objDealer)
        {
            _objDealer = objDealer;
        }
        /// <summary>
        /// Makewise Dealer City List
        /// </summary>
        /// <param name="makeId">Make Id</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CityBase>))]
        public IHttpActionResult Get(uint makeId)
        {
            NewBikeDealersListEntity objDealerList = null;
            try
            {
                objDealerList = _objDealer.GetDealersCitiesListByMakeId(makeId);
                if (objDealerList != null && objDealerList.CityWiseDealers.Count > 0)
                {
                    var cityList = objDealerList.CityWiseDealers.Select(
                    city => new CityBase()
                    {
                        CityId = city.CityId,
                        CityName = city.CityName,
                        CityMaskingName = city.CityId + "_" + city.CityMaskingName.Trim()
                    }).OrderBy(x => x.CityName).ToList();

                    objDealerList.CityWiseDealers.Clear();
                    objDealerList.CityWiseDealers = null;

                    objDealerList.StatesList.Clear();
                    objDealerList.StatesList = null;

                    return Ok(cityList);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.City.DealerCityController.Get");
               
                return InternalServerError();
            }
            return NotFound();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 22 Mar 2016
        /// Make wise Dealer City List
        /// It Includes BW Dealer Cities and AB Dealer Cities
        /// Modified by :   Sumit Kate on 13 May 2016
        /// Description :   If android, IOS client send the response similar to PQCityList API
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(CityList)), Route("api/v2/DealerCity/")]
        public IHttpActionResult GetV2(uint makeId)
        { 
            IEnumerable<CityEntityBase> cities = null;
            Bikewale.DTO.PriceQuote.City.v2.PQCityList objDTOCityList = null;
            try
            {
                if (makeId > 0)
                {
                    cities = _objDealer.FetchDealerCitiesByMake(makeId);
                    if (cities != null && cities.Any())
                    {
                        // If android, IOS client send the response similar to PQCityList API
                        string platformId = string.Empty;

                        if (Request.Headers.Contains("platformId"))
                        {
                            platformId = Request.Headers.GetValues("platformId").First().ToString();
                        }

                        if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                        {
                            objDTOCityList = new Bikewale.DTO.PriceQuote.City.v2.PQCityList();
                            objDTOCityList.Cities = PQCityListMapper.ConvertV2(cities);

                            cities = null;

                            return Ok(objDTOCityList);
                        }
                        else
                        {
                            CityList dtoCities = new CityList();
                            dtoCities.City = CityListMapper.Convert(cities);
                            return Ok(dtoCities);
                        }
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
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.City.DealerCityController.Get");
               
                return InternalServerError();
            }
        }
    }
}
