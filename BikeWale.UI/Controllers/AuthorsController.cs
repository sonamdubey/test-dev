using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Interfaces.Videos;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.BikeModels;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class AuthorController : Controller
    {
      

        // GET: Models
        [Route("m/authors/listing/"), Filters.DeviceDetection]
        public ActionResult Listing_Mobile()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }

        // GET: Models
        [Route("m/authors/details/"), Filters.DeviceDetection]
        public ActionResult Details_Mobile()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }

        // GET: Models
        [Route("authors/listing/"), Filters.DeviceDetection]
        public ActionResult Listing_Desktop()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }
        // GET: Models
        [Route("authors/details/"), Filters.DeviceDetection]
        public ActionResult Details_Desktop()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }

    }   // class
}   // namespace