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

        [HttpPost, Route("api/adoperations/id/save/")]
        public IHttpActionResult Save([FromBody]PromotedBike objPromotedBike)
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
    }
}
