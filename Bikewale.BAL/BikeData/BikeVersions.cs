using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.Entities.BikeData;

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
        private IBikeVersions<T, U> versionRepository = null;
        private IBikeVersionCacheRepository<T, U> _versionCacheRepository;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public BikeVersions(IApiGatewayCaller apiGatewayCaller)
        {
            _apiGatewayCaller = apiGatewayCaller;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IBikeVersions<T, U>, BikeVersionsRepository<T, U>>();
                versionRepository = container.Resolve<IBikeVersions<T, U>>();
                container.RegisterType<IBikeVersionCacheRepository<T, U>, BikeVersionsCacheRepository<T, U>>(new InjectionConstructor(new ResolvedParameter<ICacheManager>(), versionRepository));
                _versionCacheRepository = container.Resolve<IBikeVersionCacheRepository<T, U>>();
            }
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

            objVersionList = versionRepository.GetVersionsByType(requestType, modelId, cityId);

            return objVersionList;
        }

        public BikeSpecificationEntity GetSpecifications(U versionId)
        {
            BikeSpecificationEntity objVersion = null;

            objVersion = versionRepository.GetSpecifications(versionId);

            return objVersion;
        }

        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> objMVSpecsMin = null;
            objMVSpecsMin = versionRepository.GetVersionMinSpecs(modelId, isNew);
            return objMVSpecsMin;
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
            T t = versionRepository.GetById(id);

            return t;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To get list of similar bikes by version id
        /// Modified By : Rajan Chauhan on 3 Apr 2018
        /// Description : Binding of specs to similarBikesList from SpecsFeatures MS
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid)
        {
            try
            {
                IEnumerable<SimilarBikeEntity> similarBikesList = _versionCacheRepository.GetSimilarBikesList(versionId, topCount, cityid);
                if (similarBikesList != null && similarBikesList.Any())
                {
                    GetVersionSpecsByItemIdAdapter adapt = new GetVersionSpecsByItemIdAdapter();
                    VersionsDataByItemIds_Input adaptInput = new VersionsDataByItemIds_Input
                    {
                        Versions = similarBikesList.Select(bike => bike.VersionBase.VersionId),
                        Items = new List<Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems>{
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.Displacement,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.MaxPowerBhp,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.MaximumTorqueNm,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.KerbWeight
                        }
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, adaptInput);
                    _apiGatewayCaller.Call();
                    if (adapt.Output != null)
                    {
                        IEnumerator<VersionMinSpecsEntity> versionMinSpecsEnumerator = adapt.Output.GetEnumerator();
                        foreach (var similarBike in similarBikesList)
                        {
                            if (versionMinSpecsEnumerator.MoveNext())
                            {
                                similarBike.MinSpecsList = versionMinSpecsEnumerator.Current.MinSpecsList;
                            }
                        }
                    }

                }
                return similarBikesList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.BikeData.Bikeversions.GetSimilarBikesList({0}, {1}, {2})", versionId, topCount, cityid));
            }
            return null;
        }

        public IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityid)
        {

            return _versionCacheRepository.GetSimilarBikesByModel(modelId, topCount, cityid);
        }
        public IEnumerable<SimilarBikeEntity> GetSimilarBudgetBikes(U modelId, uint topCount, uint cityid)
        {

            return versionRepository.GetSimilarBudgetBikes(modelId, topCount, cityid);
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

            objColors = versionRepository.GetColorByVersion(versionId);

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
            return versionRepository.GetColorsbyVersionId(versionId);
        }

        /// <summary>
        /// Created by sajal gupta on 23-05-2017 to get version segmets details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeModelVersionsDetails> GetModelVersions()
        {
            try
            {
                IEnumerable<BikeVersionsSegment> bikeVersions = versionRepository.GetModelVersionsDAL();

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
                    GetVersionSpecsByItemIdAdapter adapt = new GetVersionSpecsByItemIdAdapter();
                    VersionsDataByItemIds_Input adaptInput = new VersionsDataByItemIds_Input
                    {
                        Versions = versionList.Select(version => (int)version.VersionId),
                        Items = new List<Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems>{
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.AntilockBrakingSystem,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.BrakeType,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.AlloyWheels,
                            Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.ElectricStart
                        }
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, adaptInput);
                    _apiGatewayCaller.Call();
                    if (adapt.Output != null)
                    {
                        IEnumerator<BikeVersionWithMinSpec> bikeVersionEnumerator = versionList.GetEnumerator();
                        IEnumerator<VersionMinSpecsEntity> versionMinSpecEnumertor = adapt.Output.GetEnumerator();
                        while (bikeVersionEnumerator.MoveNext() && versionMinSpecEnumertor.MoveNext())
                        {
                            bikeVersionEnumerator.Current.MinSpecsList = versionMinSpecEnumertor.Current.MinSpecsList;
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
    }   // Class
}   // namespace
