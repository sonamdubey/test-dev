using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.PWA.Entities.Photos;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Photos
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 12 Feb 2018.
    /// Description : Provides APIs related to photos for PWA app.
    /// </summary>
    public class PwaPhotosController : ApiController
    {
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity;

        public PwaPhotosController(IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            _objModelEntity = objModelEntity;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12 Feb 2018.
        /// Description : API to get images of bike models. Bike models are sorted by most popularity.
        /// </summary>
        /// <param name="modelCount">Count of bike models for which image list is to be fetched.</param>
        /// <param name="imageCount">Count of images to be fetched per bike model.</param>
        /// <returns>List of bike models with their images.</returns>
        [ResponseType(typeof(IEnumerable<PwaModelImages>)), Route("api/pwa/images/models/")]
        public IHttpActionResult Get(uint modelCount, int imageCount)
        {
            try
            {
                if (modelCount > 0 && imageCount > 0)
                {
                    return Ok(ConverterUtility.PwaConvert(FetchModelImagesList(modelCount, imageCount)));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controllers.Photos.PwaPhotosController.Get_modelCount_{0}", modelCount));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12 Feb 2018.
        /// Description : Method to getch bike models with their images list.
        /// </summary>
        /// <param name="modelCount">Count of bike models for which image list is to be fetched.</param>
        /// <param name="imageCount">Count of images to be fetched per bike model.</param>
        /// <returns>List of bike models with their images.</returns>
        private IEnumerable<ModelImages> FetchModelImagesList(uint modelCount, int imageCount)
        {
            try
            {
                IEnumerable<ModelIdWithBodyStyle> objModelIds = _objModelEntity.GetModelIdsForImages(0, EnumBikeBodyStyles.AllBikes, 1, modelCount);
                string modelIds = string.Join(",", objModelIds.Select(m => m.ModelId));
                IList<EnumCMSContentType>  contentType = new List<EnumCMSContentType>()
                {
                    EnumCMSContentType.PhotoGalleries,
                    EnumCMSContentType.RoadTest
                };
                string categoryIds = CommonApiOpn.GetContentTypesString(contentType);
                return _objModelEntity.GetBikeModelsPhotos(modelIds, categoryIds, imageCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controllers.Photos.PwaPhotosController.FetchModelImagesList_modelCount_{0}", modelCount));
                return Enumerable.Empty<ModelImages>();
            }
        }
    }
}
