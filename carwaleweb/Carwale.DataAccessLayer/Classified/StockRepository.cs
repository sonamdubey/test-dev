using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Nest;
using Carwale.DAL.CoreDAL;
using System.Configuration;

namespace Carwale.DAL.Classified.Stock
{
    public class StockRepository : RepositoryBase, IStockRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;
        public List<StockSummary> GetSimilarUsedModels(int modelId)
        {
            string sql = string.Empty;

            var stockList = new List<StockSummary>();

            sql = "select makename, modelname, cmo.maskingname, versionname, profileid, versionid, cityname, price, kilometers as km, year(makeyear) makeyear"
                + " from livelistings ll inner join cwmasterdb.carmodels cmo on ll.modelid=cmo.id where showdetails = 1 and modelid = @ModelId limit 4 ";

            try
            {
                var param = new DynamicParameters();
                param.Add("@ModelId", modelId, DbType.Int32);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    stockList = con.Query<StockSummary>(sql, param, commandType: CommandType.Text).AsList();
                    LogLiveSps.LogSpInGrayLog(sql);
                }
                
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "CarMakesDAL.GetListedUsedCarModels()");
                objErr.LogException();
                throw;
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "CarMakesDAL.GetListedUsedCarModels()");
                objErr.LogException();
                throw;
            }
            return stockList;
        }




        public ImageGalleryEntity GetImagesByProfileId(string inquiryId, bool isDealer)
        {
            ImageGalleryEntity imageList = new ImageGalleryEntity{ ImgFullURLs = new List<string>(), ImgThumbURLs = new List<string>()};
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId, DbType.Int32);
                param.Add("v_isdealer", isDealer);
                LogLiveSps.LogSpInGrayLog("getallimagesforprofileid_15_8_1");
                using (var con = ClassifiedMySqlReadConnection)
                {
                    using (var reader = con.QueryMultiple("getallimagesforprofileid_15_8_1", param, commandType: CommandType.StoredProcedure))
                    {
                        var imageDetails = reader.Read();
                        foreach (dynamic imageDetail in imageDetails)
                        {
                            imageList.ImgFullURLs.Add(ImageSizes.CreateImageUrl(_imgHostUrl ,ImageSizes._891X501, imageDetail.OriginalImgPath));
                            imageList.ImgThumbURLs.Add(ImageSizes.CreateImageUrl(_imgHostUrl, ImageSizes._110X61, imageDetail.OriginalImgPath));
                        }
                        imageList.YoutubeURL = reader.Read<string>().FirstOrDefault();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return imageList;
        }

        /// <summary>
        /// Get Comparison data for desktop compare cars
        /// </summary>
        /// <param name="cityIds"></param>
        /// <param name="rootIds"></param>
        /// <returns></returns>
        public static List<UsedCarComparisonEntity> GetUsedCarComparison(List<int> cityIds, List<int> rootIds)
        {
            string elasticIndex = ConfigurationManager.AppSettings["ElasticIndexName"];
            ElasticClient client = ElasticClientInstance.GetInstance();
            var result = client.Search<StockBaseEntity>(s => s.Index(elasticIndex).Type("stock")
                .Query(q =>  q
                        .Bool(b => b
                            .Must(m => {
                                QueryContainer qc = m.Terms(t => t.Field("cityIds").Terms<int>(cityIds));
                                qc &=  m.Terms(t => t.Field(f => f.RootId).Terms<int>(rootIds)) ;
                                return qc;
                                } 
                            )
                        )
                )
                .Size(0)//Query Size - we only want aggregation result
                .Aggregations(a => a
                    .Terms("root_count", st => st
                        .Field( f => f.RootId)
                        .Size(rootIds.Count)//Aggregation Size
                        .Aggregations(na => na
                            .Min("min_price", m => m
                                .Field(mf => mf.Price)
                            )
                        )
                    )
                )
            );
            var rootCountAggregation = result.Aggs.Terms("root_count");
            List<UsedCarComparisonEntity> comparisonList = new List<UsedCarComparisonEntity>();
            foreach (var item in rootCountAggregation.Buckets)
            {
                UsedCarComparisonEntity comparison = new UsedCarComparisonEntity();
                comparison.MinPrice = item.Min("min_price").Value.ToString();
                comparison.RootId = Convert.ToInt32(item.Key);
                comparison.Count = item.DocCount.ToString();
                comparisonList.Add(comparison);
            }
            return comparisonList;
        }

        /// <summary>
        /// Created Date : 8/4/2015
        /// Author : Aditi Dhaybar
        /// Desc : Method to fetch used luxury cars recommendation to show on Models/Version Page
        /// </summary>
        /// <returns></returns>
        public List<StockSummary> GetLuxuryCarRecommendations(int carId, int dealerId, int pageId)
        {
            var stockList = new List<StockSummary>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_carid", carId, DbType.Int32);
                param.Add("v_dealerid", dealerId, DbType.Int32);
                param.Add("v_pageid", pageId, DbType.Int32);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    stockList = con.Query<StockSummary>("usedluxurycarrecommendations_15_8_1", param, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("usedluxurycarrecommendations_15_8_1");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Classified.StockDAL.GetLuxuryCarRecommendations()");
                objErr.LogException();
                throw;
            }

            return stockList;
        }

        public IEnumerable<string> GetProfileIdsOfDealer(int dealerId)
        {
            var param = new DynamicParameters();
            param.Add("v_dealerId", dealerId, DbType.Int32);
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<string>("GetProfileIdsFromDealerId", param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<string> GetLiveStocksByCertProgId(int certificationId)
        {
            string query = "select profileid from livelistings where CertProgId = @id";
            var param = new DynamicParameters();
            param.Add("id", certificationId);
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<string>(query, param, commandType: CommandType.Text);
            }
        }

        public bool IsStockLive(string profileId)
        {
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<bool>("select 1 from livelistings where profileId = @v_profileId", new { v_profileId = profileId }, commandType: CommandType.Text).FirstOrDefault();
            }
        }

    }//class
}//namespace
