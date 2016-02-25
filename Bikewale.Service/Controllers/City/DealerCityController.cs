using Bikewale.DTO.City;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.City
{
    /// <summary>
    /// Dealer City Controller
    /// Author  :   Sumit Kate
    /// Created :   04 Sept 2015
    /// </summary>
    public class DealerCityController : ApiController
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
    }
}
