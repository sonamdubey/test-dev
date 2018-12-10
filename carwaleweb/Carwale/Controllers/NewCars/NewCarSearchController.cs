using Carwale.DTOs.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.Home;
using Carwale.Notifications;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Linq;
using Carwale.Interfaces.CMS;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CarData;
using Carwale.UI.ClientBL;
using Carwale.UI.Filters;
using Carwale.BL.CMS;
using AutoMapper;
using Carwale.Interfaces;
using Carwale.DTOs.CMS.Media;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.Enum;
using Carwale.Utility;
using Carwale.Entity.ElasticEntities;
using System.Collections.Specialized;
using Carwale.BL.Elastic.NewCarSearch;
using System.Web;
using Carwale.Entity.CarData;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Elastic;
using Carwale.Entity.PriceQuote;

namespace Carwale.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NewCarSearchController : Controller
    {
        private readonly static List<int> _sponsoredVersionIds = Utility.ExtensionMethods.ConvertStringToList<int>((ConfigurationManager.AppSettings["NewCarsSponsoredVersionIds"] ?? ""), ',');
        protected IUnityContainer _container;
        protected ICarPriceQuoteAdapter _versionPrice;
        protected ICarVersionCacheRepository _pqAdapter;
        private const int _dastunGoPrice = 329000;
        private const int _dastunGoPlusPrice = 383000;
        private readonly static string[] _makesForDatsunCampaign = { "4", "5","7","8","9","10","16","21","56" };

        public NewCarSearchController(IUnityContainer container, ICarPriceQuoteAdapter versionPrice, ICarVersionCacheRepository pqAdapter)
        {
            _container = container;
            _versionPrice = versionPrice;
            _pqAdapter = pqAdapter;

        }

        [Route("new/search/")]
        public ActionResult Index()
        {
            object Model = new Object();
            try
            {
                return View("~/Views/NewCar/NewCarSearch.cshtml", Model);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("/");
        }

        [Route("new/search/result/")]
        public ActionResult Result()
        {
            try
            {
                ElasticCarData Model = new ElasticCarData();
                NameValueCollection nvc = new NameValueCollection(Request.QueryString);
                NewCarSearchInputs inputs = new NewCarSearchInputs();
                var cityFromCookie = CookiesCustomers.MasterCityId;
                if (cityFromCookie > 0)
                {
                    inputs.cityId = cityFromCookie;
                }
                bool showSponsoredAd = false;
                if (RegExValidations.IsPositiveNumber(nvc["pn"]))
                    inputs.pageNo = Convert.ToInt32(nvc["pn"]);
                else inputs.pageNo = 1;
                ViewBag.pageNo = inputs.pageNo;

                int paginationCounter = inputs.pageNo;

                while (paginationCounter > 1 && paginationCounter > inputs.pageNo - 2) paginationCounter--;

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["ep"]))
                {
                    string[] powers = nvc["ep"].Split(',');

                    foreach (var power in powers)
                    {
                        switch (power)
                        {
                            case "1": inputs.enginePower.Add(new RangeLimit() { LowerLimit = 0, UpperLimit = 75 }); break;
                            case "2": inputs.enginePower.Add(new RangeLimit() { LowerLimit = 76, UpperLimit = 100 }); break;
                            case "3": inputs.enginePower.Add(new RangeLimit() { LowerLimit = 101, UpperLimit = 150 }); break;
                            case "4": inputs.enginePower.Add(new RangeLimit() { LowerLimit = 151, UpperLimit = 200 }); break;
                            case "5": inputs.enginePower.Add(new RangeLimit() { LowerLimit = 201, UpperLimit = 1000 }); break;
                        }
                    }
                }

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["seat"]))
                {
                    string[] seating = nvc["seat"].Split(',');

                    foreach (var seat in seating)
                    {
                        switch (seat)
                        {
                            case "1": inputs.seatingCapacity.Add(new RangeLimit() { LowerLimit = 0, UpperLimit = 5 }); showSponsoredAd = true; break;
                            case "2": inputs.seatingCapacity.Add(new RangeLimit() { LowerLimit = 6, UpperLimit = 8 }); showSponsoredAd = true; break;
                            case "3": inputs.seatingCapacity.Add(new RangeLimit() { LowerLimit = 9, UpperLimit = 1000 }); break;
                        }
                    }
                }

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["bs"]))
                {
                    inputs.bodytype = nvc["bs"].Split(',');
                    if (inputs.bodytype.Contains("6") && !inputs.bodytype.Contains("9"))
                    {
                        var temp = inputs.bodytype.ToList();
                        temp.Add("9");
                        inputs.bodytype = temp.ToArray();
                    }
                    if (inputs.bodytype.Contains("3"))
                    {
                        showSponsoredAd = true;
                    }
                }

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["transmission"]))
                {
                    inputs.transmission = nvc["transmission"].Split(',');
                    if (inputs.transmission.Contains("2"))
                    {
                        showSponsoredAd = true;
                    }
                }

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["fuel"]))
                {
                    inputs.fueltype = nvc["fuel"].Split(',');
                    if(inputs.fueltype.Contains("1"))
                    {
                        showSponsoredAd = true;
                    }
                }

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["make"]))
                {
                    inputs.makes = nvc["make"].Split(',');
                    if(_makesForDatsunCampaign.Any(inputs.makes.Contains))
                    {
                        showSponsoredAd = true;
                    }
                }

                if (RegExValidations.ValidateNumericRange(nvc["emi"]))
                {
                    var emi = nvc["emi"].Split('-');
                    inputs.EMI = new RangeLimit() { LowerLimit = Convert.ToInt32(emi[0]), UpperLimit = Convert.ToInt32(emi[1]) };
                    inputs.useEMI = true;
                }

                if (RegExValidations.ValidateCommaSeperatedNumbers(nvc["budget"]))
                {
                    string[] budgets = nvc["budget"].Split(',');
                    foreach (var budget in budgets)
                    {
                        switch (budget)
                        {
                            case "1": inputs.budgets.Add(new RangeLimit() { LowerLimit = 0, UpperLimit = 300000 }); break;
                            case "2": inputs.budgets.Add(new RangeLimit() { LowerLimit = 300001, UpperLimit = 400000 }); showSponsoredAd = true; break;
                            case "3": inputs.budgets.Add(new RangeLimit() { LowerLimit = 400001, UpperLimit = 600000 }); showSponsoredAd = true; break;
                            case "4": inputs.budgets.Add(new RangeLimit() { LowerLimit = 600001, UpperLimit = 800000 }); break;
                            case "5": inputs.budgets.Add(new RangeLimit() { LowerLimit = 800001, UpperLimit = 1200000 }); break;
                            case "6": inputs.budgets.Add(new RangeLimit() { LowerLimit = 1200001, UpperLimit = 1800000 }); break;
                            case "7": inputs.budgets.Add(new RangeLimit() { LowerLimit = 1800001, UpperLimit = 2500000 }); break;
                            case "8": inputs.budgets.Add(new RangeLimit() { LowerLimit = 2500001, UpperLimit = 4000000 }); break;
                            case "9": inputs.budgets.Add(new RangeLimit() { LowerLimit = 4000001, UpperLimit = 1007483647 }); break;
                        }
                    }
                }

                INewCarElasticSearch elasticSearchBl = _container.Resolve<INewCarElasticSearch>();
                Model = elasticSearchBl.GetVersions(inputs, true);
                if (showSponsoredAd)
                {
                    Model.SponsoredVersions = GetSponsoredVersions();
                    if (Model.SponsoredVersions != null && Model.SponsoredVersions.Count > 0)
                    {
                        Model.ShowSponsoredAd = true;
                    }
                }
                ViewBag.start = inputs.pageNo * 10 - 9;
                ViewBag.end = inputs.pageNo * 10;
                ViewBag.paginationCounter = inputs.pageNo;
                ViewBag.totalPages = Model.TotalModels / 10 + (Model.TotalModels % 10 == 0 ? 0 : 1);
                while (ViewBag.paginationCounter > 1 && ViewBag.paginationCounter > inputs.pageNo - 2) { ViewBag.paginationCounter--; }

                nvc.Remove("pn");
                ViewBag.hashParam = String.Join("&", nvc.AllKeys.Select(a => a + "=" + HttpUtility.UrlDecode(nvc[a])));

                if (inputs.pageNo < ViewBag.totalPages)
                {
                    nvc["pn"] = (inputs.pageNo + 1).ToString();
                    ViewBag.nextUrl = String.Join("&", nvc.AllKeys.Select(a => a + "=" + HttpUtility.UrlDecode(nvc[a])));
                }
                if (inputs.pageNo > 1)
                {
                    nvc["pn"] = (inputs.pageNo - 1).ToString();
                    ViewBag.prevUrl = String.Join("&", nvc.AllKeys.Select(a => a + "=" + HttpUtility.UrlDecode(nvc[a])));
                }

                return PartialView("~/Views/NewCar/NewCarSearchResult.cshtml", Model);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Content("An error occurred. We are looking into it, check back soon...");
        }

        private List<CarVersionDetails> GetSponsoredVersions()
        {
            List<CarVersionDetails> sponsoredVersions = new List<CarVersionDetails>();
            Dictionary<int, int> sponsoredVersionPrice = new Dictionary<int, int>();
            sponsoredVersionPrice.Add(_sponsoredVersionIds[0], _dastunGoPrice);
            sponsoredVersionPrice.Add(_sponsoredVersionIds[1], _dastunGoPlusPrice);
            ushort carOrder = 0;
            try
            {
                foreach (var versionId in _sponsoredVersionIds)
                {
                    CarVersionDetails getSponsoredVersionDetails = _pqAdapter.GetVersionDetailsById(versionId);
                    PriceOverview sponsoreVersionPrices = new PriceOverview()
                    {
                        Price = sponsoredVersionPrice[versionId]
                    };
                    getSponsoredVersionDetails.PriceOverview = sponsoreVersionPrices;
                    sponsoredVersions.Add(getSponsoredVersionDetails);
                    carOrder++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return sponsoredVersions;
        }
    }

}