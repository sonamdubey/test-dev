using Bikewale.Entities.Videos;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class NewsController : Controller
    {
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        public NewsController(ICMSCacheContent articles, IVideos videos)
        {
            _articles = articles;
            _videos = videos;
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

        /// <summary>
        /// This is a test controller
        /// </summary>
        /// <returns></returns>
        [Route("testnews/")]
        public ActionResult TestNews()
        {
            ICollection<string> videoCatList = new Collection<string>();
                videoCatList.Add(((int)EnumVideosCategory.BikeReview).ToString());
                videoCatList.Add(((int)EnumVideosCategory.Motorsports).ToString());
                videoCatList.Add(((int)EnumVideosCategory.Misc).ToString());
                videoCatList.Add(((int)EnumVideosCategory.FirstRide).ToString());
                videoCatList.Add(((int)EnumVideosCategory.LaunchAlert).ToString());
                videoCatList.Add(((int)EnumVideosCategory.TopMusic).ToString());
                videoCatList.Add(((int)EnumVideosCategory.Specials).ToString());
                videoCatList.Add(((int)EnumVideosCategory.FirstLook).ToString());
                videoCatList.Add(((int)EnumVideosCategory.Blockbuster).ToString());

            uint totalRec = 10, makeid = 1, modelid = 411;
            string makeName = "Bajaj", makemasking = "bajaj";
            string modelName = "Pulsar RS200", modelmasking = "pulsar";
            testPageModel obj = new testPageModel(_articles, _videos);
            TestVM objData = new TestVM();
            objData = obj.PopulatePageModel(totalRec, 0, 0, null, null, null, null, 1, 5,videoCatList,VideosSortOrder.FeaturedAndLatest, EnumVideosCategory.FeaturedAndLatest);
            // objData = obj.PopulatePageModel(totalRec, makeid, modelid, makeName, makemasking, modelName, modelmasking);
            return View(objData);

        }


    }
}