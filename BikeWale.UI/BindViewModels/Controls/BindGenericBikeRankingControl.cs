using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Aditi Srivastava on 12 Jan 2017
    /// Description: To get bike ranking in a category by model id
    /// </summary>
    public class BindGenericBikeRankingControl
    {
        public uint ModelId { get; set; }
        public string StyleName { get; set; }
        public string BikeType { get; set; }
        public string RankText { get; set; }

        public BikeRankingEntity GetBikeRankingByModel()
        {
            BikeRankingEntity bikeRankObj = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
                    ;
                    var _objGenericBike = container.Resolve<IBikeModelsCacheRepository<int>>();


                    bikeRankObj = _objGenericBike.GetBikeRankingByCategory(ModelId);
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


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindGenericBikeInfo.GetGenericBikeInfo");
            }
            return bikeRankObj;
        }
    }
}