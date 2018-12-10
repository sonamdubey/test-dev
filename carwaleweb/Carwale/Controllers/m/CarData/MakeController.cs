using System;
using System.Web.Mvc;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using Carwale.DTOs.NewCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Utility;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.CMS;
using System.Configuration;
using Carwale.Entity.CMS.Articles;
using System.Collections.Generic;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using System.Linq;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using Carwale.DTOs;
using Carwale.UI.ViewModels.NewCars.Make;

namespace Carwale.UI.Controllers.m.CarData
{
    public class MakeController : Controller
    {
        protected short cityIdFromCookie = 0;
        protected MakePageDTO_Mobile makeDTO;
        private readonly IServiceAdapterV2 _makePageAdapter;
        private readonly ICarMakesCacheRepository _carMakeCacheRepo;
        private readonly ICarVersionCacheRepository _carVersionCacheRepo;
        private readonly ICarVersions _carVersions;

        public MakeController(Func<string, IServiceAdapterV2> adaptorFactory, ICarMakesCacheRepository carMakeCacheRepo,
            ICarVersionCacheRepository carVersionCacheRepo, ICarVersions carVersions)
        {
            try
            {
                _makePageAdapter = adaptorFactory("MakePageMobile");
                _carMakeCacheRepo = carMakeCacheRepo;
                _carVersionCacheRepo = carVersionCacheRepo;
                _carVersions = carVersions;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dependency Injection Bolck at mobile Make Controller");
            }
        }
        public ActionResult Index(int makeId = 0, bool amp = false)
        {
            try
            {
                if (makeId > 0 && _makePageAdapter != null)
                {
                    if (CookiesCustomers.MasterCityId > 0)
                    {
                        cityIdFromCookie = CustomParser.parseShortObject(CookiesCustomers.MasterCityId);

                    }
                    MakePageInputParam input = new MakePageInputParam
                    {
                        MakeId = makeId,
                        CityId = cityIdFromCookie,
                        CWCCookie = CurrentUser.CWC,
                    };
                    makeDTO = _makePageAdapter.Get<MakePageDTO_Mobile, MakePageInputParam>(input);
                    if (makeDTO != null)
                    {
                        if (cityIdFromCookie > 0)
                        {
                            City cityDetails = new City
                            {
                                CityId = cityIdFromCookie,
                                CityName = CookiesCustomers.MasterCity
                            };
                            makeDTO.CityDetails = cityDetails;
                        }
                        return View(amp ? "~/Views/m/amp/Make.cshtml" : "~/Views/m/CarData/Make.cshtml", makeDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MakeController.Index()");
            }
            return Redirect("/new/");
        }

        [Carwale.UI.Common.OutputCacheAttr("pageId", Duration = 86400)]
        public ActionResult BrandWidget()
        {
            List<CarMakeEntityBase> makeList = _carMakeCacheRepo.GetCarMakesByType("new");
            return PartialView("~/Views/Shared/m/_BrandsWidget.cshtml", makeList);
        }

        [Route("make/action/versionlist/")]
        public ActionResult VersionList(int modelId, int cityId, string makeName)
        {
            var versionViewModel = new VersionListViewModel();
            versionViewModel.Versions = _carVersions.MapCarVersionDtoWithCarVersionEntity(modelId, cityId);
            versionViewModel.MakeName = makeName;
            return PartialView("~/Views/Shared/m/Make/_VersionList.cshtml", versionViewModel);
        }
    }
}