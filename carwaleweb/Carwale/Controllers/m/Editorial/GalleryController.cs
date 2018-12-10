using Carwale.BL.Deals;
using Carwale.BL.PriceQuote;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Media;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.SessionState;
using Carwale.Interfaces.NewCars;
using Carwale.DTOs.CMS;
using Carwale.Utility;
using System.Linq;
using Carwale.UI.PresentationLogic;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Campaigns;
using Carwale.BL.CMS;

namespace Carwale.UI.Controllers.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class GalleryController : Controller
    {
        private readonly LeadAndInquirySource _leadAndInquirySource;
        private readonly IPriceQuoteBL _priceQuoteBL;
        private readonly IServiceAdapterV2 _galleryAdapter;
        private readonly IDeals _deals;
        private readonly IMediaBL _mediaBL;
        private readonly ICampaign _campaign;
        private readonly ICarModels _carModelBl;
        private readonly IUnityContainer _container;

        public GalleryController(IUnityContainer container, ICampaign campaign, LeadAndInquirySource leadAndInquirySource, IPriceQuoteBL priceQuoteBL, DealsBL deals, IMediaBL mediaBL, ICarModels carModelBl)
        {
            _leadAndInquirySource = leadAndInquirySource;
            _priceQuoteBL = priceQuoteBL;
            _deals = deals;
            _mediaBL = mediaBL;
            _campaign = campaign;
            _carModelBl = carModelBl;
            _container = container;
            _galleryAdapter = container.Resolve<IServiceAdapterV2>("MobileGalleryAdapter");
        }

        public ActionResult Index()
        {
            string imageLanding = CMSCommon.GetImageUrl(null, null, null, 0, true);
            try
            {
                if (this.HttpContext.Request.QueryString["model"] != null || this.HttpContext.Request.QueryString["videoName"] != null)
                {
                    string model = this.HttpContext.Request.QueryString["model"];
                    ModelMaskingValidationEntity cmr = null;
                    if(!string.IsNullOrEmpty(model))
                        cmr = _carModelBl.FetchModelIdFromMaskingName(this.HttpContext.Request.QueryString["model"], string.Empty);
                    var isVideosPage = Request.QueryString["section"] != null && Request.QueryString["section"] == "videos";

                    if (cmr != null)
                    {
                        if (!cmr.IsValid)
                        {
                            return RedirectPermanent(isVideosPage ? CMSCommon.GetVideoUrl(null, null, null, 0, true) : imageLanding);
                        }
                        else if (cmr.IsRedirect)
                        {
                            return RedirectPermanent("/m" + cmr.RedirectUrl + Regex.Replace(Request.RawUrl, @"\/.*-cars\/[^\/]*\/", ""));
                        }
                    }

                    Dictionary<string, string> queryString = Request.QueryString.Keys.Cast<string>().Where(key=>!string.IsNullOrWhiteSpace(key)).ToDictionary(key => key, value => Request.QueryString[value]);
                    queryString.Add("modelId", cmr == null ? string.Empty : cmr.ModelId.ToString());
                    var galleryDTO = _galleryAdapter.Get<PhotoGalleryDTO_V2, Dictionary<string, string>>(queryString);
                    if (galleryDTO.ModelDetails == null || (galleryDTO.GalleryState.ActiveSection == 0 && galleryDTO.GalleryState.ActiveFilter != GalleryFilters.Colors) || (galleryDTO.GalleryState.ActiveFilter == GalleryFilters.Colors && !galleryDTO.ShowModelColors))
                    {
                        return Redirect("/m" + Regex.Replace(Request.RawUrl, @"\/colours\/", "/"));
                    }

                    if (galleryDTO.GalleryState.ActiveSection == GallerySections.VideosSection && (galleryDTO.ModelVideos == null || galleryDTO.ModelVideos.Count == 0))
                    {
                        return Redirect(CMSCommon.GetVideoUrl(null, null, null, 0, true));
                    }
                    else if (galleryDTO.GalleryState.ActiveSection == GallerySections.ColorSection && !galleryDTO.ShowModelColors)
                    {
                        return Redirect(CMSCommon.GetImageUrl(galleryDTO.ModelDetails.MakeName, galleryDTO.ModelDetails.MaskingName, null, 0));
                    }
                    else if (galleryDTO.GalleryState.ActiveSection == GallerySections.Photos && (galleryDTO.ModelImages == null || galleryDTO.ModelImages.Count == 0))
                    {
                        return Redirect(imageLanding);
                    }
                    if (galleryDTO.GalleryState.ActiveSlideIndex < 0)
                    {
                        return RedirectPermanent(CMSCommon.GetImageUrl(galleryDTO.ModelDetails.MakeName, galleryDTO.ModelDetails.MaskingName, null, 0, true));
                    }
                    bool isPartial = this.HttpContext.Request.QueryString["isPartial"] != null && this.HttpContext.Request.QueryString["isPartial"] == "true";
                    if (!isPartial)
                    {
                        var sponsoredDealerCityId = 0;
                        var sponsoredDealerZoneId = string.Empty;
                        ViewBag.ZoneId = CookiesCustomers.MasterZoneId;
                        sponsoredDealerCityId = CookiesCustomers.MasterCityId;
                        sponsoredDealerZoneId = ViewBag.ZoneId.ToString();
                        BindSponsoredDealer(galleryDTO.ModelDetails.ModelId, sponsoredDealerCityId, sponsoredDealerZoneId);
                        ViewBag.IsAdAvailable = ViewBag.EmiAssistVisible;
                        if (CookiesCustomers.MasterCityId < 0 || !ViewBag.EmiAssistVisible)
                        {
                            var discountSummary = _deals.GetDealsSummaryByModelandCity(galleryDTO.ModelDetails.ModelId, CookiesCustomers.MasterCityId > 0 ? CookiesCustomers.MasterCityId : 0);
                            galleryDTO.DiscountSummary = _deals.CarOverviewDiscountSummary(discountSummary);
                        }
                        ViewBag.OptionsCount = 4 + (ViewBag.IsAdAvailable ? 2 : 0) + (galleryDTO.DiscountSummary == null ? 0 : 1);

                        if (galleryDTO.GalleryState.ActiveSection == GallerySections.VideosSection)
                        {
                            return View("~/Views/m/Editorial/Gallery.cshtml", galleryDTO);
                        }
                        else
                        {
                            return View("~/Views/m/Gallery/MGalleryLandingPage.cshtml", galleryDTO);
                        }
                    }
                    else
                        if (galleryDTO.GalleryState.ActiveSection == GallerySections.ColorSection)
                            return PartialView("~/Views/Shared/m/Editorial/_ColorImages.cshtml", galleryDTO);
                        else if (galleryDTO.GalleryState.ActiveSection == GallerySections.VideosSection)
                            return PartialView("~/Views/Shared/m/Editorial/_Videos.cshtml", galleryDTO);
                        else
                            return PartialView("~/Views/Shared/m/Editorial/_ImageList.cshtml", galleryDTO);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GalleryController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return Redirect(imageLanding);
        }


        /// <summary>
        /// Gets PQSponsored Details for modelid,cityid,zoneid combitnation
        /// For EMI Assistance
        /// </summary>
        /// <param name="container"></param>
        private void BindSponsoredDealer(int modelId, int cityId, string zoneId)
        {
            ViewBag.SponsoredDealer = _campaign.GetSponsorDealerAd(modelId, (int)Platform.CarwaleMobile, new Entity.Geolocation.Location { CityId = cityId, ZoneId = CustomParser.parseIntObject(zoneId.Equals("-1") ? string.Empty : zoneId) });
            ViewBag.EmiAssistVisible = ViewBag.SponsoredDealer != null ? (ViewBag.SponsoredDealer.DealerId > 1) : false;
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;applicationId")]
        public ActionResult GetModelImageCard(int modelId, ushort applicationId)
        {
            //Hard coded parameters to reduce permutations of output for output caching
            ArticleByCatURI queryString = new ArticleByCatURI();
            queryString.ApplicationId = applicationId;
            queryString.ModelId = modelId;
            queryString.CategoryIdList = "10";
            queryString.StartIndex = 1;
            queryString.EndIndex = 1;

            var mediaCard = _mediaBL.GetMediaListing(queryString);

            return PartialView("~/Views/m/Editorial/ModelImageCard.cshtml", mediaCard);
        }

        [Route("m/videosdetails")]
        public ActionResult GetVideoCard(int basicId)
        {
            GenericContentDetailDTO contentPage = new GenericContentDetailDTO();
            try
            {
                _container.RegisterInstance<int>(Convert.ToInt32(basicId));
                _container.RegisterInstance<Entity.EnumGenericContentType>(Entity.EnumGenericContentType.videos);
                IServiceAdapter genArticleAdapter = _container.Resolve<IServiceAdapter>("GenericContentDetailAdaptor");
                contentPage = genArticleAdapter.Get<GenericContentDetailDTO>();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GalleryController.VideoCard()");
                objErr.LogException();
            }
            return PartialView("~/Views/Shared/m/Editorial/_VideoCard.cshtml", contentPage.VideoContent);
        }

        [Route("m/suggestedmodelgallery")]
        public ActionResult GetSuggestedModelGallery(int modelId, int count, bool isMsite = true, bool horizontal = false)
        {
            List<CarModelDetails> modelDetailsList = null;
            try
            {
                modelDetailsList = _mediaBL.GetUserHistoryModelDetails(modelId, count);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            if (isMsite)
            {
                return PartialView("~/Views/Shared/m/Editorial/_SuggestedModelGallery.cshtml", modelDetailsList);
            }
            else
            {
                return PartialView("~/Views/Shared/Editorial/_SuggestedModelGallery.cshtml", new Tuple<List<CarModelDetails>, bool>(modelDetailsList, horizontal));
            }
        }
    }
}