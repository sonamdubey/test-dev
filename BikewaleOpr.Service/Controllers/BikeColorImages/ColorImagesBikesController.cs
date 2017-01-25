
using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeColorImages;
using BikewaleOpr.Interface.BikeColorImages;
using System;
using System.Web.Http;
namespace BikewaleOpr.Service.Controllers.BikeColorImages
{
    /// <summary>
    /// Created By :- Subodh Jain 09 jan 2017
    /// Summary :- Bikes Images Details 
    /// </summary>
    public class ColorImagesBikesController : ApiController
    {
        private readonly IColorImagesBikeRepository _objColorImagesBikes = null;
        public ColorImagesBikesController(IColorImagesBikeRepository objColorImagesBikes)
        {
            _objColorImagesBikes = objColorImagesBikes;
        }
        /// <summary>
        /// Created By :- Subodh Jain 9 Jan 2017
        /// Summary :- Fetch Photo Id 
        /// </summary>
        /// <param name="objBikeColorDetails"></param>
        /// <returns></returns>
        [HttpPost, Route("api/model/images/color/")]
        public IHttpActionResult FetchPhotoId([FromBody]ColorImagesBikeEntities objBikeColorDetails)
        {

            try
            {
                if (objBikeColorDetails != null && ModelState.IsValid)
                {


                    return Ok(_objColorImagesBikes.FetchPhotoId(objBikeColorDetails));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ColorImagesBikesController.SaveBikeColorDetails");
                return InternalServerError();
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 jan 2017
        /// Summary :- To delete Bike Color details 
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/image/delete/modelid/")]
        public IHttpActionResult DeleteBikeColorDetails(uint photoId)
        {

            try
            {
                if (photoId > 0)
                {
                    return Ok(_objColorImagesBikes.DeleteBikeColorDetails(photoId));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ColorImagesBikesController.DeleteBikeColorDetails");
                return InternalServerError();
            }
        }
    }
}