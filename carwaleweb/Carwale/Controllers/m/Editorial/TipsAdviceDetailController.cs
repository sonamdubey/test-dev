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
    public class TipsAdviceDetailController : Controller
    {
        private readonly ICMSContent _cmsRepo;

        public TipsAdviceDetailController(ICMSContent cmsRepo)
        {
            _cmsRepo = cmsRepo;
        }

        [Route("m/tipsadvicedetail/{basicId}")]       
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
            return View("~/views/m/editorial/tipsadviceDetail.cshtml", contentDetailPagesDTO);
        } 
    }
}