
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IBikeMakes _makesRepo;
        private readonly IBikeModels _modelsRepo;

        public ModelsController(IBikeMakes makesRepo, IBikeModels modelsRepo)
        {
            _makesRepo = makesRepo;
            _modelsRepo = modelsRepo;
        }

        // GET: Models
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsedModelImageUpload(uint? makeId)
        {
            try
            {
                IEnumerable<UsedBikeModelImageData> objBikeModelsImage = null;
                if (makeId.HasValue && makeId.Value > 0)
                {
                    objBikeModelsImage = _modelsRepo.GetUsedBikeModelImageByMake((uint)makeId);
                }
                else
                {
                    objBikeModelsImage = TempData["modelImageList"] as List<UsedBikeModelImageData>;
                }
                ViewBag.MakeList = _makesRepo.GetMakes("NEW");
                return View(objBikeModelsImage);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelsController/UsedModelImageUpload");
                return null;
            }
        }

        public ActionResult FetchModelsImage(uint? makeId)
        {
            try
            {
                if (makeId.HasValue && makeId.Value > 0)
                {
                    TempData["modelImageList"] = _modelsRepo.GetUsedBikeModelImageByMake((uint)makeId);
                }
                return RedirectToAction("UsedModelImageUpload");
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelsController/FetchModels");
                return null;
            }
        }
    }
}