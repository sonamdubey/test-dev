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
        private readonly IBikeModelsRepository _IBikeModelRepo;
        private readonly IUsedBikes _IUsedBikes;
        private readonly IBikeModels _IBikeModel;

        public HomePage(IBikeModelsRepository bikeModelRepos, IUsedBikes usedBikes, IBikeModels bikeModel)
        {
            _IBikeModelRepo = bikeModelRepos;
            _IUsedBikes = usedBikes;
            _IBikeModel = bikeModel;
        }

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
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikewaleOpr.BAL.HomePage.GetHomePageData");
            }
            return objHomePageData;
        }
    }
}
