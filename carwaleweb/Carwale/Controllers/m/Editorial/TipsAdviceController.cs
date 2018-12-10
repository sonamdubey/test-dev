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
    public class TipsAdviceController : Controller
    {
        private readonly ICMSContent _cmsRepo;

        public TipsAdviceController(ICMSContent cmsRepo)
        {
            _cmsRepo = cmsRepo;
        }

        [Route("m/tipsadvice")]
        public ActionResult Index()
        {
            ushort pageNo = 1;
            ushort _startIndex;
            ushort _endIndex;
            ushort pageSize = 10;
            ushort subCategoryId = 0;
            int categoryId = 5;

            if (Request.QueryString["pn"] != null && CommonOpn.CheckId(Request.QueryString["pn"].ToString()) == true)
                UInt16.TryParse(Request.QueryString["pn"], out pageNo);

            if (Request.QueryString["subCatId"] != null && CommonOpn.CheckId(Request.QueryString["subCatId"].ToString().Trim()) == true)
                UInt16.TryParse(Request.QueryString["subCatId"], out subCategoryId);

            ViewBag.SubCategoryId = subCategoryId;

            Calculation.GetStartEndIndex(pageNo, pageSize, out _startIndex, out _endIndex);

            var contentQuery = new ArticleBySubCatURI()
            {
                ApplicationId = (int)CMSAppId.Carwale,
                CategoryIdList = categoryId.ToString(),
                SubCategoryId = subCategoryId,
                StartIndex = _startIndex,
                EndIndex = _endIndex
            };

            var cmsContent = _cmsRepo.GetContentListBySubCategory(contentQuery);
            
            var metaTags = _cmsRepo.GetContentMetaTags(categoryId);
            
            var pages = PageList.GetPages(cmsContent.RecordCount, Convert.ToInt16(pageSize));
            
            var cmsPages = new PageListDTO()
            {
                Pages = pages,
                CurrentPageNo = pageNo
            };

            var contentListDTO = new ContentListDTO()
            {
                Articles = cmsContent.Articles.ToList(),
                SeoTags = metaTags,
                CMSPages = cmsPages
            };

            ViewBag.SubCategories = _cmsRepo.GetCMSSubCategories(categoryId);

            return View("~/views/m/editorial/tipsadvice.cshtml", contentListDTO);
        }
    }
}