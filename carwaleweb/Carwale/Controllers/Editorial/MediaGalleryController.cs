using Carwale.BL.CMS;
using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Editorial
{
    public class MediaGalleryController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IServiceAdapterV2 _galleryAdapter;
        private readonly ICarModels _carModelBl;

        public MediaGalleryController(IUnityContainer container, ICarModels carModelBl)
        {
            _unityContainer = container;
            _galleryAdapter = container.Resolve<IServiceAdapterV2>("DesktopGalleryAdapter");
            _carModelBl = carModelBl;
        }

        [DeviceDetectionFilter]
        public ActionResult Index()
        {
            bool isPartial = false;
            bool isVideosPage = false;
            PhotoGalleryDTO_V2 galleryDTO = null;
            ModelMaskingValidationEntity cmr = null;
            try
            {
                isPartial = Request.QueryString["isPartial"] != null && Request.QueryString["isPartial"] == "true";
                isVideosPage = Request.QueryString["cat"] != null && Request.QueryString["cat"] == "videos";

                string modelMaskingName = Request["model"] != null ? Request.QueryString["model"] : string.Empty;
                int basicId = Request["basicId"] != null ? Convert.ToInt32(Request.QueryString["basicId"]) : 0;

                if (String.IsNullOrEmpty(modelMaskingName) && basicId <= 0)
                    return Redirect(CMSCommon.GetImageUrl(null, null));

                if(!String.IsNullOrEmpty(modelMaskingName))
                    cmr = _carModelBl.FetchModelIdFromMaskingName(modelMaskingName, string.Empty);

                if (cmr != null)
                {
                    if (!cmr.IsValid)
                    {
                        return RedirectPermanent(isVideosPage ? CMSCommon.GetVideoUrl(null, null, null, 0, false) : CMSCommon.GetImageUrl(null, null));
                    }
                    else if (cmr.IsRedirect)
                    {
                        return RedirectPermanent(cmr.RedirectUrl + Regex.Replace(Request.RawUrl, @"\/.*-cars\/[^\/]*\/", ""));
                    }
                }

                Dictionary<string, string> queryString = Request.QueryString.Keys.Cast<string>().ToDictionary(key => key, value => Request.QueryString[value]);
                
                queryString.Add("modelId", cmr == null ? "0" : cmr.ModelId.ToString());
                
                galleryDTO = _galleryAdapter.Get<PhotoGalleryDTO_V2, Dictionary<string, string>>(queryString);

                if (galleryDTO == null || (galleryDTO.GalleryState.ActiveFilter == GalleryFilters.Colors && !galleryDTO.ShowModelColors))
                    return Redirect(Regex.Replace(Request.RawUrl, @"\/colours\/", "/"));

                if (isVideosPage)
                {
                    if (galleryDTO.ModelVideos == null || galleryDTO.ModelVideos.Count == 0)
                        return Redirect(CMSCommon.GetVideoUrl(null, null, null, 0, false));
                    
                    return View("~/Views/Editorial/Videos.cshtml", galleryDTO);
                }

                if (galleryDTO.GalleryState.ActiveFilter != GalleryFilters.Colors && (galleryDTO.ModelImages == null || galleryDTO.ModelImages.Count == 0))
                    return Redirect(CMSCommon.GetImageUrl(null, null));

                if (galleryDTO.GalleryState.ActiveSlideIndex < 0)
                    return RedirectPermanent(CMSCommon.GetImageUrl(galleryDTO.ModelDetails.MakeName, galleryDTO.ModelDetails.MaskingName));
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                return Redirect(CMSCommon.GetImageUrl(null, null));
            }

            if (isPartial && galleryDTO.GalleryState.ActiveFilter == GalleryFilters.Colors)
                return PartialView("~/Views/Shared/Editorial/_ColoursList.cshtml", galleryDTO);

            if (isPartial)
                return PartialView("~/Views/Shared/Editorial/_ImageList.cshtml", galleryDTO);

            return View("~/Views/Editorial/Gallery.cshtml", galleryDTO);
        }
    }
}