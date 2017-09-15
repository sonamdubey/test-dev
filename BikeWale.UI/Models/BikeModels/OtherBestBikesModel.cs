﻿using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Utility;
using Bikewale.Utility.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 14 Sep 2017
    /// Summary     :   Model for more bikes from make widget
    /// </summary>
    public class OtherBestBikesModel
    {
        private readonly uint _makeId;
        private readonly uint _modelId;
        private readonly uint? _cityId;
        private readonly string _makeName;
        private readonly bool _isMakePresentinConfig;
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
            _isMakePresentinConfig = IsMakePresentInConfig(makeId);
        }

        public bool IsMakePresentInConfig(uint makeId)
        {
            return BWConfiguration.Instance.BestBikesMakes.Split(',').Contains(_makeId.ToString());
        }

        public OtherBestBikesVM GetData()
        {
            OtherBestBikesVM otherBestBikes = null;
            IEnumerable<BestBikeEntityBase> bestBikes = null;

            try
            {
                otherBestBikes = new OtherBestBikesVM();
                otherBestBikes.IsMakePresentInConfig = _isMakePresentinConfig;

                if (!_isMakePresentinConfig)
                {
                    bestBikes = _objBestBikes.GetBestBikesByCategory(_bodyStyleType, _cityId);

                    if (bestBikes != null && bestBikes.Count() > 0)
                        otherBestBikes.BestBikes = bestBikes.Reverse().Take(3);

                    var pageMaskingName = GenericBikesCategoriesMapping.BodyStyleByType(_bodyStyleType);
                    otherBestBikes.OtherBestBikesHeading = Bikewale.Utility.StringExtention.StringHelper.ToTitleCase(pageMaskingName, '-', ' ');
                    otherBestBikes.OtherBestBikesHeading = string.Format("Here's a list of best {0}", otherBestBikes.OtherBestBikesHeading);
                }
                else
                {
                    otherBestBikes.BestBikes = _objBestBikes.GetBestBikesByModelInMake(_modelId, _cityId);
                    otherBestBikes.OtherBestBikesHeading = string.Format("More bikes from {0}", _makeName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.GetData()"));
            }

            return otherBestBikes;
        }
    }
}