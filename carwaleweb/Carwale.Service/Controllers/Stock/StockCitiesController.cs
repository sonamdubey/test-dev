using Carwale.BL.Stock;
using Carwale.DTOs.Stock;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.Utility;
using Carwale.Utility.Classified;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Web.Http;

namespace Carwale.Service.Controllers.Stock
{
    public class StockCitiesController: ApiController
    {
        private readonly IStockCitiesBL _stockCitiesBL;
        private readonly IStockBL _stockBL;
        private static readonly string _invalidProfileIdMsg;
        private static readonly string _stockSoldOutMsg;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public StockCitiesController(IStockCitiesBL stockCitiesBL, IStockBL stockBL)
        {
            _stockCitiesBL = stockCitiesBL;
            _stockBL = stockBL;
        }

        static StockCitiesController()
        {
            _invalidProfileIdMsg = "ProfileId must be valid.";
            _stockSoldOutMsg = "Stock has been sold out.";
        }

        [HttpPut,Route("api/stocks/{profileId}/cities/")]
        [ApiAuthorization, LogApi, HandleException, ValidateModel("stockCities")]
        public IHttpActionResult Put(string profileId, StockCitiesDTO stockCities)
        {
            int inquiryId = StockBL.GetInquiryId(profileId);
            if (inquiryId < 1)
            {
                ModelState.AddModelError("profileId", _invalidProfileIdMsg);
                return BadRequest(ModelState);
            }
            CarDetailsEntity stock = _stockBL.GetStock(profileId);
            if (stock == null || stock.BasicCarInfo == null || stock.IsSold)
            {
                return Content(HttpStatusCode.NotFound, _stockSoldOutMsg);
            }
            bool isAdded = _stockCitiesBL.AddStockCities(inquiryId, StockBL.IsDealerStock(profileId) ? SellerType.Dealer : SellerType.Individual
                , CustomParser.parseIntObject(stock.BasicCarInfo.CityId), stockCities.CityIds);
            if (isAdded)
            {
                _stockBL.RefreshESStock(profileId);
                return Ok("Successfully updated stock cities.");
            }
            else
                return InternalServerError();
        }

        [HttpGet, Route("api/stocks/{profileId}/cities/"), HandleException, LogApi]
        public IHttpActionResult Get(string profileId)
        {
            if (!StockValidations.IsProfileIdValid(profileId))
            {
                ModelState.AddModelError("profileId", _invalidProfileIdMsg);
                return BadRequest(ModelState);
            }
            var cities = _stockCitiesBL.GetStockCities(profileId);
            if (cities == null)
            {
                return Content(HttpStatusCode.NotFound, _stockSoldOutMsg);
            }
            else
            {
                return Json(cities, _serializerSettings);
            }
        }

        [HttpDelete, Route("api/stocks/{profileId}/cities/")]
        [ApiAuthorization, LogApi, HandleException]
        public IHttpActionResult Delete(string profileId)
        {
            int inquiryId = StockBL.GetInquiryId(profileId);
            if (inquiryId < 1)
            {
                ModelState.AddModelError("profileId", _invalidProfileIdMsg);
                return BadRequest(ModelState);
            }
            bool isDeleted = _stockCitiesBL.DeleteStockCities(inquiryId, StockBL.IsDealerStock(profileId) ? SellerType.Dealer : SellerType.Individual);
            if (isDeleted)
            {
                _stockBL.RefreshESStock(profileId);
                return Ok("Successfully deleted stock cities.");
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}
