using Carwale.BL.CMS;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.NewCars;
using Carwale.UI.ClientBL;
using Carwale.UI.Filters;
using Carwale.UI.PresentationLogic;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Editorial
{
    public class MThreeSixtyViewController : Controller
    {
        private readonly ICarModels _carModelsBL;
        private readonly IServiceAdapterV2 _threeSixtyViewAdapter;
        private readonly IThreeSixtyView _threeSixtyViewBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;

        public MThreeSixtyViewController(IUnityContainer container, ICarModels carModelsBL, IThreeSixtyView threeSixtyViewBL, ICarModelCacheRepository carModelsCacheRepo)
        {
            _threeSixtyViewAdapter = container.Resolve<IServiceAdapterV2>("ThreeSixtyAdapter");
            _carModelsBL = carModelsBL;
            _threeSixtyViewBL = threeSixtyViewBL;
            _carModelsCacheRepo = carModelsCacheRepo;
        }

        public ActionResult Index()
        {
            ThreeSixtyDTO threeSixtyDTO = new ThreeSixtyDTO();
            string makeName = Request.QueryString["makeName"];
            string maskingName = Request.QueryString["maskingName"];
            string category = Request.QueryString["category"];

            if (string.IsNullOrEmpty(makeName))
                return HttpNotFound();
            if (string.IsNullOrEmpty(maskingName))
                return Redirect(ManageCarUrl.CreateMakeUrl(makeName, true));

            ModelMaskingValidationEntity modelValidation = _carModelsBL.FetchModelIdFromMaskingName(maskingName, string.Empty);

            if (modelValidation.IsRedirect)
                return RedirectPermanent(string.Format("/m{0}360-view/", modelValidation.RedirectUrl));
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

                threeSixtyDTO.ModelDetails = modelDetails;
                if (CMSCommon.CheckCategoryAvailable(modelDetails, enumCategory))
                {
                    threeSixtyDTO = _threeSixtyViewAdapter.Get<ThreeSixtyDTO, Tuple<CarModelDetails, ThreeSixtyViewCategory>>(new Tuple<CarModelDetails, ThreeSixtyViewCategory>(modelDetails, enumCategory));
                    return View("~/Views/m/Editorial/360View.cshtml", threeSixtyDTO);
                }

                enumCategory = CMSCommon.Get360DefaultCategory(AutoMapper.Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDetails));

                return Redirect(EditorialContent.Get360PageUrl(makeName, maskingName, enumCategory, true));
            }
        }

        //[Carwale.UI.Common.OutputCacheAttr("modelDetails.modelId;category;isMsite")]
        public ActionResult ThreeSixtySlug(CarModelDetails modelDetails, ThreeSixtyViewCategory category, bool isMsite = true)
        {
            var threeSixtyDTO = new ThreeSixtyDTO {
                ModelDetails = modelDetails,
                ActiveState = category != null ? category.ToString() : ThreeSixtyViewCategory.Closed.ToString(),
                Category = category
            };
            return View("~/Views/Shared/m/Editorial/_ThreeSixtySlug.cshtml", new Tuple<ThreeSixtyDTO, bool>(threeSixtyDTO, isMsite));
        }
    }
}