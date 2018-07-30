using Bikewale.DTO.BikeBooking.City;
using Bikewale.Entities.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Microsoft.Practices.Unity;
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
            List<CityEntityBase> lstCity = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    lstCity = objPriceQuote.GetBikeBookingCities(null);
                }

                BBCityList objDTOCityList = null;
                if (lstCity != null && lstCity.Count > 0)
                {
                    objDTOCityList = new BBCityList();
                    objDTOCityList.Cities = BBCityListMapper.Convert(lstCity);
                    return Ok(objDTOCityList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.City.BBCityListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
