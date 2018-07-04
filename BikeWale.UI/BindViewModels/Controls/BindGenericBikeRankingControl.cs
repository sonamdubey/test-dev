using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.CacheHelper.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Aditi Srivastava on 12 Jan 2017
    /// Description: To get bike ranking in a category by model id
    ///Modified By :- Subodh Jain 30 jan 2017
    ///Summary:- Shifted generic to bikemodel repository
    /// </summary>
    public class BindGenericBikeRankingControl
    {
        public uint ModelId { get; set; }
        public string StyleName { get; set; }
        public string BikeType { get; set; }
        public string RankText { get; set; }
        private readonly ICacheManager _cacheManager;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels;
        private readonly IPager _pager;
        private readonly IBikeModelsCacheHelper _bikeModelsCacheHelper;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCacheRepository;


        public BindGenericBikeRankingControl( ICacheManager cacheManager,
                                                IBikeModels<BikeModelEntity, int> bikeModels,
                                                IPager pager,
                                                IBikeModelsCacheHelper bikeModelsCacheHelper,
                                                IBikeModelsCacheRepository<int> bikeModelsCacheRepository)
        {
            _cacheManager = cacheManager;
            _bikeModels=bikeModels;
            _pager=pager;
            _bikeModelsCacheHelper=bikeModelsCacheHelper;
            _bikeModelsCacheRepository=bikeModelsCacheRepository;
        }

        public BikeRankingEntity GetBikeRankingByModel()
        {
            BikeRankingEntity bikeRankObj = null;
            try
            {

                bikeRankObj = _bikeModelsCacheRepository.GetBikeRankingByCategory(ModelId);
                if (bikeRankObj != null)
                {
                    switch (bikeRankObj.BodyStyle)
                    {

                        case EnumBikeBodyStyles.Mileage:
                            StyleName = "Mileage Bikes";
                            BikeType = "Mileage Bike";
                            break;
                        case EnumBikeBodyStyles.Sports:
                            StyleName = "Sports Bikes";
                            BikeType = "Sports Bike";
                            break;
                        case EnumBikeBodyStyles.Cruiser:
                            StyleName = "Cruisers";
                            BikeType = "Cruiser";
                            break;
                        case EnumBikeBodyStyles.Scooter:
                            StyleName = "Scooters";
                            BikeType = "Scooter";
                            break;
                        case EnumBikeBodyStyles.AllBikes:
                        default:
                            StyleName = "Bikes";
                            BikeType = "Bike";
                            break;

                    }
                    int rank = bikeRankObj.Rank;
                    RankText = Bikewale.Utility.Format.FormatRank(rank);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindGenericBikeInfo.GetGenericBikeInfo");
            }
            return bikeRankObj;
        }
    }
}