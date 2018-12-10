using Carwale.Entity.Deals;
using Carwale.Interfaces.Deals;
using System;
using System.Data.SqlClient;
using Carwale.DAL.CoreDAL;
using System.Data;
using Carwale.Notifications;
using System.Web;
using Carwale.Utility;
using Carwale.Entity;
using Carwale.Entity.Geolocation;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Dapper;
using Carwale.Entity.Dealers;
using System.Linq;
using Carwale.Notifications.Logs;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Carwale.DTOs.Deals;

namespace Carwale.DAL.Deals
{
    public class DealsRepository : RepositoryBase, IDealsRepository
    {
        static string apiUrl = ConfigurationManager.AppSettings["DealsAutobizHostUrl"];
        /// <summary>
        /// Created By : Anchal Gupta
        /// Date : 26 Feb, 2016
        /// Description : return  maxDiscount, Masking Name and SKUCount for given City and CarModel
        /// </summary>
        /// <param name="ModelId"></param>
        /// <param name="CityId"></param>
        /// <returns></returns>
        public DiscountSummary GetDealsMaxDiscount(int ModelId, int CityId)
        {
            DiscountSummary dealsMaxCount = new DiscountSummary();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_modelId", ModelId);
                    param.Add("v_cityId", CityId);
                    dealsMaxCount = con.Query<DiscountSummary>("getmaxdiscountbymodel", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("getmaxdiscountbymodel");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Deals.GetDealsMaxDiscount " + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return dealsMaxCount;
        }

        public DealsStock GetAdvantageAdContent(int modelId, int cityId, byte subSegmentId, int versionId, bool isVersionSpecific)
        {
            DealsStock data = null;
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    data = con.Query<MakeEntity, ModelEntity, CarImageBase, City, VersionBase, DealsStock, DealsStock>("getAdvantageAdContent_v16_12_9",
                        (make, model, image, city, version, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.CarImageDetails = image; dealsStock.City = city; dealsStock.Version = version; return dealsStock; },
                        new { v_cityId = cityId, v_modelId = modelId, v_subSegmentId = subSegmentId, v_versionId = versionId, v_isVersionSpecific = isVersionSpecific }, commandType: CommandType.StoredProcedure, splitOn: "ModelId, HostURL, CityId,VersionId,OnRoadPrice").FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("getAdvantageAdContent_v16_12_9");                    
                }                            
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetAdvantageAdContent" + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return data;
        }

        public DealsStock GetStockDetails(int stockId, int cityId)
        {
            DealsStock dealsStockDetails = null;
            try
            {
                using (var con = AdvantageMySqlMasterConnection)
                {
                    dealsStockDetails = con.Query<MakeEntity, ModelEntity, VersionBase, CarImageBase, City, ColorEntity, DealsStock, DealsStock>("getstockdetails",
                        (make, model, version, image, city, color, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.Version = version; dealsStock.CarImageDetails = image; dealsStock.City = city; dealsStock.Color = color; return dealsStock; },
                        new { v_cityid = cityId, v_stockId = stockId }, commandType: CommandType.StoredProcedure, splitOn: "ModelId,VersionId, HostURL, CityId, ColorId, OnroadPrice").FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("getstockdetails");

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Carwale.DAL.Deals.DealsRepository.GetStockDetails() \n Exception :" + ex.Message);
                objErr.SendMail();
                throw ex;
            }
            return dealsStockDetails;
        }

        public DealsPriceBreakupEntity GetDealsPriceBreakUp(int stockId, int cityId)
        {
            DealsPriceBreakupEntity dealsBreakUp = new DealsPriceBreakupEntity();
            string offerListJson = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = client.GetAsync(String.Format("deals/webapi/pricebreakup/?stockId={0}&cityId={1}", stockId, cityId), HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            offerListJson = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Deals.GetDealsPriceBreakUp " + err.ToString());
                objErr.SendMail();
                throw err;
            }
            if(!String.IsNullOrEmpty(offerListJson))
            {
                dealsBreakUp = JsonConvert.DeserializeObject<DealsPriceBreakupEntity>(offerListJson);
            }
            return dealsBreakUp;
        }

        public List<DealsStock> GetRecommendedDeals(string modelIds, int recommendationCount, int cityId, int dealerId)
        {
            List<DealsStock> discountOnModelList = new List<DealsStock>();
            var param = new DynamicParameters();
            param.Add("v_modelid", modelIds);
            param.Add("v_recommendationcount", recommendationCount);
            param.Add("v_cityid", cityId);
            param.Add("v_dealerid", dealerId);
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getmaxdiscountbymodellist");
                    return con.Query<MakeEntity, ModelEntity, VersionBase, City, CarImageBase, DealsStock, DealsStock>("getmaxdiscountbymodellist",
                        (make, model, version, city, carImageBase, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.Version = version; dealsStock.City = city; dealsStock.CarImageDetails = carImageBase; return dealsStock; }, param, commandType: CommandType.StoredProcedure, splitOn: "ModelId,VersionId,CityId,HostUrl,Savings").AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Deals.GetRecommendedDeals " + err.ToString());
                objErr.SendMail();
            }
            return discountOnModelList;
        }

