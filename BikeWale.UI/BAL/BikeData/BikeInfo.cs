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
using log4net;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.ObjectModel;
using Bikewale.Interfaces.NewBikeSearch;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 Jan 2017
    /// Summary : Class have methods to get the models for the bike info
    /// </summary>
    public class BikeInfo : IBikeInfo
    {
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BikeInfo));
        private readonly IBikeSearch _bikeSearch;
        /// <summary>
        /// Constructor to initialize all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        /// <param name="_genericBike"></param>
        public BikeInfo(IBikeModelsCacheRepository<int> modelCache, IApiGatewayCaller apiGatewayCaller, IBikeSearch bikeSearch)
        {
            _modelCache = modelCache;
            _apiGatewayCaller = apiGatewayCaller;
            _bikeSearch = bikeSearch;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 25 jan 2017
        /// Summary : Function will return the bike info model to bind with view.
        /// Modified By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public Bikewale.Models.Shared.BikeInfo GetBikeInfo(uint modelId)
        {
            Bikewale.Models.Shared.BikeInfo objBikeInfo = null;

            try
            {
                GenericBikeInfo objBikes = _modelCache.GetBikeInfo(modelId);

                if (objBikes != null)
                {
                    objBikeInfo = new Bikewale.Models.Shared.BikeInfo();

                    objBikeInfo.Info = objBikes;
                    objBikeInfo.ModelId = modelId;

                    if (objBikes.Make != null)
                        objBikeInfo.Url = string.Format("{0}/m/{1}-bikes/{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objBikes.Make.MaskingName, objBikes.Model.MaskingName);

                    if (objBikes.Model != null)
                        objBikeInfo.Bike = string.Format("{0} {1}", objBikes.Make.MakeName, objBikes.Model.ModelName);

                    objBikeInfo.PQSource = (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_GenricBikeInfo_Widget;

                };
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeInfo.GetBikeInfo_{0}", modelId));
            }
            return objBikeInfo;
        }   // End of GetBikeInfo
        /// <summary>
        /// Created By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// Modified By : Rajan Chauhan on 9 Apr 2018
        /// Description : Added MinSpecsBinding to Generic BikeInfo
        /// Modified By : Rajan Chauhan on 23 Apr 2018
        /// Description : Added null check on genericBike
        /// Modified By : Sanjay George on 1 Oct 2018
        /// Description : Seperated flow for used bike info
        /// </summary>
        public GenericBikeInfo GetBikeInfo(uint modelId, uint cityId, bool isUsedBikeFetched)
        {
            GenericBikeInfo genericBike = null;
            UsedBikeInfo usedBikeInfo = null;
            
            try
            {
                
                if (modelId > 0)
                {
                    if (cityId <= 0)
                    {
                        genericBike = _modelCache.GetBikeInfo(modelId);
                    }
                    else if (!isUsedBikeFetched)
                    {
                        // fetch from new version of sp and bind with price
                        genericBike = _modelCache.GetBikeInfo(modelId, cityId);
                        if(genericBike != null)
                        {
                            genericBike.PriceInCity = GetPriceFromES(modelId, cityId, (uint)genericBike.VersionId);
                        }
                        
                    }
                    else
                    {
                        // fetch from new sp + price + usedbikesp
                        genericBike = _modelCache.GetBikeInfo(modelId, cityId);
                        usedBikeInfo = _modelCache.GetUsedBikeInfo(modelId, cityId);

                        if (genericBike != null && usedBikeInfo != null)
                        {
                            genericBike.PriceInCity = GetPriceFromES(modelId, cityId, (uint)genericBike.VersionId);
                            genericBike.UsedBikeCount = usedBikeInfo.UsedBikeCount;
                            genericBike.UsedBikeMinPrice = usedBikeInfo.UsedBikeMinPrice;
                        }
                    }
                    if (genericBike != null && genericBike.VersionId > 0)
                    {
                        GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                        VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                        {
                            Versions = new List<int> { genericBike.VersionId },
                            Items = new List<EnumSpecsFeaturesItems>() {
                            EnumSpecsFeaturesItems.Displacement,
                            EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                            EnumSpecsFeaturesItems.MaxPowerBhp,
                            EnumSpecsFeaturesItems.KerbWeight
                        }
                        };
                        adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                        _apiGatewayCaller.Call();
                        var bikeVersionMinSpecList = adapt.Output;
                        if (bikeVersionMinSpecList != null && bikeVersionMinSpecList.Any())
                        {
                            genericBike.MinSpecsList = bikeVersionMinSpecList.FirstOrDefault().MinSpecsList;
                            if (genericBike.MinSpecsList.Any(spec => !string.IsNullOrEmpty(spec.Value)))
                            {
                                genericBike.IsSpecsAvailable = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeInfo.GetBikeInfo {0}, {1}", modelId, cityId));
            }
            
            return genericBike;
        }
        /// <summary>
        /// Created By : Prabhu Puredla on 1 oct 2018
        /// Description : Get the price of a bike from Elastic Search
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        private uint GetPriceFromES(uint modelId, uint cityId, uint versionId)
        {
            uint versionPrice = 0;
            try
            {
                ICollection<int> modelIds = new Collection<int> { (int)modelId };
                IEnumerable<BikeTopVersion> similarBikePrices = _bikeSearch.GetBikePriceSearchList(modelIds, cityId, BikeSearchEnum.PriceList);
                if (similarBikePrices != null && similarBikePrices.Any())
                {
                    IEnumerable<PriceEntity> priceList = null;
                    foreach (var version in similarBikePrices.First().VersionPrice)
                    {
                        if (version.VersionId == versionId)
                        {
                            priceList = similarBikePrices.First().VersionPrice.First().PriceList;
                        }
                    }
                    if (priceList != null)
                    {
                        versionPrice = (uint)priceList.Sum(price => price.PriceValue);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeInfo.GetPriceFromES {0}, {1}, {2}", modelId, cityId, versionId));
            }
            return versionPrice;
        }
    }   // class
}   // namespace
