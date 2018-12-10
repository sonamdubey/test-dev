using System;
using System.Collections.Generic;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Carwale.Notifications;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Dapper;
using System.Linq;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.DAL.PriceQuote
{
    public class PQRepository : RepositoryBase, IPQRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;

        /// <summary>
        /// For Getting PriceQuote For Sponsored car based on city id and version id (this function not inserts new entry into NewcarPurchageInquries Table)
        /// Written By : Ashish Verma
        /// </summary>
        /// <param name="PQId">cityid,versionid,zoneid</param>
        /// <returns>Price Quote For Sponsored Car </returns>
        public PQ GetPQ(int cityId, int versionId)
        {
            var priceQuote = new PQ();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetOnRoadPrice_v17_8_8"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarVersionId", DbType.Int32, versionId));

                    var priceQuoteKeyValue = new List<PQItem>();

                    using (var dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            priceQuoteKeyValue.Add(new PQItem()
                            {
                                Key = dr["categoryItem"].ToString(),
                                Value = Convert.ToInt64(dr["Value"]),
                                IsMetallic = Convert.ToBoolean(dr["isMetallic"])
                            });

                            //priceQuote.OnRoadPrice += string.IsNullOrEmpty(dr["Value"].ToString()) ? 0 : Convert.ToUInt64(dr["Value"]);
                        }

                        priceQuote.PriceQuoteList = priceQuoteKeyValue;
                    }
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                throw;
            }
            return priceQuote;
        }

        public List<ModelPrices> GetModelPrices(int modelId, int cityId)
        {
            var modelPrices = new List<ModelPrices>();

            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetPQByModelCity_v17_8_8"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int64, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int64, cityId));

                    using (var dr = MySqlDatabase.SelectQuery(cmd,DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            modelPrices.Add(new ModelPrices()
                            {
                                VersionId = Convert.ToInt32(dr["VersionId"]),
                                VersionName = dr["VersionName"].ToString(),
                                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                CategoryItemId = Convert.ToInt32(dr["CategoryItemId"]),
                                CategoryItemName = dr["CategoryItemName"].ToString(),
                                CategoryItemValue = Convert.ToInt32(dr["CategoryItemValue"]),
                                IsMetallic = Convert.ToBoolean(dr["isMetallic"])
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return modelPrices;
        }

        /// <summary>
        /// Private metod To send parametes To sp and returns SQL Command
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private SqlCommand GetPQSqlCommand(PQInput pqInput)
        {
            var pqCmd = new SqlCommand("[GetOnRoadPriceandPQIdV1.1]");
            pqCmd.CommandType = CommandType.StoredProcedure;

            pqCmd.Parameters.Add("@CarVersionId", SqlDbType.Decimal).Value = string.IsNullOrEmpty(pqInput.CarVersionId.ToString()) ? Convert.DBNull : pqInput.CarVersionId;
            pqCmd.Parameters.Add("@CityId", SqlDbType.Decimal).Value = string.IsNullOrEmpty(pqInput.CityId.ToString()) ? Convert.DBNull : pqInput.CityId;
            pqCmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(pqInput.Name) ? Convert.DBNull : pqInput.Name;
            pqCmd.Parameters.Add("@BuyTime", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(pqInput.BuyTimeDaysText) ? Convert.DBNull : pqInput.BuyTimeDaysText;
            pqCmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(pqInput.Email) ? Convert.DBNull : pqInput.Email;
            pqCmd.Parameters.Add("@PhoneNo", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(pqInput.Mobile) ? Convert.DBNull : pqInput.Mobile;
            pqCmd.Parameters.Add("@ForwardedLead", SqlDbType.Bit).Value = (string.IsNullOrEmpty(pqInput.BuyTimeDaysValue.ToString()) || pqInput.BuyTimeDaysValue > 65) ? 0 : 1;
            pqCmd.Parameters.Add("@SourceId", SqlDbType.TinyInt).Value = string.IsNullOrEmpty(pqInput.SourceId.ToString()) ? Convert.DBNull : pqInput.SourceId;
            pqCmd.Parameters.Add("@PQPageId", SqlDbType.SmallInt).Value = string.IsNullOrEmpty(pqInput.PageId) ? Convert.DBNull : Convert.ToInt32(pqInput.PageId);
            pqCmd.Parameters.Add("@ZoneId", SqlDbType.Int).Value = string.IsNullOrEmpty(pqInput.ZoneId) ? Convert.DBNull : Convert.ToInt32(pqInput.ZoneId);
            pqCmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(pqInput.ClientIp) ? Convert.DBNull : pqInput.ClientIp;
            pqCmd.Parameters.Add("@InterestedInLoan", SqlDbType.Bit).Value = string.IsNullOrEmpty(pqInput.InterestedInLoan.ToString()) ? false : pqInput.InterestedInLoan;
            pqCmd.Parameters.Add("@MobVerified", SqlDbType.Bit).Value = string.IsNullOrEmpty(pqInput.IsMobileVerified.ToString()) ? false : pqInput.IsMobileVerified;

            return pqCmd;
        }


        private SqlCommand GetPQSqlCommand_New(PQInput pqInput)
        {
            var pqCmd = new SqlCommand("[GetOnRoadPriceandPQIdV1.1]");
            pqCmd.CommandType = CommandType.StoredProcedure;

            pqCmd.Parameters.Add("@CarVersionId", SqlDbType.Decimal).Value = string.IsNullOrEmpty(pqInput.CarVersionId.ToString()) ? Convert.DBNull : pqInput.CarVersionId;
            pqCmd.Parameters.Add("@CityId", SqlDbType.Decimal).Value = string.IsNullOrEmpty(pqInput.CityId.ToString()) ? Convert.DBNull : pqInput.CityId;
            pqCmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(pqInput.Name) ? Convert.DBNull : pqInput.Name;
            pqCmd.Parameters.Add("@BuyTime", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(pqInput.BuyTimeDaysText) ? Convert.DBNull : pqInput.BuyTimeDaysText;
            pqCmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(pqInput.Email) ? Convert.DBNull : pqInput.Email;
            pqCmd.Parameters.Add("@PhoneNo", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(pqInput.Mobile) ? Convert.DBNull : pqInput.Mobile;
            pqCmd.Parameters.Add("@ForwardedLead", SqlDbType.Bit).Value = (string.IsNullOrEmpty(pqInput.BuyTimeDaysValue.ToString()) || pqInput.BuyTimeDaysValue > 65) ? 0 : 1;
            pqCmd.Parameters.Add("@SourceId", SqlDbType.TinyInt).Value = string.IsNullOrEmpty(pqInput.SourceId.ToString()) ? Convert.DBNull : pqInput.SourceId;
            pqCmd.Parameters.Add("@PQPageId", SqlDbType.SmallInt).Value = string.IsNullOrEmpty(pqInput.PageId) ? Convert.DBNull : Convert.ToInt32(pqInput.PageId);
            pqCmd.Parameters.Add("@ZoneId", SqlDbType.Int).Value = string.IsNullOrEmpty(pqInput.ZoneId) ? Convert.DBNull : Convert.ToInt32(pqInput.ZoneId);
            pqCmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(pqInput.ClientIp) ? Convert.DBNull : pqInput.ClientIp;
            pqCmd.Parameters.Add("@InterestedInLoan", SqlDbType.Bit).Value = string.IsNullOrEmpty(pqInput.InterestedInLoan.ToString()) ? false : pqInput.InterestedInLoan;
            pqCmd.Parameters.Add("@MobVerified", SqlDbType.Bit).Value = string.IsNullOrEmpty(pqInput.IsMobileVerified.ToString()) ? false : pqInput.IsMobileVerified;

            return pqCmd;
        }

        /// <summary>
        /// ashish Verma on 23/09/14
        /// Insert pqInputs into NewPurchaseCities and NewCarPurchaseInquiries table
        /// </summary>
        /// <param name="PQId">cityid,versionid</param>
        /// <returns>PriceQuote Id </returns>
        
        public List<LeadSource> GetLeadSource(int platformId, int adType)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_PlatformId", platformId);
                param.Add("v_AdType", adType);

                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<LeadSource>("GetLeadSource_v16_11_7", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQRepository.GetLeadSource();   PlatformId = " + platformId + " AdType = " + adType);
                objErr.LogException();
                throw;
            }
        }

        /// <summary>
        /// Author : Sachin Bharti on 30/06/2016
        /// Purpose : Get versions average price based on modelId
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IDictionary<int, VersionAveragePrice> GetModelsVersionAveragePrices(int modelId, bool isNew, bool isMasterConnection)
        {
            var connectionString = isMasterConnection ? NewCarMySqlMasterConnection : NewCarMySqlReadConnection;
            var param = new DynamicParameters();
            param.Add("v_ModelId", modelId);
            param.Add("v_IsNew", isNew);
            try
            {
                using (var con = connectionString)
                {
                    return con.Query("GetModelsVersionAveragePrice_v18_3_8", param, commandType: CommandType.StoredProcedure).ToDictionary(row => (int)row.VersionId, row => new VersionAveragePrice() { VersionId = (int)row.VersionId, VersionName = (string)row.VersionName, CarAveragePrice = (int)row.AveragePrice });
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                throw;
            }
        }

        /// <summary>
        /// Get average or Ex showroom price for each version of a perticular model
        /// Written By : Chetan Thambad on <20/07/2016>
        /// </summary>
        public IEnumerable<VersionPrice> GetAllVersionsPriceByModelCity(int modelId, int cityId)
        {
            IEnumerable<VersionPrice> data = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                param.Add("v_CityId", cityId);

                using (var con = NewCarMySqlReadConnection)
                {
                    data = con.Query<VersionBase, City, VersionPrice, VersionPrice>("GetPrices_v16_12_2",
                        (versionBase, city, versionPrice) => { versionPrice.VersionBase = versionBase; versionPrice.City = city; return versionPrice; },
                        param , commandType: CommandType.StoredProcedure, splitOn: "CityId, IsVersionBlocked").AsList();
                }
                LogLiveSps.LogSpInGrayLog("[GetPrices_v16_8_5]");
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQRepository.GetAllVersionsPriceByModelCity()");
                objErr.LogException();
                throw;
            }
            return data;
        }

        public List<VersionPrices> GetVersionsPriceList(int modelId, int cityId, bool isNew)
        {
            List<VersionPrices> pricesList = null;

            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId, DbType.Int32, direction: ParameterDirection.Input);
                param.Add("v_CityId", cityId, DbType.Int32, direction: ParameterDirection.Input);
                param.Add("v_IsNew", isNew, dbType: DbType.Boolean, direction: ParameterDirection.Input);

                using (var conn = NewCarMySqlMasterConnection)
                {
                    pricesList = conn.Query<VersionPrices>("GetPrice_v17_11_1", param, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("[dbo].[GetPrice]");
                }

                if (pricesList.Count > 0)
                {
                    return pricesList;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw;
            }
            return null;
        }
    }//Class
}// Namespace
