
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
        private readonly IBikeModelsRepository _modelsRepo;

        public ModelsController(IBikeMakes makesRepo, IBikeModelsRepository modelsRepo)
        {
            _makesRepo = makesRepo;
            _modelsRepo = modelsRepo;
        }

        // GET: Models
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 09-03-2017
        /// Description : Action method for binding UsedModelImageUpload page
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ActionResult UsedModelImageUpload(uint? makeId)
        {
            try
            {
                IEnumerable<UsedBikeModelImageData> objBikeModelsImage = null;
                if (makeId.HasValue && makeId.Value > 0)
                {
                    objBikeModelsImage = _modelsRepo.GetUsedBikeModelImageByMake((uint)makeId);
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

    }
}