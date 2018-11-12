using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.Gallery
{
    public class ModelGalleryWidget
    {

        private readonly uint _modelId, _cityId;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private ModelGalleryVM _modelGallery = null;
        private readonly IBikeSeries _bikeSeries = null;

        public bool IsGalleryDataAvailable { get; set; }
        public bool IsJSONRequired { get; set; }
        public bool IsBikeInfoRequired { get; set; }

        public ModelGalleryWidget(uint modelId, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            _objModelEntity = objModelEntity;
            _modelId = modelId;
        }

        public ModelGalleryWidget(uint modelId, IBikeModels<BikeModelEntity, int> objModelEntity, IBikeInfo objGenericBike, ICityCacheRepository objCityCache, uint cityId, IBikeSeries bikeSeries)
        {
            _objModelEntity = objModelEntity;
            _modelId = modelId;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _cityId = cityId;
            _bikeSeries = bikeSeries;
        }

        public ModelGalleryWidget(BikeMakeEntityBase make, BikeModelEntityBase model, IEnumerable<ColorImageBaseEntity> imagesList, IEnumerable<BikeVideoEntity> videosList, BikeInfoVM bikeInfo)
        {
            IsGalleryDataAvailable = true;
            _modelGallery = new ModelGalleryVM()
            {
                BikeInfo = bikeInfo,
                Make = make,
                Model = model,
                VideosList = videosList,
                ImageList = imagesList
            };
        }

        public ModelGalleryVM GetData()
        {
            if (!IsGalleryDataAvailable && _objModelEntity != null)
            {
                ModelPhotoGalleryEntity photoGallery = _objModelEntity.GetPhotoGalleryData(Convert.ToInt32(_modelId));
                SetGalleryProperties(photoGallery);

            }

            if (_modelGallery != null && (string.IsNullOrEmpty(_modelGallery.ImagesJSON) || string.IsNullOrEmpty(_modelGallery.VideosJSON)))
            {
                SetJSONproperties();
            }

            return _modelGallery;

        }


        private void SetGalleryProperties(ModelPhotoGalleryEntity photoGallery)
        {

            if (photoGallery != null)
            {
                _modelGallery = new ModelGalleryVM()
                {
                    VideosList = photoGallery.VideosList,
                    ImageList = photoGallery.ImageList
                };

                if (photoGallery.ObjModelEntity != null)
                {
                    _modelGallery.Make = photoGallery.ObjModelEntity.MakeBase;
                    _modelGallery.Model = new BikeModelEntityBase()
                    {
                        MaskingName = photoGallery.ObjModelEntity.MaskingName,
                        ModelId = photoGallery.ObjModelEntity.ModelId,
                        ModelName = photoGallery.ObjModelEntity.ModelName
                    };
                }

            }

            if (_modelGallery != null && IsBikeInfoRequired && _objGenericBike != null && _objCityCache != null)
            {
                _modelGallery.BikeInfo = (new BikeInfoWidget(_objGenericBike, _objCityCache, _modelId, _cityId, 4, Entities.GenericBikes.BikeInfoTabType.Image, _objModelEntity, _bikeSeries)).GetData();
            }


        }

        private void SetJSONproperties()
        {
            if (IsJSONRequired && _modelGallery != null)
            {
                if (_modelGallery.ImageList != null)
                {
                    _modelGallery.ImagesJSON = EncodingDecodingHelper.EncodeTo64(JsonConvert.SerializeObject(_modelGallery.ImageList));
                }

                if (_modelGallery.VideosList != null)
                {
                    _modelGallery.VideosJSON = EncodingDecodingHelper.EncodeTo64(JsonConvert.SerializeObject(_modelGallery.VideosList));
                }

            }
        }
    }
}