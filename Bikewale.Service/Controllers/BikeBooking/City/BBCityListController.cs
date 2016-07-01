using Bikewale.DTO.BikeBooking.City;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
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
            string _apiUrl = "/api/DealerPriceQuote/getBikeBookingCities/";
            List<BBCityBase> lstCity = null;
            try
            {
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    lstCity = objClient.GetApiResponseSync<List<BBCityBase>>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, lstCity);
                }

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
