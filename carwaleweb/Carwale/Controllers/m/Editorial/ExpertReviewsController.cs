using Carwale.DTOs.CMS;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Utility;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.m.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class ExpertReviewsController : Controller
    {
        private readonly ICMSContent _cmsRepo;
        private readonly ICarMakesCacheRepository _carMakeRepo;
        private readonly ICarModelCacheRepository _carModelCache;
        public ExpertReviewsController(ICMSContent cmsRepo, ICarMakesCacheRepository carMakeRepo, ICarModelCacheRepository carModelCache)
        {
            _cmsRepo = cmsRepo;
            _carMakeRepo = carMakeRepo;
            _carModelCache = carModelCache;
        }

        [Route("m/expertreviews")]
        public ActionResult Index()
        {
            ushort pageNo = 1;
            ushort _startIndex;
            ushort _endIndex;
            ushort pageSize = 10;
            int makeId = 0;
            int modelId = 0;
            int categoryId = 8;
            string maskingName = string.Empty;
            string makeName = string.Empty, modelName = string.Empty,canonical = string.Empty;

            NameValueCollection Qs = Request.QueryString;

            if (Qs["pn"] != null && CommonOpn.CheckId(Qs["pn"]) == true)
                UInt16.TryParse(Qs["pn"], out pageNo);

            if (Qs["makeId"] != null && CommonOpn.CheckId(Qs["makeId"]) == true)
                Int32.TryParse(Qs["makeId"], out makeId);

            if (!string.IsNullOrWhiteSpace(Qs["makeName"]))
                makeName = Qs["makeName"];

            if (makeId > 0 && !string.IsNullOrWhiteSpace(Qs["modelId"]))
            {
                maskingName = Qs["modelId"];
                modelId = GetModelIdFromMaskingName(maskingName);
            }

            if (!string.IsNullOrWhiteSpace(Qs["modelName"]))
                modelName = Qs["modelName"];

            ViewBag.MakeId = makeId;
            ViewBag.ModelId = modelId;
            ViewBag.CarMakes = _carMakeRepo.GetCarMakesByType("RoadTest");

            if (makeName != "" && modelName != "")//append make and model name both
            {
                canonical = makeName + "-cars/" + modelName+"/expert-reviews";
            }
            else if (makeName != "" && modelName == "")//append make name only
            {
                canonical = makeName + "-cars/expert-reviews";
            }
            else if (makeName == "" && modelName == "")//no make and model name
            {
                canonical = "expert-reviews";
            }

            if (pageNo > 1)//append page number if on the second page or so
            {
                canonical += "/p" + pageNo;
            }
            ViewBag.CanonicalString = canonical;

            Calculation.GetStartEndIndex(pageNo, pageSize, out _startIndex, out _endIndex);

            var contentQuery = new ArticleByCatURI()
            {
                ApplicationId = (int)CMSAppId.Carwale,
                CategoryIdList = "2,8",
                StartIndex = _startIndex,
                EndIndex = _endIndex,
                MakeId = makeId,
                ModelId = modelId
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

            return View("~/views/m/editorial/expertreviews.cshtml", contentListDTO);
        }

        private int GetModelIdFromMaskingName(string maskingName)
        {
            var cmr = _carModelCache.GetModelByMaskingName(maskingName);

            if (cmr.Redirect)
            {
                if (cmr.MakeId != -1)
                {
                    Response.Redirect(ManageCarUrl.CreateExpertReviewUrl(cmr.MakeName, cmr.MaskingName,true), false);
                    Response.StatusCode = 301;
                    Response.End();
                }
                else
                    HttpNotFound();
            }

            return cmr.ModelId;
        }
    }
}