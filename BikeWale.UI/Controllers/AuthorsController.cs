using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.Authors;
using Bikewale.Interfaces.Authors;
using Bikewale.Models.Authors;
using Bikewale.Models.BikeModels;
using Grpc.CMS;
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

        [Route("authors/"), Filters.DeviceDetection]
        public ActionResult Index_List()
        {

            AuthorsListModel objAuthorsVM = new AuthorsListModel(_Authors);
            return View(objAuthorsVM.GetData());

        }

        [Route("m/authors/listing/"), Filters.DeviceDetection]
        public ActionResult Index_List_Mobile()
        {
            AuthorsListModel objAuthorsVM = new AuthorsListModel(_Authors);
            return View(objAuthorsVM.GetData());
        }

        [Route("authors/{author}/details")]
        public ActionResult Details()
        {
            ModelPageVM obj = new ModelPageVM();
            var data = GrpcMethods.GetAuthorDetails(148);
            AuthorEntity objAuthorDetails = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(data);
            return View(obj);
        }

        [Route("m/authors/{author}/details/")]
        public ActionResult Details_Mobile()
        {
            ModelPageVM obj = new ModelPageVM();
            var data = GrpcMethods.GetAuthorDetails(148);
            AuthorEntity objAuthorDetails = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(data);
            return View(obj);
        }
    }        
}