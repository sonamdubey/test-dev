
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.GenericBikes
{
    public class BestBikesBL : IBestBikes
    {
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;

        public BestBikesBL(ISearchResult searchResult, IProcessFilter processFilter)
        {
            _searchResult = searchResult;
            _processFilter = processFilter;
        }

        public IEnumerable<BestBikeEntityBase> BestBikesByType(EnumBikeBodyStyles bodyStyle)
        {
            ICollection<BestBikeEntityBase> bikes = null;

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
            SearchOutputEntity objSearchList = _searchResult.GetSearchResult(filterInputs, filterInput);

            if (objSearchList.TotalCount > 0)
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
                            UnitsSold = bike.UnitsSold
                        };
                bikes = b.ToList();
            }

            return bikes;
        }
    }
}
