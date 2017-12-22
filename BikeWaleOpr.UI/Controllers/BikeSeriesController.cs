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
    /// Modified by: Rajan Chauhan on 12th Dec 2017
    /// Description: Added bodystyle interface in constructor
    /// </summary>
    [Authorize]
    public class BikeSeriesController : Controller
    {
        private readonly IBikeMakes _makes = null;
        private readonly IBikeSeries _series = null;
        private readonly IBikeBodyStyles _bodystyles = null;
        public BikeSeriesController(IBikeMakes makes, IBikeSeries series, IBikeBodyStyles bodystyles)
        {
            _series = series;
            _makes = makes;
            _bodystyles = bodystyles;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 11th Sep 2017
        /// Summary: UI bindings for index page
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("series/")]
        public ActionResult Index()
        {
            BikeSeriesPageModel objBikeSeriesModel = new BikeSeriesPageModel(_makes, _series, _bodystyles);
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