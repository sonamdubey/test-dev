using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Bikewale.Service.Controllers.UsedBikes
{
    public class UsedBikePhotosController : CompressionApiController
    {
         private readonly ISellBikes _usedBikesRepo = null;

         public UsedBikePhotosController(ISellBikes UsedBikesRepo)
        {
            _usedBikesRepo = UsedBikesRepo;
        }

         [HttpPost, Route("api/used/{profileId}/image/{photoId}/delete/")]
         public IHttpActionResult RemoveBikePhotos(string profileId, string photoId)
         {
             ulong customerId;
             bool isSuccess = false;
             string platformId = "";
             try
             {
                 if (Request.Headers.Contains("platformId"))
                 {
                     platformId = Request.Headers.GetValues("platformId").First().ToString();
                     if (!String.IsNullOrEmpty(platformId) && Utility.CommonValidators.IsValidNumber(platformId))
                     {
                         customerId = Convert.ToUInt64(Request.Headers.GetValues("customerId").First());
                         if (customerId > 0)
                         {
                             isSuccess = _usedBikesRepo.RemoveBikePhotos(customerId, profileId, photoId);
                         }
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
                 ErrorClass objErr = new ErrorClass(ex, String.Format("RemoveBikePhotos({0},{1})", profileId, photoId));
                 objErr.SendMail();
                 return InternalServerError();
             }
             
         }
    }
}