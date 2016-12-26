
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.GenericBikes
{
    /// <summary>
    /// Created By : Sumit Kate on 23rd Dec 2016
    /// Description : BAL Layer to manage bussiness logic related to generic pages
    /// </summary>
    public class BestBikesBL : IBestBikes
    {
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;
        private readonly IBestBikesCacheRepository _bestBikeCache = null;
        private readonly IArticles _objArticles = null;
        private readonly ICMSCacheContent _cmsCache = null;
        public BestBikesBL(IBestBikesCacheRepository bestBikeCache, ISearchResult searchResult, IProcessFilter processFilter, IArticles objArticles, ICMSCacheContent cmsCache)
        {
            _bestBikeCache = bestBikeCache;
            _searchResult = searchResult;
            _processFilter = processFilter;
            _objArticles = objArticles;
            _cmsCache = cmsCache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Dec 2016
        /// Description :   Get Top 10 Bikes for Generic Page
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <returns></returns>
        public IEnumerable<BestBikeEntityBase> BestBikesByType(EnumBikeBodyStyles bodyStyle)
        {
            IEnumerable<BestBikeEntityBase> bikes = null;
            SearchOutputEntity objSearchList = null;
            try
            {
                InputBaseEntity filterInput = new InputBaseEntity();
                filterInput.PageSize = "10";
                switch (bodyStyle)
                {

                    case EnumBikeBodyStyles.AllBikes:
                        break;
                    case EnumBikeBodyStyles.Cruiser:
                    case EnumBikeBodyStyles.Sports:
                    case EnumBikeBodyStyles.Scooter:
                        filterInput.RideStyle = Convert.ToString((int)bodyStyle);
                        break;
                    case EnumBikeBodyStyles.Mileage:
                        filterInput.Mileage = "1";
                        break;
                    default:
                        break;
                }

                FilterInput filterInputs = _processFilter.ProcessFilters(filterInput);

                bikes = _bestBikeCache.BestBikesByType(bodyStyle, filterInputs, filterInput);
                if (objSearchList != null && objSearchList.TotalCount > 0)
                {
                    DateTime startOfTime = new DateTime();

                    var b = from bike in objSearchList.SearchResult
                            select new BestBikeEntityBase()
                            {
                                BikeName = bike.BikeName,
                                Model = bike.BikeModel,
                                Make = bike.BikeModel.MakeBase,
                                HostUrl = bike.BikeModel.HostUrl,
                                OriginalImagePath = bike.BikeModel.OriginalImagePath,
                                MinSpecs = new Entities.BikeData.MinSpecsEntity()
                                {
                                    Displacement = bike.Displacement,
                                    FuelEfficiencyOverall = bike.FuelEfficiency,
                                    KerbWeight = bike.KerbWeight,
                                    MaximumTorque = bike.MaximumTorque,
                                    MaxPower = SqlReaderConvertor.ToNullableFloat(bike.Power)
                                },
                                Price = SqlReaderConvertor.ParseToUInt32(bike.BikeModel.MinPrice),
                                SmallModelDescription = bike.SmallDescription,
                                FullModelDescription = bike.FullDescription,
                                LaunchDate = (!bike.LaunchedDate.Equals(startOfTime) ? bike.LaunchedDate : default(Nullable<DateTime>)),
                                PhotosCount = bike.PhotoCount,
                                VideosCount = bike.VideoCount,
                                UnitsSold = bike.UnitsSold,
                                TotalVersions = bike.VersionCount,
                                TotalModelColors = bike.ColorCount
                            };
                    bikes = b.ToList();
                }
                foreach (var bike in bikes)
                {
                    var articles = _cmsCache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), 1, 10, (int)bike.Make.MakeId, (int)bike.Model.ModelId);
                    if (articles != null)
                    {
                        bike.ExpertReviewsCount = articles.RecordCount;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesBL.BestBikesByType({0})", bodyStyle));
            }
            return bikes;
        }
    }
}
