using Carwale.UI.ClientBL;
using Carwale.BL.PaymentGateway;
using Carwale.Interfaces.NewCars;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Entity.CMS;
using Newtonsoft.Json;
using Carwale.Entity;
using Carwale.Interfaces.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.UI.Common;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.DTOs.CMS.UserReviews;
using Carwale.Notifications.Logs;
using Carwale.DTOs.CMS;
using Carwale.UI.ViewModels.NewCars;
using Carwale.UI.ViewModels.UserReview;
using Carwale.BL.UserReview;
using Carwale.Entity.UserReview;
using Newtonsoft.Json.Linq;
using Carwale.Interfaces.UesrReview;
using Carwale.UI.ViewModels.UserReviews;
using Carwale.UI.PresentationLogic.UserReviews;

namespace Carwale.UI.Controllers.UserReview
{
    public class UserReviewController : Controller
    {
        private readonly IUserReviews _userReview;
        private readonly ICarVersions _carVersions;
        private readonly ICarModelCacheRepository _carModelsCache;
        private readonly ICarMakesCacheRepository _makeCacheRepo;
        private readonly ICarModels _carModelBl;
        private readonly IUserReviewLogic _userReviewLogic;
        private readonly IUnityContainer _container;

        private readonly Func<string, IServiceAdapterV2> _adaptorFactory;

        public UserReviewController(
        IUnityContainer container,
        IUserReviews userReview,
        ICarVersionCacheRepository carVersionsCache,
        ICarModelCacheRepository carModelsCache,
        ICarMakesCacheRepository makeCacheRepo,
        ICarModels carModelBl,
        ICarVersions carVersions,
        Func<string, IServiceAdapterV2> adaptorFactory,
        IUserReviewLogic userReviewLogic)
        {
            _adaptorFactory = adaptorFactory;
            _userReview = userReview;
            _carVersions = carVersions;
            _carModelsCache = carModelsCache;
            _makeCacheRepo = makeCacheRepo;
            _carModelBl = carModelBl;
            _userReviewLogic = userReviewLogic;
            _container = container;
        }

        [Route("userreviews/")]
        public ActionResult Index_New()
        {
            Response.AddHeader("Vary", "User-Agent");
            UserReviewLanding userReview = new UserReviewLanding();
            _container.RegisterType<IServiceAdapterV2, UserReviewLangingPageAdapter>("UserReviewLangingPage", new ContainerControlledLifetimeManager());
            var isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
            IServiceAdapterV2 _landingAdaptor = _adaptorFactory("UserReviewLangingPage");
            userReview = _landingAdaptor.Get<UserReviewLanding, bool>(isMobile);
            return isMobile ? View("~/Views/m/UserReview/Index_New.cshtml", userReview) :
                View("~/Views/UserReview/Index_New.cshtml", userReview);
        }

        [Route("userreviews/rate-car/")]
        public ActionResult RateCar(int modelId, int versionId = 0)
        {
            Response.AddHeader("Vary", "User-Agent");
            if (modelId > 0)
            {
                var isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                ViewBag.IsMobile = isMobile;
                List<CarVersions> versions = _carVersions.GetCarVersions(modelId, Status.All);
                CarModelDetails carModelDetails = _carModelsCache.GetModelDetailsById(modelId);
				RateCarViewModel rateCarViewModel = new RateCarViewModel
				{
					ModelName = carModelDetails.ModelName,
					MakeName = carModelDetails.MakeName,
					ModelImageSmall = carModelDetails.ModelImageSmall,
					Versions = versions,
					RateCarDetails = RateCarObject.GetRateCarDetails(),
					IsMobile = isMobile
				};
				if(versionId <= 0)
                {
                    string reviewedVersion = HttpUtility.UrlDecode(CookieManager.GetCookie("reviewedVersions"));
                    versionId = !string.IsNullOrEmpty(reviewedVersion) ? _userReviewLogic.GetVersion(reviewedVersion, modelId) : 0;
                }
                
				if (versionId > 0) // to handel refresh when version is selected
				{
					CarVersions selectedVersion = versions.Where(version => version.Id == versionId)?.FirstOrDefault();
					rateCarViewModel.VersionId = selectedVersion != null ? selectedVersion.Id : 0;
					rateCarViewModel.VersionName =  selectedVersion?.Version  ??  string.Empty;
				}
                return View("~/Views/UserReview/RateCar.cshtml", rateCarViewModel);
            }
            else
            {
                return Redirect("/userreviews/");
            }
        }

