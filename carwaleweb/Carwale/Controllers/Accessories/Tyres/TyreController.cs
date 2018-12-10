using AutoMapper;
using Carwale.DTOs.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.UI.ClientBL;
using Carwale.UI.Filters;
using Carwale.UI.Filters.ActionFilters;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Accessories.Tyres
{
    public class TyreController : Controller
    {        
        private readonly IUnityContainer _container;
        private readonly ITyresBL _tyresBl;
        private readonly ICMSContent _cmsRepo;
        private readonly ITyresRepository _tyresRepo;
        private readonly ICarVersionCacheRepository _versionCacheRepo;
        public TyreController(IUnityContainer container, ITyresBL tyresBl, ICMSContent cmsRepo, ITyresRepository tyresRepo, ICarVersionCacheRepository versionCacheRepo)
        {
            _container = container;
            _tyresBl = tyresBl;
            _cmsRepo = cmsRepo;
            _tyresRepo = tyresRepo;
            _versionCacheRepo = versionCacheRepo;
        }

        [DeviceDetectionFilter]
        [Route("tyres/")]
        public ActionResult Index()
        {
            ushort _startIndex = 1;
            ushort _endIndex = 15;
            ushort subCategoryId = 64;
            int categoryId = 6;
            var cmsContent = new CMSContent();
            try
            {
                var contentQuery = new ArticleBySubCatURI()
                {
                    ApplicationId = (int)CMSAppId.Carwale,
                    CategoryIdList = categoryId.ToString(),
                    SubCategoryId = subCategoryId,
                    StartIndex = _startIndex,
                    EndIndex = _endIndex
                };
                cmsContent = _cmsRepo.GetContentListBySubCategory(contentQuery);
                if (cmsContent != null)
                {
                    return View("~/Views/Accessories/Tyres/Index.cshtml", cmsContent.Articles);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("~/Views/Accessories/Tyres/Index.cshtml", null);
        }

        [Route("tyres/search/")]
        public ActionResult TyreSearchList([System.Web.Http.FromUri]TyresSearchInput input)
        {
            TyresDTO listversions = new TyresDTO();
            ViewBag.Year = input.Year;
            var tyreDesktopAdapter = _container.Resolve<IServiceAdapterV2>("TyresSearchDesktop");
            if (input.CMIds != null && input.CMIds.Length > 0)
            {
                listversions = tyreDesktopAdapter.Get<TyresDTO, TyresSearchInput>(input);
            }
            return View("~/Views/Accessories/Tyres/TyreSearchList.cshtml", listversions);
        }

        [Route("tyreDetails/{itemId}/")]
        public ActionResult TyreDetails(int itemId)
        {
            ushort subCategoryId = 64;
            int categoryId = 6;
            try
            {
                if (itemId > 0)
                {
                    var contentQuery = new ArticleBySubCatURI()
                    {
                        ApplicationId = (int)CMSAppId.Carwale,
                        CategoryIdList = categoryId.ToString(),
                        SubCategoryId = subCategoryId,
                        StartIndex = 1,
                        EndIndex = 15
                    };

                    var cmsContent = _cmsRepo.GetContentListBySubCategory(contentQuery);
                    TyreDetailSummary tyreModel = new TyreDetailSummary();
                    tyreModel = _tyresBl.GetTyreDataByItemId(itemId);
                    try
                    {
                        tyreModel.Articles = cmsContent.Articles;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        tyreModel.Articles = null;
                    }
                    return View("~/Views/Accessories/Tyres/TyreDetailsPage.cshtml", tyreModel);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("tyres/");
        }

        [Route("tyres/brand/{brandName}/")]
        public ActionResult GetTyresByBrand(string brandName)
        {
            Response.AddHeader("Vary", "User-Agent");
            var brandTyresList = new VersionTyresDTO();
            var pageNo = 1;
            try
            {
                if (string.IsNullOrEmpty(brandName))
                {
                    return Redirect("/tyres/");
                }

                int brandId = _tyresRepo.GetBrandIdFromMaskingName(brandName);

                if (brandId <= 0)
                {
                    return HttpNotFound();
                }

                var isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                ViewBag.IsMobile = isMobile;
                var pageSize = isMobile ? 10 : 16;

                var tyresList = _tyresBl.GetTyresByBrandId(brandId, pageNo, pageSize);

                brandTyresList.Tyres = Mapper.Map<List<TyreSummary>, List<TyreSummaryDTO>>(tyresList.Tyres);
                brandTyresList.Count = tyresList.Count;
                ViewBag.PageNo = pageNo;

                if (isMobile) return View("~/Views/m/Accessories/Tyres/TyreMake.cshtml", brandTyresList);
                else return View("~/Views/Accessories/Tyres/TyreMake.cshtml", brandTyresList);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("/tyres/");
        }

        [Route("tyrelist/{versionId}/tyres"), ResponsiveViewHeaders]
        public ActionResult Index(int versionId, int makeYear, int pageSize)
        {
            var pageNo = 1;
            var versionTyresList = new VersionTyresList();
            try
            {
                versionTyresList = Mapper.Map<CarVersionDetails, VersionTyresList>(_versionCacheRepo.GetVersionDetailsById(versionId));
                versionTyresList.TyresList = _tyresBl.GetTyresByCarVersion(versionId, pageNo, pageSize);
                versionTyresList.MakeYear = makeYear;                                 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return (DeviceDetectionManager.IsMobile(this.HttpContext)) 
                ? PartialView("~/Views/Shared/m/Accessories/Tyres/_TyresCarousal.cshtml", versionTyresList) 
                : PartialView("~/Views/Used/CarDetailsPartials/_tyresDetail.cshtml", versionTyresList);                
        }
        [Route("new/michelin-dealers/india/"), ResponsiveViewHeaders]
        public ActionResult IndexTyre()
        {
            var isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
            ViewBag.IsMobile = isMobile;
            return View("~/Views/Accessories/Tyres/MichelinTyre.cshtml");
        }
    }
}