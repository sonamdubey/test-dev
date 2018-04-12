using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
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
        private IBikeVersions<T, U> versionRepository = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public BikeVersions()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeVersions<T, U>, BikeVersionsRepository<T, U>>();
                container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
                versionRepository = container.Resolve<IBikeVersions<T, U>>();
                _apiGatewayCaller = container.Resolve<IApiGatewayCaller>();
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
        [Obsolete("Use Specification and Features Micro Service to get all specs.", true)]
        public BikeSpecificationEntity GetSpecifications(U versionId)
        {
            return versionRepository.GetSpecifications(versionId);
        }

        public IEnumerable<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            IEnumerable<BikeVersionMinSpecs> versionsList = null;
            try
            {
                versionsList = versionRepository.GetVersionMinSpecs(modelId, isNew);
                GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                var specItemInput = new VersionsDataByItemIds_Input
                {
                    Versions = versionsList.Select(v => v.VersionId),
                    Items = new List<EnumSpecsFeaturesItems>
                    {
                        EnumSpecsFeaturesItems.BrakeType,
                        EnumSpecsFeaturesItems.AlloyWheels,
                        EnumSpecsFeaturesItems.ElectricStart,
                        EnumSpecsFeaturesItems.AntilockBrakingSystem
                    }
                };
                adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                _apiGatewayCaller.Call();
                var specsResponseList = adapt1.Output;
                if (specsResponseList != null)
                {
                    var specsEnumerator = specsResponseList.GetEnumerator();
                    var bikesEnumerator = versionsList.GetEnumerator();
                    while (bikesEnumerator.MoveNext() && specsEnumerator.MoveNext())
                    {
                        bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
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
                IEnumerable<SimilarBikeEntity> similarBikesList = versionRepository.GetSimilarBikesList(versionId, topCount, cityid);
                if (similarBikesList != null && similarBikesList.Any())
                {
                    var specItemLIst = new List<EnumSpecsFeaturesItems>{
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.MaximumTorqueNm,
                        EnumSpecsFeaturesItems.KerbWeight
                    };
                    BindMinSpecs(similarBikesList, specItemLIst);
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

            return versionRepository.GetSimilarBikesByModel(modelId, topCount, cityid);
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
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId)
        {
            return versionRepository.GetDealerVersionsByModel(dealerId, modelId);
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
                    GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                    var specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = bikesList.Select(m => m.VersionBase.VersionId),
                        Items = specItemList
                    };
                    adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();

                    IEnumerable<VersionMinSpecsEntity> specsResponseList = adapt1.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = bikesList.GetEnumerator();
                        while (bikesEnumerator.MoveNext() && specsEnumerator.MoveNext())
                        {
                            bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeVersions.BindMinSpecs_bikesList_{0}_specItemList_{1}", bikesList, specItemList));
            }
        }
    }   // Class
}   // namespace
