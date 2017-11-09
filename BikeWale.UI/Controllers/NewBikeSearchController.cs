using Bikewale.Filters;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.NewBikeSearch;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 08 Nov 2017
    /// Summary: Controller for New Bike Search
    /// </summary>
    public class NewBikeSearchController : Controller
    {
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;

        public NewBikeSearchController(ICMSCacheContent articles, IVideos videos, ISearchResult searchResult, IProcessFilter processFilter)
        {
            _articles = articles;
            _videos = videos;
            _searchResult = searchResult;
            _processFilter = processFilter;
        }

        [Route("newbikesearch/")]
        [DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            string q = Request.Url.Query;
            NewBikeSearchModel model = new NewBikeSearchModel(q, _articles,_videos, _searchResult, _processFilter);
            return View(model.GetData());
        }

        [Route("m/newbikesearch/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            string q = Request.Url.Query;
            NewBikeSearchModel model = new NewBikeSearchModel(q, _articles, _videos, _searchResult, _processFilter);
            return View(model.GetData());
        }
    }
}