        public Dictionary<int, DiscountSummary> BestVersionDealsByModel(int ModelId, int CityId)
        {
            Dictionary<int, DiscountSummary> versionDiscountByModel = new Dictionary<int, DiscountSummary>();
            List<DiscountSummary> dealsMaxCountForModel = new List<DiscountSummary>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    dealsMaxCountForModel = con.Query<DiscountSummary>("getbestversiondealsbymodel", new { v_cityid = CityId > 0 ? CityId : Convert.DBNull, v_modelId = ModelId }, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("getbestversiondealsbymodel");
                }
                versionDiscountByModel = dealsMaxCountForModel.ToDictionary(x => x.VersionId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Deals.BestVersionDealsByModel " + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return versionDiscountByModel;
        }

        public List<DealsStock> GetDealsByDiscount(int cityId, int carCount)
        {
            List<DealsStock> dealsStockLst = new List<DealsStock>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getDealsbyDiscount");
                    var value = con.Query<MakeEntity, ModelEntity, City, CarImageBase, DealsStock, DealsStock>("getDealsbyDiscount",
                          (make, model, city, carImageBase, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.City = city; dealsStock.CarImageDetails = carImageBase; return dealsStock; }, new { v_cityid = cityId, v_noofcars = carCount }, commandType: CommandType.StoredProcedure, splitOn: "ModelId,CityId,HostUrl,Savings").AsList();                   
                        if (value != null && value.Count > 0) value.ForEach(item => item.CarImageDetails.HostUrl = CWConfiguration._imgHostUrl);                  
                    return value;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Carwale.DAL.Deals.DealsRepository.GetDealsByDiscount() \n Exception :" + ex.Message);
                objErr.SendMail();
                throw ex;
            }
        }

        public List<MakeEntity> GetDealMakesByCity(int cityId)
        {
            List<MakeEntity> dealsMake = new List<MakeEntity>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getdealmakesbycity");
                    return con.Query<MakeEntity>("getdealmakesbycity", new { v_cityid = cityId }, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetDealMakesByCity " + err.ToString());
                objErr.SendMail();
                throw err;
            }
        }

        public List<ModelEntity> GetDealModelsByMakeAndCity(int cityId, int makeId)
        {
            List<ModelEntity> dealsModel = new List<ModelEntity>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getdealmodelsbymakeandcity");
                    return con.Query<ModelEntity>("getdealmodelsbymakeandcity", new { v_cityid = cityId, v_makeid = makeId }, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetDealModelsByMakeAndCity " + err.ToString());
                objErr.SendMail();
                throw err;
            }
        }

