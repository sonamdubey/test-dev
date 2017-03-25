using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class NewsController : Controller
    {
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        public NewsController(ICMSCacheContent articles,IVideos videos)
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

        [Route("testnews/")]
        public ActionResult TestNews()
        {
            uint totalRec=10,makeid=1,modelid=411;
            string makeName="Bajaj", makemasking="bajaj";
            string modelName="Pulsar RS200",modelmasking="pulsar";
            testPageModel obj = new testPageModel(_articles,_videos);
            TestVM objData=new TestVM();
                objData.AdTags = new AdTags();
                objData.PageMetaTags = new PageMetaTags();
                objData = obj.PopulatePageModel(totalRec, 0, 0, null, null, null, null,1,5);
               // objData = obj.PopulatePageModel(totalRec, makeid, modelid, makeName, makemasking, modelName, modelmasking);
                return View(objData);
            
        }

        
    }
}