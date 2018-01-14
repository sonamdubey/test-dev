using Bikewale.DTO.CMS.Photos;
using Bikewale.DTO.Images;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Images;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Images;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Images;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.Service.AutoMappers.CMS;

namespace Bikewale.Service.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 Nov 2016
    /// Description :   Image Upload Controller
    /// </summary>
    public class ImageController : CompressionApiController
    {
        private readonly IImage _objImageBL = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity;
        public ImageController(IImage objImageBL, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            _objImageBL = objImageBL;
            _objModelEntity = objModelEntity;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Nov 2016
        /// Description :   Process Image Upload Request and on success it returns image upload AWS token
        /// </summary>
        /// <param name="objImage"></param>
        /// <returns></returns>
        [Route("api/image/request/"), HttpPost, ResponseType(typeof(DTO.Images.ImageTokenDTO))]
        public IHttpActionResult ProcessRequest([FromBody]ImageDTO objImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Image objImageEntity = ImageMapper.Convert(objImage);
                    ImageToken token = _objImageBL.GenerateImageUploadToken(objImageEntity);
                    ImageTokenDTO dto = ImageMapper.Convert(token);
                    if (dto != null && dto.Status)
                    {
                        return Ok(dto);
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("ProcessRequest{0}", Newtonsoft.Json.JsonConvert.SerializeObject(objImage)));
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Nov 2016
        /// Description :   Complete image upload process
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("api/image/"), HttpPost, ResponseType(typeof(bool))]
        public IHttpActionResult Post([FromBody]ImageTokenDTO dto)
        {
            if (ModelState.IsValid)
            {
                ImageToken entity = ImageMapper.Convert(dto);
                if (entity != null)
                {
                    entity = _objImageBL.ProcessImageUpload(entity);
                    if (entity.Status)
                    {
                        return Ok(true);
                    }
                    else if (entity.ServerError)
                    {
                        return InternalServerError();
                    }
                    else
                    {
                        return BadRequest("Server refused the request");
                    }
                }
                else
                {
                    return BadRequest("Server refused the request");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        

    }
}
