using Bikewale.Notifications;
using BikewaleOpr.DTO.Images;
using BikewaleOpr.Entities.Images;
using BikewaleOpr.Interface.Images;
using BikewaleOpr.Service.AutoMappers.Images;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 Nov 2016
    /// Description :   Image Upload Controller
    /// </summary>
    public class ImageController : ApiController
    {
        private readonly IImage _objImageBL = null;
        public ImageController(IImage objImageBL)
        {
            _objImageBL = objImageBL;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Nov 2016
        /// Description :   Process Image Upload Request and on success it returns image upload AWS token
        /// </summary>
        /// <param name="objImage"></param>
        /// <returns></returns>
        [HttpPost, Route("api/image/request/"), ResponseType(typeof(ImageTokenDTO))]
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("ProcessRequest{0}", Newtonsoft.Json.JsonConvert.SerializeObject(objImage)));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Nov 2016
        /// Description :   Complete image upload process
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("api/image/savepath/"), HttpPost, ResponseType(typeof(bool))]
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
