using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class BikeCareController : Controller
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        #endregion

        #region Constructor
        public BikeCareController(ICMSCacheContent cmsCache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _cmsCache = cmsCache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Action method for bike care listing -desktop
        /// </summary>
        [Route("bikecarelanding/")]
        [Filters.DeviceDetection()]
        public ActionResult Index(ushort? pageNo)
        {
            BikeCareIndexPage obj = new BikeCareIndexPage(_cmsCache, _objPager, _upcoming, _bikeModels);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                BikeCareIndexPageVM objData = new BikeCareIndexPageVM();
                obj.TopCount = 4;
                if (pageNo.HasValue && pageNo.Value > 0)
                    obj.CurPageNo = pageNo.Value;
                objData = obj.GetData();
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Action method for bike care listing - Mobile
        /// </summary>
        [Route("m/bikecarelanding/")]
        public ActionResult Index_Mobile(ushort? pageNo)
        {
            BikeCareIndexPage obj = new BikeCareIndexPage(_cmsCache, _objPager, _upcoming, _bikeModels);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else
            {
                BikeCareIndexPageVM objData = new BikeCareIndexPageVM();
                obj.IsMobile = true;
                obj.TopCount = 9;
                if (pageNo.HasValue && pageNo.Value > 0)
                    obj.CurPageNo = pageNo.Value;
                objData = obj.GetData();
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/m/pageNotFound.aspx");
            }
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Details_Mobile()
        {
            return View();
        }
        #endregion
    }
}