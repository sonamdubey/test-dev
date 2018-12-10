using Carwale.UI.ClientBL;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.Dealers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class DealerRenewalController : Controller
    {
        private readonly IUnityContainer _container;

        public DealerRenewalController(IUnityContainer container)
        {
            _container = container;
        }

        public ActionResult Index()
        {
            bool isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
            Response.AddHeader("Vary", "User-Agent");

            if (isMobile)
            {
                return View("~/views/m/dealers/dealerrenewal.cshtml");
            }
            else
            {
                return View("~/views/dealers/dealerrenewal.cshtml");
            }
        }
    }
}
