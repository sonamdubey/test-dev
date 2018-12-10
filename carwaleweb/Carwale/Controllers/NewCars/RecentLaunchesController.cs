
using Carwale.Interfaces.CarData;
using Carwale.Service;
using Carwale.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.DTOs.NewCars;
namespace Carwale.UI.Controllers.NewCars
{
    public class RecentLaunchesController : Controller
    {
        // GET: RecentLaunches
        public ActionResult Index()
        {
            string pageNo = Request.QueryString["page"]??"1";
            ushort currentPageNo = 1;
            if(CommonOpn.CheckId(pageNo))
            {
                currentPageNo = UInt16.Parse(pageNo);
            }
            RecentLaunchesDTO_Desktop recentLaunchesDTO = new RecentLaunchesDTO_Desktop();
          
            Pagination page = new Pagination() {PageNo = currentPageNo, PageSize=10 };
            
            ICarModels _carModelsBL = UnityBootstrapper.Resolve<ICarModels>();
            recentLaunchesDTO.RecentLaunches  = _carModelsBL.GetLaunchedCarModelsV1(page, CookiesCustomers.MasterCityId, true);
            recentLaunchesDTO.Pageno = currentPageNo;
            recentLaunchesDTO.RecordCount = recentLaunchesDTO.RecentLaunches.FirstOrDefault().RecordCount;
            recentLaunchesDTO.PageSize = 10;
            return View("~/Views/NewCar/RecentLaunches.cshtml", recentLaunchesDTO);
        }
       
    }
}