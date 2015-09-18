using Bikewale.DTO.BikeBooking.Make;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking.Make
{
    /// <summary>
    /// BikeBooking Make list controller
    /// Author  : Sumit Kate
    /// Created On  :   20 Aug 2015
    /// </summary>
    public class BBMakeListController : ApiController
    {
        /// <summary>
        /// List of Bikebooking Makes
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns></returns>
        [ResponseType(typeof(BBMakeList))]
        public IHttpActionResult Get(uint cityId)
        {
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json";
            string _apiUrl = String.Format("/api/DealerPriceQuote/GetBikeMakesInCity/?cityId={0}", cityId);
            List<BBMakeBase> lstMake = null;
            try
            {
                lstMake = BWHttpClient.GetApiResponseSync<List<BBMakeBase>>(_abHostUrl, _requestType, _apiUrl, lstMake);
                BBMakeList objDTOMakeList = null;
                if (lstMake != null && lstMake.Count > 0)
                {
                    objDTOMakeList = new BBMakeList();
                    objDTOMakeList.Makes = lstMake;
                    return Ok(objDTOMakeList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.Make.BBMakeListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
