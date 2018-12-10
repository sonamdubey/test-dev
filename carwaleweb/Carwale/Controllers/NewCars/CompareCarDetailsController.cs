using Carwale.Entity.CompareCars;
using Carwale.Entity.Geolocation;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.NewCars
{
    public class CompareCarDetailsController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ICarModels _carModelBl;
        public string modelPageUrl = string.Empty;
        public CompareCarDetailsController(IUnityContainer container, ICarModels carModelBl)
        {
            _container = container;
            _carModelBl = carModelBl;
        }
        [DeviceDetectionFilter]
        public ActionResult CompareCars()
        {
            var compareCarsDetailModel = new CompareCarsDetailModel();
            try
            {
                string url = GetRedirectionUrl();
                if (!string.IsNullOrWhiteSpace(url))
                    return RedirectPermanent(url);
                List<int> _versionIDs = ProcessQueryString();
                Location custLocation = new Location
                {
                    CityId = CookiesCustomers.MasterCityId,
                    ZoneId = CookiesCustomers.MasterZoneId,
                    AreaId = CookiesCustomers.MasterAreaId

                };
                if (_versionIDs.Count <= 0 || _versionIDs.Count > 4)
                {
                    return RedirectPermanent("/comparecars/");
                }
                else if (_versionIDs.Count == 1)
                {
                    string redirectUrl = string.IsNullOrEmpty(modelPageUrl) ? "/comparecars/" : modelPageUrl;
                    return RedirectPermanent(redirectUrl);
                }
                else
                {
                    CompareCarInputParam input = new CompareCarInputParam
                    {
                        VersionIds = _versionIDs,
                        CustLocation = custLocation,
                        CwcCookie = CurrentUser.CWC
                    };
                    IServiceAdapterV2 comparePageAdapter = _container.Resolve<IServiceAdapterV2>("CompareCarDetailsAdapterDesktop");
                    compareCarsDetailModel = comparePageAdapter.Get<CompareCarsDetailModel, CompareCarInputParam>(input);
                    if (compareCarsDetailModel != null)
                        return View("~/Views/NewCar/CompareCarDetails.cshtml", compareCarsDetailModel);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("/comparecars/");
        }
        private List<int> ProcessQueryString()
        {
            NameValueCollection nvc = new NameValueCollection(Request.QueryString);
            string queryString = HttpUtility.UrlDecode(Convert.ToString(Request.QueryString));
            HashSet<int> versionIDS = new HashSet<int>();
            try
            {
                if (queryString.Contains("c1"))
                {
                    if (nvc.Count < 12)
                    {
                        for (int v = 1; v <= nvc.Count; v++)
                        {
                            int VersionID;
                            if (int.TryParse(nvc.Get("c" + v.ToString()), out VersionID) && VersionID > 0)
                            {
                                versionIDS.Add(VersionID);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(nvc.Get("mk")) && !string.IsNullOrEmpty(nvc.Get("mo")))
                    {
                        List<int> makes = nvc.Get("mk").Split(',').Select(int.Parse).ToList();
                        string[] models = nvc.Get("mo").Split(',');
                        for (int i = 0; i < makes.Count; i++)
                        {
                            if (makes[i] != 0)
                            {
                                var modelObj = _carModelBl.FetchModelIdFromMaskingName(models[i], string.Empty);
                                int modelId = 0;
                                if (modelObj != null && modelObj.ModelId > 0)
                                {
                                    modelId = modelObj.ModelId;
                                }
                                if (modelId > 0)
                                {
                                    ICarModelCacheRepository _carmodelCache = _container.Resolve<ICarModelCacheRepository>();
                                    var modelDetails = _carmodelCache.GetModelDetailsById(modelId);
                                    if (modelDetails != null)
                                    {
                                        versionIDS.Add(modelDetails.PopularVersion);
                                        if (string.IsNullOrEmpty(modelPageUrl))
                                        {
                                            modelPageUrl = ManageCarUrl.CreateModelUrl(modelDetails.MakeName, modelDetails.MaskingName);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return versionIDS != null ? versionIDS.ToList() : new List<int>();
        }


        public string GetRedirectionUrl()
        {
            ICarModelCacheRepository _carmodelCache = _container.Resolve<ICarModelCacheRepository>();
            NameValueCollection nvc = new NameValueCollection(Request.QueryString);
            string queryString = HttpUtility.UrlDecode(Convert.ToString(Request.QueryString));
            List<int> makeIds = null;
            List<int> modelIds = new List<int>();
            string[] models = null;
            string returnUrl = string.Empty;
            var dict = new List<Tuple<int, string>>();
            try
            {
                if (!string.IsNullOrEmpty(nvc.Get("mk")) && !string.IsNullOrEmpty(nvc.Get("mo")))
                {
                    makeIds = nvc.Get("mk").Split(',').Select(int.Parse).ToList();
                    models = nvc.Get("mo").Split(',');
                }
                if (makeIds != null)
                {
                    for (int i = 0; i < models.Count(); i++)
                    {
                        var modelObj = _carModelBl.FetchModelIdFromMaskingName(models[i], string.Empty);
                        int modelId = 0;
                        if (modelObj != null && modelObj.ModelId > 0)
                        {
                            modelId = modelObj.ModelId;
                            modelIds.Add(modelId);
                            var modelDetails = _carmodelCache.GetModelDetailsById(modelId);
                            dict.Add(Tuple.Create(modelId, UrlRewrite.FormatSpecial(modelDetails.MakeName) + "-" + models[i]));
                        }

                    }

                }
                if (queryString.Contains("car"))
                {
                    if (!modelIds.IsInDescOrder())
                        returnUrl = string.Format("/comparecars/{0}/", Format.GetCompareUrl(dict));
                    else
                        returnUrl = Request.RawUrl.Split('?')[0];
                }
                else
                {
                    if (!modelIds.IsInDescOrder())
                        returnUrl = string.Format("/comparecars/{0}{1}", Format.GetCompareUrl(dict), (!string.IsNullOrWhiteSpace(queryString) && queryString.IndexOf("c1") > 0 ? "/?" + queryString.Substring(queryString.IndexOf("c1")) : "/" + string.Empty));
                    else
                        returnUrl = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return returnUrl;
        }
    }
}