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
        private readonly IBikeSeries _series = null;
        public BikeSeriesController(IBikeMakes makes, IBikeSeries series)
        {
            _series = series;
            _makes = makes;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 11th Sep 2017
        /// Summary: UI bindings for index page
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("series/")]
        public ActionResult Index()
        {
            BikeSeriesPageModel objBikeSeriesModel = new BikeSeriesPageModel(_makes, _series);
            BikeSeriesPageVM objBikeSeriesVM = null;
            try
            {
                objBikeSeriesVM = objBikeSeriesModel.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeSeriesController: Index");
            }
            return View(objBikeSeriesVM);
        }
    }
}