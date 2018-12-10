using Carwale.DTOs.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.m.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class FeaturesDetailController : Controller
    {
        private readonly ICMSContent _cmsRepo;

        public FeaturesDetailController(ICMSContent cmsRepo)
        {
            _cmsRepo = cmsRepo;
        }

        [Route("m/featuresdetail/{basicId}")]       
        public ActionResult Index(ulong basicId)
        {
            var articleContentURI = new ArticleContentURI()
            {
                BasicId = basicId
            };

            var contentDetailPagesDTO = new ContentDetailPagesDTO()
            {
                ArticlePages = _cmsRepo.GetContentPages(articleContentURI)
            };
            if (contentDetailPagesDTO.ArticlePages == null)
                return new HttpStatusCodeResult(500);
            contentDetailPagesDTO.ArticlePages.Description = Format.GetPlainTextFromHTML(contentDetailPagesDTO.ArticlePages.Description);
            return View("~/views/m/editorial/featuresdetail.cshtml", contentDetailPagesDTO);
        } 
    }
}