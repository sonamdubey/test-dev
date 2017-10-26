using Bikewale.CoreDAL;
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
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        public GenericBestBikesController(IBikeModelsCacheRepository<int> objBestBikes, IBikeMakesCacheRepository bikeMakes)
        {
            _objBestBikes = objBestBikes;
            _bikeMakes = bikeMakes;
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model Index;
        /// </summary>
        [Route("bestbikes/")]
        [Filters.DeviceDetection()]
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
                    if (obj != null)
                        return View(obj);
                    else
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

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
                    objBestBikes.IsMobile = true;
                    objBestBikes.makeTopCount = 6;
                    IndexBestBikesVM obj = new IndexBestBikesVM();
                    obj = objBestBikes.GetData();
                    if (obj != null)
                        return View(obj);
                    else
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
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