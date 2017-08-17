using Bikewale.Notifications;
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

    /// <summary>
    /// Controller for 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    /// <author>
    /// Sangram Nandkhile on 17-Aug-2017
    /// </author>
    public class PageMetasController : Controller
    {
        private readonly IBikeMakesRepository _makesRepo = null;
        private readonly IPageMetasRepository _pageMetasRepo = null;

        public PageMetasController(IBikeMakesRepository makesRepo, IPageMetasRepository pageMetasRepo)
        {
            _makesRepo = makesRepo;
            _pageMetasRepo = pageMetasRepo;
        }

        // GET: PageMetasConfigure
        [Authorize]
        public ActionResult Index(ushort? pageMetaStatus)
        {
            ConfigurePageMetaSearchVM objSearchData = null;
            try
            {
                PageMetasSearch objPage = new PageMetasSearch(_pageMetasRepo);
                objSearchData = objPage.GetData(pageMetaStatus ?? 1);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasController.Index : {0}", pageMetaStatus));
            }
            return View(objSearchData);
        }

        public ActionResult SaveMetas(PageMetasEntity objMetas)
        {
            uint pageMetaId = 0;
            try
            {
                pageMetaId = _pageMetasRepo.SavePageMetas(objMetas);
                return RedirectToAction("Index", new { id = pageMetaId });
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasController.SaveMetas : {0}", pageMetaId));
                return null;
            }        
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Action method to search active or inactive or both page metas.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("pageMetas/add/")]
        public ActionResult Search_Index(uint? id)
        {
            ConfigurePageMetas objPage = new ConfigurePageMetas(_makesRepo, _pageMetasRepo, id);
            ConfigurePageMetasVM PageModel = objPage.GetData();
            return View(PageModel);
        }
    }
}