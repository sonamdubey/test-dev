using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;

namespace Bikewale.Service.Controllers.UsedBikes
{
    /// <summary>
    /// Created By  : Aditi Srivastava on 27 Oct 2016
    /// Description : API for adding and deleting photos
    /// </summary>
    public class UsedBikePhotosController : CompressionApiController
    {
        private readonly ISellBikes _usedBikesRepo = null;

        public UsedBikePhotosController(ISellBikes UsedBikesRepo)
        {
            _usedBikesRepo = UsedBikesRepo;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 27 Oct 2016
        /// Description : Function to remove bike photos
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/used/{profileId}/image/{photoId}/delete/")]
        public IHttpActionResult RemoveBikePhotos(string profileId, string photoId)
        {
            ulong customerId;
            bool isSuccess = false;
            string platformId = "";
            try
            {
                if (Request.Headers.Contains("platformId") && Request.Headers.Contains("customerId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                    customerId = Convert.ToUInt64(Request.Headers.GetValues("customerId").First());
                    if (!String.IsNullOrEmpty(platformId) && Utility.CommonValidators.IsValidNumber(platformId) && Utility.UsedBikeProfileId.IsValidProfileId(profileId))
                    {
                        isSuccess = _usedBikesRepo.RemoveBikePhotos(customerId, profileId, photoId);

                    }
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("RemoveBikePhotos({0},{1})", profileId, photoId));
               
                return InternalServerError();
            }

        }

        /// <summary>
        /// Created By: Sumit Kate on 03 Nov 2016
        /// Description : Marks Main image for given profile id
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/used/{profileId}/image/{photoId}/markmainimage/")]
        public IHttpActionResult MarkMainBikePhoto(string profileId, uint photoId)
        {
            ulong customerId;
            bool isSuccess = false;
            string platformId = "";
            try
            {
                if (Request.Headers.Contains("platformId") && Request.Headers.Contains("customerId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                    customerId = Convert.ToUInt64(Request.Headers.GetValues("customerId").First());
                    if (!String.IsNullOrEmpty(platformId) && Utility.CommonValidators.IsValidNumber(platformId) && Utility.UsedBikeProfileId.IsValidProfileId(profileId))
                    {
                        isSuccess = _usedBikesRepo.MakeMainImage(photoId, customerId, profileId);

                    }
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("RemoveBikePhotos({0},{1})", profileId, photoId));
               
                return InternalServerError();
            }

        }
    }
}