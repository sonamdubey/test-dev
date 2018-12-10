using Carwale.DTOs.CMS.Models;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class UserReviewsListController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ICarModels _carModelBl;
        public UserReviewsListController(IUnityContainer container, ICarModels carModelBl)
        {
            _container = container;
            _carModelBl = carModelBl;
        }

        [Route("userreviewlist/")]
        public ActionResult Index(string carName, int page = 0, int versionId = -1)
        {
            try
            {
                var modelResponse = _carModelBl.FetchModelIdFromMaskingName(carName, string.Empty);

                if (modelResponse != null)
                {
                    var url = Request.ServerVariables["HTTP_X_REWRITE_URL"];
                    if (Request.Url.Query.EndsWith("m=1"))
                    {
                        ViewBag.IsMobile = true;
                    }
                    else if (url != null)
                    {
                        ViewBag.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                        if (ViewBag.IsMobile) return Redirect("/m" + url);
                    }

                    if (modelResponse.IsRedirect)
                    {
                        return RedirectPermanent(modelResponse.RedirectUrl + "userreviews/");
                    }
                    var userreviewInput = new UserReviewUriEntity()
                                                                {
                                                                    ModelId = modelResponse.ModelId,
                                                                    VersionId = versionId,
                                                                    PageNo = page > 0 ? page : 1,
                                                                    PageSize = 10,
                                                                    SortCriteria = 1,
                                                                    CityId = CookiesCustomers.MasterCityId
                                                                };
                    IServiceAdapterV2 userReviewListAdapter = _container.Resolve<IServiceAdapterV2>("UserReviewsList");
                    var _userReviewList = userReviewListAdapter.Get<UserReviewsListDTO, UserReviewUriEntity>(userreviewInput);
                    _userReviewList.QuickMenuDetails.PQPageId = 54;
                    _userReviewList.QuickMenuDetails.PageId = 43;
                    _userReviewList.QuickMenuDetails.subNavOnCarCompare = false;
                    _userReviewList.BreadcrumbEntitylist = BindBreadCrumb(_userReviewList.ModelDetails);
                    ViewBag.Url = url;
                    ViewBag.ShortUrl = string.Format("{0}/{1}-cars/{2}/userreviews", (ViewBag.IsMobile ? "/m" : string.Empty), UrlRewrite.FormatSpecial(_userReviewList.ModelDetails.MakeName), _userReviewList.ModelDetails.MaskingName);
                    ViewBag.CarName = string.Format("{0} {1}", _userReviewList.ModelDetails.MakeName, _userReviewList.ModelDetails.ModelName);
                    ViewBag.UserReviewInput = userreviewInput;
                    ViewBag.Page = page > 0 ? page : 1;
                    ViewBag.IntialPage = page;
                    ViewBag.CityName = CookiesCustomers.MasterCity;
                    _userReviewList.PageList = GetPageList(_userReviewList, ViewBag.Page);
                    return View("~/Views/Editorial/UserReviewsList.cshtml", _userReviewList);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserReviewsListController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return HttpNotFound();
        }

        public List<int> GetPageList(UserReviewsListDTO userReviewDTO, int page)
        {
            var pageList = new List<int>();
            if (userReviewDTO.PageNumber > 1)
            {
                int start = 0, end = userReviewDTO.PageNumber;
                if (page == userReviewDTO.PageNumber)
                {
                    end = userReviewDTO.PageNumber;
                    if (userReviewDTO.PageNumber > 2) start = page - 2;
                    else start = page - 1;
                }
                else if (page == 1)
                {
                    start = 1;
                    if (userReviewDTO.PageNumber > 2) end = page + 2;
                    else end = page + 1;
                }
                else if ((page + 1) == userReviewDTO.PageNumber)
                {
                    start = page - 1;
                    end = page + 1;
                }
                else { start = page; end = page + 2; }
                pageList.Add(page - 1);
                for (int pageno = start; pageno <= end; pageno++) pageList.Add(pageno);
                pageList.Add(page + 1);
            }
            return pageList;
        }
        private List<Carwale.Entity.BreadcrumbEntity> BindBreadCrumb(CarModelDetails modelDetails)
        {
            string makeName = UrlRewrite.FormatSpecial(modelDetails.MakeName);
            List<Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = string.Format("{0} Cars", modelDetails.MakeName), Link = string.Format("/{0}-cars/", makeName), Text = modelDetails.MakeName });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = modelDetails.ModelName, Link = string.Format("/{0}-cars/{1}/", makeName, modelDetails.MaskingName), Text = modelDetails.ModelName });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = "User Reviews" });
            return _BreadcrumbEntitylist;
        }

    }
}