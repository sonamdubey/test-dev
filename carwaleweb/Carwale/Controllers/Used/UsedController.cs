using Carwale.Entity.ViewModels;
using Carwale.Interfaces.Classified;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class UsedController : Controller
    {
        private readonly ICommonOperationsRepository _commonRepo;
        public UsedController(ICommonOperationsRepository commonRepo)
        {
            _commonRepo = commonRepo;
        }

        [DeviceDetectionFilter]        
        public ActionResult Default()
        {
            var model = new UsedCarModel();
            try
            {                
                model = _commonRepo.GetUsedCarListings();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UsedController.Default()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return View(model);
        }

        public ActionResult MDefault()
        {
            return View("~/Views/m/Used/Default.cshtml");
        }
    }
}