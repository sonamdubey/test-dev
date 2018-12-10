using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.Deals;
using Carwale.Interfaces.Deals;
using Carwale.Utility;
using System;
using Carwale.Notifications;
using Carwale.Entity.Dealers;
using Carwale.BL.PaymentGateway;
using Carwale.Interfaces;
using Carwale.Entity.Enum;
using System.Collections.Generic;
using Newtonsoft.Json;
using Carwale.Interfaces.Deals.Cache;
using Carwale.DTOs.Deals;
using AutoMapper;
using System.Linq;
using System.Net.Http.Headers;
using System.Configuration;
using System.Net.Http;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Campaigns;
using Carwale.DAL.ApiGateway;
using AEPLCore.Cache;

namespace Carwale.BL.Deals
{
    public class DealsBL : DealsBLBase, IDeals
    {
        private readonly IRepository<DealsInquiryDetail> _dealsRepository;
        private readonly IDealsCache _dealsCache;
        public IDealsCache dealsCache { get { return _dealsCache; } }
        private readonly IDealsRepository _dealsRepo;
        private readonly ICampaign _campaignBL;

        public DealsBL(IRepository<DealsInquiryDetail> dealsRepository, IDealsCache dealsCache, IDealsRepository dealsRepo , ICampaign campaignBL)
            : base(dealsCache)
        {
            _dealsRepository = dealsRepository;
            _dealsCache = dealsCache;
            _dealsRepo = dealsRepo;
            _campaignBL = campaignBL;
        }

        public bool IsShowDeals(int cityId, bool noCitySelected = false)
        {
            if (noCitySelected && (cityId <=0))
                return true;

            List<Carwale.DTOs.City> CityDetails = GetAdvantageCities() ?? new List<Carwale.DTOs.City>();
            if (CityDetails != null && CityDetails.Where(xx => xx.CityId.Equals(cityId)).Count() > 0)
                return true;
            else
                return false;
        }

