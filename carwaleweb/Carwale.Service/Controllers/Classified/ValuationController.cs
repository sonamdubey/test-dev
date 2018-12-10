using AutoMapper;
using Carwale.BL.Classified.CarValuation;
using Carwale.DTOs.Classified.CarValuation;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Classified
{
    public class ValuationController : ApiController
    {
        private readonly IValuationBL _valuationBL;
        private readonly IStockBL _stockBL;

        private static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter
        {
            SerializerSettings =
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Ignore
            }
        };

        public ValuationController(IValuationBL valuationBL, IStockBL stockBL)
        {
            _valuationBL = valuationBL;
            _stockBL = stockBL;
        }

        [Route("api/valuation/"), HttpGet, EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult ValuationReport(short year, int version, int city, int? askingPrice = null, UsedCarOwnerTypes owner = UsedCarOwnerTypes.NA, string profileId = null, int? kms = null)
        {
            try
            {
                bool isAmp = !string.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"]);
                ValidateInput(year, version, city, askingPrice, kms);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ValuationReport report = _valuationBL.GetValuationReport(year, version, city, kms, owner);
                if (report == null)
                {
                    return InternalServerError();
                }
                report.SellerAskingPrice = askingPrice;
                if(!string.IsNullOrWhiteSpace(profileId))
                {
                    CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                    if(carDetails!=null)
                    {
                        report.DealerRatingText = carDetails.DealerInfo?.RatingText;
                        report.IsChatAvailable = carDetails.BasicCarInfo != null ? carDetails.BasicCarInfo.IsChatAvailable : false;
                    }
                }
                if (report.Valuation == null)
                {
                    return Content(HttpStatusCode.PartialContent, report, _jsonFormatter);
                }
                else if (isAmp)
                {
                    var valuation = report.Valuation;
                    var valuationReportAmp = Mapper.Map<ValuationReportAmp>(report);
                    if (askingPrice != null && valuation.FairPrice > 0 && valuation.GoodPrice > 0)
                    {
                        valuationReportAmp.IndicatorPosition = (int)(100 + 2.68 * ValuationBL.GetIndicatorPosition(valuation.FairPrice, valuation.GoodPrice, askingPrice.Value));   //Initial width: 100px(Left padding), Total bar width: 268px in AMP
                    }
                    return Content(HttpStatusCode.OK, valuationReportAmp, _jsonFormatter);
                }
                else
                {
                    return Content(HttpStatusCode.OK, report, _jsonFormatter);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        private void ValidateInput(short year, int version, int city, int? askingPrice, int? kms)
        {
            if (year <= 0)
            {
                ModelState.AddModelError("Year", "Invalid Year");
            }
            if (version <= 0)
            {
                ModelState.AddModelError("Car", "Invalid CarId");
            }
            if (city <= 0)
            {
                ModelState.AddModelError("City", "Invalid City");
            }
            if (askingPrice != null && askingPrice <= 0)
            {
                ModelState.AddModelError("Asking Price", "Invalid Price");
            }
            if (kms != null && (kms <= 0 || kms >= 1000000))
            {
                ModelState.AddModelError("Kms", "Invalid Kms");
            }
        }
    }
}
