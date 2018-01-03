using BikewaleOpr.Entity.AdOperations;
using BikewaleOpr.Interface.AdOperation;
using BikewaleOpr.Models.AdOperation;
using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Descritpion: Controller for Ad Operations(promotion  , monetization) management
    /// </summary>
    [Authorize]
    public class AdOperationController : Controller
    {
        private readonly IAdOperation _adOperations;
        AdOperation pageModel = null;
        public AdOperationController(IAdOperation adOperations)
        {
            _adOperations = adOperations;
        }

        /// <summary>
        /// Created by : Snehal Dange on 2nd Jan 2018
        /// Description: Action created to show all promoted bike list
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AdOperationVM viewModel = new AdOperationVM();
            pageModel = new AdOperation(_adOperations);
            viewModel = pageModel.GetPromotedBikes();
            return View(viewModel);
        }

        /// <summary>
        /// Created By: Snehal Dange on 2nd Jan 2018
        /// Desc : Action created to add promoted bike
        /// </summary>
        /// <param name="objPromotedBike"></param>
        /// <returns></returns>
        public ActionResult Save(PromotedBike objPromotedBike)
        {
            try
            {
                pageModel.SavePromotedBike(objPromotedBike);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "AdOperationController/Save");
            }

            return RedirectToAction("Index");
        }

    }
}