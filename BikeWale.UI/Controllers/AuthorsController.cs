using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Authors;
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
using Bikewale.Models.Authors;
using Bikewale.Models.BikeModels;
using System;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthors _Authors;

        public AuthorController(IAuthors Authors)
        {
            _Authors = Authors;
        }

        // GET: Models
        [Route("authors/"), Filters.DeviceDetection]
        public ActionResult Index_List()
        {

            AuthorsListModel objAuthorsVM = new AuthorsListModel(_Authors);
            return View(objAuthorsVM.GetData());

        }

        // GET: Models
        [Route("m/authors/listing/"), Filters.DeviceDetection]
        public ActionResult Index_List_Mobile()
        {
            AuthorsListModel objAuthorsVM = new AuthorsListModel(_Authors);
            return View(objAuthorsVM.GetData());
        }

        // GET: Models
        [Route("m/authors/details/"), Filters.DeviceDetection]
        public ActionResult Details_Mobile()
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