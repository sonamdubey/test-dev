using Bikewale.Entities;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Models.Authors;
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

        /// <summary>
        /// Created by: Ashutosh Sharma on 20-Sep-2017
        /// Description : Action method for Author List desktop page.
        /// </summary>
        /// <returns></returns>
        [Route("authors/"), Filters.DeviceDetection]
        public ActionResult Index_List()
        {

            AuthorsListModel objAuthorsVM = new AuthorsListModel(_authors, _articles);
            return View(objAuthorsVM.GetData());

        }

        /// <summary>
        /// Created by: Ashutosh Sharma on 20-Sep-2017
        /// Description : Action method for Author List mobile page.
        /// </summary>
        /// <returns></returns>
        [Route("m/authors/")]
        public ActionResult Index_List_Mobile()
        {
            AuthorsListModel objAuthorsVM = new AuthorsListModel(_authors, _articles);
            return View(objAuthorsVM.GetData());
        }

        /// <summary>
        /// Created by : Vivek Singh on 20-Sep-2017
        /// Description : Action method for Author details desktop page.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [Route("authors/{author}/"), Filters.DeviceDetection]
        public ActionResult Details(string author)
        {
            AuthorDetailsPageVM objAuthorDetailsVM = null;
            AuthorsDetailsPageModel objAuthorModel = new AuthorsDetailsPageModel(_authors, _articles, _authorsCacheRepository, author);
            if (objAuthorModel.status.Equals(StatusCodes.ContentFound))
            {
                objAuthorDetailsVM = objAuthorModel.GetData();
            }
            else
            {
                return HttpNotFound();
            }
            return View(objAuthorDetailsVM);
        }

        /// <summary>
        /// Created by : Vivek Singh on 20-Sep-2017
        /// Description : Action method for Author details mobile page.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
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
                return HttpNotFound();
            }
            return View(objAuthorDetailsVM);
        }
    }
}