using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By :- Subodh Jain 18 May 2017
    /// Summary :- Generic Bike Model Controller;
    /// </summary>
    public class GenericBestBikesController : Controller
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        public GenericBestBikesController(IBikeModelsCacheRepository<int> objBestBikes, IBikeMakes<BikeMakeEntity, int> bikeMakes)
        {
            _objBestBikes = objBestBikes;
            _bikeMakes = bikeMakes;
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model Index;
        /// </summary>
        [Route("bestbikes/")]
        public ActionResult Index()
        {
            IndexGenericBestBikes objBestBikes = new IndexGenericBestBikes(_objBestBikes, _bikeMakes);

            if (objBestBikes != null)
            {

                if (objBestBikes.status == Entities.StatusCodes.ContentFound)
                {
                    IndexBestBikesVM obj = new IndexBestBikesVM();
                    objBestBikes.makeTopCount = 10;
                    obj = objBestBikes.GetData();
                    return View(obj);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }

            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

            }


        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model Index_Mobile;
        /// </summary>
        [Route("m/bestbikes/")]
        public ActionResult Index_Mobile()
        {
            IndexGenericBestBikes objBestBikes = new IndexGenericBestBikes(_objBestBikes, _bikeMakes);

            if (objBestBikes != null)
            {

                if (objBestBikes.status == Entities.StatusCodes.ContentFound)
                {
                    objBestBikes.makeTopCount = 6;
                    IndexBestBikesVM obj = new IndexBestBikesVM();
                    obj = objBestBikes.GetData();
                    return View(obj);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }

            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

            }


        }
    }
}