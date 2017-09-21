using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities;
using Bikewale.Entities.Authors;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Models.Authors;
using Bikewale.Models.BikeModels;
using Grpc.CMS;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthors _authors = null;
        private readonly IArticles _articles = null;
        private readonly IAuthorsCacheRepository _authorsCacheRepository = null;
        public AuthorController(IAuthors authors, IArticles articles, IAuthorsCacheRepository authorsCacheRepository)
        {
            _authors = authors;
            _articles = articles;
            _authorsCacheRepository = authorsCacheRepository;
        }

        [Route("authors/"), Filters.DeviceDetection]
        public ActionResult Index_List()
        {

            AuthorsListModel objAuthorsVM = new AuthorsListModel(_authors, _articles);
            return View(objAuthorsVM.GetData());

        }

        [Route("m/authors/"), Filters.DeviceDetection]
        public ActionResult Index_List_Mobile()
        {
            AuthorsListModel objAuthorsVM = new AuthorsListModel(_authors, _articles);
            return View(objAuthorsVM.GetData());
        }

        [Route("authors/{author}/")]
        public ActionResult Details(string author)
        {
            AuthorDetailsPageVM objAuthorDetailsVM = null;
            AuthorsDetailsPageModel objAuthorModel = new AuthorsDetailsPageModel(_authors, _articles, _authorsCacheRepository, author);
            if(objAuthorModel.status.Equals(StatusCodes.ContentFound))
            {
                objAuthorDetailsVM = objAuthorModel.GetData();
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
            return View(objAuthorDetailsVM);
        }

        [Route("m/authors/{author}/")]
        public ActionResult Details_Mobile(string author)
        {
            AuthorDetailsPageVM objAuthorDetailsVM = null;
            AuthorsDetailsPageModel objAuthorModel = new AuthorsDetailsPageModel(_authors, _articles, _authorsCacheRepository, author);
            if (objAuthorModel.status.Equals(StatusCodes.ContentFound))
            {
                objAuthorDetailsVM = objAuthorModel.GetData();
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
            return View(objAuthorDetailsVM);
        }
    }        
}