using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Linq;

namespace Bikewale.Models.Photos
{
    public class SimilarBikesWithPhotosWidget
    {
        public ushort TotalRecords { get; set; }
        public string BikeName { get; set; }
        private readonly ushort _totalRecords = 9;
        private readonly uint _modelId, _cityId;

        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache;

        public SimilarBikesWithPhotosWidget(IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache, uint modelId)
        {
            _objModelCache = objModelCache;
            _modelId = modelId;
        }
        public SimilarBikesWithPhotosWidget(IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache, uint modelId, uint cityId)
        {
            _objModelCache = objModelCache;
            _modelId = modelId;
            _cityId = cityId;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2016
        /// Summary :  To bind similar bikes with photos count from cache
        /// </summary>
        public SimilarBikesWithPhotosVM GetData()
        {

            SimilarBikesWithPhotosVM objData = new SimilarBikesWithPhotosVM();
            if (TotalRecords <= 0) TotalRecords = _totalRecords;

            try
            {
                objData.Bikes = _objModelCache.GetSimilarBikeWithPhotos((int)_modelId, TotalRecords, _cityId);
                if (objData.Bikes != null && objData.Bikes.Any())
                {
                    objData.FetchedRecordsCount = objData.Bikes.Count();
                    var firstModel = objData.Bikes.First();
                    var bodyStyle = (firstModel.BodyStyle.Equals((sbyte)EnumBikeBodyStyles.Scooter) ? "Scooters" : "Bikes");
                    objData.WidgetHeading = string.Format("{0} similar to {1}", bodyStyle, BikeName);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Photos.GetData : ModelId - {0},CityId - {1} ", _modelId, _cityId));
            }
            return objData;
        }


    }
}