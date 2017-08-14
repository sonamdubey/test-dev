using BikewaleOpr.Entity.ConfigurePageMetas;
using Bikewale.Notifications;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ConfigurePageMetas;
using BikewaleOpr.Models.ConfigurePageMetas;
using BikewaleOpr.Models.PageMetasConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class PageMetasConfigureController : Controller
    {
        private readonly IBikeMakesRepository _makesRepo = null;
        private readonly IConfigurePageMetasRepository _pageMetasRepo = null;

        public PageMetasConfigureController(IBikeMakesRepository makesRepo, IConfigurePageMetasRepository pageMetasRepo)
        {
            _makesRepo = makesRepo;
            _pageMetasRepo = pageMetasRepo;
        }

        // GET: PageMetasConfigure
        public ActionResult Index(uint? id)
        {
            ConfigurePageMetas objPage = new ConfigurePageMetas(_makesRepo, _pageMetasRepo, id);
            ConfigurePageMetasVM PageModel = objPage.GetData();
            return View(PageModel);
        }

        public ActionResult SaveMetas(PageMetasEntity objMetas)
        {
            try
            {
                uint pageMetaId = _pageMetasRepo.SavePageMetas(objMetas);
                return RedirectToAction("Index", new { id = pageMetaId });
            }
            catch
            {
                return null;
            }
                        
        }
        
        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Action method for landing page for page metas search.
        /// </summary>
        /// <returns></returns>
        [Route("pageMetasconfigure/search/index/")]
        public ActionResult Search_Index()
        {
            ConfigurePageMetaSearchVM objSearchData = null;
            try
            {
                PageMetasSearch objPage = new PageMetasSearch(_pageMetasRepo);
                objSearchData = objPage.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasConfiguration.Search_Index"));
            }
            return View(objSearchData);
        }
                    
        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Action method to search active or inactive or both page metas.
        /// </summary>
        /// <returns></returns>
        [Route("pageMetasconfigure/search/index/{pageMetaStatus}/")]
        public ActionResult Search_Index(ushort pageMetaStatus)
        {
            ConfigurePageMetaSearchVM objSearchData = null;
            try
            {
                PageMetasSearch objPage = new PageMetasSearch(_pageMetasRepo);
                objSearchData = objPage.GetData(pageMetaStatus);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasConfiguration.Search_Index_pageMetaStatus : {0}", pageMetaStatus));
            }
            return View(objSearchData);
        }

       
    }
}