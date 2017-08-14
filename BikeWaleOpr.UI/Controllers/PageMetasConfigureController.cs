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
        public ActionResult Index()
        {
            ConfigurePageMetas objPage = new ConfigurePageMetas(_makesRepo, _pageMetasRepo);
            ConfigurePageMetasVM PageModel = objPage.GetData();
            return View(PageModel);
        }


        [Route("pageMetasconfigure/search/index/")]
        public ActionResult Search_Index()
        {
            ConfigurePageMetaSearchVM objSearchData = null;
            try
            {
                ConfigurePageMetas objPage = new ConfigurePageMetas(_makesRepo, _pageMetasRepo);
                objSearchData = objPage.GetPageMetasData();
            }
            catch (Exception)
            {

                throw;
            }
            return View(objSearchData);
        }


        [Route("pageMetasconfigure/search/index/{pageMetaStatus}/")]
        public ActionResult Search_Index(ushort pageMetaStatus)
        {
            ConfigurePageMetaSearchVM objSearchData = null;
            try
            {
                ConfigurePageMetas objPage = new ConfigurePageMetas(_makesRepo, _pageMetasRepo);
                objSearchData = objPage.GetPageMetasData(pageMetaStatus);
            }
            catch (Exception)
            {

                throw;
            }
            return View(objSearchData);
        }
    }
}