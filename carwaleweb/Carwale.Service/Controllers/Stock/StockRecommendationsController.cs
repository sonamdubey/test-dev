using AutoMapper;
using Carwale.BL.Stock;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.DTOs.Stock.SimiliarCars;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.Utility;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Stock
{
    public class StockRecommendationsController : ApiController
    {
        private readonly IStockRecommendationsBL _stockRecommendationsBL;

        public StockRecommendationsController(IStockRecommendationsBL stockRecommendationsBL)
        {
            _stockRecommendationsBL = stockRecommendationsBL;
        }

        [HandleException, ValidateModel("recoParams"), EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult Get([FromUri]StockRecoParams recoParams)
        {
            string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            Platform requestSource;
            if (!string.IsNullOrWhiteSpace(ampOrigin))
            {
                requestSource = Platform.CarwaleMobile;
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }
            else
            {
                requestSource = HttpContextUtils.GetHeader<Platform>("SourceId");
            }
            List<StockBaseEntity> recommendations;
            switch (requestSource)
            {
                case Platform.CarwaleDesktop:
                case Platform.CarwaleMobile:
                case Platform.CarwaleAndroid:
                case Platform.CarwaleiOS:
                    {
                        recommendations = _stockRecommendationsBL.GetRecommendations(recoParams, (int)requestSource);
                        return ProcessRecommendations(recommendations, requestSource);
                    }
                default:
                    return BadRequest("Incorrect Source Id");
            }
        }

        private IHttpActionResult ProcessRecommendations(List<StockBaseEntity> recommendations, Platform requestSource)
        {
            if (recommendations.IsNotNullOrEmpty())
            {
                if (requestSource == Platform.CarwaleMobile || requestSource == Platform.CarwaleDesktop)
                {
                    var result = recommendations.Select((stock, i) =>
                    {
                        var dto = Mapper.Map<StockSummaryDTO>(stock);
                        dto.Rank = i;
                        dto.Url = StockBL.AddRankInUrl(stock.Url, (i + 1));
                        return dto;
                    });
                    return Ok(result);
                }
                else
                {
                    var result = Mapper.Map<List<StockBaseEntity>, List<UsedCar>>(recommendations);
                    return Ok(result);
                }
            }
            return Ok(recommendations);
        }
    }
}