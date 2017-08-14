using BikewaleOpr.Entity.ConfigurePageMetas;
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


        [Route("pageMetasconfigure/search/index/")]
        public ActionResult Search_Index()
        {
        }
    }
}