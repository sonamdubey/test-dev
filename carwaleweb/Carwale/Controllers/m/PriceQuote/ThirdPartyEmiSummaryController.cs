using Carwale.Entity.Enum;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using System;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.m.PriceQuote
{
    public class ThirdPartyEmiSummaryController : Controller
    {
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;

        public ThirdPartyEmiSummaryController(IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        public ActionResult Index(int inputVersionId, bool isMetallic, int orp, int platform, int pageId)
        {
            try
            {
                var emiCalculatorModelData = _emiCalculatorAdapter.GetEmiSummary(inputVersionId, isMetallic, orp);
                if (emiCalculatorModelData != null && emiCalculatorModelData.ThirdPartyEmiDetails != null)
                {
                    emiCalculatorModelData.Platform = platform == (int)Platform.CarwaleDesktop ? "Desktop" : "Msite";
                    emiCalculatorModelData.PageName = Enum.GetName(typeof(CwPages), pageId);
                    return PartialView("~/Views/Shared/m/PriceQuote/_EmiSummary.cshtml", emiCalculatorModelData);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }
    }
}