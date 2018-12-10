using AutoMapper;
using Carwale.DTOs.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.m.Accessories.Tyres
{
    public class TyreListController : Controller
    {
        private readonly ITyresBL _tyresBL;

        public TyreListController(ITyresBL tyresBL)
        {
            _tyresBL = tyresBL;
        }
        [Route("tyresList/")]
        public ActionResult Index(int platformId, int pageNo=1, int pageSize = 10, int versionId = -1, string modelIds = "-1")
        {
            var versionTyresList = new VersionTyresDTO();
            try
            {
                ViewBag.IsMobile = platformId == (short)Platform.CarwaleMobile;
                ViewBag.ShowAd = pageNo == 1 ? true : false;
                if (versionId != -1)
                {
                    var tyresList = _tyresBL.GetTyresByCarVersion(versionId, pageNo, pageSize);
                    versionTyresList = Mapper.Map<VersionTyres, VersionTyresDTO>(tyresList);
                }
                else
                {
                    var modelTyres = _tyresBL.GetTyresByCarModels(modelIds, pageNo, pageSize);
                    versionTyresList.Tyres = Mapper.Map<List<TyreSummary>, List<TyreSummaryDTO>>(modelTyres.Tyres);
                    versionTyresList.Count = modelTyres.Count;
                    versionTyresList.LoadAdslot = modelTyres.LoadAdslot;
                }
                ViewBag.PageNo = pageNo;
            }
            catch(Exception err)
            {
                Logger.LogException(err);
            }
            return PartialView("m/Accessories/Tyres/_TyresList", versionTyresList);
        }

        [Route("tyresList/make/")]
        public ActionResult Index(int platformId, int brandId, int pageNo = 1, int pageSize = 10)
        {
            var brandTyresList = new VersionTyresDTO();
            try
            {
                ViewBag.IsMobile = platformId == (short)Platform.CarwaleMobile;
                ViewBag.PageNo = pageNo;
                if (brandId > 0)
                {
                    var modelTyres = _tyresBL.GetTyresByBrandId(brandId, pageNo, pageSize);
                    brandTyresList.Tyres = Mapper.Map<List<TyreSummary>, List<TyreSummaryDTO>>(modelTyres.Tyres);
                    brandTyresList.Count = modelTyres.Count;
                }                
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return PartialView("m/Accessories/Tyres/_TyresList", brandTyresList);
        }
    }
}