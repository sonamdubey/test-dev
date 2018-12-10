using Carwale.DTOs.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.UI.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.m.Accessories.Tyres
{
    public class TyreController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ITyresBL _tyresBl;
        private readonly ICMSContent _cmsRepo;
        public TyreController(IUnityContainer container, ITyresBL tyresBl, ICMSContent cmsRepo)
        {
            _container = container;
            _tyresBl = tyresBl;
            _cmsRepo = cmsRepo;
        }

        [Route("m/tyres/")]
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
                    return View("~/Views/m/Accessories/Tyres/Index.cshtml", cmsContent.Articles);
                }
            }
            catch(Exception ex )
            {
                Logger.LogException(ex);
            }
            return View("~/Views/m/Accessories/Tyres/Index.cshtml", null);
        }

        [Route("m/tyres/search/")]
        public ActionResult TyreSearchList([System.Web.Http.FromUri]TyresSearchInput input) 
        {
            TyresDTO listversions = new TyresDTO();
            ViewBag.Year = input.Year;
            var tyresMobileAdapter = _container.Resolve<IServiceAdapterV2>("TyresSearchMobile");
            if (input.CMIds != null && input.CMIds.Length > 0)
            {
                listversions = tyresMobileAdapter.Get<TyresDTO, TyresSearchInput>(input);
            }
            return View("~/Views/m/Accessories/Tyres/MTyreSearchList.cshtml", listversions);
        }

        [Route("m/tyreDetails/{itemId}/")]
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
                    catch(Exception ex)
                    {
                        Logger.LogException(ex);
                        tyreModel.Articles = null;
                    }
                    return View("~/Views/m/Accessories/Tyres/MTyreDetailsPage.cshtml", tyreModel);
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("m/tyres/");
        }
    }
}