using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Utility;
using Bikewale.Utility.GenericBikes;
using System;
using System.Globalization;
using System.Linq;

namespace Bikewale.Models.BikeModels
{
    public class OtherBestBikesModel
    {
        private uint _makeId;
        private uint _modelId;
        private uint? _cityId;
        private string _makeName;
        public EnumBikeBodyStyles _bodyStyleType { get; set; }
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;

        public OtherBestBikesModel(uint makeId, string makeName, uint modelId, EnumBikeBodyStyles bodyStyleType, IBikeModelsCacheRepository<int> objBestBikes, uint? cityId = null)
        {
            _makeId = makeId;
            _makeName = makeName;
            _modelId = modelId;
            _bodyStyleType = bodyStyleType;
            _objBestBikes = objBestBikes;
            _cityId = cityId;
        }

        public OtherBestBikesVM GetData()
        {
            OtherBestBikesVM otherBestBikes = null;

            try
            {
                otherBestBikes = new OtherBestBikesVM();
                otherBestBikes.IsMakePresentInConfig = BWConfiguration.Instance.BestBikesMakes.Split(',').Select(makeId => Convert.ToUInt32(makeId)).Contains((uint)_makeId);
                if (!otherBestBikes.IsMakePresentInConfig)
                {
                    otherBestBikes.BestBikes = _objBestBikes.GetBestBikesByCategory(_bodyStyleType, _cityId).Reverse().Take(3);
                    var pageMaskingName = GenericBikesCategoriesMapping.BodyStyleByType(_bodyStyleType);
                    otherBestBikes.OtherBestBikesHeading = new CultureInfo("en-US", false).TextInfo.ToTitleCase(pageMaskingName).Replace("-", " ");
                    otherBestBikes.OtherBestBikesHeading = string.Format("Here's a list of best {0}", otherBestBikes.OtherBestBikesHeading);
                }
                else
                {
                    otherBestBikes.BestBikes = _objBestBikes.GetBestBikesByModelInMake(_modelId, _cityId).Take(3);
                    otherBestBikes.OtherBestBikesHeading = string.Format("More bikes from {0}", _makeName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels"));
            }

            return otherBestBikes;
        }
    }
}