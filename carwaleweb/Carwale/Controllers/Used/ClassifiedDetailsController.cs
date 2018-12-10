using AutoMapper;
using Carwale.BL.Stock;
using Carwale.DAL.Customers;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Stock;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class ClassifiedDetailsController : Controller
    {
        protected CarDetailsEntity entity;
        public string buyerId = string.Empty,urlToRedirect = string.Empty;
        private const string c_strHyphen1 = "-1";

        protected string cityNameFromId = string.Empty;
        private bool isPermanantRedirect = false;

        private readonly ICommonOperationsRepository _commonRepo;
        private readonly ICarDetail _carDetailsRepo;
        private readonly ISearchUtility _searchUtility;
        private readonly IGeoCitiesCacheRepository _geoCacheRepo;
        private readonly IListingDetails _carDetailsRepository;
        private readonly IStockCertificationCacheRepository _stockCertificationCacheRepo;
        private readonly static string[] _insuranceStates = (System.Configuration.ConfigurationManager.AppSettings["InsuranceStates"] ?? "-2").Split(',');
        private static readonly string _cartradeCertificationId = ConfigurationManager.AppSettings["CartradeCertificationId"].ToString();

        public ClassifiedDetailsController(ICommonOperationsRepository commonRepo, ICarDetail carDetailsRepo, IGeoCitiesCacheRepository geoCacheRepo, IListingDetails carDetailsRepository, IStockCertificationCacheRepository stockCertificationCacheRepo, ISearchUtility searchUtility)
        {
            _commonRepo = commonRepo;
            _carDetailsRepo = carDetailsRepo;
            this._geoCacheRepo = geoCacheRepo;
            _carDetailsRepository = carDetailsRepository;
            _stockCertificationCacheRepo = stockCertificationCacheRepo;
            _searchUtility = searchUtility;
        }

        // GET: ClassifiedDetails
        [DeviceDetectionFilter]
        public ActionResult CarDetails(string car)
        {
            //variables required on the page
            ViewBag.hasShowroom = false;
            ViewBag.sellerId = 0;
            ViewBag.noWarrantyDetails = " Want more information? Contact the dealer";
            ViewBag.sellInqId = string.Empty;

            var model = new CarDetailsModel();

            if (IsProfileIdNonValid(car))
            {
                return RedirectPermanent(urlToRedirect);
            }

            if (GetCarDetails(model, "", Platform.CarwaleDesktop))
            {
				if (urlToRedirect.ToLower() == "/pagenotfound.aspx") return HttpNotFound();
                if (isPermanantRedirect)
                {
                    return RedirectPermanent(urlToRedirect);
                }
                return Redirect(urlToRedirect);
            }
            model.CustomerDetails = CheckUserCredentials();

            RequestPhotos(buyerId);

            if (ViewBag.sellerId == 1)
                ValidateDealerId();

            PresentationLogic(model);

            model.RankFromQueryString = Request.QueryString["rk"];
            model.IsPremiumFromQueryString = Request.QueryString["isP"];
            model.ListingsTrackingPlatform = ConfigurationManager.AppSettings["ListingsTrackingPlatform"].ToString();
            model.DeliveryCity = GetCityNameFromId(Request.QueryString["dc"]);
            int deliveryCityId, priceNumeric, versionSubSegmentId;
            Int32.TryParse(Request.QueryString["dc"], out deliveryCityId);
            model.DeliveryCityId = deliveryCityId;
            Int32.TryParse(model.carDetailEntity.BasicCarInfo.PriceNumeric, out priceNumeric);
            int rootId = Convert.ToInt32(model.carDetailEntity.BasicCarInfo.RootId);
            int cityId = Convert.ToInt32(model.carDetailEntity.BasicCarInfo.CityId);
            Int32.TryParse(model.carDetailEntity.BasicCarInfo.VersionSubSegmentID, out versionSubSegmentId);

            model.carDetailEntity.BasicCarInfo.StockRecommendationUrl = StockRecommendationsBL.GetStockRecommendationsUrl(model.carDetailEntity.BasicCarInfo.ProfileId, rootId, cityId, model.DeliveryCityId, priceNumeric, versionSubSegmentId);

            model.carDetailEntity.BasicCarInfo.ShowInsuranceLink = ShowInsuranceLink((int)model.carDetailEntity.BasicCarInfo.CityId);
            if(model.carDetailEntity.Finance != null)
                model.carDetailEntity.Finance.EmiFormatted = Format.GetValueInINR(model.carDetailEntity.Finance.Emi);
            //price range set targeting
            double price = model.carDetailEntity.BasicCarInfo.PriceNumeric != null ? (CustomParser.parseDoubleObject(model.carDetailEntity.BasicCarInfo.PriceNumeric))/100000 : 0.0;
            if (price > 0)
                SetTargetingParam(price);
            else
                ViewBag.UsedCarBudgetRange = "0";            
            
            return View("~/Views/Used/CarDetails.cshtml", model);
        }

        public ActionResult MCarDetails(string car,bool isPopup = false, string dc = null)
        {
            ViewBag.noWarrantyDetails = " Want more information? Contact the dealer";

            var model = new CarDetailsModel();
            ViewBag.noWarrantyDetails = " Want more information? Contact the dealer";

            if (IsProfileIdNonValid(car))
                return RedirectPermanent(urlToRedirect);
            if (GetCarDetails(model, "/m", Platform.CarwaleMobile)) 
            {
				if (urlToRedirect.ToLower() == "/pagenotfound.aspx") return HttpNotFound();
                if (isPermanantRedirect)
                {
                    return RedirectPermanent(urlToRedirect);
                }
                return Redirect(urlToRedirect); 
            }                          

            model.CustomerDetails = CheckUserCredentials();
            if (ViewBag.sellerId == 1)
                ValidateDealerId();
            model.RankFromQueryString = Request.QueryString["rk"];
            model.DeliveryCity = GetCityNameFromId(dc);
            int deliveryCityId, priceNumeric;
            Int32.TryParse(dc, out deliveryCityId);
            model.DeliveryCityId = deliveryCityId;
            Int32.TryParse(model.carDetailEntity.BasicCarInfo.PriceNumeric, out priceNumeric);
            int rootId = Convert.ToInt32(model.carDetailEntity.BasicCarInfo.RootId);
            int cityId = Convert.ToInt32(model.carDetailEntity.BasicCarInfo.CityId);
            int versionSubSegmentId;
            Int32.TryParse(model.carDetailEntity.BasicCarInfo.VersionSubSegmentID, out versionSubSegmentId);

            model.carDetailEntity.BasicCarInfo.SimilarCarsUrl = StockRecommendationsBL.GetSimilarCarsUrl(model.carDetailEntity.BasicCarInfo.ProfileId, rootId, cityId, model.DeliveryCityId, priceNumeric, versionSubSegmentId);
            model.carDetailEntity.BasicCarInfo.StockRecommendationUrl = StockRecommendationsBL.GetStockRecommendationsUrl(model.carDetailEntity.BasicCarInfo.ProfileId, rootId, cityId, model.DeliveryCityId, priceNumeric, versionSubSegmentId);

            model.carDetailEntity.BasicCarInfo.ShowInsuranceLink = ShowInsuranceLink((int)model.carDetailEntity.BasicCarInfo.CityId);
            if(model.carDetailEntity.Finance != null)
                model.carDetailEntity.Finance.EmiFormatted = Format.GetValueInINR(model.carDetailEntity.Finance.Emi);
            model.IsCarwaleReferrer = Request.UrlReferrer != null && Request.UrlReferrer.Host.Contains(ConfigurationManager.AppSettings["Domain"]);
            model.PhotoGalleryUrl = GetPhotoGalleryUrl(model);
            return View("~/Views/m/Used/CarDetails.cshtml", model);
        }

        public ActionResult MCarDetailsApi(string car)
        {
            var model = new CarDetailsModel();
            model.NoWarrantyDetails = " Want more information? Contact the dealer";

            if (IsProfileIdNonValid(car))
            {
                model.UrlToRedirect = urlToRedirect; //not needed here as this is page not found case.
                return Json(model, JsonRequestBehavior.AllowGet);
                //return Redirect(urlToRedirect);
            }
                
            if (GetCarDetails(model, "/m", Platform.CarwaleMobile)) //return true if sold out or car basic info is null. 
            {
                if (urlToRedirect.Equals("/pagenotfound.aspx"))
                {
                    model.UrlToRedirect = urlToRedirect;
                }
                else
                {
                    model.UrlToRedirect = "m/" + urlToRedirect;
                }
                return Json(model, JsonRequestBehavior.AllowGet);
                //return Redirect(urlToRedirect);MaskingNumberFormatted
            }            
            model.carDetailEntity.BasicCarInfo.ModelNameFormatted = Format.FormatURL(model.carDetailEntity.BasicCarInfo.ModelName);
            model.carDetailEntity.BasicCarInfo.MakeYearFormatted = model.carDetailEntity.BasicCarInfo.MakeYear.Year;
            model.carDetailEntity.BasicCarInfo.MakeMonthFormatted = model.carDetailEntity.BasicCarInfo.MakeYear.ToString("MMM", CultureInfo.InvariantCulture);
            model.carDetailEntity.BasicCarInfo.MakeNameFormatted = model.carDetailEntity.BasicCarInfo.MakeName.Replace(' ', '-');
            model.carDetailEntity.BasicCarInfo.CityNameFormatted = model.carDetailEntity.BasicCarInfo.CityName.Replace(' ', '-');
            model.carDetailEntity.BasicCarInfo.PriceFormatted = model.carDetailEntity.BasicCarInfo.Price.Replace(' ', '-');
            if (model.carDetailEntity.DealerInfo != null)
            {
                model.carDetailEntity.DealerInfo.MaskingNumberFormatted = Format.FormatURL(model.carDetailEntity.DealerInfo.MaskingNumber); 
            }
            model.CustomerDetails = CheckUserCredentials();
            if (ViewBag.sellerId == 1)
            {
                ValidateDealerId();
                model.HasShowroom = ViewBag.hasShowroom;
            }
            model.carDetailEntity.BasicCarInfo.ShowInsuranceLink = ShowInsuranceLink((int)model.carDetailEntity.BasicCarInfo.CityId);
            model.RankFromQueryString = Request.QueryString["rk"];
            model.IsPremiumFromQueryString = Request.QueryString["isP"];
            if(model.carDetailEntity.Finance !=null)
            model.carDetailEntity.Finance.EmiFormatted = Format.GetValueInINR(model.carDetailEntity.Finance.Emi);
            model.viewAllTyreUrl= ManageCarUrl.TyreSrpPageUrl(model.carDetailEntity.BasicCarInfo.MakeName,model.carDetailEntity.BasicCarInfo.ModelName.Split('[')[0], model.carDetailEntity.BasicCarInfo.ModelId, model.carDetailEntity.BasicCarInfo.MakeYear.ToString("yyyy"), true,model.carDetailEntity.BasicCarInfo.VersionId);

           // model.DeliveryCity = GetCityNameFromId();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        CustomerDetailsEntity CheckUserCredentials()
        {
            CustomerDetailsEntity objCustomerDetails = new CustomerDetailsEntity();
            // If used is loged in, Prefill all the available details
            if (!CurrentUser.Id.Equals("-1"))
            {
                buyerId = CurrentUser.Id;
                CustomerDetails cdObj = new CustomerDetails(buyerId);
                objCustomerDetails.BuyerName = cdObj.Name;
                objCustomerDetails.BuyerMobile = cdObj.Mobile;
                objCustomerDetails.BuyerEmail = cdObj.Email;
            }
            else if (Request.Cookies["TempCurrentUser"] != null && Request.Cookies["TempCurrentUser"].Value != "")
            {
                string userData = Request.Cookies["TempCurrentUser"].Value;
                if (userData.Length > 0 && userData.IndexOf(':') >= 0)
                {
                    string[] details = userData.Split(':');
                    objCustomerDetails.BuyerName = details[0];
                    objCustomerDetails.BuyerMobile = details[1];
                    objCustomerDetails.BuyerEmail = details[2];
                    buyerId = Utility.CarwaleSecurity.DecryptUserId(Convert.ToInt64(details[3]));
                }
            }
            return objCustomerDetails;
        }

        bool GetCarDetails(CarDetailsModel model,string redirectUrl, Platform platformType)
        {
            try
            {
                entity = _carDetailsRepo.GetCompleteCarDetails(ViewBag.profileId, platformType);
                model.carDetailEntity = Mapper.Map<CarDetailsWeb>(entity);
                if (model.carDetailEntity != null && model.carDetailEntity.BasicCarInfo != null)
                {

                    if (model.carDetailEntity.IsSold)
                    {
                        urlToRedirect = $"{ redirectUrl }{ _searchUtility.GetURL(entity.BasicCarInfo.MakeName, entity.BasicCarInfo.RootName, entity.BasicCarInfo.CityMaskingName) }?issold=true";
                        isPermanantRedirect = true;
                        return true;
                    }
                    else
                    {
                        ViewBag.sellerId = model.carDetailEntity.BasicCarInfo.SellerId;
                        if (model.carDetailEntity.BasicCarInfo != null && model.carDetailEntity.BasicCarInfo.CertificationId == _cartradeCertificationId)
                        {
                            model.certification = _stockCertificationCacheRepo.GetCarCertification(Convert.ToInt32(model.carDetailEntity.BasicCarInfo.InquiryId),StockBL.IsDealerStock(model.carDetailEntity.BasicCarInfo.ProfileId));
                        }
                        model.AbsureMainTabs = GetTabs(model);
                    }
                }
                else
                {
                    urlToRedirect = "/pagenotfound.aspx";
                    isPermanantRedirect = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                _commonRepo.SendErrorMail(ex, "GetCarDetails");
            }
            return false;
        }

        public string GetBackToSearch()
        {
            BasicCarInfo carInfo = entity.BasicCarInfo;
            return string.Format("/used/{0}-{1}-cars-in-{2}/", UrlRewrite.FormatSpecial(carInfo.MakeName), carInfo.MaskingName,
              UrlRewrite.FormatSpecial(carInfo.CityName));
        }


        void ValidateDealerId()
        {
            try
            {
                if (!String.IsNullOrEmpty(entity.DealerInfo.DealerId))
                    ViewBag.hasShowroom = CommonDealers.IsValidUsedDealer(entity.DealerInfo.DealerId.ToString());
            }
            catch (Exception ex)
            {
                _commonRepo.SendErrorMail(ex, "ValidateDealerId");
            }
        }

        private bool IsProfileIdNonValid(string car)
        {
            bool isValid = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(car))
                {
                    ViewBag.profileId = car;
                    if (!Validations.IsValidProfileId(ViewBag.profileId))
                        isValid = false;
                    else
                    {
                        ViewBag.sellInqId = CommonOpn.GetProfileNo(ViewBag.profileId);
                        if (!CommonOpn.CheckId(ViewBag.sellInqId))
                            isValid = false;
                    }
                }
                else
                    isValid = false;
            }
            catch (Exception ex)
            {
                _commonRepo.SendErrorMail(ex, "ValidateProfileId");
            }
            if (!isValid)
            {
                urlToRedirect = "/pageNotFound.aspx";
            }
            return !isValid;

        }

        Dictionary<string, string> GetTabs(CarDetailsModel model)
        {
            Dictionary<string, string> tabs = new Dictionary<string, string>();
            if (model.certification != null)
                tabs.Add("carcondition", "Certification");
            else if (entity.NonAbsureCarCondition != null)
                tabs.Add("carcondition", "Car Condition");
            if (entity.IndividualWarranty != null)
                tabs.Add("cwguarantee", "Warranty");

            return tabs;
        }

        void RequestPhotos(string buyerid)
        {
            // If photos not uploaded by the seller
            // Hide photos contents and make 'requestPhotos' div visible where buyer can request seller to upload photos.
            if (entity.BasicCarInfo.PhotoCount == 0)
            {
                if (String.IsNullOrEmpty(buyerid))
                    ViewBag.reqPhotos = true;
                else
                {
                    int consumerType = ViewBag.sellerId;
                    ViewBag.reqPhotos = !_carDetailsRepository.PhotoRequestDone(Convert.ToInt32(ViewBag.sellInqId), Convert.ToInt32(buyerId), consumerType);
                }
            }
        }

        void PresentationLogic(CarDetailsModel model)
        {
            model.VideoCdtn = (entity.BasicCarInfo.VideoCount > 0 && entity.BasicCarInfo.IsPremium == true);
            model.IsCertified = (entity.BasicCarInfo.CertificationId != "" && entity.BasicCarInfo.CertificationId != "0" && entity.BasicCarInfo.CertificationId != "-1" && entity.BasicCarInfo.CertificationId != _cartradeCertificationId);
            if (ViewBag.sellerId == 1)
            {
                model.DealerProfileImgCdtn = (!String.IsNullOrEmpty(entity.DealerInfo.DealerProfileHostUrl) && !String.IsNullOrEmpty(entity.DealerInfo.DealerProfileImagePath));
            }
        }
        public string GetCityNameFromId(string cityId)
        {
            if (!String.IsNullOrEmpty(cityId))
            {
                int deliveryCityId;
                bool isValidCity = int.TryParse(cityId.Trim(), out deliveryCityId);
                if (isValidCity) 
                {
                    if (cityId.Equals("3000"))
                        return "Mumbai";
                    else if (cityId.Equals("3001"))
                        return "Delhi NCR";
                    cityNameFromId = _geoCacheRepo.GetCityNameById(deliveryCityId.ToString());
                    if (!(String.IsNullOrEmpty(cityNameFromId)))
                        return cityNameFromId;

                } 
            }
            return string.Empty;
        }

        private bool ShowInsuranceLink(int cityId)
        {
            bool isInsuranceCity = false;
            if (_insuranceStates[0] == "-1")
                isInsuranceCity = true;
            else if (_insuranceStates[0] == "0")
                isInsuranceCity = false;
            else
            {
                var state = _geoCacheRepo.GetStateByCityId(cityId);
                if (state != null && state.StateId > 0)
                    isInsuranceCity = Array.IndexOf(_insuranceStates, state.StateId.ToString()) == -1 ? false : true;
            }
            return isInsuranceCity;
        }
        
        private bool SetTargetingParam(double price)
        {
            var budgetRange = new Dictionary<Func<double, bool>, Action>
            {  
             { x => x <= 3 , () => SetBudget("upto3")},
             { x => x <= 4 , () => SetBudget("3-4")},
             { x => x <= 6 , () => SetBudget("4-6")},
             { x => x <= 8 , () => SetBudget("6-8")},
             { x => x <= 12 ,() => SetBudget("8-12")},
             { x => x <= 20 ,() => SetBudget("12-20")},
             { x => x <= 30 ,() => SetBudget("20-30")},
             { x => x <= 40 ,() => SetBudget("30-40")},
             { x => x > 40 ,() => SetBudget("above40")},
            };
            budgetRange.First(sw => sw.Key(price)).Value();

            return true;
        }

        private bool SetBudget(string budget)
        {
            ViewBag.UsedCarBudgetRange = budget;
            return true;
        }

        private string GetPhotoGalleryUrl(CarDetailsModel model)
        {
            StringBuilder photoGalleryUrl = new StringBuilder("/m/used/PhotoGallery.aspx?inquiryid=").Append(model.carDetailEntity.BasicCarInfo.InquiryId)
                                                .Append("&isdealer=").Append(model.carDetailEntity.BasicCarInfo.SellerId == 1)
                                                .Append("&profileid=").Append(model.carDetailEntity.BasicCarInfo.ProfileId)
                                                .Append("&deliverycity=").Append(model.DeliveryCity)
                                                .Append("&dc=").Append(model.DeliveryCityId)
                                                .Append("&rk=").Append(model.RankFromQueryString)
                                                .Append("&ischatavail=").Append(model.carDetailEntity.BasicCarInfo.IsChatAvailable ? "1" : "0")
                                                .Append("&istoprated=").Append(string.IsNullOrWhiteSpace(model.carDetailEntity.DealerInfo?.RatingText));
            string tempSlot = Request.QueryString["slot"] ?? string.Empty;
            if (!string.IsNullOrEmpty(tempSlot))
            {
                photoGalleryUrl.Append("&slot=").Append(tempSlot);
            }
            return photoGalleryUrl.ToString();
        }
    }
}