using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    public class BestBikeByBodyStyle
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public EnumBikeBodyStyles BodyStyleType;

        public uint topCount { get; set; }
        public BestBikeByBodyStyle(IBikeModelsCacheRepository<int> objBestBikes, IApiGatewayCaller apiGatewayCaller)
        {
            _objBestBikes = objBestBikes;
            _apiGatewayCaller = apiGatewayCaller;
        }

        /// <summary>
        /// Modified By : Snehal Dange on 24th Nov 2017
        /// Description: Added EnumBikeBodyStyles  for each category
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Added method call to get MinSpecs data from grpc
        /// </summary>
        /// <returns></returns>
        public BestBikeByCategoryVM GetData()
        {
            BestBikeByCategoryVM objData = null;
            try
            {
                objData = new BestBikeByCategoryVM();

                objData.objBestScootersList = FetchBestBikesList(EnumBikeBodyStyles.Scooter);
                objData.objBestSportsBikeList = FetchBestBikesList(EnumBikeBodyStyles.Sports);
                objData.objBestCruiserBikesList = FetchBestBikesList(EnumBikeBodyStyles.Cruiser);
                objData.objBestMileageBikesList = FetchBestBikesList(EnumBikeBodyStyles.Mileage);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.GetData()");
            }
            return objData;
        }

        /// <summary>
        /// Created By : Pratibha Verma on 27 Mar 2018
        /// Summary : Bind MinSpecs to Generic Bike List
        /// Modified by : Ashutosh Sharma on 09 Apr 2017
        /// </summary>
        private void BindMinSpecs(IEnumerable<BestBikeEntityBase> genericBikeList)
        {
            try
            {
                if (genericBikeList != null && genericBikeList.Any())
                {
                    GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = genericBikeList.Select(m => m.VersionId),
                        Items = new List<EnumSpecsFeaturesItems>
                        {
                            EnumSpecsFeaturesItems.Displacement,
                            EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                            EnumSpecsFeaturesItems.MaxPowerBhp,
                            EnumSpecsFeaturesItems.KerbWeight
                        }
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();
                    var specsList = adapt.Output;
                    if (specsList != null)
                    {
                        var specsEnumerator = specsList.GetEnumerator();
                        var bikesEnumerator = genericBikeList.GetEnumerator();
                        while (bikesEnumerator.MoveNext())
                        {
                            if (!bikesEnumerator.Current.VersionId.Equals(0) && specsEnumerator.MoveNext())
                            {
                                bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BestBikeByBodyStyle.BindMinSpecs({0})", genericBikeList));
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model FetchBestBikesList;
        /// </summary>
        private IEnumerable<BestBikeEntityBase> FetchBestBikesList(EnumBikeBodyStyles BodyStyleType)
        {
            IEnumerable<BestBikeEntityBase> objBikesList = null;
            try
            {


                objBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType);

                if (objBikesList != null)
                {
                    objBikesList = objBikesList.Reverse();
                    objBikesList = objBikesList.Take((int)topCount);
                    if (objBikesList.Any())
                    {
                        objBikesList.First().CurrentPage = BodyStyleType;
                    }
                    BindMinSpecs(objBikesList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("FetchBestBikesList BodyStyle:{0}", BodyStyleType));
            }
            return objBikesList;
        }
    }
}