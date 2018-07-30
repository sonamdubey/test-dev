using Bikewale.DTO.BikeBooking.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
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
            List<BikeMakeEntityBase> lstMake = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    lstMake = objPriceQuote.GetBikeMakesInCity(cityId);
                }

                BBMakeList objDTOMakeList = null;
                if (lstMake != null && lstMake.Count > 0)
                {
                    objDTOMakeList = new BBMakeList();
                    objDTOMakeList.Makes = BBMakeListMapper.Convert(lstMake);
                    return Ok(objDTOMakeList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.Make.BBMakeListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
