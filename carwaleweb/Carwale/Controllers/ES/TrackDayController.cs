using Carwale.DTOs.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
    public class TrackDayESController : Controller
    {
        private readonly ICMSContent _cmsCacheRepo;
        private readonly IPhotos _cmsPhotosCacheRepo;

        public TrackDayESController(ICMSContent cmsRepo, IPhotos cmsPhotosCacheRepo)
        {
            _cmsCacheRepo = cmsRepo;
            _cmsPhotosCacheRepo = cmsPhotosCacheRepo;
        }

        [DeviceDetectionFilter, Route("trackday2016")]
        public ActionResult Index()
        {
            return View("~/Views/ES/TrackDayLandingPage.cshtml");
        }
        [DeviceDetectionFilter, Route("trackday2016/details/{basicId}")]
        public ActionResult TrackDayDetails(ulong basicId)
        {
           
            var articleContentURI = new ArticleContentURI()
            {
                BasicId = basicId
            };
            var articlePhotoURI = new ArticlePhotoUri()
            {
                basicId = basicId
            };
            var articleByCatURI = new ArticleByCatURI()
            {
                ApplicationId = 1,
                CategoryIdList = "23",
                StartIndex = 1,
                EndIndex = 10
            };

            var contentDetailPagesDTO = new ContentDetailDTO()
            {
                Article = _cmsCacheRepo.GetContentDetails(articleContentURI),
            };
            if (contentDetailPagesDTO.Article == null)
                return new HttpStatusCodeResult(500);

            ViewBag.ArticleImages = _cmsPhotosCacheRepo.GetArticlePhotos(articlePhotoURI);
            ViewBag.RelatedArticles = _cmsCacheRepo.GetContentListByCategory(articleByCatURI);

            if (contentDetailPagesDTO.Article.Content != null)
            {
                
                return View("~/Views/ES/TrackDayDetailsPage.cshtml", contentDetailPagesDTO);
            }

            return Redirect("/trackday2016/");
        }

    }
}