using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Carwale.Utility;
using Carwale.Notifications.Logs;
using Carwale.Entity.Price;
using Carwale.DTOs.PriceQuote;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Prices;
using Carwale.UI.Common;
using Carwale.UI.ClientBL;
using Carwale.DTOs.OfferAndDealerAd;
using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.OffersV1;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.Campaigns;
using Carwale.Entity.Enum;
using Carwale.DTOs.Geolocation;

namespace Carwale.UI.Controllers.NewCars
{
    public class PriceQuoteController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly IEmiCalculatorBl _emiCalculatorBl;
        private readonly ICarModels _carModels;

        public PriceQuoteController(IUnityContainer container, IEmiCalculatorBl emiCalculatorBl, ICarModels carModels)
        {
            _container = container;
            _emiCalculatorBl = emiCalculatorBl;
            _carModels = carModels;
        }

        // GET: PriceQuote
        [Route("quotation")]
        public ActionResult Index([Bind(Prefix = "m")] int? modelId, [Bind(Prefix = "v")] int? versionId, [Bind(Prefix = "c")] int? cityId,
            [Bind(Prefix = "a")] int? areaId, [Bind(Prefix = "cs")] bool? isCrossSellPriceQuote, [Bind(Prefix = "p")] int? pageId, [Bind(Prefix = "hc")] bool? hideCampaign)
        {
            try
            {
                var input = new PriceQuoteInput
                {
                    ModelId = CustomParser.parseIntObject(modelId),
                    VersionId = CustomParser.parseIntObject(versionId),
                    CityId = CustomParser.parseIntObject(cityId),
                    AreaId = CustomParser.parseIntObject(areaId),
                    IsCrossSellPriceQuote = CustomParser.parseBoolObject(isCrossSellPriceQuote),
                    PageId = CustomParser.parseIntObject(pageId),
                    HideCampaign = CustomParser.parseBoolObject(hideCampaign)
                };

                if (DeviceDetectionManager.IsMobile(this.HttpContext))
                {
                    var priceQuoteAdapter = _container.Resolve<IServiceAdapterV2>("MobileQuotationAdapter");
                    List<QuotationPageMobileDTO> quotationDTO = priceQuoteAdapter.Get<List<QuotationPageMobileDTO>, PriceQuoteInput>(input);

                    if (quotationDTO != null && quotationDTO.Count > 0)
                    {
                        if (input.IsCrossSellPriceQuote)
                        {
                            return PartialView("~/Views/Shared/m/PriceQuote/_PriceQuoteCard.cshtml", quotationDTO[0]);
                        }
                        else
                        {
                            return View("~/Views/m/New/PriceQuote/Quotation.cshtml", quotationDTO);
                        }
                    }

                    if (BrowserUtils.IsAndroidWebView())
                    {
                        return Content("<script>Android.openLastPage();</script>");
                    }
                    else if (BrowserUtils.IsIosWebView())
                    {
                        return Content("<script>window.location='ios:openLastPage()';</script>");
                    }
                }
                else
                {
                    var priceQuoteAdapter = _container.Resolve<IServiceAdapterV2>("DesktopQuotationAdapter");
                    QuotationPageDesktopDTO quotationDTO = priceQuoteAdapter.Get<QuotationPageDesktopDTO, PriceQuoteInput>(input);

                    if (quotationDTO != null)
                    {
                        return View("~/Views/NewCar/PriceQuote/Quotation.cshtml", quotationDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                return Redirect("/quotation/landing/");
            }
            else
            {
                return Redirect("/new/prices.aspx");
            }
        }

        [Route("quotation/modelChange")]
        public ActionResult Index(int modelId, int? versionId, int cityId, int? areaId, string excludingModels)
        {
            try
            {
                var parsedCityId = CustomParser.parseIntObject(cityId);
                var parsedAreaId = CustomParser.parseIntObject(areaId);
                int similarModelsCountLimit = Carwale.Utility.CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["PqPageSimilarModelsCountLimit"]);
                var pqModelChangeDTO = new PqModelChangeDTO();

                pqModelChangeDTO.Location.CityId = parsedCityId;
                pqModelChangeDTO.Location.AreaId = parsedAreaId;
                string cwcCookie = CurrentUser.CWC ?? string.Empty;
                pqModelChangeDTO.AlternateCarList = _carModels.GetSimilarCarsWithORP(modelId, cityId, cwcCookie, similarModelsCountLimit, excludingModels);

                return PartialView("~/Views/Shared/m/PriceQuote/_ModelChange.cshtml", pqModelChangeDTO);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

    }
}