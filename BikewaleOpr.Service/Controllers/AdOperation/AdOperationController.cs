using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    public class AdOperationController : ApiController
    {
        private readonly IAdOperation _adOperations;
        public AdOperationController(IAdOperation adOperations)
        {
            _adOperations = adOperations;
        }

        [HttpPost, Route("api/adoperation/save/")]
        public IHttpActionResult Save(PromotedBike objPromotedBike)
        {

            try
            {
                bool status = _adOperations.SavePromotedBike(objPromotedBike);
                return Ok(status);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "AdOperationController.Save");
                return InternalServerError();
            }


        }

        /// <summary>
        /// Created by : Snehal Dange on 4th Jan 2017
        /// Desc : Service controller method to change status of latest launched bike to 'inactive' 
        /// </summary>
        /// <param name="objPromotedBike"></param>
        /// <returns></returns>
        [HttpPost, Route("api/adoperation/update/")]
        public IHttpActionResult Update(PromotedBike objPromotedBike)
        {
            try
            {
                bool status = _adOperations.UpdatePromotedBike(objPromotedBike);
                return Ok(status);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "AdOperationController.Update");
                return InternalServerError();
            }
        }

    }
}
