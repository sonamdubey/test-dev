using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Utility;

namespace Bikewale.Models.Images
{
    public class ImageCarausel
    {
        public uint MakeId { get; set; }
        public uint RecordCount { get; set; }
        public uint ImageCount { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }

        public IList<EnumCMSContentType> ContentType { get; set; }
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        public ImageCarausel(uint makeId, uint recordCount, uint imageCount, EnumBikeBodyStyles bodyStyle, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            ContentType = new List<EnumCMSContentType>() { EnumCMSContentType.PhotoGalleries, EnumCMSContentType.RoadTest };
            MakeId = makeId;
            RecordCount = recordCount;
            ImageCount = imageCount;
            BodyStyle = bodyStyle;
            _objModelEntity = objModelEntity;
        }
        public ImageWidgetVM GetData()
        {           
            ImageWidgetVM _objData = null;
            try
            {
                _objData = new ImageWidgetVM();
                _objData.ModelList =  GetModelPhotos();

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Images.ImageCarausel : GetData");
            }

            return _objData;
        }
        private IEnumerable<ModelImages> GetModelPhotos()
        {
            IEnumerable<ModelImages> modelList = null;
            try
            {  
                IEnumerable<ModelIdWithBodyStyle> objModelIds = _objModelEntity.GetModelIdsForImages(MakeId, BodyStyle, ImageCount, RecordCount);
                string modelIds = string.Join(",", objModelIds.Select(m => m.ModelId));
                string categoryIds = CommonApiOpn.GetContentTypesString(ContentType);
                modelList = _objModelEntity.GetBikeModelsPhotos(modelIds, categoryIds, (int)ImageCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.Images.ImageCarausel : GetModelPhotos"));
            }
            return modelList;
        }

    }
}