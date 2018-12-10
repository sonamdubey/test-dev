using Carwale.Entity;
using Carwale.Entity.CompareCars;
using Carwale.Interfaces.CompareCars;
using MobileWeb.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Carwale.UI.Controllers
{
    public class HotComparisonController : Controller
    {

        private readonly ICompareCarsBL _compRepo;

        public HotComparisonController(ICompareCarsBL compRepo)
        {
            
            _compRepo = compRepo;
            
        }

        [Route("hotComparison/")]
        public ActionResult Index(ushort pageNo, ushort pageSize)
        {
            var hotComparisons = new List<HotCarComparison>();
            Pagination page = new Pagination() { PageNo = pageNo, PageSize = pageSize };

            int cityId = CookiesCustomers.MasterCityId;

            hotComparisons = _compRepo.GetHotComaprisons(page, cityId, true);
            return PartialView("FRQ/_TopComparisons", hotComparisons);
        }        
    }
}