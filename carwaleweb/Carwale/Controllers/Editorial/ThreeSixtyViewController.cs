using Carwale.BL.CMS;
using Carwale.BL.NewCars;
using Carwale.Cache.CarData;
using Carwale.DTOs.CMS;
using Carwale.DTOs.NewCars;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Carwale.BL.CMS.Photos;
using Carwale.UI.PresentationLogic;
using Carwale.Entity.Enum;
using System;
using Carwale.DTOs.CMS.ThreeSixtyView;

namespace Carwale.UI.Controllers.Editorial
{
    public class ThreeSixtyViewController : Controller
    {
        private readonly ICarModels _carModelsBL;
        private readonly IServiceAdapterV2 _threeSixtyViewAdapter;
        private readonly IThreeSixtyView _threeSixtyViewBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;

        public ThreeSixtyViewController(IUnityContainer container, ICarModels carModelsBL, IThreeSixtyView threeSixtyViewBL, ICarModelCacheRepository carModelsCacheRepo)
        {
            _threeSixtyViewAdapter = container.Resolve<IServiceAdapterV2>("ThreeSixtyAdapter");
            _carModelsBL = carModelsBL;
            _threeSixtyViewBL = threeSixtyViewBL;
            _carModelsCacheRepo = carModelsCacheRepo;
        }

        [DeviceDetectionFilter]
        public ActionResult Index()
        {
            ThreeSixtyDTO threeSixtyDTO = new ThreeSixtyDTO();
            string makeName = Request.QueryString["makeName"];
            string maskingName = Request.QueryString["maskingName"];
            string category = Request.QueryString["category"];

            if (string.IsNullOrEmpty(makeName))
                return HttpNotFound();
            if (string.IsNullOrEmpty(maskingName))
                return Redirect(ManageCarUrl.CreateMakeUrl(makeName, false));

            ModelMaskingValidationEntity modelValidation = _carModelsBL.FetchModelIdFromMaskingName(maskingName, string.Empty);

            if (modelValidation.IsRedirect)
                return RedirectPermanent(modelValidation.RedirectUrl + "360-view/");
            if (!modelValidation.IsValid)
                return HttpNotFound();

            CarModelDetails modelDetails = _carModelsCacheRepo.GetModelDetailsById(modelValidation.ModelId);

            if (!CMSCommon.IsThreeSixtyViewAvailable(modelDetails))
                return Redirect(ManageCarUrl.CreateModelUrl(makeName, maskingName));
            else
            {
                ThreeSixtyViewCategory enumCategory;
                category = category ?? "closed";
                Enum.TryParse<ThreeSixtyViewCategory>(category, true, out enumCategory);
                ViewBag.PhotoCount = modelDetails.PhotoCount;

                threeSixtyDTO.ModelDetails = modelDetails;
                if (CMSCommon.CheckCategoryAvailable(modelDetails, enumCategory))
                {
                    threeSixtyDTO = _threeSixtyViewAdapter.Get<ThreeSixtyDTO, Tuple<CarModelDetails, ThreeSixtyViewCategory>>(new Tuple<CarModelDetails, ThreeSixtyViewCategory>(modelDetails, enumCategory));
                    return View("~/Views/Editorial/360View.cshtml", threeSixtyDTO);
                }

                enumCategory = CMSCommon.Get360DefaultCategory(AutoMapper.Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDetails));

                return Redirect(EditorialContent.Get360PageUrl(makeName, maskingName, enumCategory, false));
            }
        }
    }
}