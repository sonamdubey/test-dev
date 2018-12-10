using AutoMapper;
using Carwale.BL.Classified.CarValuation;
using Carwale.DTOs.Classified;
using Carwale.DTOs.Classified.CarValuation;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarValuation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Configuration;
using Carwale.Utility;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.BL.Stock;

namespace Carwale.Service.Controllers.Classified
{
    public class CarValuationController : ApiController
    {
        private readonly ICarValuation _carValuation;
        private static readonly string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];
        public static readonly ushort defaultImageQuality = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["DefaultImageQuality"] ?? "85");

        public CarValuationController(ICarValuation carValuation)
        {
            _carValuation = carValuation;
        }

        //[AthunticateBasic]
        public IHttpActionResult Get()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            var sourceId = ((string[])(Request.Headers.GetValues("sourceid")))[0];

            if (sourceId == "74") // For the mistake in Android App Release as there was no better known option available(month in android app starts from 0)
            {
                if (!string.IsNullOrEmpty(nvc["mfg_month"]))
                {
                    nvc["mfg_month"] = (Convert.ToInt16(nvc["mfg_month"].ToString()) + 1).ToString();
                }
            }

            if (!ValuationInputs.Validate(nvc))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var valuationRequest = new ValuationRequest()
            {
                VersionId = Convert.ToInt16(nvc["version"]),
                RequestSource = Convert.ToInt16(nvc["source"]),
                ManufactureYear = Convert.ToInt16(nvc["mfg_year"]),
                ManufactureMonth = Convert.ToInt16(nvc["mfg_month"]),
                KmsTraveled = Convert.ToInt32(nvc["kms"]),
                CityID = Convert.ToInt16(nvc["city"]),
                ActualCityID = Convert.ToInt16(nvc["city"])
            };

            CarValuationResults valuationResults = _carValuation.GetValuation(valuationRequest);
            Mapper.CreateMap<CarValuationResults, ValuationResultsDTO>();
            ValuationResultsDTO valuationResultsDTO = Mapper.Map<CarValuationResults, ValuationResultsDTO>(valuationResults);

            if (sourceId != "1")
            {
                ResultsRecommendation valuationRecommendations = _carValuation.GetValuationSuggestions(valuationRequest, valuationResults);

                Mapper.CreateMap<StockBaseEntity, ValuationRecommendationDTO>()
                    .ForMember(dto => dto.CarDetailUrl, map => map.MapFrom(stock => "http://" + _hostUrl + "/api/UsedCarDetails/?car=" + stock.ProfileId))
                    .ForMember(dto => dto.ImageUrlSmall, map => map.MapFrom(stock => ImageSizes.CreateImageUrl(stock.HostUrl, ImageSizes._310X174, stock.OriginalImgPath, defaultImageQuality)))
                    .ForMember(dto => dto.ImageUrlMedium, map => map.MapFrom(stock => ImageSizes.CreateImageUrl(stock.HostUrl, ImageSizes._762X429, stock.OriginalImgPath, defaultImageQuality)))
                    .ForMember(dto => dto.CertificationScore, map => map.MapFrom(stock => StockCertificationBL.FormatCertificationScore(stock.CertificationScore)));

                var valuationData = new ValuationMobileDTO()
                {
                    ValuationResults = valuationResultsDTO,
                    ValuationRecommendations = Mapper.Map<List<StockBaseEntity>, List<ValuationRecommendationDTO>>(valuationRecommendations.ResultsData.ToList())
                };
                return Ok(valuationData);
            }
            else
            {
                return Ok(valuationResultsDTO);
            }
        }
    }
}
