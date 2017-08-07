﻿
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    [Authorize]
    public class ModelsController : Controller
    {
        private readonly IBikeMakesRepository _makesRepo;
        private readonly IBikeModelsRepository _modelsRepo;

        public ModelsController(IBikeMakesRepository makesRepo, IBikeModelsRepository modelsRepo)
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
                    objBikeModelsImage = _modelsRepo.GetUsedBikeModelImageByMake(makeId.Value);
                }

                ViewBag.MakeList = _makesRepo.GetMakes("ALL");
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