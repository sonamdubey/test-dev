using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities;
using Bikewale.Entities.Authors;
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
using Grpc.CMS;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class AuthorController : Controller
    {
        [Route("authors/{author}/details/")]
        public ActionResult Details(string author)
        {
            ModelPageVM obj = new ModelPageVM();
            var data = GrpcMethods.GetAuthorDetails(148);
            AuthorEntity objAuthorDetails = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(data);
            return View(obj);
        }

        [Route("m/authors/{author}/details/")]
        public ActionResult Mobile_Details(string author)
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }

    }
}