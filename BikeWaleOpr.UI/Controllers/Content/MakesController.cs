using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.Notifications;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;
using Microsoft.Practices.Unity;

namespace BikeWaleOpr.MVC.UI.Controllers.Content
{
    /// <summary>
    /// Created By : Ashish G. kamble on 1 Feb 2017
    /// </summary>
    [Authorize, RoutePrefix("content")]
    public class MakesController : Controller
    {
        private readonly IBikeMakes makesRepo;

        public MakesController(IBikeMakes _makesRepo)
        {
            makesRepo = _makesRepo;
        }

        public ActionResult Index()
        {
            IEnumerable<BikeMakeEntity> objMakes = null;

            string msg = Convert.ToString(TempData["msg"]);

            try
            {
                if (!String.IsNullOrEmpty(msg))
                {
                    ViewBag.SuccessMsg = msg;
                }
                else
                {
                    ViewBag.SuccessMsg = "";
                }

                objMakes = makesRepo.GetMakesList();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MakesController/Index");
            }

            return View(objMakes);
        }

        /// <summary>
        /// Function to add the new make to the database
        /// </summary>
        /// <returns></returns>  
        [HttpPost]
        public ActionResult Add(BikeMakeEntity make)
        {            
            try
            {
                short isMakeExist = 0;
                int makeId = 0;

                make.UpdatedBy = BikeWaleOpr.Common.CurrentUser.Id;
                makesRepo.AddMake(make, ref isMakeExist, ref makeId);

                if (isMakeExist == 1)
                    TempData["msg"] = "Make name or make masking name already exists. Can not insert duplicate name.";
                else if (isMakeExist == 0 && !string.IsNullOrEmpty(make.MakeName))
                    TempData["msg"] = make.MakeName + " make added successfully";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MakesController/Add");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to update the given make details
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult Update(BikeMakeEntity make)
        {
            try
            {
                make.UpdatedBy = BikeWaleOpr.Common.CurrentUser.Id;
                makesRepo.UpdateMake(make);

                TempData["msg"] = make.MakeName + " Make Updated Successfully";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MakesController/Update");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to delete the given make by using makeid
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int makeId)
        {
            try
            {
                int updatedBy = Convert.ToInt32(BikeWaleOpr.Common.CurrentUser.Id);
                makesRepo.DeleteMake(makeId, updatedBy);

                TempData["msg"] = "Make Deleted Successfully";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MakesController/Delete");
            }

            return RedirectToAction("Index");
        }
    
    }   // class
}   // namespace