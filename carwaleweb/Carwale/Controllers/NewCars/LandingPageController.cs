using Carwale.DTOs.LandingPage;
using Carwale.Interfaces.LandingPage;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.NewCars
{
    public class LandingPageController : Controller//Desktop
    {
        private readonly IUnityContainer _unityContainer;
        public LandingPageController(IUnityContainer container)
        {
            _unityContainer = container;
        }

        [DeviceDetectionFilter, Route("campaigns/")]
        public ActionResult Index(string id, int? modelId)
        {
            if (Carwale.Utility.RegExValidations.IsPositiveNumber(id))
            {
                int decryptedCampaignId = CustomParser.parseIntObject(LandingPageSecurity.DecryptUserId(Convert.ToUInt64(id)));

                if (decryptedCampaignId < 1)
                {
                    return HttpNotFound();
                }

                ILandingPageBL resolve = _unityContainer.Resolve<ILandingPageBL>();
                var landingPage_dto = resolve.Get(decryptedCampaignId, modelId);

                if (landingPage_dto != null && landingPage_dto.CampaignDetails != null)
                {
                    return View("~/Views/NewCar/LandingPage.cshtml", landingPage_dto);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [DeviceDetectionFilter, Route("v1/campaigns/")]
        public ActionResult GetLandingPageDetails(string id, int? modelId)
        {
            try
            {
                int decryptedCampaignId;
                try
                {
                    decryptedCampaignId = CustomParser.parseIntObject(LandingPageSecurity.DecodeFrom64(id));
                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "LandingPageController.GetLandingPageDetails : " + ex.Message);
                    objErr.LogException();
                    return HttpNotFound();
                }

                if (decryptedCampaignId < 1)
                {
                    return HttpNotFound();
                }

                ILandingPageBL resolve = _unityContainer.Resolve<ILandingPageBL>();
                var landingPage_dto = resolve.Get(decryptedCampaignId, modelId);

                if (landingPage_dto != null && landingPage_dto.CampaignDetails != null)
                {
                    return View("~/Views/NewCar/LandingPage.cshtml", landingPage_dto);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "LandingPageController.GetLandingPageDetails : " + ex.Message);
                objErr.LogException();
                return HttpNotFound();
            }
        }
    }
}