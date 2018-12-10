using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.Chat;
using Carwale.Entity.Dealers;
using Carwale.Entity.UsedCarsDealer;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Dealers
{
    public class DealerRepository : RepositoryBase, IDealerRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;
        //vinayak
        public DealerDetails GetDealerDetailsOnDealerId(int dealerId)
        {
            var detail = new DealerDetails();

            try
            {
                    using (var cmd = DbFactory.GetDBCommand("DealerDetailsOnDealerId_v18_4_1"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int16, dealerId));

                        cmd.Parameters.Add(DbFactory.GetDbParam("v_OutDealerId", DbType.Int16, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, 200, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Address", DbType.String, 400, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Pincode", DbType.String, 20, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_EMailId", DbType.String, 100, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_WebSite", DbType.String, 100, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ShowroomStartTime", DbType.String, 50, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ShowroomEndTime", DbType.String, 50, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_WorkingHours", DbType.String, 50, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Latitude", DbType.Decimal, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Longitude", DbType.Decimal, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CityName", DbType.String, 50, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_StateName", DbType.String, 30, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_MobileNo", DbType.String, 100, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_LandLineNo", DbType.String, 50, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_StateId", DbType.Int16, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_FaxNo", DbType.String, 30, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_HostUrl", DbType.String, 100, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ProfilePhoto", DbType.String, 250, direction: ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerArea", DbType.String, 50,direction: ParameterDirection.Output));
                        LogLiveSps.LogSpInGrayLog(cmd);
                        MySqlDatabase.ExecuteNonQuery(cmd,DbConnections.NewCarMySqlReadConnection);
                        detail.DealerId = CustomParser.parseIntObject(cmd.Parameters["v_OutDealerId"].Value);
                        detail.CityId = CustomParser.parseIntObject(cmd.Parameters["v_CityId"].Value);
                        detail.Name = CustomParser.parseStringObject(cmd.Parameters["v_Name"].Value);
                        detail.Address = CustomParser.parseStringObject(cmd.Parameters["v_Address"].Value);
                        detail.Pincode = CustomParser.parseStringObject(cmd.Parameters["v_Pincode"].Value);
                        detail.EMailId = CustomParser.parseStringObject(cmd.Parameters["v_EMailId"].Value);
                        detail.WebSite = CustomParser.parseStringObject(cmd.Parameters["v_WebSite"].Value);
                        detail.ShowroomStartTime = CustomParser.parseStringObject(cmd.Parameters["v_ShowroomStartTime"].Value);
                        detail.ShowroomEndTime = CustomParser.parseStringObject(cmd.Parameters["v_ShowroomEndTime"].Value);
                        detail.WorkingHours = CustomParser.parseStringObject(cmd.Parameters["v_WorkingHours"].Value);
                        detail.Latitude = CustomParser.parseDoubleObject(cmd.Parameters["v_Latitude"].Value);
                        detail.Longitude = CustomParser.parseDoubleObject(cmd.Parameters["v_Longitude"].Value);
                        detail.CityName = CustomParser.parseStringObject(cmd.Parameters["v_CityName"].Value);
                        detail.StateName = CustomParser.parseStringObject(cmd.Parameters["v_StateName"].Value);
                        detail.Mobile = CustomParser.parseStringObject(cmd.Parameters["v_MobileNo"].Value);
                        detail.LandLineNo = CustomParser.parseStringObject(cmd.Parameters["v_LandLineNo"].Value);
                        detail.StateId = CustomParser.parseIntObject(cmd.Parameters["v_StateId"].Value);
                        detail.FaxNo = CustomParser.parseStringObject(cmd.Parameters["v_FaxNo"].Value);
                        detail.HostURL = CustomParser.parseStringObject(cmd.Parameters["v_HostUrl"].Value);
                        detail.ProfilePhoto = CustomParser.parseStringObject(_imgHostUrl);
                        detail.DealerArea = CustomParser.parseStringObject(cmd.Parameters["v_DealerArea"].Value);

                }// SqlConnection object closed and disposed here.
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealerRepository.GetDealerDetailsOnDealerId()");
                objErr.LogException();
                throw;
            }
            return detail;
        }

        /// <summary>
        /// Gets the list of all models based on makeId passed
        /// Written By : Shalini on 08/07/2014
        /// Moved it to an dealer_Repo(vinayak)
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<CarModelSummary> GetDealerModelsOnMake(int makeId, int dealerId)
        {
            var carModelsList = new List<CarModelSummary>();
            try
            {

                using (var cmd = DbFactory.GetDBCommand("GetDealerModels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarMakeId", DbType.Int16, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int16, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            carModelsList.Add(new CarModelSummary()
                            {
                                ModelImage =_imgHostUrl + ImageSizes._110X61 + dr["OriginalImgPath"].ToString(),
                                LargeImage = _imgHostUrl + ImageSizes._210X118 + dr["OriginalImgPath"].ToString(),
                                XLargeImage = _imgHostUrl + ImageSizes._640X348 + dr["OriginalImgPath"].ToString(),
                                ModelName = dr["Model"].ToString(),
                                MaskingName = dr["MaskingName"].ToString(),
                                ModelRating = Convert.ToSingle(dr["ReviewRate"]),
                                ReviewCount = Convert.ToInt16(dr["ReviewCount"]),
                                MinPrice = (dr["MinPrice"] != DBNull.Value ? Convert.ToDouble(dr["MinPrice"]) : 0),
                                MaxPrice = (dr["MaxPrice"] != DBNull.Value ? Convert.ToDouble(dr["MaxPrice"]) : 0),
                                New = Convert.ToBoolean(dr["New"]),
                                HostUrl = _imgHostUrl,
                                OriginalImage = dr["OriginalImgPath"].ToString(),
                                ModelId = Convert.ToInt16(dr["ID"])
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModRepository. GetModelsByMake()");
                objErr.LogException();
                throw;
            }
            return carModelsList;
        }

        public DealerChatInfo GetDealerMobileFromChatToken(string chatToken)
        {
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<DealerChatInfo>("select Id, MobileNo, ChatUserToken from cwmasterdb.dealers where ChatUserToken = @v_chatToken;", new { v_chatToken = chatToken }, commandType: CommandType.Text).FirstOrDefault();
            }
        }
    }
}
