using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Gallery
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 4th Oct 2017
    /// Summary : API for gallery related data
    /// </summary>
    public class GalleryController : ApiController
    {
        private readonly IBikeModels<BikeModelEntity, int> _bikeModel = null;

        public GalleryController(IBikeModels<BikeModelEntity, int> bikeModel)
        {
            _bikeModel = bikeModel;
        }

        [ResponseType(typeof(ModelGallery)), Route("api/model/{modelId}/gallary/")]
        public IHttpActionResult GalleryComponents(int modelId)
        {
            ModelGallery objGallery = null;
            try
            {
                if (modelId <= 0)
                {
                    return BadRequest();
                }
                if (Request.Headers.Contains("platformId"))
                {
                    string platformId = Request.Headers.GetValues("platformId").First().ToString();
                    if (platformId == "3")
                    {
                        BikeModelPageEntity objModelPage = null;
                        objModelPage = _bikeModel.GetModelPageDetails(modelId);
                        objGallery = ModelMapper.ConvertToModelGallery(objModelPage, modelId);
                        return Ok(objGallery);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Service.Controllers.GalleryController: GalleryComponents, modelid = {0}", modelId));
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
