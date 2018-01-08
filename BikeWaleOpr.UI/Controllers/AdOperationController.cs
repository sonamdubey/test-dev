using BikewaleOpr.Interface;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Models;
using BikewaleOpr.Models.AdOperation;
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
        private readonly IBikeMakesRepository _objBikeMake = null;

        public AdOperationController(IAdOperation adOperations, IBikeMakesRepository objBikeMake)
        {
            _adOperations = adOperations;
            _objBikeMake = objBikeMake;

        }

        /// <summary>
        /// Created by : Snehal Dange on 2nd Jan 2018
        /// Description: Action created to show all promoted bike list
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(uint? makeId, uint? modelId)
        {
            AdOperationVM viewModel = new AdOperationVM();
            
            AdOperation pageModel = null;
            pageModel = new AdOperation(_adOperations, _objBikeMake);
            viewModel = pageModel.GetData();
            viewModel.MakeId = makeId ?? 0;
            viewModel.ModelId = modelId ?? 0;
            return View(viewModel);
        }

    }
}