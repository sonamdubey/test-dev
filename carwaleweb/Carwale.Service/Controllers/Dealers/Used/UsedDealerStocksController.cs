using AutoMapper;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Service.Filters;
using Carwale.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Carwale.Service.Controllers.Dealers.Used
{
    public class UsedDealerStocksController : ApiController
    {
        private readonly IUsedDealerStocksBL _usedDealerStocksBL;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public UsedDealerStocksController(IUsedDealerStocksBL usedDealerStocksBL)
        {
            _usedDealerStocksBL = usedDealerStocksBL;
        }

        [HttpGet]
        [Route("api/dealers/{dealerId}/stocks"), HandleException]
        public IHttpActionResult GetDealerStocks(int dealerId, int from = 0, int size = 0)
        {
            var errorDictionary = _usedDealerStocksBL.ValidateDealerStocksApiInputs(dealerId, from, size);
            if (errorDictionary.Count > 0)
            {
                foreach (var entry in errorDictionary)
                {
                    ModelState.AddModelError(entry.Key, entry.Value);
                }
                return BadRequest(ModelState);
            }
            Platform requestSource = HttpContextUtils.GetHeader<Platform>("sourceId");
            switch (requestSource)
            {
                case Platform.CarwaleAndroid:
                case Platform.CarwaleiOS:
                    {
                        var usedDealerStocksEntity = _usedDealerStocksBL.GetDealerStocksEntity(dealerId, from, size);
                        if (usedDealerStocksEntity?.Stocks != null)
                        {
                            var result = Mapper.Map<IEnumerable<StockBaseEntity>, IEnumerable<UsedCar>>(usedDealerStocksEntity.Stocks);
                            return Json(new { stocks = result.IsNotNullOrEmpty() ? result : null, nextPageUrl = usedDealerStocksEntity.NextPageUrl }, _serializerSettings);
                        }
                        return Content(HttpStatusCode.InternalServerError, "Something went wrong. Please try after some time");
                    }
                default:
                    return BadRequest("Incorrect Source Id");
            }
        }
    }
}
