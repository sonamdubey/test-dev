using Carwale.DTOs.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.m.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class PitstopDetailController : Controller
    {
        private readonly ICMSContent _cmsRepo;

        public PitstopDetailController(ICMSContent cmsRepo)
        {
            _cmsRepo = cmsRepo;
        }

        [Route("m/pitstopdetail/{basicId:int}")]       
        public ActionResult Index(ulong basicId)
        {
            var articleContentURI = new ArticleContentURI()
            {
                BasicId = basicId
            };

            var contentDetailPagesDTO = new ContentDetailDTO()
            {
                Article = _cmsRepo.GetContentDetails(articleContentURI)
            };
            if (contentDetailPagesDTO.Article == null)
                return new HttpStatusCodeResult(500);
            ViewBag.isRelatedArticle = false;
            return View("~/views/m/editorial/pitstopdetail.cshtml", contentDetailPagesDTO);
        } 
    }
}