        public ProductDetails GetProductDetails(int modelId, int versionId, int cityId)
        {
            ProductDetails productDetails = null;
            dynamic result;
            var param = new DynamicParameters();
            param.Add("v_modelid", modelId);
            param.Add("v_cityid", cityId);
            param.Add("v_versionid", versionId);
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getbestdealdetails_v18_11_13");
                    using (var reader = con.QueryMultiple("getbestdealdetails_v18_11_13", param, commandType: CommandType.StoredProcedure))
                    {
                        productDetails = reader.Read<MakeEntity, ModelEntity, CarImageBase, ProductDetails>((make, model, carImage) => { return new ProductDetails { Make = make, Model = model, CarImage = carImage }; }, splitOn: "ModelName,HostUrl").FirstOrDefault();
                        if (productDetails != null)
                        {
                            productDetails.Version = reader.Read<CarVersionEntity>().AsList();
                            result = reader.Read<ProductDetails>().FirstOrDefault();
                            productDetails.CurrentVersionId = result != null ? result.CurrentVersionId : 0;
                            productDetails.ModelColorsEntity = reader.Read<ColorEntity>().AsList();
                            productDetails.DealsStock = reader.Read<DealsStock, ColorEntity, City, AreaCode, DealsStock>((dealsStock, colorEntity, city, area) => { dealsStock.Color = colorEntity; dealsStock.City = city; dealsStock.Area = area; return dealsStock; }, splitOn: "ColorId, CityId, AreaId").AsList();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetProductDetails " + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return productDetails;
        }

        public List<City> GetAdavantageCities(int ModelId, int VersionId, int MakeId)
        {
            List<City> cityList = new List<City>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getadvantagecities_v17_1_2");
                    cityList = con.Query<City>("getadvantagecities_v17_1_2", new { v_modelid = ModelId, v_versionid = VersionId, v_makeid = MakeId }, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetAdavntageCities" + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return cityList;
        }

        public List<DealsStock> GetRecommendationsBySubSegment(int modelId, int cityId)
        {
            List<DealsStock> recommendationsList = new List<DealsStock>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("dealssimilarcars");
                    recommendationsList = con.Query<MakeEntity, ModelEntity, City, CarImageBase, DealsStock, DealsStock>("dealssimilarcars",
                          (make, model, city, carImageBase, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.City = city; dealsStock.CarImageDetails = carImageBase; return dealsStock; },
                          new { v_cityid = cityId, v_modelid = modelId }, commandType: CommandType.StoredProcedure, splitOn: "ModelId,CityId,HostUrl,Savings").AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetRecommendationsBySubSegment " + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return recommendationsList;
        }

        public List<DealsStock> GetDealsSimilarCarsBySubSegment(int modelId, int cityId, int subsegmentId)
        {
            List<DealsStock> recommendationsList = new List<DealsStock>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("dealsSimilarCarsBySubSegmentId");
                    return con.Query<MakeEntity, ModelEntity, CarImageBase, City, DealsStock, DealsStock>("dealsSimilarCarsBySubSegmentId",
                       (make, model, image, city, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.CarImageDetails = image; dealsStock.City = city; return dealsStock; },
                       new { v_cityid = cityId, v_modelid = modelId, v_subsegmentId = subsegmentId }, commandType: CommandType.StoredProcedure, splitOn: "ModelId, HostURL,CityId, StockCount").AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetDealsSimilarCarsBySubSegment " + err.ToString());
                objErr.SendMail();
                throw err;
            }
        }

        public DealerInquiryDetails GetTransactionDetails(int transactionId)
        {
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("GetTransactionDetails");
                    return con.Query<DealerInquiryDetails>("GetTransactionDetails", new { v_transid = transactionId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetTransactionDetails" + err.ToString());
                objErr.SendMail();
            }
            return null;
        }

        public List<DealsStock> GetAllVersionsDeals(int modelId, int cityId)
        {
            List<DealsStock> versionDeals = new List<DealsStock>();
            try
            {
                using(var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getallversionsdeals_v17_12_6");
                    versionDeals = con.Query<MakeEntity, ModelEntity, VersionBase, DealsStock, DealsStock>("getallversionsdeals_v17_12_6",
                        (make, model, version, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.Version = version; return dealsStock; }, new { v_cityid = cityId, v_modelid = modelId }, commandType: CommandType.StoredProcedure, splitOn: "ModelName, VersionId, OfferPrice").AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetAllVersionsDeals" + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return versionDeals;
        }

        public List<int> GetCitiesWithMoreModels(int minimumStockCount)
        {
            List<int> cityList = new List<int>();
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("GetCitiesWithMoreModels");
                    cityList = con.Query<int>("GetCitiesWithMoreModels", new { v_mincount = minimumStockCount }, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetTransactionDetails" + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return cityList;
        }

        public AdvantageSearchResults GetDeals(Filters filter)
        {
            AdvantageSearchResults advantage = new AdvantageSearchResults();
            advantage.FilterCount = new FilterCountEntity();

            try
            {
                var param = new DynamicParameters();
                param.Add("v_cityid", filter.CityId);
                param.Add("v_startindex", filter.StartIndex);
                param.Add("v_pagesize", filter.PS);
                param.Add("v_makes", filter.Makes);
                param.Add("v_fuels", filter.Fuels);
                param.Add("v_transmissions", filter.Transmissions);
                param.Add("v_bodytypes", filter.BodyTypes);
                param.Add("v_sortby", filter.SC);
                param.Add("v_sortorder", filter.SO);
                param.Add("v_startbudget", filter.StartBudget);
                param.Add("v_endbudget", filter.EndBudget);

                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getdeals_v_16_12_8");
                    using (var reader = con.QueryMultiple("getdeals_v_16_12_8", param, commandType: CommandType.StoredProcedure))
                    {
                        advantage.Deals = reader.Read<DealsStock, MakeEntity, ModelEntity, VersionBase, CarImageBase, City, DealsStock>((dealsStock, make, model, version, image, city) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.Version = version; dealsStock.CarImageDetails = image; dealsStock.City = city; return dealsStock; }, splitOn: "MakeName, ModelName, VersionId, HostURL, CityId").AsList();
                        advantage.TotalCount = reader.Read<AdvantageSearchResults>().SingleOrDefault().TotalCount;
                        advantage.FilterCount.Makes = reader.Read<Carwale.Entity.Classified.StockMake>().ToList();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetDeals" + err.ToString());
                objErr.SendMail();
            }
            return advantage;
        }

        public bool UpdateCustomerInfo(int inquiryId, DealsInquiryDetail dealsInquiry)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_InquiryId", inquiryId);
                param.Add("v_CustomerName", dealsInquiry.CustomerName);
                param.Add("v_CustomerEmail", dealsInquiry.CustomerEmail);
                param.Add("v_CustomerMobile", dealsInquiry.CustomerMobile);
                using (var con = AdvantageMySqlMasterConnection)
                {
                    LogLiveSps.LogSpInGrayLog("UpdateCustomerInfo_DealInquiry");
                    return con.Execute("UpdateCustomerInfo_DealInquiry", param, commandType: CommandType.StoredProcedure) > 0 ? true : false;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return false;
        }

        public int GetCarCountByCity(int cityId)
        {
            int carCount = 0;
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getcarcountbycity");
                    carCount = con.Query<int>("getcarcountbycity", new { v_cityid = cityId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                throw err;
            }
            return carCount;
        }

        public DealsStock GetOfferOfWeekDetails(int modelId, int cityId)
        {
            DealsStock data = null;
            try
            {

                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getofferoftheweek");
                    data = con.Query<MakeEntity, ModelEntity, VersionBase, CarImageBase, City, DealsStock, DealsStock>("getofferoftheweek",
                        (make, model, version, image, city, dealsStock) => { dealsStock.Make = make; dealsStock.Model = model; dealsStock.Version = version; dealsStock.CarImageDetails = image; dealsStock.City = city; return dealsStock; },
                        new { v_cityid = cityId, v_modelid = modelId }, commandType: CommandType.StoredProcedure, splitOn: "ModelId,VersionId, HostURL,CityId,MakeYear").FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetOfferOfWeekDetails" + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return data;
        }

        public List<DealsTestimonialEntity> GetDealsTestimonials(int cityId = 0)
        {
            List<DealsTestimonialEntity> testimonials = null;
            try
            {
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getdealstestimonial");
                    testimonials = con.Query<MakeEntity, ModelEntity, City, CarImageBase, DealsTestimonialEntity, DealsTestimonialEntity>("getdealstestimonial", (make, model, city, image, testimonial) => { testimonial.Make = make; testimonial.Model = model; testimonial.City = city; testimonial.ImageDetails = image; return testimonial; },
                        new { v_cityid = cityId }, commandType: CommandType.StoredProcedure, splitOn: "ModelName,CityName,HostUrl,CustomerName").AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GerDealsTestimonials" + err.ToString());
                objErr.SendMail();
                throw err;
            }
            finally
            {
                if(testimonials == null)
                    testimonials = new List<DealsTestimonialEntity>();
            }
            return testimonials;
        }

        public DealsDealers GetDealerDetails(int dealerId)
        {
            DealsDealers dealerDetails = new DealsDealers();
             string dealerInfo = string.Empty;
             try
             {
                 using (var client = new HttpClient())
                 {
                     client.BaseAddress = new Uri(apiUrl);
                     client.DefaultRequestHeaders.Accept.Clear();
                     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                     using (HttpResponseMessage response = client.GetAsync(String.Format("deals/webapi/dealerDetails/?dealerId={0}", dealerId), HttpCompletionOption.ResponseHeadersRead).Result)
                     {
                         if (response.IsSuccessStatusCode)
                         {
                             dealerInfo = response.Content.ReadAsStringAsync().Result; 
                         }
                     }
                 }
             }
             catch (Exception err)
             {
                 ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Deals.GetDealerDetails " + err.ToString());
                 objErr.SendMail();
                  throw err;
             }
             if (!String.IsNullOrEmpty(dealerInfo))
            {
                dealerDetails = JsonConvert.DeserializeObject<DealsDealers>(dealerInfo);
            }
             return dealerDetails;
        }

        public string GetDealsOfferList(string stockIds, int cityId)
        {
            string offerListJson = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(stockIds))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (HttpResponseMessage response = client.GetAsync(String.Format("deals/webapi/advoffers/?stockIds={0}&cityId={1}", stockIds, cityId), HttpCompletionOption.ResponseHeadersRead).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                offerListJson = response.Content.ReadAsStringAsync().Result;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Deals.GetDealsOfferList " + err.ToString());
                objErr.SendMail();
                throw err;
            }
            return offerListJson;
        }

        public List<MakeModelVersionColor> GetMakeModelVersionColor()
        {
            LogLiveSps.LogSpInGrayLog("getMakeModelVersionColor");
            return CarDataMySqlReadConnection.Query<MakeModelVersionColor>("getMakeModelVersionColor", commandType: CommandType.StoredProcedure).ToList();
        }


         public IEnumerable<VersionEntity> GetAdvantageVersions(int modelId, int cityId)
         {
             IEnumerable<VersionEntity> versions = null;
             try
             {
                 using (var con = AdvantageMySqlReadConnection)
                 {
                     LogLiveSps.LogSpInGrayLog("getadvantageversions");
                     versions = con.Query<VersionEntity>("getadvantageversions", new { v_modelid = modelId, v_cityid = cityId }, commandType: CommandType.StoredProcedure).AsList();
                 }
             }
             catch (Exception err)
             {
                 ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetAdvantageVersions" + err.ToString());
                 objErr.SendMail();
                 throw err;
             }
             return versions;
         }


         public IEnumerable<ColorEntity> GetAdvantageVersionColors(int versionId, int cityId)
         {
             IEnumerable<ColorEntity> colors = null;
             try
             {
                 using (var con = AdvantageMySqlReadConnection)
                 {
                     LogLiveSps.LogSpInGrayLog("getadvantageversioncolors");
                     colors = con.Query<ColorEntity>("getadvantageversioncolors", new { v_versionId = versionId, v_cityid = cityId }, commandType: CommandType.StoredProcedure).AsList();
                 }
             }
             catch (Exception err)
             {
                 ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetAdvantageVersionColors" + err.ToString());
                 objErr.SendMail();
                 throw err;
             }
             return colors;
         }


         public IEnumerable<int> GetAdvantageColorYears(int versionId, int colorId, int cityId)
         {
             IEnumerable<int> years = null;
             try
             {
                 using (var con = AdvantageMySqlReadConnection)
                 {
                     LogLiveSps.LogSpInGrayLog("getadvantagecoloryears");
                     years = con.Query<int>("getadvantagecoloryears", new { v_versionId = versionId, v_colorId = colorId, v_cityid = cityId }, commandType: CommandType.StoredProcedure).AsList();
                 }
             }
             catch (Exception err)
             {
                 ErrorClass objErr = new ErrorClass(err, "Carwale.DAL.Deals.GetAdvantageColorYears" + err.ToString());
                 objErr.SendMail();
                 throw err;
             }
             return years;
         }

        public int GetDealsDealerId(int stockId, int cityId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_stockId", stockId);
                param.Add("v_cityId", cityId);
                using (var con = AdvantageMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("GetDealerIdByStockCity");
                    return con.Query<int>("GetDealerIdByStockCity", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return 0;
        }
    }
}


