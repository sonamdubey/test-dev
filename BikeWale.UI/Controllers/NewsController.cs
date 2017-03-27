using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class NewsController : Controller
    {
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        public NewsController(ICMSCacheContent articles, IVideos videos, IBikeModelsCacheRepository<int> models)
        {
            _articles = articles;
            _videos = videos;
            _models = models;
        }

        [Route("news/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("m/news/")]
        public ActionResult Index_Mobile()
        {
            return View();
        }

        [Route("news/{basicid}/")]
        public ActionResult Details(string basicid)
        {
            return View();
        }

        [Route("m/news/{basicid}/")]
        public ActionResult Details_Mobile(string basicid)
        {
            return View();
        }

    }
}