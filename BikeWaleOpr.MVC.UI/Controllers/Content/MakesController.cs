using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;
using Microsoft.Practices.Unity;

namespace BikeWaleOpr.MVC.UI.Controllers.Content
{
    [Authorize, RoutePrefix("content")]
    public class MakesController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<BikeMakeEntity> objMakes = null;

            string msg = Convert.ToString(TempData["msg"]);

            if (!String.IsNullOrEmpty(msg))
            {
                ViewBag.SuccessMsg = msg;
            }
            else
            {
                ViewBag.SuccessMsg = "";
            }

            using(IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes, BikeMakesRepository>();

                IBikeMakes makesRepo = container.Resolve<IBikeMakes>();

                objMakes = makesRepo.GetMakesList();
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
            short isMakeExist = 0;
            int makeId = 0;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes, BikeMakesRepository>();

                IBikeMakes makesRepo = container.Resolve<IBikeMakes>();

                make.UpdatedBy = BikeWaleOpr.MVC.UI.common.CurrentUser.Id;
                makesRepo.AddMake(make, ref isMakeExist, ref makeId);
            }

            if (isMakeExist == 1)
                TempData["msg"] = "Make name or make masking name already exists. Can not insert duplicate name.";
            else if (isMakeExist == 0 && !string.IsNullOrEmpty(make.MakeName))
                TempData["msg"] = make.MakeName + " make added successfully";

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to update the given make details
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult Update(BikeMakeEntity make)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes, BikeMakesRepository>();

                IBikeMakes makesRepo = container.Resolve<IBikeMakes>();

                make.UpdatedBy = BikeWaleOpr.MVC.UI.common.CurrentUser.Id;
                makesRepo.UpdateMake(make);
            }

            TempData["msg"] = make.MakeName + " Make Updated Successfully";

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to delete the given make by using makeid
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int makeId)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes, BikeMakesRepository>();

                IBikeMakes makesRepo = container.Resolve<IBikeMakes>();

                int updatedBy = Convert.ToInt32(BikeWaleOpr.MVC.UI.common.CurrentUser.Id);
                makesRepo.DeleteMake(makeId, updatedBy);
            }

            TempData["msg"] = "Make Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}