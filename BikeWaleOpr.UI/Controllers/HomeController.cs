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
        private readonly IBikeModels _IBikeModel;
        private readonly IUsedBikes _IUsedBikes;

        public HomeController(IBikeModels bikeModel, IUsedBikes usedBikes)
        {
            _IBikeModel = bikeModel;
            _IUsedBikes = usedBikes;
        }
        public ActionResult Index()
        {
            try
            {
                ViewBag.IsSoldBikeDataUpatedShown = false;
                ViewBag.IsUsedBikeModelsAvailable = false;
                // If customer id matches the user id from the config file then send the notification to the user

                string[] allowedUsers = Bikewale.Utility.BWOprConfiguration.Instance.NotificationOtherUserId.Split(',');
                if (allowedUsers.Length> 0 && allowedUsers.Contains(CurrentUser.Id))
                {
                    // Sold bike data updates
                    if (DateTime.Now.Day > 15)
                    {
                        SoldUnitData dataObj = _IBikeModel.GetLastSoldUnitData();
                        if (dataObj != null)
                        {
                            ViewBag.DataObj = dataObj;
                            ViewBag.IsSoldBikeDataUpatedShown = _IUsedBikes.SendUnitSoldEmail(dataObj, CurrentUser.UserName);
                        }
                    }
                    // Get models for which used model image has not been updated
                    List<string> usedBikeList = _IBikeModel.GetPendingUsedBikesWithoutModelImage();
                    if (usedBikeList != null && usedBikeList.Count > 0)
                    {
                        if (usedBikeList.Count >= 5)
                        {
                            ViewBag.BikeNames = usedBikeList;
                            ViewBag.BikeNameShort = string.Join(", ", usedBikeList.Take(5));
                        }
                        else
                        {
                            ViewBag.BikeNameShort = string.Join(", ", usedBikeList);
                        }
                        ViewBag.IsUsedBikeModelsAvailable = true;
                        _IUsedBikes.SendUploadUsedModelImageEmail(ViewBag.BikeNameShort);
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