        [Route("userreviews/write-review/")]
        public ActionResult WriteReview(string reviewId)
        {
            try
            {
                Response.AddHeader("Vary", "User-Agent");
                string decryptedHash = Utils.Utils.DecryptTripleDES(reviewId);
                int id;
                int.TryParse(decryptedHash.Split('|')[0], out id);
                if (id > 0)
                {
                    WriteReview writeReviewViewModel = new WriteReview
                    {
                        ReviewDetails = _userReviewLogic.GetWriteReviewPageDetails(id)
                    };
                    if (writeReviewViewModel.ReviewDetails != null)
                    {
                        writeReviewViewModel.ReviewDetails.MoreRatings.RatingOptions.Sort(new Comparison<UserReviewRatingOptions>((x, y) => y.Value.CompareTo(x.Value)));
                        writeReviewViewModel.Hash = reviewId;
                        writeReviewViewModel.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                        writeReviewViewModel.Channel = Request.QueryString["channel"] ?? "other";
                        return View("~/Views/UserReview/WriteReview.cshtml", writeReviewViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("/userreviews/");
        }

        [Route("userreviews/submit-review/")]
        public void SubmitReview(UserReviewDetail userReviewDetail)
        {
            userReviewDetail.CustomerName = PGCookie.CustomerName.Trim();
            var customerEmail = PGCookie.CustomerEmail.Trim();
            var customerId = RegisterCustomer(userReviewDetail.CustomerName, customerEmail);
            if (_userReview.CheckVersionReview(userReviewDetail.VersionId, customerEmail, customerId, userReviewDetail.ModelId))
            {
                CookieManager.Add(new Cookie("ReviewRepeated") { Value = "true", Expires = DateTime.Now.AddMinutes(1) });
                Response.Redirect("/userreviews/");
                return;
            }
            userReviewDetail.CustomerId = customerId;
            userReviewDetail.Comments = SanitizeHTML.ToSafeHtml(userReviewDetail.Comments);
            string recordId = _userReview.SaveDetails(userReviewDetail, Server.UrlDecode(CookieManager.GetCookie("userOverallRating")));
            SourceIdCommon.UpdateSourceId(EnumTableType.CustomerReviews, recordId);
            CookieManager.Add(new Cookie("isNewReview") { Value = "true", Expires = DateTime.Now.AddMinutes(1) });
            Response.Redirect(string.Format("/m/{0}-cars/{1}/userreviews/{2}/", Format.FormatSpecial(userReviewDetail.MakeName), userReviewDetail.MaskingName, recordId));
        }

        public ActionResult UserReviewsDetails(int id)
        {
            try
            {
				string makeMaskingName = this.HttpContext.Request.QueryString["make"];
				string modelMaskingName = this.HttpContext.Request.QueryString["model"];
				var cmr = _carModelBl.FetchModelIdFromMaskingName(modelMaskingName, string.Empty);
                if (!cmr.IsValid)
                {
                    return HttpNotFound();
                }

                UserReviewDetailDesktopDto userReviewDetails = _userReview.GetUserReviewsDetails(id);
				if (userReviewDetails != null)
				{
					if (int.Parse(userReviewDetails.ModelId) != cmr.ModelId || cmr.IsRedirect)
					{
						string modelUrl = int.Parse(userReviewDetails.ModelId) != cmr.ModelId ?  ManageCarUrl.CreateModelUrl(userReviewDetails.Make, userReviewDetails.MaskingName) : cmr.RedirectUrl;
						return RedirectPermanent(string.Format("/m{0}userreviews/{1}/", modelUrl, id));
					}

					ViewBag.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
					return View("~/views/userreview/userreviewsdetails.cshtml", userReviewDetails);
				}
				else
				{
					string modelUrl = cmr.IsRedirect ? cmr.RedirectUrl : ManageCarUrl.CreateModelUrl(makeMaskingName, modelMaskingName);
					return RedirectPermanent(string.Format("/m{0}userreviews/", modelUrl));
				}
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("/userreviews/");
        }

        public ActionResult Index()
        {
            UserReviewLanding userReview = new UserReviewLanding();
            userReview.Makes = _makeCacheRepo.GetCarMakesByType("All");
            userReview.NewMakes = userReview.Makes.Where(x => x.New).ToList();
            userReview.MostReviewdCars = _userReview.GetMostReviewedCars();
            userReview.MostReadReviews = _userReview.GetUserReviewsByType(UserReviewsSorting.Viewed);
            userReview.MostHelpfulReviews = _userReview.GetUserReviewsByType(UserReviewsSorting.Liked);
            userReview.MostRecentReviews = _userReview.GetUserReviewsByType(UserReviewsSorting.EntryDateTime);
            userReview.MostRatedReviews = _userReview.GetUserReviewsByType(UserReviewsSorting.OverallR);

            return View("~/Views/UserReview/Index.cshtml", userReview);
        }

        private string RegisterCustomer(string name, string email)
        {
            AutomateRegistration ar = new AutomateRegistration();
            AutomateRegistrationResult arr = ar.ProcessRequest(name, email, "", "", "", "", "");
            return arr.CustomerId;
        }

        private bool ValidateParameters(CarEntity carEntity)
        {
            return (carEntity.MakeId > 0 && carEntity.ModelId > 0);
        }
    }
}