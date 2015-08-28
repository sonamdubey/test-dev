using Bikewale.DTO.BikeBooking.City;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking.City
{
    /// <summary>
    /// BikeBooking City List controller
    /// Author  :   Sumit Kate
    /// Created On  : 20 Aug 2015
    /// </summary>
    public class BBCityListController : ApiController
    {
        /// <summary>
        /// Bikebooking City List
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(BBCityList))]
        public IHttpActionResult Get()
        {
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json";
            string _apiUrl = "/api/DealerPriceQuote/getBikeBookingCities/";
            List<BBCityBase> lstCity = null;
            try
            {
                lstCity = BWHttpClient.GetApiResponseSync<List<BBCityBase>>(_abHostUrl, _requestType, _apiUrl, lstCity);
                BBCityList objDTOCityList = null;
                if (lstCity != null && lstCity.Count > 0)
                {
                    objDTOCityList = new BBCityList();
                    objDTOCityList.Cities = lstCity;
                    return Ok(objDTOCityList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.City.BBCityListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
