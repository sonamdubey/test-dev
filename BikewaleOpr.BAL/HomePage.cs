using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Used;
using System;
using System.Linq;

namespace BikewaleOpr.BAL
{
    public class HomePage : IHomePage
    {
        /// <summary>
        /// created by : Sajal Gupta on 21-3-2017
        /// Description : BAL layer for getting homepage data
        /// </summary>
        /// <returns></returns>
        private readonly IBikeModelsRepository _IBikeModelRepo;
        private readonly IUsedBikes _IUsedBikes;
        private readonly IBikeModels _IBikeModel;

        public HomePage(IBikeModelsRepository bikeModelRepos, IUsedBikes usedBikes, IBikeModels bikeModel)
        {
            _IBikeModelRepo = bikeModelRepos;
            _IUsedBikes = usedBikes;
            _IBikeModel = bikeModel;
        }

        /// <summary>
        /// created by : Sajal Gupta on 21-3-2017
        /// Description : BAL layer function for getting homepage data
        /// </summary>
        /// Modified by : Vivek Singh Tomar on 31 July 2017
        /// Description : Add Another call to get models with missing color images
        /// <returns></returns>
        public HomePageData GetHomePageData(string id, string userName)
        {
            HomePageData objHomePageData = null;
            try
            {
                // If customer id matches the user id from the config file then send the notification to the user

                string[] allowedUsers = Bikewale.Utility.BWOprConfiguration.Instance.NotificationOtherUserId.Split(',');
                if (allowedUsers != null && allowedUsers.Length > 0 && allowedUsers.Contains(id))
                {
                    objHomePageData = new HomePageData();

                    // Sold bike data updates
                    if (DateTime.Now.Day > Bikewale.Utility.BWOprConfiguration.Instance.UnitSoldDataNotificationDay)
                    {
                        objHomePageData.SoldUnitsData = _IBikeModelRepo.GetLastSoldUnitData();
                        if (objHomePageData.SoldUnitsData != null)
                        {
                            objHomePageData.IsSoldBikeDataUpatedShown = _IUsedBikes.SendUnitSoldEmail(objHomePageData.SoldUnitsData, userName);
                        }
                    }

                    // Get models for which used model image has not been updated                    
                    objHomePageData.UsedModelsData = _IBikeModel.GetPendingUsedBikesWithoutModelImage();

                    if (objHomePageData.UsedModelsData != null)
                    {
                        objHomePageData.IsUsedBikeModelsAvailable = true;
                        if (objHomePageData.UsedModelsData.IsNotify)
                            _IUsedBikes.SendUploadUsedModelImageEmail();
                    }

                    //Get models with their respective make id whose any color variation image is yet to be uploaded.

                    objHomePageData.BikeModelByMakeList = _IBikeModel.GetModelsWithMissingColorImage();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.HomePage.GetHomePageData id {0}, name {1}", id, userName));
            }
            return objHomePageData;
        }
    }
}
