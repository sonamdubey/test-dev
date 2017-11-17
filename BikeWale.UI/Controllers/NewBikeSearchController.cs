using Bikewale.Entities.PriceQuote;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData;
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
        private readonly IBikeMakesCacheRepository _makes;
        private readonly IBikeSearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;

        public NewBikeSearchController(ICMSCacheContent articles, IVideos videos, IBikeMakesCacheRepository makes, IBikeSearchResult searchResult, IProcessFilter processFilter)
        {
            _makes = makes;
            _articles = articles;
            _videos = videos;
            _searchResult = searchResult;
            _processFilter = processFilter;
        }

        [Route("newbikesearch/")]
        [DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            NewBikeSearchModel model = new NewBikeSearchModel(Request, _articles, _videos, _makes, _searchResult, _processFilter, PQSourceEnum.Desktop_NewBikeSearch);
            model.PageSize = 30;
            model.EditorialTopCount = 3;
            model.BaseUrl = "/new/bike-search/";
            return View(model.GetData());
        }

        [Route("m/newbikesearch/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            NewBikeSearchModel model = new NewBikeSearchModel(Request, _articles, _videos, _makes, _searchResult, _processFilter, PQSourceEnum.Mobile_NewBikeSearch);
            model.PageSize = 30;
            model.EditorialTopCount = 3;
            model.IsMobile = true;
            model.BaseUrl = "/m/new/bike-search/";
            return View(model.GetData());
        }
    }
}
