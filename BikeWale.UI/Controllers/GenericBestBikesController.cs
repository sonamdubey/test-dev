using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.CoreDAL;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
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
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public GenericBestBikesController(IBikeModelsCacheRepository<int> objBestBikes, IBikeMakesCacheRepository bikeMakes, ICMSCacheContent objArticles, IApiGatewayCaller apiGatewayCaller)
        {
            _objBestBikes = objBestBikes;
            _bikeMakes = bikeMakes;
            _objArticles = objArticles;
            _apiGatewayCaller = apiGatewayCaller;
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model Index;
        /// </summary>
        [Route("bestbikes/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            IndexGenericBestBikes objBestBikes = new IndexGenericBestBikes(_objBestBikes, _bikeMakes, _objArticles, _apiGatewayCaller);

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
                        return HttpNotFound();

                }
                else
                {
                    return HttpNotFound();
                }

            }
            else
            {
                return HttpNotFound();

            }


        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model Index_Mobile;
        /// </summary>
        [Route("m/bestbikes/")]
        public ActionResult Index_Mobile()
        {
            IndexGenericBestBikes objBestBikes = new IndexGenericBestBikes(_objBestBikes, _bikeMakes, _objArticles, _apiGatewayCaller);

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
                        return HttpNotFound();
                }
                else
                {
                    return HttpNotFound();
                }

            }
            else
            {
                return HttpNotFound();

            }


        }
    }
}