        public Carwale.DTOs.Deals.DealsStockDTO GetAdvantageAdContent(int modelId, int cityId, byte subSegmentId, int versionId = 0, bool isVersionSpecific = false)
        {
            Carwale.DTOs.Deals.DealsStockDTO dealsStockDTO = null;
            try
            {
                var dealStock = _dealsCache.GetAdvantageAdContent(modelId, cityId, subSegmentId, versionId);
                dealsStockDTO = Mapper.Map<DealsStock, Carwale.DTOs.Deals.DealsStockDTO>(dealStock);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetAdvantageAdContent()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return dealsStockDTO;
        }

        public Carwale.DTOs.Deals.DealsStockDTO GetOfferOfWeekDetails(int modelId, int cityId)
        {
            Carwale.DTOs.Deals.DealsStockDTO dealsStockDTO = null;
            try
            {
                var dealStock = _dealsCache.GetOfferOfWeekDetails(modelId, cityId);
                dealStock.Model.ModelName = Format.FilterModelName(dealStock.Model.ModelName);
                dealsStockDTO = Mapper.Map<DealsStock, Carwale.DTOs.Deals.DealsStockDTO>(dealStock);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetAdvantageAdContent()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return dealsStockDTO;
        }

        public DiscountSummary GetDealsSummaryByModelandCity(int modelId, int cityId)
        {

            DiscountSummary discountSummary = null;

            try
            {
                var discountDetails = _dealsCache.GetDealsMaxDiscount(modelId, cityId);
                if (discountDetails != null && (discountDetails.MaxDiscount > 0 || (discountDetails.MaxDiscount == 0 && !string.IsNullOrWhiteSpace(discountDetails.Offers))))
                {
                    discountSummary = new DiscountSummary();
                    discountSummary.MaxDiscount = discountDetails.MaxDiscount;
                    discountSummary.CityId = discountDetails.CityId;
                    discountSummary.MaskingName = discountDetails.MaskingName == null ? "" : discountDetails.MaskingName;
                    discountSummary.DealsCount = discountDetails.DealsCount;
                    discountSummary.ModelId = discountDetails.ModelId;
                    discountSummary.VersionId = discountDetails.VersionId;
                    discountSummary.ModelName = discountDetails.ModelName == null ? "" : discountDetails.ModelName;
                    discountSummary.Offers = discountDetails.Offers == null ? "" : discountDetails.Offers;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetDealsSummaryByModelandCity()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return discountSummary;
        }

        public Carwale.DTOs.CarData.DiscountSummaryDTO CarOverviewDiscountSummary(DiscountSummary discountSummary)
        {
            if (discountSummary != null)
                return Mapper.Map<DiscountSummary, Carwale.DTOs.CarData.DiscountSummaryDTO>(discountSummary);
            else
                return new Carwale.DTOs.CarData.DiscountSummaryDTO() { MaskingName = "", ModelName = "" };
        }

        public BookingAndroid_DTO GetStockDetails(int stockId, int cityId)
        {
            BookingAndroid_DTO bookingAndroid_DTO = null;
            try
            {
                DealsStock dealsStock = _dealsRepo.GetStockDetails(stockId, cityId);
                bookingAndroid_DTO = Mapper.Map<DealsStock, BookingAndroid_DTO>(dealsStock);
                bookingAndroid_DTO.Reasons = GetReasonsText(dealsStock, dealsStock.Make.MakeName, dealsStock.Model.ModelName);
                if (dealsStock != null && dealsStock.PriceBreakUpId > 0)
                {
                    bookingAndroid_DTO.BreakUpList = _dealsCache.GetDealsPriceBreakUp(stockId, cityId).BreakUpList;
                    bookingAndroid_DTO.IsBreakUpAvailable = true;
                }
                if (!bookingAndroid_DTO.PriceUpdated)
                    bookingAndroid_DTO.DisclaimerText = "The onroad price shown is a tentative amount. The exact onroad price will be confirmed once you are connected with the dealership.";
                bookingAndroid_DTO.TollFreeNumber = System.Configuration.ConfigurationManager.AppSettings["CarwaleAdvantageMaskingNumber"];
                bookingAndroid_DTO.BookingAmount = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["AgedCarBlockingAmount"]);
                bookingAndroid_DTO.PayBtnText = "PROCEED TO PAY \u20B9 " + bookingAndroid_DTO.BookingAmount;
                bookingAndroid_DTO.OfferList = FillOfferList(GetOffers(stockId.ToString(), cityId));
                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBL.GetStockDetails()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return bookingAndroid_DTO;
        }

        public List<DealsStock> GetRecommendedDeals(string recommendedModelIds, int recommendationCount, int cityId, string userHistory, int dealerId)
        {
            string ModelIds;
            List<DealsStock> recommendedCar = new List<DealsStock>();

            try
            {
                if (userHistory != "" && recommendedModelIds != "")
                {
                    //Get Distinct ModelIds
                    ModelIds = GetDistinctModelIds(userHistory, recommendedModelIds);
                }
                else if (userHistory == "")
                    ModelIds = recommendedModelIds;
                else
                    ModelIds = userHistory;

                //Get the Cars to be recommended from the database
                recommendedCar = _dealsRepo.GetRecommendedDeals(ModelIds, recommendationCount, cityId, dealerId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "DealsBL" + "Carwale.BL.Deals.GetRecommendedDeals " + err.ToString());
                objErr.SendMail();
            }

            return recommendedCar;
        }

        public int[] GetIntArrayforModelIdString(string modelIds)
        {
            string[] modelIdArray = { };
            int[] modelId = { };

            if (!String.IsNullOrEmpty(modelIds))
            {
                modelIdArray = (modelIds.Split(','));
                modelId = modelIdArray.Select(int.Parse).ToArray();
            }
            return modelId;
        }

        private string GetDistinctModelIds(string userHistory, string recommendedModelIds)
        {
            string finalModelIds = "";
            string allModelIds = recommendedModelIds + "," + userHistory;
            int[] modelIds = GetIntArrayforModelIdString(allModelIds);
            int[] distinctModelId = modelIds.Distinct().ToArray();
            for (int model = 0; model < distinctModelId.Length; model++)
            {
                finalModelIds = finalModelIds + ',' + distinctModelId[model];
            }
            finalModelIds = finalModelIds.TrimStart(',');
            return finalModelIds;
        }

        public List<DealsRecommendationDTO> GetDealsRecommendation(string recommendedModelIds, int recommendationCount, int cityId, string userHistory, int dealerId, string currentModel)
        {
            List<DealsStock> dealsRecommendation = new List<DealsStock>();
            List<DealsRecommendationDTO> recommendedDeals = new List<DealsRecommendationDTO>();
            try
            {
                userHistory = RemoveCurrentModel(userHistory, currentModel);
                dealsRecommendation = GetRecommendedDeals(recommendedModelIds, recommendationCount, cityId, userHistory, dealerId);
                recommendedDeals = Mapper.Map<List<DealsStock>, List<DealsRecommendationDTO>>(dealsRecommendation);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetDealsRecommendation()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return recommendedDeals;
        }

        public string RemoveCurrentModel(string userHistory, string currentModel)
        {
            if (!String.IsNullOrEmpty(currentModel))
            {
                userHistory = userHistory.Replace(currentModel, "");
                if (userHistory.IndexOf(",,") > 0)
                    userHistory = userHistory.Replace(",,", ",");
            }
            return userHistory;
        }

        public string RemoveAdvHistoryModels(string advHistory, string pqHistory)
        {
            if (!String.IsNullOrEmpty(advHistory))
            {
                string[] modelIds = (advHistory.Split(','));
                modelIds.Each(model => pqHistory = pqHistory.Replace(model, ""));
                if (pqHistory.IndexOf(",,") > 0)
                    pqHistory = pqHistory.Replace(",,", ",");
            }
            return pqHistory;
        }

        private List<DealsStock> GetOrderedHistoryDeals(string history, List<DealsStock> historyDeals, int recommendationCount)
        {
            List<DealsStock> tempDeals = new List<DealsStock>();
            List<DealsStock> orderedDeals = new List<DealsStock>();
            string[] historyArray;
            try
            {
                if (!String.IsNullOrWhiteSpace(history))
                {
                    historyArray = history.Split(',');
                    foreach (var model in historyArray)
                    {
                        if (orderedDeals.Count <= recommendationCount)
                        {
                            tempDeals = historyDeals.Where(pq => pq.Model.ModelId == Convert.ToInt32(model)).ToList();
                            if (tempDeals.Count > 0 && tempDeals[0].Savings > 0)
                                orderedDeals.Add(tempDeals[0]);
                        }
                        else
                            break;
                    }
                    orderedDeals.AddRange(historyDeals.Where(pq => pq.IsSimilarCar == true).ToList());
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetOrderedHistoryDeals()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return orderedDeals;
        }

        public List<DealsRecommendationDTO> GetAdvUserHistoryDeals(string advHistory, int recommendationCount, int cityId, int currentModel)
        {
            List<DealsRecommendationDTO> advUserHistory;
            List<DealsStock> advUserHistoryDeals;

            try
            {
                advUserHistoryDeals = GetRecommendedDeals(String.Empty, advHistory.Split(',').Length, cityId, RemoveCurrentModel(advHistory, currentModel.ToString()), 0);
                advUserHistory = Mapper.Map<List<DealsStock>, List<DealsRecommendationDTO>>(GetOrderedHistoryDeals(advHistory, advUserHistoryDeals, recommendationCount));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetAdvUserHistoryDeals()\n Exception : " + ex.Message);
                objErr.LogException();
                advUserHistory = new List<DealsRecommendationDTO>();
            }
            return advUserHistory;
        }
        /// <summary>
        /// This function brings the deals for pqhistory models and remove deals which are alreadty present in advhistory
        /// </summary>
        /// <param name="advHistory"></param>
        /// <param name="pqHistory"></param>
        /// <param name="cityId"></param>
        /// <returns> List of DealsRecommendationDTO</returns>
        public List<DealsRecommendationDTO> GetPQUserHistoryDeals(List<DealsRecommendationDTO> advHistory, int recommendationCount, string pqHistory, int cityId, int currentModel)
        {
            List<DealsRecommendationDTO> pqUserHistory;
            try
            {
                if (!String.IsNullOrWhiteSpace(pqHistory) && advHistory != null)
                {
                    List<DealsStock> pqUserHistoryDeals = GetRecommendedDeals(String.Empty, pqHistory.Split(',').Length, cityId, pqHistory, 0);

                    foreach (var model in advHistory)
                        pqUserHistoryDeals = pqUserHistoryDeals.Where(pq => pq.Model.ModelId != model.Model.ModelId).ToList();

                    pqUserHistoryDeals = pqUserHistoryDeals.Where(pq => pq.Model.ModelId != currentModel).ToList();

                    pqUserHistoryDeals = GetOrderedHistoryDeals(pqHistory, pqUserHistoryDeals, recommendationCount);
                    pqUserHistory = Mapper.Map<List<DealsStock>, List<DealsRecommendationDTO>>(pqUserHistoryDeals);
                }
                else
                    pqUserHistory = new List<DealsRecommendationDTO>();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetPQUserHistoryDeals()\n Exception : " + ex.Message);
                objErr.LogException();
                pqUserHistory = new List<DealsRecommendationDTO>();
            }
            return pqUserHistory;

        }

        /// <summary>
        /// This function will bring the best saving cars for a city and remove the current modelid from it
        /// </summary>
        /// <param name="recommendationCount"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns> List of DealsRecommendationDTO </returns>
        public List<DealsRecommendationDTO> GetBestSavingsCar(int recommendationCount, int modelId, int cityId)
        {
            List<DealsRecommendationDTO> bestSavings;
            try
            {
                List<DealsStock> bestSavingDeals = _dealsCache.GetDealsByDiscount(cityId, recommendationCount);
                bestSavingDeals = bestSavingDeals.Where(bs => bs.Model.ModelId != modelId).ToList();
                bestSavings = Mapper.Map<List<DealsStock>, List<DealsRecommendationDTO>>(bestSavingDeals);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.GetBestSavingsCar()\n Exception : " + ex.Message);
                objErr.LogException();
                bestSavings = new List<DealsRecommendationDTO>();
            }
            return bestSavings;
        }

        public DiscountSummaryDTO_Android GetDealsSummaryByModelandCity_Android(int modelId, int cityId)
        {
            DiscountSummary discountSummary = GetDealsSummaryByModelandCity(modelId, cityId);
            DiscountSummaryDTO_Android discountSummaryAndroid = Mapper.Map<DiscountSummary, DiscountSummaryDTO_Android>(discountSummary);
            if (discountSummaryAndroid != null)
            {
                discountSummaryAndroid.LinkText = GetAndroidLinkText(discountSummary, cityId);
            }
            return discountSummaryAndroid;
        }

        public bool AutobizPushPaidLead(bool isPaymentSuccess, string sourceIdForAutobiz, DealsStock dealsStock)
        {
            bool isPushResponse = false;
            try
            {
                DealerInquiryDetails dealerInquiryDetails = new DealerInquiryDetails();
                dealerInquiryDetails.Name = PGCookie.CustomerName;
                dealerInquiryDetails.Email = PGCookie.CustomerEmail;
                dealerInquiryDetails.Mobile = PGCookie.CustomerMobile;
                dealerInquiryDetails.VersionId = dealsStock != null ? (int)dealsStock.Version.VersionId : -1;
                dealerInquiryDetails.CityId = Convert.ToInt32(PGCookie.CustomerCity);
                dealerInquiryDetails.ModelId = dealsStock != null ? (int)dealsStock.Model.ModelId : -1;
                dealerInquiryDetails.RequestType = 1;
                dealerInquiryDetails.BuyTimeValue = 7;
                dealerInquiryDetails.InquirySourceId = sourceIdForAutobiz;
                dealerInquiryDetails.IsPaymentSuccess = isPaymentSuccess;
                dealerInquiryDetails.DealsStockId = dealsStock != null ? (int)dealsStock.StockId : 0;
                dealerInquiryDetails.DealerId = dealsStock != null ? (int)dealsStock.DealerId : 0;
                APIResponseEntity apiResponseEntity = Request(dealerInquiryDetails);
                PostBookingOpr(dealerInquiryDetails.DealsStockId, apiResponseEntity.ResponseId);

                DealsInquiryDetail dealsInquiryDetail = new DealsInquiryDetail();
                dealsInquiryDetail.CustomerName = PGCookie.CustomerName;
                dealsInquiryDetail.CustomerMobile = PGCookie.CustomerMobile;
                dealsInquiryDetail.CustomerEmail = PGCookie.CustomerEmail;
                dealsInquiryDetail.ResponseId = apiResponseEntity.ResponseId;
                dealsInquiryDetail.StockId =  dealsStock != null ? (int)dealsStock.StockId : -1;
                dealsInquiryDetail.RecordId = Convert.ToInt32(PGCookie.ResponseId);
                dealsInquiryDetail.IsPaid = 1;
                isPushResponse = _dealsRepository.Update(dealsInquiryDetail);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.AutobizPushPaidLead()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return isPushResponse;
        }

        public bool AutoBizDealsLeadProcessApp(DealerInquiryDetails dealerInquiryDetails, bool IsPaymentSuccess)
        {
            bool isPushResponse = false;
            try
            {
                dealerInquiryDetails.RequestType = 1;
                dealerInquiryDetails.BuyTimeValue = 7;
                dealerInquiryDetails.InquirySourceId = ((int)LeadSourceIdForAutobiz.CarwaleDesktop).ToString();
                dealerInquiryDetails.IsPaymentSuccess = IsPaymentSuccess;
                APIResponseEntity apiResponseEntity = Request(dealerInquiryDetails);
                PostBookingOpr(dealerInquiryDetails.DealsStockId, apiResponseEntity.ResponseId);

                DealsInquiryDetail dealsInquiryDetail = new DealsInquiryDetail();
                dealsInquiryDetail.CustomerName = dealerInquiryDetails.Name;
                dealsInquiryDetail.CustomerMobile = dealerInquiryDetails.Mobile;
                dealsInquiryDetail.CustomerEmail = dealerInquiryDetails.Email;
                dealsInquiryDetail.ResponseId = apiResponseEntity.ResponseId;
                dealsInquiryDetail.StockId = dealerInquiryDetails.DealsStockId;
                dealsInquiryDetail.RecordId = (int)(dealerInquiryDetails.PQId);
                dealsInquiryDetail.IsPaid = 1;
                isPushResponse = _dealsRepository.Update(dealsInquiryDetail);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.AutoBizDealsLeadProcess()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return isPushResponse;
        }

        private object GetInquiryJSON(DealerInquiryDetails dealerInquiryDetails)
        {
            return new
            {
                CustomerName = dealerInquiryDetails.Name,
                CustomerEmail = dealerInquiryDetails.Email,
                CustomerMobile = dealerInquiryDetails.Mobile,
                VersionId = dealerInquiryDetails.VersionId,
                BranchId = dealerInquiryDetails.DealerId,
                IsPaymentSuccess = dealerInquiryDetails.IsPaymentSuccess,
                CityId = dealerInquiryDetails.CityId,
                DealsStockId = dealerInquiryDetails.DealsStockId,
                InquirySourceId = dealerInquiryDetails.InquirySourceId,
                Eagerness = -1,
                IsAutoVerified = false,
                ApplicationId = 1
            };
        }

        private APIResponseEntity Request(DealerInquiryDetails dealerInquiryDetails)
        {
            APIResponseEntity responseEntity = new APIResponseEntity();
            try
            {
                var inquiryJSON = GetInquiryJSON(dealerInquiryDetails);
                ulong inquiryId;
                ulong.TryParse(PushLeadToAutobiz(inquiryJSON), out inquiryId);
                responseEntity.ResponseId = inquiryId;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.Request()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return responseEntity;
        }

        private bool PostBookingOpr(int stockId, ulong inquiryId)
        {
            bool isSuccess = false;
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["DealsAutobizHostUrl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = client.PostAsJsonAsync(String.Format("deals/api/postbookingopr/?stockId={0}&inquiryId={1}", stockId, inquiryId), "").Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarDealsBL.PostBookingOpr()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return isSuccess;
        }

        public List<DealsSummaryDesktop_DTO> GetDealsByDiscount(int cityId, int carCount)
        {
            List<DealsStock> dealsSummaryList = _dealsCache.GetDealsByDiscount(cityId, carCount);
            List<DealsSummaryDesktop_DTO> dealsSummaryListDesktop = Mapper.Map<List<DealsStock>, List<DealsSummaryDesktop_DTO>>(dealsSummaryList);
            return dealsSummaryListDesktop;
        }

        public List<CarMakesDTO> GetDealMakesByCity(int cityId)
        {
            List<MakeEntity> dealsMake = _dealsCache.GetDealMakesByCity(cityId);
            List<CarMakesDTO> dealsMakeDTO = Mapper.Map<List<MakeEntity>, List<CarMakesDTO>>(dealsMake);
            return dealsMakeDTO;
        }

        public List<CarModelsDTO> GetDealModelsByMakeAndCity(int cityId, int makeId)
        {
            List<ModelEntity> dealsModels = _dealsCache.GetDealModelsByMakeAndCity(cityId, makeId);
            List<CarModelsDTO> dealsModelsDTO = Mapper.Map<List<ModelEntity>, List<CarModelsDTO>>(dealsModels);
            return dealsModelsDTO;
        }

        public List<DealsSummaryMobile_DTO> GetDealsByDiscountMobile(int cityId, int carCount)
        {
            List<DealsStock> dealsSummaryList = _dealsCache.GetDealsByDiscount(cityId, carCount);
            List<DealsSummaryMobile_DTO> dealsSummaryListMobile = Mapper.Map<List<DealsStock>, List<DealsSummaryMobile_DTO>>(dealsSummaryList);
            return dealsSummaryListMobile;
        }

        public ProductDetailsDTO_Android GetProductDetails(int modelId, int versionId, int cityId)
        {
            var productDetails = _dealsCache.GetProductDetails(modelId, versionId, cityId);
            ProductDetailsDTO_Android productDetailsDTO = null;
            try
            {
                if (productDetails != null)
                {
                    string stockIds = string.Empty;
                    foreach (var deal in productDetails.DealsStock)
                    {
                        if (stockIds != "")
                            stockIds += "," + deal.StockId.ToString();
                        else
                            stockIds += deal.StockId.ToString();
                    }
                    productDetails.OffersList = GetOffers(stockIds, cityId);
                    productDetailsDTO = Mapper.Map<Entity.Deals.ProductDetails, ProductDetailsDTO_Android>(productDetails);
                    productDetailsDTO.CarColorsDetails = new List<CarColorAndroid_DTO>();
                    if (productDetails.ModelColorsEntity != null)
                    {
                        foreach (var colors in productDetails.ModelColorsEntity)
                        {
                            var carColorDTO = new CarColorAndroid_DTO();
                            carColorDTO.Deals = new List<DealsStockAndroid_DTO>();
                            int maxDiscount = 0;
                            foreach (var deals in productDetails.DealsStock)
                            {
                                if (colors.ColorId == deals.Color.ColorId)
                                {
                                    carColorDTO.Deals.Add(Mapper.Map<DealsStock, DealsStockAndroid_DTO>(deals));
                                    carColorDTO.Deals.Last().ReasonsSlug = GetReasonsText(deals, productDetailsDTO.Make.MakeName, productDetailsDTO.Model.ModelName);
                                    carColorDTO.Deals.Last().DealsEmiDetails = new DealsEmiDTO();
                                    carColorDTO.Deals.Last().DealsEmiDetails.EmiValue = Carwale.BL.Calculation.Calculation.CalculateEmi(Convert.ToInt32(0.85 * carColorDTO.Deals.Last().OnRoadPrice));
                                    carColorDTO.Deals.Last().DealsEmiDetails.EmiMessage = "Assuming 10.5% rate of interest and a tenure of 60 months. For exact EMI quotes get in touch with CarWale.";
                                    
                                    carColorDTO.Deals.Last().OfferList = FillOfferList(productDetails.OffersList, deals.StockId);
                                    carColorDTO.Deals.Last().CampaignId =  _campaignBL.FetchCampaignIdByDealerId(deals.DealerId);
                                 
                                    if (maxDiscount <= deals.Savings)
                                    {
                                        maxDiscount = deals.Savings;
                                        carColorDTO.CurrentYear = deals.ManufacturingYear;
                                    }
                                }
                            }
                            carColorDTO.CarColor = Mapper.Map<ColorEntity, CarColorDTO>(colors);
                            carColorDTO.CarImage = new CarImageBaseDTO { HostUrl = productDetails.CarImage.HostUrl, OriginalImgPath = "/cars/deals/" + Format.FormatSpecial(productDetailsDTO.Make.MakeName) + "/" + productDetails.Model.MaskingName + "/" + Format.FormatUrlColor(carColorDTO.CarColor.ColorName) + ".jpg" };
                            productDetailsDTO.CarColorsDetails.Add(carColorDTO);
                        }
                    }
                    productDetailsDTO.ButtonText = new ProductButtonText_DTO { GetEmiButtonText = "GET EMI ASSISTANCE", FloatingButtonText = "NEXT", LeadCaptureText = "SHOW FINAL OFFER" };
                    productDetailsDTO.TollFreeNumber = System.Configuration.ConfigurationManager.AppSettings["CarwaleAdvantageMaskingNumber"];
                    productDetailsDTO.BookingAmount = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["AgedCarBlockingAmount"]);
                    productDetailsDTO.Model.ModelName = Format.FilterModelName(productDetailsDTO.Model.ModelName);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBL.GetProductDetails()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return productDetailsDTO;
        }

        public List<DealsRecommendationDTO> GetRecommendationsBySubSegment(int modelId, int cityId = 1)
        {
            List<DealsStock> recommedationsList = _dealsCache.GetRecommendationsBySubSegment(modelId, cityId);
            List<DealsRecommendationDTO> recommendationList = Mapper.Map<List<DealsStock>, List<DealsRecommendationDTO>>(recommedationsList);
            return recommendationList;
        }

        public List<Carwale.DTOs.Deals.DealsStockDTO> GetAllVersionDeals(int modelId, int cityId)
        {
            List<DealsStock> allVersionDeals = _dealsCache.GetAllVersionDeals(modelId, cityId);
            List<Carwale.DTOs.Deals.DealsStockDTO> allVersionDealsList = Mapper.Map<List<DealsStock>, List<Carwale.DTOs.Deals.DealsStockDTO>>(allVersionDeals);
            List<int> dealerIds = allVersionDealsList.Select(x => x.DealerId).Distinct().ToList();
            
            Dictionary<int, int> dealerCampaign = _campaignBL.FetchCampaignByDealers(dealerIds);

            foreach (var version in allVersionDealsList)
            {
                if(dealerCampaign.ContainsKey(version.DealerId))
                {
                    version.CampaignId = dealerCampaign[version.DealerId];
                }
            }
            return allVersionDealsList;
        }

        public string GetAndroidLinkText(DiscountSummary discountSummary, int cityId)
        {
            string linkText = string.Empty;
            try
            {
                if (discountSummary != null)
                {
                    if (discountSummary.MaxDiscount <= 0 && !string.IsNullOrWhiteSpace(discountSummary.Offers))
                    {
                        linkText = "Great offers on " + Format.FilterModelName(discountSummary.ModelName);
                    }
                    else if (discountSummary.MaxDiscount > 0)
                    {
                        if (cityId > 0)
                        {
                            linkText = "<b>\u20B9 " + Format.FormatNumericCommaSep(discountSummary.MaxDiscount.ToString()) + "</b> discount on " + Format.FilterModelName(discountSummary.ModelName);
                        }
                        else
                        {
                            linkText = "Save <b>BIG</b> on this car";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBL.GetAndroidLinkText()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return linkText;
        }

        public List<int> GetCitiesWithMoreModels(int minimumStockCount)
        {
            return _dealsCache.GetCitiesWithMoreModels(minimumStockCount);
        }

        public AdvantageSearchResultsDTO GetDeals(Filters filter)
        {
            filter.StartIndex = (filter.PN - 1) * filter.PS + 1;
            filter.EndIndex = filter.PN * filter.PS;
            filter.Makes = filter.Makes != null ? filter.Makes.Replace(' ', ',') : null;
            filter.Fuels = filter.Fuels != null ? filter.Fuels.Replace(' ', ',') : null;
            filter.Transmissions = filter.Transmissions != null ? filter.Transmissions.Replace(' ', ',') : null;
            if(filter.BodyTypes != null) {
                filter.BodyTypes = filter.BodyTypes.Replace(' ', ',');
                if (filter.BodyTypes.Split(',').Select(int.Parse).Contains(1)) {
                    filter.BodyTypes += ",10";
                }
            }
            AdvantageSearchResults advantage = _dealsRepo.GetDeals(filter);
            advantage.Deals.ForEach(deal => deal.Model.ModelName = Format.FilterModelName(deal.Model.ModelName));
            AdvantageSearchResultsDTO advantageResults = Mapper.Map<AdvantageSearchResults, AdvantageSearchResultsDTO>(advantage);
            
            // advantageResults.Deals.ForEach(x => x.PercentSaving = Convert.ToInt32(Math.Ceiling((double)(x.OnRoadPrice - x.OfferPrice) * 100 / x.OnRoadPrice)));
            if (advantageResults.TotalCount == 0)
                filter.StartIndex = 0;
            advantageResults.VisibleCarsCount = string.Format("{0}-{1}", filter.StartIndex, filter.EndIndex <= advantageResults.TotalCount ? filter.EndIndex : advantageResults.TotalCount);
            int remainingCarCount = advantageResults.TotalCount - (filter.PN * filter.PS);
            if (remainingCarCount > 0)
            {
                advantageResults.nextPageURL = String.Format("{0}advantage/getDeals/?cityId={1}&sc={2}&so={3}&pn={4}&ps={5}", System.Configuration.ConfigurationManager.AppSettings["WebApiHostUrl"], filter.CityId, filter.SC, filter.SO, filter.PN + 1, filter.PS);
            }

            advantageResults.Deals.ForEach(x => x.OfferText = GetOfferText(x.Offers, x.Savings, x.OfferValue));
            return advantageResults;
        }

        public int GetCarCountByCity(int cityId)
        {
            int carCount = 0;
            carCount = _dealsCache.GetCarCountByCity(cityId);
            return carCount;
        }

        public List<DiscountSummaryDTO_Android> BestVersionDealsByModel(int modelId, int cityId)
        {
            var versionList = _dealsCache.BestVersionDealsByModel(modelId, cityId);
            IEnumerable<DiscountSummary> versionsDealList;
            if (versionList != null)
                versionsDealList = versionList.Select(x => x.Value);
            else
                versionsDealList = new List<DiscountSummary>();
            IEnumerable<DiscountSummaryDTO_Android> dealList = Mapper.Map<IEnumerable<DiscountSummary>, IEnumerable<DiscountSummaryDTO_Android>>(versionsDealList);
            return dealList.Select(x => { x.LinkText = GetVersionDealsLinkText(cityId, x.MaxDiscount); x.ModelId = modelId; return x; }).ToList();
        }

        private string GetVersionDealsLinkText(int cityId, int discount)
        {
            string linkText;
            if (discount <= 0)
            {
                linkText = "Great offers";
            }
            else
            {
                if (cityId <= 0)
                {
                    linkText = "Save Big";
                }
                else
                {
                    linkText = String.Format("Save \u20B9 {0}", Format.FormatNumericCommaSep(discount.ToString()));
                }
            }
            return linkText;
        }

        public void PushLeadToAutobiz(DealsInquiryDetailDTO dealerInquiryDetails, bool isPaymentSuccess, int inquirySourceId)
        {
            if (dealerInquiryDetails != null)
            {
                int branchId = _dealsCache.GetDealsDealerId(dealerInquiryDetails.StockId, dealerInquiryDetails.CityId);
                object dealInquiries = new
                {
                    CustomerName = dealerInquiryDetails.CustomerName,
                    CustomerEmail = dealerInquiryDetails.CustomerEmail,
                    CustomerMobile = dealerInquiryDetails.CustomerMobile,
                    BranchId = branchId,
                    IsPaymentSuccess = isPaymentSuccess,
                    CityId = dealerInquiryDetails.CityId,
                    DealsStockId = dealerInquiryDetails.StockId,
                    InquirySourceId = inquirySourceId,
                    Eagerness = -1,
                    IsAutoVerified = false,
                    ApplicationId = 1,
                    CampaignId = _campaignBL.FetchCampaignIdByDealerId(branchId)
                };
                PushLeadToAutobiz(dealInquiries);
            }
        }

        private string PushLeadToAutobiz(object dealInquiries)
        {
            string resContent = "-1";
            string apiUrl = ConfigurationManager.AppSettings["DealsApiHostUrl"];
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = client.PostAsJsonAsync("webapi/NewCarInquiries/Post/", dealInquiries).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resContent = response.Content.ReadAsStringAsync().Result;
                            if (!string.IsNullOrWhiteSpace(resContent))
                                resContent = resContent.Replace("\\", "").Trim(new char[1] { '"' });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBLPushLeadToAutobiz.()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return resContent;
        }




        public List<DealsTestimonialDTO> GetDealsTestimonials(int cityId = 0)
        {
            List<DealsTestimonialEntity> testimonialsList = _dealsCache.GetDealsTestimonials(cityId);
            if (testimonialsList == null)
                testimonialsList = new List<DealsTestimonialEntity>();
            List<DealsTestimonialDTO> testimonials = Mapper.Map<List<DealsTestimonialEntity>, List<DealsTestimonialDTO>>(testimonialsList);
            return testimonials;
        }

        public SimilarDealsRecommendationDTO GetSimilarDeals(int modelId, int cityId)
        {
            SimilarDealsRecommendationDTO similarDeals = new SimilarDealsRecommendationDTO();
            List<DealsStock> recommedationsList = _dealsCache.GetRecommendationsBySubSegment(modelId, cityId);
            List<DealsSummaryDesktop_DTO> recommendationList = Mapper.Map<List<DealsStock>, List<DealsSummaryDesktop_DTO>>(recommedationsList);
            recommendationList.ForEach(x => x.OfferText = GetOfferText(x.Offers, x.Savings, x.OfferValue));
            recommendationList.ForEach(deal => deal.Model.ModelName = Format.FilterModelName(deal.Model.ModelName));
            similarDeals.DealsRecommedations = recommendationList;
            similarDeals.Heading = "Offers You May Like";
            similarDeals.DealsCount = _dealsCache.GetCarCountByCity(cityId);
            return similarDeals;
        }

        private string GetOfferText(string offers, int savings, int offerValue)
        {
            string offerText = null;

            if (!String.IsNullOrWhiteSpace(offers) && savings <= 0 && offerValue <= 0)
                offerText = "Offer available";
            else if (offerValue > 0 && savings <= 0)
                offerText = String.Format("Offers worth \u20B9 {0}", Format.FormatNumericCommaSep(offerValue.ToString()));
            else if (offerValue > 0 && savings > 0)
                offerText = String.Format("Extra offers worth \u20B9 {0}", Format.FormatNumericCommaSep(offerValue.ToString()));
            return offerText;
        }

        public List<KeyValuePair<string, string>> FillOfferList(List<DealsOfferEntity> offerList, int stockId = 0)
        {

            if (offerList == null)
                return null;
            List<KeyValuePair<string, string>> offers = new List<KeyValuePair<string, string>>();
            var offerCategories = new List<string> { "Free Accesories", "Exchange Bonus", "Extended Warranty", "Assured Gifts" };
            try
            {
                foreach (var offer in offerList)
                {
                    if (stockId == 0 || offer.StockId == stockId)
                    {
                        bool contains = offerCategories.Contains(offer.Description, StringComparer.OrdinalIgnoreCase);
                        var offerText = "";
                        if (contains)
                        {
                            offerText = offer.Description;
                            offerText += offer.OfferWorth > 0 ? " worth \u20B9 " + Format.FormatNumericCommaSep(offer.OfferWorth.ToString()) : "";
                            offerText += offer.AdditionalComments != "" ? " (" + offer.AdditionalComments + ")." : ".";
                        }
                        else if (offer.Description == "Generic Offer")
                        {
                            offerText = offer.AdditionalComments;
                            offerText += offer.OfferWorth > 0 ? " worth \u20B9 " + Format.FormatNumericCommaSep(offer.OfferWorth.ToString()) + "." : ".";
                        }
                        else if (offer.Description == "Free Insurance" && offer.OfferWorth > 0)
                        {
                            offerText = "Includes " + offer.Description + " worth \u20B9 " + Format.FormatNumericCommaSep(offer.OfferWorth.ToString()) + ".";
                        }

                        if (offerText != "")
                        {
                            offers.Add(new KeyValuePair<string, string>(offer.Description.Replace(" ", "-").ToLower(), offerText));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBLFillOfferList()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return offers;
        }
        public List<DealsRecommendationDTO> GetDealsSimilarCarsBySubSegment(int modelId, int cityId, int subsegmentId)
        {
            List<DealsStock> similarDeals = _dealsCache.GetDealsSimilarCarsBySubSegment(modelId, cityId, subsegmentId);
            return Mapper.Map<List<DealsStock>, List<DealsRecommendationDTO>>(similarDeals);
        }


        public List<DealsOfferEntity> GetOffers(string stockIds, int cityId)
        {
            List<DealsOfferEntity> offersList = null;
            try
            {
                string offersListJson = string.Empty;
                offersListJson = _dealsCache.GetDealsOfferList(stockIds, cityId);
                offersList = JsonConvert.DeserializeObject<List<DealsOfferEntity>>(offersListJson);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetOffers()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return offersList;
        }

        public DealsPriceBreakupDTO GetDealsPriceBreakup(int stockId, int cityId)
        {
            throw new NotImplementedException();
        }

        public string GetMakeModelVersionColor()
        {
            var data = _dealsRepo.GetMakeModelVersionColor();
            return Format.XMLSerialize<List<MakeModelVersionColor>>(data);
        }


        public IEnumerable<CarVersionsDTO> GetAdvantageVersions(int modelId, int cityId)
        {
            IEnumerable<CarVersionsDTO> versionList = null;
            try
            {
                return Mapper.Map<IEnumerable<VersionEntity>, IEnumerable<CarVersionsDTO>>(_dealsCache.GetAdvantageVersions(modelId, cityId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetAdvantageVersions()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return versionList;
        }


        public IEnumerable<CarColorDTO> GetAdvantageVersionColors(int versionId, int cityId)
        {
            IEnumerable<CarColorDTO> colorList = null;
            try
            {
                return Mapper.Map<IEnumerable<ColorEntity>, IEnumerable<CarColorDTO>>(_dealsCache.GetAdvantageVersionColors(versionId, cityId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetAdvantageVersionColors()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return colorList;
        }


        public IEnumerable<int> GetAdvantageColorYears(int versionId, int colorId, int cityId)
        {
            IEnumerable<int> yearList = null;
            try
            {
                return _dealsCache.GetAdvantageColorYears(versionId, colorId, cityId);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetAdvantageVersionColors()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return yearList;
        }

        public List<Tuple<int,int>> GetDealsByVersionList(List<Tuple<int,int>> modelVersionList, int cityId)
        {
            try
            {
                var memcahcheDeals = CreateKeyList(modelVersionList, cityId);
                if (memcahcheDeals != null)
                {
                    var Deals = _dealsCache.GetDealsByVersionList(memcahcheDeals);
                    List<Tuple<int, int>> versionDeals = new List<Tuple<int, int>>();
                    foreach (var deal in Deals)
                    {
                        if (deal.Value != null)
                        {
                            if (deal.Value.Savings > 0)
                                versionDeals.Add(Tuple.Create(deal.Value.Version.VersionId, deal.Value.Savings));
                            else
                                versionDeals.Add(Tuple.Create(deal.Value.Version.VersionId, 0));                  // if there is only offers(savings = 0) on the version
                        }
                        else
                            versionDeals.Add(Tuple.Create(Convert.ToInt32(GetVersionIdFromKey(deal.Key)), -1));                    // if there is no savings and offer on the version
                    }

                    if (versionDeals == null)
                    {
                        int size = modelVersionList.Count;
                        for (int i = 0; i < size; i++)
                            versionDeals.Add(Tuple.Create(modelVersionList[i].Item2, -1));
                    }
                    return versionDeals;
                }
            }
            catch(Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetDealsByVersionList()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return null;
        }

        private Dictionary<string, MultiGetCallback<DealsStock>> CreateKeyList(List<Tuple<int, int>> modelVersionList, int cityId)
        {
            try
            {
                var memcacheList = new Dictionary<string, MultiGetCallback<DealsStock>>();
                string key = null;
                foreach (var car in modelVersionList)
                {
                    key = string.Format("AdvantageAd-{0}-{1}-{2}-{3}-1", 0, car.Item1, car.Item2, cityId);
                    var memcacheDeal = new MultiGetCallback<DealsStock>();
                    memcacheDeal.DbCallback = () => _dealsRepo.GetAdvantageAdContent(car.Item1, cityId, 0, car.Item2, true);
                    memcacheList.Add(key, memcacheDeal);
                }
                return memcacheList;
            }
            catch(Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CreateKeyList()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return null;
        }

        private string GetVersionIdFromKey(string key)
        {
            string[] arr = key.Split('-');
            return arr[3];
        }

        public List<DealsStockDTO> GetFilteredVersionDeals(int modelId, int versionId, Filters filter)
        {
            try
            {
                List<DealsStockDTO> allVersionDeals = new List<DealsStockDTO>();
                List<DealsStockDTO> filteredVersionDeals = new List<DealsStockDTO>();
                allVersionDeals = GetAllVersionDeals(modelId, filter.CityId);
                if (allVersionDeals != null)
                {
                    filteredVersionDeals =  allVersionDeals.Where(x => x.Version.ID != versionId && x.OfferPrice >= filter.StartBudget && (filter.EndBudget == 0 || x.OfferPrice <= filter.EndBudget)
                    && (String.IsNullOrEmpty(filter.Fuels) || filter.Fuels.Split(',').Contains(x.FuelType.ToString()))
                    && (String.IsNullOrEmpty(filter.Transmissions) || filter.Transmissions.Split(',').Contains(x.TransmissionType.ToString()))).ToList();
                }
                return filteredVersionDeals;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetFilteredVersionDeals()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return null;
        }
    }
}
