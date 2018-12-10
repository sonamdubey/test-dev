using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Utility;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.m.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class PitstopController : Controller
    {
        private readonly ICMSContent _cmsRepo;

        public PitstopController(ICMSContent cmsRepo)
        {
            _cmsRepo = cmsRepo;
        }

        [Route("m/pitstop")]
        public ActionResult Index()
        {
            ushort pageNo = 1;
            ushort _startIndex;
            ushort _endIndex;
            ushort pageSize = 10;
            int categoryId = 12;

            if (Request.QueryString["pn"] != null && CommonOpn.CheckId(Request.QueryString["pn"].ToString()) == true)
                UInt16.TryParse(Request.QueryString["pn"], out pageNo);

            Calculation.GetStartEndIndex(pageNo, pageSize, out _startIndex, out _endIndex);

            var contentQuery = new ArticleByCatURI()
            {
                ApplicationId = (int)CMSAppId.Carwale,
                CategoryIdList = categoryId.ToString(),
                StartIndex = _startIndex,
                EndIndex = _endIndex
            };
            var cmsContent = _cmsRepo.GetContentListByCategory(contentQuery);

            UInt16.TryParse(pageSize.ToString(), out pageSize);
            var pages = PageList.GetPages(cmsContent.RecordCount, pageSize);

            var contentListDTO = new ContentListDTO()
            {
                Articles = cmsContent.Articles.ToList(),
                SeoTags = _cmsRepo.GetContentMetaTags(categoryId),
                CMSPages = new PageListDTO() { Pages = pages, CurrentPageNo = pageNo }
            };

            return View("~/views/m/editorial/pitstop.cshtml", contentListDTO);
        }
    }
}