using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Entities.GenericBikes;
using Microsoft.Practices.Unity;
using Bikewale.Cache.GenericBikes;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.DAL.GenericBikes;
using Bikewale.Notifications;

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
            BikeRankingEntity bikeRankObj=null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBestBikesCacheRepository, BestBikesCacheRepository>()
                            .RegisterType<IGenericBikeRepository, GenericBikeRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    var _objGenericBike = container.Resolve<IGenericBikeRepository>();

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
                            case EnumBikeBodyStyles.AllBikes :
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