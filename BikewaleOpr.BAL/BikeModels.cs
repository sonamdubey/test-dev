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
        private readonly IBikeModelsRepository _IBikeModelRepository;

        public BikeModels(IBikeModelsRepository bikeModelRepository)
        {
            _IBikeModelRepository = bikeModelRepository;
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
                UsedBikeImagesNotificationData usedBikeNotificationDataList = _IBikeModelRepository.GetPendingUsedBikesWithoutModelImage();
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
                objBikeDataList = _IBikeModelRepository.GetModelsWithMissingColorImage();

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
        /// Created by : Ashutosh Sharma on 01 Apr 2018
        /// Description : Method to fetch model id of input version id to check if version is Top version among other versions of a bike model.
        /// </summary>
        /// <param name="versionId">VersionId of bike version.</param>
        /// <returns>ModelId if top version, otherwise 0.</returns>
        public int GetModelIdIfTopVersion(int versionId)
        {
            int modelId = 0;
            try
            {
                modelId = _IBikeModelRepository.GetModelIdIfTopVersion(versionId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeModels.GetModelIdIfTopVersion_versionId_{0}", versionId));
            }
            return modelId;
        }
    }
}
