using Bikewale.Notifications;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Models.BikeSeries;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Sep 2017
    /// Summary: Controller for bike series page
    /// </summary>
    [Authorize]
    public class BikeSeriesController : Controller
    {
        private readonly IBikeMakes _makes = null;
        private readonly IBikeSeriesRepository _seriesRepo = null;
        public BikeSeriesController(IBikeMakes makes, IBikeSeriesRepository seriesRepo)
        {
            _seriesRepo = seriesRepo;
            _makes = makes;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 11th Sep 2017
        /// Summary: UI bindings for index page
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("bikeseries/")]
        public ActionResult Index()
        {
            BikeSeriesPageModel objBikeSeriesModel = new BikeSeriesPageModel(_makes, _seriesRepo);
            BikeSeriesPageVM objBikeSeriesVM = null;
            try
            {
                objBikeSeriesVM = objBikeSeriesModel.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeSeriesController: Index");
            }
            return View(objBikeSeriesVM);
        }
    }
}