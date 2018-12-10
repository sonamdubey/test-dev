using AutoMapper;
using Carwale.BL.Stock;
using Carwale.DAL.CoreDAL;
using Carwale.DTOs.Stock;
using Carwale.DTOs.Stock.Details;
using Carwale.DTOs.Stock.SimiliarCars;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Utility;
using Carwale.Utility.Classified;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Carwale.Service.Controllers.Stock
{
    public class StocksController : ApiController
    {
        private readonly IGetUsedCarDealerStatus _getUsedCarDealerStatus;
        private readonly IStockBL _stockBL;
        private readonly IStockRecommendationsBL _stockRecommendationsBL;
        private readonly ISearchBL _searchBL;
        private readonly IESStockQuery _ESStockQuery;
        private readonly IStockScoreRepository _stockScoreRepository;
        private readonly ICarDataLogic _carDataLogic;

        private static readonly string _usedCarsStockQueueName = ConfigurationManager.AppSettings["UsedCarStockQueue"];
        private static readonly string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public StocksController(IGetUsedCarDealerStatus getUsedCarDealerStatus, IStockBL stockBL,
            IStockRecommendationsBL stockRecommendationsBL, ISearchBL searchBL, IESStockQuery ESStockQuery, IStockScoreRepository stockScoreRepository,
            ICarDataLogic carDataLogic)
        {
            _getUsedCarDealerStatus = getUsedCarDealerStatus;
            _stockBL = stockBL;
            _stockRecommendationsBL = stockRecommendationsBL;
            _searchBL = searchBL;
            _ESStockQuery = ESStockQuery;
            _stockScoreRepository = stockScoreRepository;
            _carDataLogic = carDataLogic;
        }

        [ValidateModel("stocks"), HandleException, ApiAuthorization, LogApi]
        public IHttpActionResult Post([FromBody]StockList stocks)
        {
            return ProcessStocksRequest(stocks);
        }

        [HttpPost,Route("api/v1/stocks/"), HandleException, ValidateModel("stocks"), ApiAuthorization, LogApi]
        public IHttpActionResult CreateStocks(StockList stocks)
        {
            if(!StockBL.AreCtePackagesValid(stocks.Stocks))
            {
                ModelState.AddModelError("stocks", "Wrong cte package id provided");
            }
            return ProcessStocksRequest(stocks);
        }


        [ApiAuthorization, HandleException, ValidateModel("stocks"), LogApi]
        public IHttpActionResult Put([FromBody]StockList stocks)
        {
            return ProcessStocksRequest(stocks);
        }

        [HttpPut,Route("api/v1/stocks/"), HandleException,ValidateModel("stocks"), ApiAuthorization, LogApi]
        public IHttpActionResult UpdateStocks(StockList stocks)
        {
            if(!StockBL.AreCtePackagesValid(stocks.Stocks))
            {
                ModelState.AddModelError("stocks", "Wrong cte package id provided");
            }
            return ProcessStocksRequest(stocks);
        }

        private IHttpActionResult ProcessStocksRequest(StockList stocks)
        {
            if (stocks.SourceId == 1 && Request.Method.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var status = _getUsedCarDealerStatus.GetDealerStatus(stocks.SellerId);
                if (!status.Equals("ok"))
                {
                    ModelState.AddModelError("stocks", status);
                }
            }
            else if (stocks.Stocks.Any(s => s.Images != null)) // Will execute for Put Request.
            {
                ModelState.AddModelError("stocks.StockList.images", "stocks api cannot update stock images.update using stock images api");
                return BadRequest(ModelState);
            }
            else
            {
                //This method is called from POST and PUT Requests.
                //which are handled by above if-elseif clauses.
                //this else block shouldn't be ideally executed.
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PushToStockQueue(stocks);
            return Ok();
        }

        [ApiAuthorization, LogApi]
        public IHttpActionResult Delete([FromBody]StockDelete deleteStock)
        {
            try
            {
                if (deleteStock == null)
                {
                    ModelState.AddModelError("deleteStock", "deleteStock should not be empty.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stockList = deleteStock.Ids.Select(x => new Carwale.Entity.Stock.Stock(){Id = x} ).ToList();
                var stocks = new StockList()
                {
                    SellerId = deleteStock.SellerId,
                    SellerType = deleteStock.SellerType,
                    SourceId = deleteStock.SourceId,
                    Stocks = stockList
                };

                PushToStockQueue(stocks);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [Route("api/stocks/finance")]
        [ApiAuthorization, LogApi]
        public IHttpActionResult Put([FromBody]List<StockFinance> financeList)
        {
            try
            {
                if (financeList == null || financeList.Count == 0)
                {
                    ModelState.AddModelError("financeList", "List should not be empty.");
                }
                if (financeList != null && financeList.Count > 1000)
                {
                    ModelState.AddModelError("financeList", "List must not contain more than 1000 objects.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                StockFinanceBL.PushToFinanceQueue(financeList);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [Route("api/stocks/{profileId}/")]
        public IHttpActionResult Get(string profileId, [FromUri] int dc = 0)
        {
            try
            {
                int inquiryId = StockBL.GetInquiryId(profileId);
                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                if (carDetails != null && carDetails.BasicCarInfo != null)
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                if (carDetails.IsSold)
                                {
                                    return StatusCode(HttpStatusCode.NoContent);
                                }
                                else
                                {
                                    StockApp stock = Mapper.Map<CarDetailsEntity, StockApp>(carDetails);
                                    stock.DeliveryCityId = dc;
                                    stock.DeliveryText = _stockBL.GetDeliveryText(dc);
                                    return Json(stock, _serializerSettings);
                                }
                            }
                        default:
                            {
                                StockDTO stock = Mapper.Map<BasicCarInfo, StockDTO>(carDetails.BasicCarInfo);
                                stock.DealerId = (carDetails.DealerInfo != null && !string.IsNullOrEmpty(carDetails.DealerInfo.DealerId)) ? Convert.ToInt32(carDetails.DealerInfo.DealerId) : 0;
                                if (carDetails.StockPackageInfo != null)
                                {
                                    stock.PackageId = carDetails.StockPackageInfo.PackageId;
                                    stock.PackageStartDate = carDetails.StockPackageInfo.PackageStartDate;
                                }
                                return Json(stock, _serializerSettings);
                            }
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/basicinfo")]
        [AuthenticateBasic]
        public IHttpActionResult GetBasicInfo(string profileId)
        {
            try
            {
                int inquiryId = StockBL.GetInquiryId(profileId);
                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                if (carDetails != null && carDetails.BasicCarInfo != null && !carDetails.IsSold)
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                BasicInfoApp basicInfo = new BasicInfoApp
                                {
                                    Overview = StockBL.GetOverview(carDetails.BasicCarInfo)
                                };
                                var carDataPresentation = _carDataLogic.GetCombinedCarData(new List<int> { Convert.ToInt32(carDetails.BasicCarInfo.VersionId) });
                                if (carDataPresentation.IsNotNullOrEmpty())
                                {
                                    //carDataPresentation will have only one item since we are passing only one versionId to GetCombinedCarData
                                    basicInfo.Features = Mapper.Map<List<FeatureApp>>(carDataPresentation[0].Features.Where(x => !x.CategoryName.Equals("Manufacturer Warranty", StringComparison.OrdinalIgnoreCase) && x.Items.IsNotNullOrEmpty()));
                                    basicInfo.Specifications = Mapper.Map<List<SpecificationApp>>(carDataPresentation[0].Specifications);
                                }
                                return Json(basicInfo, _serializerSettings);
                            }
                        default:
                            return BadRequest("Invalid SourceId.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/extrainfo")]
        [AuthenticateBasic]
        public IHttpActionResult GetExtraInfo(string profileId)
        {
            try
            {
                int inquiryId = StockBL.GetInquiryId(profileId);
                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                if (carDetails != null &&
                    ((carDetails.OwnerComments != null && (!String.IsNullOrEmpty(carDetails.OwnerComments.SellerNote) || !String.IsNullOrEmpty(carDetails.OwnerComments.ReasonForSell))) ||
                    (carDetails.Modifications != null && !String.IsNullOrEmpty(carDetails.Modifications.Comments)) ||
                    (carDetails.IndividualWarranty != null && !String.IsNullOrEmpty(carDetails.IndividualWarranty.WarrantyDescription))))
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                ExtraInfoApp extraInfo = Mapper.Map<CarDetailsEntity, ExtraInfoApp>(carDetails);
                                return Json(extraInfo, _serializerSettings);
                            }
                        default:
                            return BadRequest("Invalid SourceId.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/condition")]
        [AuthenticateBasic]
        public IHttpActionResult GetCondition(string profileId)
        {
            try
            {
                int inquiryId = StockBL.GetInquiryId(profileId);
                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                if (carDetails != null && carDetails.NonAbsureCarCondition != null && !String.IsNullOrEmpty(carDetails.NonAbsureCarCondition.OverAll))
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                CarConditionApp condition = Mapper.Map<CarDetailsEntity, CarConditionApp>(carDetails);
                                return Json(condition, _serializerSettings);
                            }
                        default:
                            return BadRequest("Invalid SourceId.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/similar")]
        public IHttpActionResult GetSimilarStocks(string profileId)
        {
            try
            {
                int inquiryId = StockBL.GetInquiryId(profileId);
                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                List<StockBaseEntity> recommendations = _stockRecommendationsBL.GetRecommendations(profileId.ToUpper());
                if (recommendations != null && recommendations.Count > 0)
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                SimilarStocksApp similarStocks = new SimilarStocksApp();
                                similarStocks.HostUrl = ConfigurationManager.AppSettings["CDNHostURL"];
                                similarStocks.Stocks = Mapper.Map<List<StockBaseEntity>, List<StockSummaryApp>>(recommendations);
                                return Json(similarStocks, _serializerSettings);
                            }
                        default:
                            {
                                SimilarStocksDTO similarStocks = new SimilarStocksDTO();
                                similarStocks.HostUrl = ConfigurationManager.AppSettings["CDNHostURL"];
                                similarStocks.Stocks = Mapper.Map<List<StockBaseEntity>, List<StockSummaryDTO>>(recommendations);
                                return Json(similarStocks, _serializerSettings);
                            }
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/stocks/{profileId}/rank/"), ApiAuthorization, HandleException]
        public IHttpActionResult GetStockScore(string profileId, [FromUri]SearchParams searchParams)
        {
            if (!StockValidations.IsProfileIdValid(profileId))
            {
                return BadRequest($"Invalid profileId {profileId}");
            }

            StockBaseEntity stock = _ESStockQuery.GetStockByProfileId(ElasticClientInstance.GetInstance(), profileId.ToUpper());
            if (stock == null)
            {
                return BadRequest($"stock {profileId} not live");
            }

            return Ok(new
            {
                sortScore = stock.SortScore,
                rank = _searchBL.GetStocksCountByField(searchParams, "sortScore", Convert.ToDouble(stock.SortScore), true),
                price = stock.Price,
                kilometer = stock.Km,
                city = stock.CityId,
                photoCount = stock.PhotoCount,
                leadCount = stock.Responses,
                alteredScoreExpiry = _stockScoreRepository.GetStockScore(profileId)?.ExpiryDate,
                urlWithCity = $"{_hostUrl}/api/stocks/{profileId}/rank/?city={stock.CityId}",
                urlWithCityMake = $"{_hostUrl}/api/stocks/{profileId}/rank/?city={stock.CityId}&car={stock.MakeId}",
                urlWithCityMakeRoot = $"{_hostUrl}/api/stocks/{profileId}/rank/?city={stock.CityId}&car={stock.MakeId}.{stock.RootId}"
            });
        }

        [HttpGet, Route("api/stocks/registrations/{regNo}"), HandleException]
        public IHttpActionResult GetDetailsPageUrl(string regNo)
        {
            if(string.IsNullOrWhiteSpace(regNo))
            {
                return BadRequest("registration number cannot be empty");
            }
            var result = _stockBL.GetDetailsPageUrlFromRegistrationNumber(regNo);
            if(string.IsNullOrWhiteSpace(result))
            {
                return NotFound();
            }
            return Ok(result);
        }

        private void PushToStockQueue(StockList stocks)
        {
            foreach (var stock in stocks.Stocks)
            {
                var stockWrapper = new StockWrapper()
                {
                    Stock = stock,
                    OperationType = Request.Method.ToString()
                };
                stockWrapper.Stock.SellerId = stocks.SellerId;
                stockWrapper.Stock.SellerType = stocks.SellerType;
                stockWrapper.Stock.SourceId = stocks.SourceId;
                if (String.IsNullOrWhiteSpace(stockWrapper.Stock.RegistrationPlace))
                {
                    stockWrapper.Stock.RegistrationPlace = null;
                }
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("StockApiWrapper", JsonConvert.SerializeObject(stockWrapper));
                RabbitMqPublish usedCarsStockQueue = new RabbitMqPublish();
                usedCarsStockQueue.PublishToQueue(_usedCarsStockQueueName, nvc);
            }
        }
    }
}
