using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Configuration;
using Carwale.Notifications.Logs;
using Newtonsoft.Json;
using Carwale.Interfaces.Stock;
using Carwale.Entity.Stock.Certification;
using Carwale.Utility;

namespace Carwale.DAL.Stock
{
    public class StockCertificationRepository : RepositoryBase, IStockCertificationRepository
    {
        private static readonly string _imageHostUrl = ConfigurationManager.AppSettings["CDNHostURL"].ToString();

        public StockCertification GetStockCertification(int inquiryId, bool isDealer)
        {
            StockCertification stockCertification = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryid", inquiryId);
                    param.Add("v_isdealer", isDealer);

                    var result = con.Query("getstockcertification_18_4_5", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("getstockcertification_18_4_5");
                    if (result != null)
                    {
                        stockCertification = new StockCertification();
                        stockCertification.InquiryId = result.InquiryId;
                        stockCertification.IsDealer = result.IsDealer;
                        stockCertification.IsActive = result.IsActive;
                        stockCertification.OverallScore = result.OverallScore;
                        stockCertification.OverallScoreColorId = result.OverallScoreColorId;
                        stockCertification.OverallCondition = result.OverallCondition;
                        stockCertification.CarExteriorImageUrl = _imageHostUrl + ImageSizes._0X0 + result.ExteriorOriginalImgPath;
                        stockCertification.ReportUrl = result.ReportUrl;
                        stockCertification.Description = JsonConvert.DeserializeObject<List<StockCertificationItem>>(result.Description);
                        stockCertification.DetailsPageUrl = ConfigurationManager.AppSettings["HostUrl"] + "/used/cardetails.aspx?car=d" + inquiryId;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return stockCertification;
        }

        public int AddStockCertification(StockCertification stockCertification)
        {
            int certId = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", stockCertification.InquiryId);
                param.Add("v_isdealer", stockCertification.IsDealer);
                param.Add("v_isactive", stockCertification.IsActive);
                param.Add("v_overallscore", stockCertification.OverallScore);
                param.Add("v_overallscorecolorid", stockCertification.OverallScoreColorId);
                param.Add("v_overallcondition", stockCertification.OverallCondition);
                param.Add("v_exteriororiginalimgpath", stockCertification.ExteriorOriginalImgPath);
                param.Add("v_reporturl", stockCertification.ReportUrl);
                param.Add("v_description", JsonConvert.SerializeObject(stockCertification.Description.OrderBy(d => d.CarItemId).ToList()));
                param.Add("v_certificationid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("addstockcertification_18_4_5", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("addstockcertification");
                    certId = param.Get<int>("v_certificationid");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return certId;
        }

        public int UpdateStockCertification(StockCertification stockCertification)
        {
            int certId = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", stockCertification.InquiryId);
                param.Add("v_isdealer", stockCertification.IsDealer);
                param.Add("v_isactive", stockCertification.IsActive);
                param.Add("v_overallscore", stockCertification.OverallScore);
                param.Add("v_overallscorecolorid", stockCertification.OverallScoreColorId);
                param.Add("v_overallcondition", stockCertification.OverallCondition);
                param.Add("v_exteriororiginalimgpath", stockCertification.ExteriorOriginalImgPath);
                param.Add("v_reporturl", stockCertification.ReportUrl);
                param.Add("v_description", JsonConvert.SerializeObject(stockCertification.Description.OrderBy(d => d.CarItemId).ToList()));
                param.Add("v_certificationid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("updatestockcertification_18_4_5", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("updatestockcertification_18_4_5");
                    certId = param.Get<int>("v_certificationid");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return certId;
        }
        
        public int UpdateStockCertificationStatus(int inquiryId, bool isDealer, bool isActive)
        {
            int certId = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId);
                param.Add("v_isdealer", isDealer);
                param.Add("v_isactive", isActive);
                param.Add("v_certificationid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("updatestockcertificationstatus_18_4_5", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("updatestockcertificationstatus_18_4_5");
                    certId = param.Get<int>("v_certificationid");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return certId;
        }
    }
}
