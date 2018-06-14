using BikewaleOpr.Entity.ServiceCenter;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ServiceCenter;
using BikewaleOpr.Models.ServiceCenter;
using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>		
    /// Created By:-Snehal Dange 28 July 2017		
    /// Summary:- For service center details		
    /// </summary>

    public class ServiceCenterController : Controller
    {
        private string _updatedBy = CurrentUser.Id;
        private readonly IServiceCenter _serviceCenter = null;
        private readonly IBikeMakes _makes = null;
        public ServiceCenterController(IServiceCenter serviceCenter, IBikeMakes makes)
        {
            _serviceCenter = serviceCenter;
            _makes = makes;
        }

        /// <summary>		
        /// Created By:-Snehal Dange 28 July 2017		
        /// Summary:- This methods runs on page load . View Model contains list of all Makes and cities. 		
        /// </summary>		
        /// <returns></returns>		

        [Route("servicecenter/search/")]

        public ActionResult SearchServiceCenter()
        {
            ServiceCenterPageVM pageVM = null;
            ServiceCenterPageModel pageModel = new ServiceCenterPageModel(_serviceCenter, _makes);
            if (pageModel != null)
            {
                pageVM = pageModel.GetData();
                return View(pageVM);
            }
            else
            {
                return HttpNotFound();
            }
        }


        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- AddServiceCenter() method runs when user selects Make and City to add service Center details.		
        /// Return the form with state and city name		
        /// </summary>		
        /// <param name="cityId"></param>		
        /// <param name="makeId"></param>		
        /// <param name="makeName"></param>		
        /// <param name="cityName"></param>		
        /// <returns></returns>		

        [Route("servicecenter/details/make/{makeId}/{makeName}/city/{cityId}/{cityName}/")]
        public ActionResult AddServiceCenter(uint cityId, uint makeId, string makeName, string cityName)
        {
            ServiceCenterCompleteDataVM objDataModel = null;
            ServiceCenterPageModel pageModel = new ServiceCenterPageModel(_serviceCenter);
            if (cityId > 0 && makeId > 0)
            {

                objDataModel = pageModel.GetFormData(cityId, makeId, makeName);
            }


            return View(objDataModel);
        }



        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- SaveServiceCenter() method saves the Service center details .		
        /// </summary>		
        /// <param name="objData"></param>		
        /// <returns></returns>		
        [Route("servicecenter/save/details/")]
        public ActionResult SaveServiceCenter([System.Web.Http.FromBody] ServiceCenterCompleteData objData)
        {

            try
            {
                if (objData != null)
                {
                    _serviceCenter.AddUpdateServiceCenter(objData, _updatedBy);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterController.SaveServiceCenter");
            }

            return RedirectToAction("SearchServiceCenter");
        }


        /// <summary>		
        /// Created By:-Snehal Dange 29 July 2017		
        /// Summary:- This is called when user clicks on edit icon. Particular service center details are retrived from database and 		
        /// bind with the ServiceCenterCompleteDataVM  model		
        /// </summary>		
        /// <param name="Id"></param>		
        /// <returns></returns>		
        [Route("servicecenter/editdetails/{Id}/")]
        public ActionResult EditServiceCenter(uint Id)
        {

            ServiceCenterCompleteDataVM objDataModel = new ServiceCenterCompleteDataVM();
            ServiceCenterPageModel pageModel = new ServiceCenterPageModel(_serviceCenter);
            try
            {
                if (Id > 0)
                {
                    objDataModel.details = pageModel.GetServiceCenterDetailsById(Id);

                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterController.EditServiceCenter");
            }
            return View("~/Views/ServiceCenter/AddServiceCenter.cshtml", objDataModel);

        }
    }
}