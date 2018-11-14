using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class have all functions related to the bike versions. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeVersions<T, U> : IBikeVersions<T, U> where T : BikeVersionEntity, new()
    {
        private IBikeVersionsRepository<T, U> _versionRepository = null;
        private IBikeVersionCacheRepository<T, U> _versionCacheRepository;
        private IBikeVersionCacheRepository<T, uint> _uVersionCacheRepository;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private readonly IBikeSearch _bikeSearch;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BikeVersions<T, U>));

        public BikeVersions(IBikeVersionsRepository<T, U> versionRepository, IBikeVersionCacheRepository<T, U> versionCacheRepository, IBikeVersionCacheRepository<T, uint> uVersionCacheRepository, IApiGatewayCaller apiGatewayCaller, IBikeSearch bikeSearch)
        {
            _apiGatewayCaller = apiGatewayCaller;
            _versionRepository = versionRepository;
            _versionCacheRepository = versionCacheRepository;
            _uVersionCacheRepository = uVersionCacheRepository;
            _bikeSearch = bikeSearch;
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 25 Aug 2014
        /// Summary : Changed return type to get price
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionList = null;

            objVersionList = _versionCacheRepository.GetVersionsByType(requestType, modelId, cityId);

            return objVersionList;
        }
        /// <summary>
        /// Modified by : Snehal Dange on 14th Nov 2018
        /// Desc :  Changed RearBrakeType to FrontBrakeType
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public IEnumerable<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            IEnumerable<BikeVersionMinSpecs> versionsList = null;
            try
            {
                versionsList = _versionCacheRepository.GetVersionMinSpecs(modelId, isNew);
                if (versionsList != null && versionsList.Any())
                {
                    GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = versionsList.Select(v => v.VersionId),
                        Items = new List<EnumSpecsFeaturesItems>
                    {
                        EnumSpecsFeaturesItems.FrontBrakeType,
                        EnumSpecsFeaturesItems.WheelType,
                        EnumSpecsFeaturesItems.StartType,
                        EnumSpecsFeaturesItems.AntilockBrakingSystem
                    }
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();
                    var specsResponseList = adapt.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = versionsList.GetEnumerator();
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
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeVersions.GetVersionMinSpecs_modelId_{0}_isNew_{1}", modelId, isNew));
            }
            return versionsList;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BikeVersionsSegment> GetModelVersionsDAL()
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            T t = _versionCacheRepository.GetById(id);

            return t;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To get list of similar bikes by version id
        /// Modified By : Rajan Chauhan on 3 Apr 2018
        /// Description : Binding of specs to similarBikesList from SpecsFeatures MS
        /// Modified By : Rajan Chauhan on 17 Apr 2018
        /// Description : Added maxTorqueRequired param to cater to SimilarBike API
        /// Modified By : Prabhu Puredla on 28 sept 2018
        /// Descritption : Fetching prices from elastic index
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <param name="maxTorqueRequired"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityId, bool maxTorqueRequired)
        {
            try
            {
                BikeVersionEntity version = _versionCacheRepository.GetById(versionId);
                IEnumerable<SimilarBikeEntity> similarBikesList = null;

                if (version != null)
                {
                    similarBikesList = _uVersionCacheRepository.GetSimilarBikesList(Convert.ToUInt32(version.ModelBase.ModelId), topCount);
                    if (similarBikesList != null && similarBikesList.Any())
                    {
                        PopulateSimilarBikePrices(similarBikesList, cityId);

                        IList<EnumSpecsFeaturesItems> specItemList = new List<EnumSpecsFeaturesItems>{
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.KerbWeight
                        };
                        if (maxTorqueRequired)
                        {
                            specItemList.Add(EnumSpecsFeaturesItems.MaximumTorqueNm);
                        }
                        BindMinSpecs(similarBikesList, specItemList);
                    }
                }

                return similarBikesList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.BikeData.Bikeversions.GetSimilarBikesList({0}, {1}, {2})", versionId, topCount, cityId));
            }
            return null;
        }
        /// <summary>
        /// Created By : Prabhu Puredla on 28 sept 2018
        /// Descritption : Populate prices from ES
        /// </summary>
        /// <param name="similarBikesList"></param>
        /// <param name="cityId"></param>
        private void PopulateSimilarBikePrices(IEnumerable<SimilarBikeEntity> similarBikesList, uint cityId)
        {
            try
            {
                IEnumerable<int> modelIds = similarBikesList.Select(model => model.ModelBase.ModelId);
                IEnumerable<BikeTopVersion> similarBikePrices = _bikeSearch.GetBikePriceSearchList(modelIds, cityId, BikeSearchEnum.PriceList);

                IDictionary<uint, uint> pricesDictionary = new Dictionary<uint, uint>();
                if (similarBikePrices != null && similarBikePrices.Any())
                {
                    string cityName = similarBikePrices.First().City.CityName;//city name is constant for all items in ienumerable
                    string cityMaskingName = similarBikePrices.First().City.CityMaskingName;
                    foreach (var bike in similarBikePrices)
                    {
                        if (bike.VersionPrice != null)
                        {
                            foreach (var bikePrice in bike.VersionPrice)
                            {
                                if (!pricesDictionary.ContainsKey(bikePrice.VersionId))
                                {
                                    pricesDictionary.Add(bikePrice.VersionId, bikePrice.PriceList.First().PriceValue);
                                }
                            }
                        }
                    }

                    foreach (var similarBike in similarBikesList)
                    {
                        uint versionPrice;
                        if (pricesDictionary.TryGetValue((uint)similarBike.VersionBase.VersionId, out versionPrice))
                        {
                            similarBike.VersionPrice = similarBike.MinPrice = (int)versionPrice;
                        }
                        similarBike.CityName = cityName;
                        similarBike.CityMaskingName = cityMaskingName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Bikeversions.PopulateSimilarBikePrices");
            }
        }

        public IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityid)
        {
            try
            {
                IEnumerable<SimilarBikeEntity> similarBikesList = _versionCacheRepository.GetSimilarBikesByModel(modelId, topCount, cityid);
                if (similarBikesList != null && similarBikesList.Any())
                {
                    var specItemLIst = new List<EnumSpecsFeaturesItems>{
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.KerbWeight
                    };
                    BindMinSpecs(similarBikesList, specItemLIst);
                }
                return similarBikesList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.BikeData.Bikeversions.GetSimilarBikesByModel_modelId_{0}_topCount_{1}_cityId_{2})", modelId, topCount, cityid));
            }
            return null;
        }
        public IEnumerable<SimilarBikeEntity> GetSimilarBudgetBikes(U modelId, uint topCount, uint cityid)
        {

            return _versionCacheRepository.GetSimilarBikesByMinPriceDiff(modelId, topCount, cityid);
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 Dec 2014
        /// Summary : get version color by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<VersionColor> GetColorByVersion(U versionId)
        {
            List<VersionColor> objColors = null;

            objColors = _versionRepository.GetColorByVersion(versionId);

            return objColors;
        }
        /// <summary>
        /// Created By: Aditi Srivastava 17 Oct 2016
        /// Description: Get version colors and group hexcodes by color id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId)
        {
            return _versionCacheRepository.GetColorsbyVersionId(versionId);
        }

        /// <summary>
        /// Created by sajal gupta on 23-05-2017 to get version segmets details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeModelVersionsDetails> GetModelVersions()
        {
            try
            {
                IEnumerable<BikeVersionsSegment> bikeVersions = _versionRepository.GetModelVersionsDAL();

                IEnumerable<BikeModelVersionsDetails> objVersionList = new List<BikeModelVersionsDetails>();

                objVersionList = bikeVersions.GroupBy(
                    p => new { p.ModelId, p.ModelMaskingName, p.ModelName, p.CCSegment, p.TopVersionId },
                    p => p.VersionId > 0 ? new BikeVersionSegmentDetails(p.Segment, p.VersionName) { VersionId = p.VersionId, BodyStyle = p.BodyStyle } : null,
                    (key, g) => new BikeModelVersionsDetails() { CCSegment = string.IsNullOrEmpty(key.CCSegment) ? "NA" : key.CCSegment, ModelId = key.ModelId, ModelName = key.ModelName, MaskingName = string.IsNullOrEmpty(key.ModelMaskingName) ? "NA" : key.ModelMaskingName, Versions = ((g != null && g.Any() && g.FirstOrDefault() != null) ? g : null), BodyStyle = (g != null && key.TopVersionId > 0 && g.FirstOrDefault(x => x != null && x.VersionId == key.TopVersionId) != null) ? g.FirstOrDefault(x => x != null && x.VersionId == key.TopVersionId).BodyStyle : "NA" }
                    );

                return objVersionList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Bikeversions.GetModelVersions");
                return null;
            }
        }

        /// <summary>
        /// Gets the dealer versions by model.
        /// Modified By : Rajan Chauhan on 10 Apr 2018
        /// Description : Added minSpec Logic
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId)
        {
            try
            {
                IEnumerable<BikeVersionWithMinSpec> versionList = _versionCacheRepository.GetDealerVersionsByModel(dealerId, modelId);
                if (versionList != null)
                {
                    GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                    VersionsDataByItemIds_Input adaptInput = new VersionsDataByItemIds_Input
                    {
                        Versions = versionList.Select(version => (int)version.VersionId),
                        Items = new List<EnumSpecsFeaturesItems>{
                            EnumSpecsFeaturesItems.AntilockBrakingSystem,
                            EnumSpecsFeaturesItems.RearBrakeType,
                            EnumSpecsFeaturesItems.WheelType,
                            EnumSpecsFeaturesItems.StartType
                        }
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, adaptInput);
                    _apiGatewayCaller.Call();
                    if (adapt.Output != null)
                    {
                        IEnumerator<BikeVersionWithMinSpec> bikeVersionEnumerator = versionList.GetEnumerator();
                        IEnumerator<VersionMinSpecsEntity> versionMinSpecEnumertor = adapt.Output.GetEnumerator();
                        while (bikeVersionEnumerator.MoveNext())
                        {
                            if (bikeVersionEnumerator.Current.VersionId > 0 && versionMinSpecEnumertor.MoveNext())
                            {
                                bikeVersionEnumerator.Current.MinSpecsList = versionMinSpecEnumertor.Current.MinSpecsList;
                            }
                        }
                    }
                }
                return versionList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.BikeData.Bikeversions.GetDealerVersionsByModel({0}, {1})", dealerId, modelId));
            }
            return null;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 11 Apr 2018.
        /// Description : Method to call specs features service and bind specs features data in bikeList object.
        /// </summary>
        /// <param name="bikesList">List of bikes object in which specs binding has to be done.</param>
        /// <param name="specItemList">List of specs ids for which specs data has to be done.</param>
        private void BindMinSpecs(IEnumerable<SimilarBikeEntity> bikesList, IEnumerable<EnumSpecsFeaturesItems> specItemList)
        {
            try
            {
                if (bikesList != null && bikesList.Any())
                {
                    GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = bikesList.Select(m => m.VersionBase.VersionId),
                        Items = specItemList
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();

                    IEnumerable<VersionMinSpecsEntity> specsResponseList = adapt.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = bikesList.GetEnumerator();
                        while (bikesEnumerator.MoveNext())
                        {
                            if (!bikesEnumerator.Current.VersionBase.VersionId.Equals(0) && specsEnumerator.MoveNext())
                            {
                                bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeVersions.BindMinSpecs_bikesList_{0}_specItemList_{1}", bikesList, specItemList));
            }
        }

        /// <summary>
        /// Author  : Kartik Rathod on 11 May 2018
        /// Desc    : Get similar bikes based on road price for emi page in finance
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topcount"></param>
        /// <param name="cityId"></param>
        /// <returns>SimilarBikesForEMIEntityList</returns>
        public IEnumerable<SimilarBikesForEMIEntity> GetSimilarBikesForEMI(int modelId, byte topcount, int cityId)
        {
            if (topcount <= 0)
            {
                topcount = (byte)9;
            }
            return _versionCacheRepository.GetSimilarBikesForEMI(modelId, topcount, cityId);
        }


    }   // Class
}   // namespace

