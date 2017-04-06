
using Bikewale.Notifications;
using BikewaleOpr.BAL;
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

                    MemCachedUtil.Remove(string.Format("BW_ModelPhotosColorWise_{0}", objBikeColorDetails.Modelid));
                    MemCachedUtil.Remove(string.Format("BW_ModelColor_{0}", objBikeColorDetails.Modelid));
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
        public IHttpActionResult DeleteBikeColorDetails(uint photoId,uint modelid)
        {

            try
            {
                if (photoId > 0)
                {
                    MemCachedUtil.Remove(string.Format("BW_ModelPhotosColorWise_{0}", modelid));
                    MemCachedUtil.Remove(string.Format("BW_ModelColor_{0}", modelid));
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