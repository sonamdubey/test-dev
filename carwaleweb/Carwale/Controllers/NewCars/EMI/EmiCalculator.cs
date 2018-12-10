
using System;
using System.Web.Mvc;
using Carwale.Notifications.Logs;

namespace Carwale.UI.Controllers.NewCars
{
    public class EmiCalculatorController : Controller
    {
        [Route("emicalculator")]
        public ActionResult Index()
        {
            try
            {
                return PartialView("~/Views/Shared/m/_EmiCalculator.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }
    }
}