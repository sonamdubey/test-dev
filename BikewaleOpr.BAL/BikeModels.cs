using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace BikewaleOpr.BAL
{
    /// <summary>
    /// created by : Sajal Gupta on 09-03-2017
    /// Description : BAL layer for calling dal functions of opr model related.
    /// </summary>
    public class BikeModels : IBikeModels
    {
        private readonly IBikeModelsRepository _IBikeModel;

        public BikeModels(IBikeModelsRepository bikeModel)
        {
            _IBikeModel = bikeModel;
        }

        /// <summary>
        /// created by : Sajal Gupta on 09-03-2017
        /// Description : BAL layer for calling dal function GetPendingUsedBikesWithoutModelImage.
        /// </summary>
        /// <returns></returns>
        public UsedBikeImagesByMakeNotificationData GetPendingUsedBikesWithoutModelImage()
        {
            UsedBikeImagesByMakeNotificationData objBikeByMakeNotificationData = new UsedBikeImagesByMakeNotificationData();
            IEnumerable<UsedModelsByMake> objBikesByMake = null;
            try
            {
                UsedBikeImagesNotificationData usedBikeNotificationDataList = _IBikeModel.GetPendingUsedBikesWithoutModelImage();
                objBikesByMake = new List<UsedModelsByMake>();
                if (usedBikeNotificationDataList != null && usedBikeNotificationDataList.Bikes != null)
                {
                    var grpMakes = usedBikeNotificationDataList.Bikes
                        .GroupBy(m => m.MakeId);

                    if (grpMakes != null)
                    {
                        objBikesByMake = grpMakes
                                        .Select(m => new UsedModelsByMake()
                                        {
                                            MakeId = m.FirstOrDefault().MakeId,
                                            MakeName = m.FirstOrDefault().MakeName,
                                            ModelList = m.Select(x => x.ModelName)
                                        });
                    }

                    objBikeByMakeNotificationData.BikesByMake = objBikesByMake;
                    objBikeByMakeNotificationData.IsNotify = usedBikeNotificationDataList.IsNotify;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikeModels.GetPendingUsedBikesWithoutModelImage");
            }
            return objBikeByMakeNotificationData;
        }

        /// <summary>
        /// Created by: vivek singh tomar on 27/07/2017        
        /// Description: Function to get bike models whose color images are not uploaded
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeModelsByMake> GetModelsWithMissingColorImage()
        {
            IEnumerable<BikeModelsByMake> objBikeModelsByMakeList = null;
            IEnumerable<BikeMakeModelData> objBikeDataList = null;

            try
            {
                objBikeDataList = _IBikeModel.GetModelsWithMissingColorImage();

                if(objBikeDataList != null)
                {
                    // grouping models according to their respective makes
                    objBikeModelsByMakeList = objBikeDataList
                        .GroupBy(m => m.BikeMake.MakeId)
                        .Select( grp => new BikeModelsByMake
                        {
                            BikeMakeEntity = grp.First().BikeMake,
                            BikeModelEntity = grp.Select(t => t.BikeModel)
                        });
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikeModels.GetModelsWithMissingColorImage");
            }
            return objBikeModelsByMakeList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: To create a NVC and push it to BWEsDocumentBuilder queue for further processing.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operation"></param>
        public void UpdateModelESIndex(string ids, string operation)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc["ids"] = ids;
            nvc["indexName"] = BWOprConfiguration.Instance.BikeModelIndex;
            nvc["documentType"] = "bikemodeldocument";
            nvc["operationType"] = operation;

            BWESDocumentBuilder.PushToQueue(nvc);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 02 Apr 2018
        /// Description : Method to update bike min specs in elastic search index when specs are updated for a version if that version is top version among other versions of bike model.
        /// </summary>
        /// <param name="versionId">Version Id for which min specs to be updated in elastic index.</param>
        /// <param name="specItemList">List of specs items with updated values which need to updated in ealstic index.</param>
        public void UpdateMinSpecsInESIndex(int versionId, IEnumerable<SpecsItem> specItemList)
        {
            try
            {
                int modelId = _IBikeModel.GetModelIdIfTopVersion(versionId);
                if (modelId > 0)
                {
                    var mileage = specItemList.SingleOrDefault(s => s.Id == (int)EnumSpecsFeaturesItems.FuelEfficiencyOverall);
                    var kerbWeight = specItemList.SingleOrDefault(s => s.Id == (int)EnumSpecsFeaturesItems.KerbWeight);
                    var maxPower = specItemList.SingleOrDefault(s => s.Id == (int)EnumSpecsFeaturesItems.MaxPowerBhp);
                    var displacement = specItemList.SingleOrDefault(s => s.Id == (int)EnumSpecsFeaturesItems.Displacement);
                    dynamic jsonSpecs = new JObject();
                    if (mileage != null)
                    {
                        jsonSpecs.mileage = mileage.Value;
                    }
                    if (kerbWeight != null)
                    {
                        jsonSpecs.kerbWeight = kerbWeight.Value;
                    }
                    if (maxPower != null)
                    {
                        jsonSpecs.power = maxPower.Value;
                    }
                    if (displacement != null)
                    {
                        jsonSpecs.displacement = displacement.Value;
                    }
                    JObject jsonTopVersionObj = new JObject();
                    jsonTopVersionObj["topVersion"] = jsonSpecs;
                    NameValueCollection nvc = new NameValueCollection();
                    nvc["indexName"] = BWOprConfiguration.Instance.BikeModelIndex;
                    nvc["documentType"] = "bikemodeldocument";
                    nvc["documentId"] = Convert.ToString(modelId);
                    nvc["operationType"] = "partialupdate";
                    nvc["documentJson"] = jsonTopVersionObj.ToString();
                    BWESIndexUpdater.PushToQueue(nvc);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeModels.UpdateMinSpecsInESIndex_versionId_{0}", versionId));
            }
        }
    }
}
