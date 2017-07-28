using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.BikeModels.GetPendingUsedBikesWithoutModelImage");
            }
            return objBikeByMakeNotificationData;
        }

        /// <summary>
        /// Description: BAL layer to call DAL function GetModelsWithMissingImage and group them with MakeId
        /// 
        /// created by: vivek singh tomar on 27/07/2017
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeModelsByMake> GetModelsWithMissingColorImage()
        {
            IEnumerable<BikeModelsByMake> objBikeModelsByMakeList = null;
            IEnumerable<BikeMakeModelData> objBikeMakeModelDataList = null;
            try
            {
                objBikeMakeModelDataList = _IBikeModel.GetModelsWithMissingColorImage();
                if(objBikeMakeModelDataList != null)
                {
                    var groupModelsByMake = objBikeMakeModelDataList
                        .GroupBy(m => m.BikeMake.MakeId)
                        .Select(grp => new {
                            MakeId = grp.Key,
                            MakeEntiy = grp.First().BikeMake,
                            ModelEntiy = grp.Select(t => t.BikeModel)
                        })
                        .ToList();
                      
                    if(groupModelsByMake != null)
                    {
                        objBikeModelsByMakeList = groupModelsByMake
                                                    .Select(m => new BikeModelsByMake()
                                                    {
                                                        BikeMakeEntity = m.MakeEntiy,
                                                        BikeModelEntity = m.ModelEntiy
                                                    });
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.BikeModels.GetModelsWithMissingColorImage");
            }
            return objBikeModelsByMakeList;
        }
    }
}
