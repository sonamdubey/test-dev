using Bikewale.DTO.City;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.City;
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.City.DealerCityController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 22 Mar 2016
        /// Make wise Dealer City List
        /// It Includes BW Dealer Cities and AB Dealer Cities
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(CityList)), Route("api/v2/DealerCity/")]
        public IHttpActionResult GetV2(uint makeId)
        {
            IEnumerable<CityEntityBase> cities = null;
            try
            {
                if (makeId > 0)
                {
                    cities = _objDealer.FetchDealerCitiesByMake(makeId);
                    if (cities != null && cities.Count() > 0)
                    {
                        CityList dtoCities = new CityList();
                        dtoCities.City = CityListMapper.Convert(cities);
                        return Ok(dtoCities);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.City.DealerCityController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
