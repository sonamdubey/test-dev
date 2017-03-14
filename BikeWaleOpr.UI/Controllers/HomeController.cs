using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Used;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BikeWaleOpr.MVC.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IBikeModelsRepository _IBikeModelRepo;
        private readonly IUsedBikes _IUsedBikes;
        private readonly IBikeModels _IBikeModel;

        public HomeController(IBikeModelsRepository bikeModelRepos, IUsedBikes usedBikes, IBikeModels bikeModel)
        {
            _IBikeModelRepo = bikeModelRepos;
            _IUsedBikes = usedBikes;
            _IBikeModel = bikeModel;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 09-03-2017
        /// Description : Added functionality for user notification panel.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                ViewBag.IsSoldBikeDataUpatedShown = false;
                ViewBag.IsUsedBikeModelsAvailable = false;
                // If customer id matches the user id from the config file then send the notification to the user

                string[] allowedUsers = Bikewale.Utility.BWOprConfiguration.Instance.NotificationOtherUserId.Split(',');
                if (allowedUsers != null && allowedUsers.Length > 0 && allowedUsers.Contains(CurrentUser.Id))
                {
                    // Sold bike data updates
                    if (DateTime.Now.Day > Bikewale.Utility.BWOprConfiguration.Instance.UnitSoldDataNotificationDay)
                    {
                        SoldUnitData dataObj = _IBikeModelRepo.GetLastSoldUnitData();
                        if (dataObj != null)
                        {
                            ViewBag.DataObj = dataObj;
                            ViewBag.IsSoldBikeDataUpatedShown = _IUsedBikes.SendUnitSoldEmail(dataObj, CurrentUser.UserName);
                        }
                    }
                    // Get models for which used model image has not been updated                    
                    IEnumerable<UsedModelsByMake> objBikeNames = _IBikeModel.GetPendingUsedBikesWithoutModelImage();

                    if (objBikeNames != null)
                    {
                        ViewBag.BikeNames = objBikeNames;
                        ViewBag.IsUsedBikeModelsAvailable = true;
                        _IUsedBikes.SendUploadUsedModelImageEmail();
                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "HomeController/Index");
            }
            return View();
        }

    }
}