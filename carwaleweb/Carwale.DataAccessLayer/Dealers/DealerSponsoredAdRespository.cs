using Carwale.DAL.CoreDAL;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using Carwale.Entity.ThirdParty.Leads;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Linq;
using Carwale.Entity.Enum;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.DAL.Dealers
{
    public class DealerSponsoredAdRespository : RepositoryBase, IDealerSponsoredAdRespository
    {
        /// <summary>
        /// Save Dealer Sponsored Ad Inquiry To PQDealerAdLeads Table
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ulong SaveDealerSponserdInquiry(DealerInquiryDetails inquiryDetails)
        {
            ulong responseId = 0;
            var nonAppsPlatform = inquiryDetails.PlatformSourceId == 1 || inquiryDetails.PlatformSourceId == 43;
            try
            {
                string sql= @"INSERT INTO pqdealeradleads (
			                    PQId
			                    ,DealerId
			                    ,LeadClickSource
			                    ,DealerLeadBusinessType
			                    ,NAME
			                    ,Email
			                    ,Mobile
			                    ,AssignedDealerID
			                    ,CityId
			                    ,ZoneId
                                ,AreaId
                                ,ModelId
			                    ,VersionId
			                    ,PlatformId
			                    ,LTSRC
			                    ,Comment
			                    ,UtmaCookie
			                    ,UtmzCookie
			                    ,CampaignId
			                    ,ModelHistory
			                    ,CWCookieValue 
			                    ,ClientIP 
			                    ,Browser 
			                    ,OperatingSystem
			                    ,ABTest 
                                ,ApplicationId
                                ,OriginalLeadId
                                ,PageId
                                ,PropertyId
                                ,SourceType
                                ,IsCitySet
			                    )
		                    VALUES (
			                    @v_PQId
			                    ,@v_DealerId
			                    ,@v_LeadClickSource
			                    ,@v_DealerLeadBusinessType
			                    ,@v_Name
			                    ,@v_Email
			                    ,@v_Mobile
			                    ,@v_AssignedDealerId
			                    ,@v_CityID
			                    ,@v_ZoneID
                                ,@v_AreaId
                                ,@v_ModelId
			                    ,@v_VersionId
			                    ,@v_PlatformId
			                    ,@v_LTSRC
			                    ,@v_Comment
			                    ,@v_UtmaCookie
			                    ,@v_UtmzCookie
			                    ,@v_CampaignId
			                    ,@v_ModelHistory
			                    ,@v_CWCookieValue 
			                    ,@v_ClientIP 
			                    ,@v_Browser 
			                    ,@v_OperatingSystem
			                    ,@v_ABTest 
                                ,@v_ApplicationId
                                ,@v_originalLeadId
                                ,@v_PageId
                                ,@v_PropertyId
                                ,@v_SourceType
                                ,@v_IsCitySet
			                    );

		                    select LAST_INSERT_ID() as pqDealerId;";

                var param = new DynamicParameters();
                    param.Add("v_PQId", inquiryDetails.PQId);
                    param.Add("v_DealerId", inquiryDetails.ActualDealerId);
                    param.Add("v_LeadClickSource", inquiryDetails.LeadClickSource);
                    param.Add("v_DealerLeadBusinessType", inquiryDetails.LeadBussinessType);
                    param.Add("v_Name", inquiryDetails.Name);
                    param.Add("v_Email", inquiryDetails.Email);
                    param.Add("v_Mobile", inquiryDetails.Mobile);
                    param.Add("v_CampaignId", inquiryDetails.DealerId);
                    param.Add("v_AssignedDealerId",  inquiryDetails.AssignedDealerId == 0 ? -1 : inquiryDetails.AssignedDealerId);
                    param.Add("v_CityId", inquiryDetails.CityId);
                    param.Add("v_ZoneId", string.IsNullOrEmpty(inquiryDetails.ZoneId) ? 0 : Convert.ToInt32(inquiryDetails.ZoneId));
                    param.Add("v_AreaId", inquiryDetails.AreaId);
                    param.Add("v_ModelId", inquiryDetails.ModelId);
                    param.Add("v_VersionId", inquiryDetails.VersionId);
                    param.Add("v_PlatformId", inquiryDetails.PlatformSourceId);
                    param.Add("v_LTSRC", inquiryDetails.Ltsrc != "-1" && !string.IsNullOrEmpty(inquiryDetails.Ltsrc) ? inquiryDetails.Ltsrc : Convert.DBNull, DbType.String);
                    param.Add("v_Comment", string.IsNullOrEmpty(inquiryDetails.Comments) ? Convert.DBNull : (inquiryDetails.Comments), DbType.String);
                    param.Add("v_UtmaCookie", string.IsNullOrEmpty(inquiryDetails.UtmaCookie) ? Convert.DBNull : inquiryDetails.UtmaCookie, DbType.String);
                    param.Add("v_UtmzCookie", string.IsNullOrEmpty(inquiryDetails.UtmzCookie) ? Convert.DBNull : inquiryDetails.UtmzCookie, DbType.String);
                    param.Add("v_CWCookieValue", inquiryDetails.CwCookie , DbType.String);
                    param.Add("v_ClientIP", UserTracker.GetUserIp() , DbType.String);
                    param.Add("v_Browser", nonAppsPlatform ? UserTracker.GetUserBrowserCapability() : inquiryDetails.UserAgent, DbType.String);
                    param.Add("v_OperatingSystem", nonAppsPlatform ? HttpContext.Current.Request.Browser.Platform : inquiryDetails.Platform, DbType.String);
                    param.Add("v_ModelHistory", string.IsNullOrEmpty(inquiryDetails.ModelsHistory) ? Convert.DBNull : (inquiryDetails.ModelsHistory), DbType.String);
                    param.Add("v_ABTest", inquiryDetails.ABTest);
                    param.Add("v_ApplicationId", CustomParser.parseIntObject(inquiryDetails.ApplicationId));
                    param.Add("v_PQDealerAdLeadId", inquiryDetails.PQDealerAdLeadId, DbType.Int64, ParameterDirection.InputOutput);
                    param.Add("v_originalLeadId", inquiryDetails.OriginalLeadId);
                    param.Add("v_PageId", inquiryDetails.PageId);
                    param.Add("v_PropertyId", inquiryDetails.PropertyId);
                    param.Add("v_SourceType", inquiryDetails.SourceType);
                    param.Add("v_IsCitySet", inquiryDetails.IsCitySet);
                using (var con = NewCarMySqlMasterConnection)
                {
                    responseId = con.Query<ulong>(sql, param, commandType: CommandType.Text).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdRespository.SavePQDealerInquiryDetails()");
            }
            return responseId;
        }

        public void UpdateDealerSponserdInquiry(DealerInquiryDetails inquiryDetails)
        {
            try
            {
                string sql = @"UPDATE pqdealeradleads
		                        SET Email = @v_Email
			                    ,DealerLeadBusinessType = @v_DealerLeadBusinessType
			                    ,NAME = @v_Name
			                    ,Mobile = @v_Mobile
			                    ,AssignedDealerID = @v_AssignedDealerID
			                    ,DealerId = @v_DealerId
			                    ,CityId = @v_CityId
			                    ,ZoneId = @v_ZoneId
			                    ,AreaId = @v_AreaId
			                    ,CampaignId = @v_CampaignId
		                        WHERE Id = @v_PQDealerAdLeadId;";

                var param = new DynamicParameters();
                param.Add("v_DealerId", inquiryDetails.ActualDealerId);
                param.Add("v_DealerLeadBusinessType", inquiryDetails.LeadBussinessType);
                param.Add("v_Name", inquiryDetails.Name);
                param.Add("v_Email", inquiryDetails.Email);
                param.Add("v_Mobile", inquiryDetails.Mobile);
                param.Add("v_CampaignId", inquiryDetails.DealerId);
                param.Add("v_AssignedDealerId", inquiryDetails.AssignedDealerId == 0 ? -1 : inquiryDetails.AssignedDealerId);
                param.Add("v_CityId", inquiryDetails.CityId);
                param.Add("v_ZoneId", string.IsNullOrEmpty(inquiryDetails.ZoneId) ? 0 : Convert.ToInt32(inquiryDetails.ZoneId));
                param.Add("v_AreaId", inquiryDetails.AreaId);
                param.Add("v_PQDealerAdLeadId", inquiryDetails.PQDealerAdLeadId, DbType.Int64, ParameterDirection.InputOutput);
                using (var con = NewCarMySqlMasterConnection)
                {
                    con.Query(sql, param, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdRespository.UpdatePQDealerInquiryDetails()");
            }
        }

        public void UpdateDealerSponserdInquiryEmail(DealerInquiryDetails inquiryDetails)
        {
            try
            {
                string sql = @"UPDATE pqdealeradleads
		                        SET Email = @v_Email
		                        WHERE Id = @v_PQDealerAdLeadId;";

                var param = new DynamicParameters();
                param.Add("v_Email", inquiryDetails.Email);
                param.Add("v_PQDealerAdLeadId", inquiryDetails.PQDealerAdLeadId, DbType.Int64, ParameterDirection.InputOutput);
                using (var con = NewCarMySqlMasterConnection)
                {
                    con.Query(sql, param, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdRespository.UpdatePQDealerInquiryDetailsEmail()");
            }
        }

        /// <summary>
        /// Gives the new car dealers List based on makeId and cityId 
        /// Written By : Ashish Verma on 16/09/2014
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<NewCarDealersList> GetNewCarDealersByMakeAndCityId(int makeId, int cityId)
        {
            var dealerList = new List<NewCarDealersList>();

            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetNCSDealersOnMakeCity_v16_11_6"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId", DbType.Int16, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, cityId));
                    
                    using (var dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            dealerList.Add(new NewCarDealersList()
                            {
                                DealerId = Convert.ToInt16(dr["ID"]),
                                DealerName = dr["DealerName"].ToString(),
                                DealerArea = dr["Area"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersRepository.BindNewCarDealers()");
                objErr.LogException();
            }
            return dealerList;
        }

        public SponsoredDealer GetDealerDetailsByDealerId(int _dealerId)
        {
            var sponsoredDealerList = new SponsoredDealer();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetDealerDetailsByActualDealerId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ActualDealerId", DbType.Int16, _dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerName", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerMobile", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerEmail", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerActualMobile", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerLeadBusinessType", DbType.Int16, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerAddress", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadPanelDealerId", DbType.Int16, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadPanelDealerMobile", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadPanelDealerEmail", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerStateId", DbType.Int16, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerTypeId", DbType.Int16, ParameterDirection.Output));
                    //LogLiveSps.LogSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.NewCarMySqlReadConnection);

                    var dealerName = cmd.Parameters["v_DealerName"].Value;
                    var dealerMobile = cmd.Parameters["v_DealerMobile"].Value;
                    var dealerEmail = cmd.Parameters["v_DealerEmail"].Value;
                    var dealerActualMobile = cmd.Parameters["v_DealerActualMobile"].Value;
                    var DealerLeadBusinessType = cmd.Parameters["v_DealerLeadBusinessType"].Value;
                    var dealerAddress = cmd.Parameters["v_DealerAddress"].Value;
                    var leadPanelDealerId = cmd.Parameters["v_LeadPanelDealerId"].Value;
                    var leadPanelDealerMobile = cmd.Parameters["v_LeadPanelDealerMobile"].Value;
                    var leadPanelDealerEmail = cmd.Parameters["v_LeadPanelDealerEmail"].Value;
                    var dealerStateId = cmd.Parameters["v_DealerStateId"].Value;
                    
                    sponsoredDealerList.DealerId = _dealerId;
                    sponsoredDealerList.DealerName = string.IsNullOrEmpty(dealerName.ToString()) ? "" : dealerName.ToString();
                    sponsoredDealerList.DealerMobile = string.IsNullOrEmpty(dealerMobile.ToString()) ? "" : dealerMobile.ToString();
                    sponsoredDealerList.DealerEmail = string.IsNullOrEmpty(dealerEmail.ToString()) ? "" : dealerEmail.ToString();
                    sponsoredDealerList.DealerActualMobile = string.IsNullOrEmpty(dealerActualMobile.ToString()) ? "" : dealerActualMobile.ToString();
                    sponsoredDealerList.DealerLeadBusinessType = string.IsNullOrEmpty(DealerLeadBusinessType.ToString()) ? -1 : Convert.ToInt16(DealerLeadBusinessType);
                    sponsoredDealerList.DealerAddress = string.IsNullOrEmpty(dealerAddress.ToString()) ? "" : dealerAddress.ToString();
                    sponsoredDealerList.TargetDealerId = string.IsNullOrEmpty(DealerLeadBusinessType.ToString()) ? -1 : Convert.ToInt32(leadPanelDealerId);
                    sponsoredDealerList.TargetDealerEmail = string.IsNullOrEmpty(dealerMobile.ToString()) ? "" : leadPanelDealerEmail.ToString();
                    sponsoredDealerList.TargetDealerMobile = string.IsNullOrEmpty(dealerEmail.ToString()) ? "" : leadPanelDealerMobile.ToString();
                    sponsoredDealerList.UserEmailEnabled = true;
                    sponsoredDealerList.UserSMSEnabled = true;
                    sponsoredDealerList.DealerSMSEnabled = true;
                    sponsoredDealerList.DealerEmailEnabled = true;

                }// SqlCommand object closed and disposed here.
            }// SqlConnection object closed and disposed here.

            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerSponsoredAdRespository.GetDealerDetailsByDealerId()");
                objErr.SendMail();
            }
            return sponsoredDealerList;
        }

        public void SaveFailedLeads(DealerInquiryDetails inquiryDetails, string errMessage)
        {
            try
            {
                using (var cmd = DbFactory.GetDBCommand("SaveFailedDealerInquiryDetails_v17_4_8"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PQId", DbType.Int32, CustomParser.parseIntObject(inquiryDetails.PQId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadClickSource", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.LeadClickSource)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, 100, CustomParser.parseStringObject(inquiryDetails.Name)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 100, CustomParser.parseStringObject(inquiryDetails.Email)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile", DbType.String, 100, CustomParser.parseStringObject(inquiryDetails.Mobile)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CampaignId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.DealerId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AssignedDealerId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.AssignedDealerId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.CityId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ZoneId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.ZoneId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.VersionId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.ModelId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.PlatformSourceId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LTSRC", DbType.String, 50, CustomParser.parseStringObject(inquiryDetails.Ltsrc)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comment", DbType.String, 500, CustomParser.parseStringObject(inquiryDetails.Comments)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ErrorMsg", DbType.String, -1, CustomParser.parseStringObject(errMessage)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ApplicationId", DbType.Int16, CustomParser.parseIntObject(inquiryDetails.ApplicationId)));
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.NewCarMySqlMasterConnection);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerSponsoredAdRespository.SaveFailedLeads()");
                objErr.SendMail();
            }
        }

        public int LogThirdPartyInquiryDetails(ThirdPartyInquiryDetails inquiryDetails)
        {
            int responseId = 0;
            try
            {
                using (var cmd = DbFactory.GetDBCommand("LogThirdPartyInquiryDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, 100, inquiryDetails.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 100, inquiryDetails.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile", DbType.String, 100, inquiryDetails.Mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, inquiryDetails.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int16, inquiryDetails.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int16, inquiryDetails.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int16, inquiryDetails.PlatformSourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PartnerSourceId", DbType.Int16, string.IsNullOrEmpty(inquiryDetails.PartnerSourceId) ? Convert.DBNull : inquiryDetails.PartnerSourceId.ToString()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ResponseId", DbType.Int16, ParameterDirection.Output));
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.NewCarMySqlMasterConnection);

                    var response = cmd.Parameters["v_ResponseId"].Value;
                    responseId = string.IsNullOrEmpty(response.ToString()) ? 0 : Convert.ToInt32(response);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerSponsoredAdRespository.LogThirdPartyInquiryDetails()");
                objErr.SendMail();
            }
            return responseId;
        }

        //vinayak//to update the third party status(failed or received)
        public void UpdateThirdPartyInquiryPushResponse(int LeadId, int pqDealerAdLeadId)
        {
            try
            {
                using (var cmd = DbFactory.GetDBCommand("UpdateThirdPartyInquiryDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int16, LeadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PqDealerAdLeadId", DbType.Int16, pqDealerAdLeadId));
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.NewCarMySqlMasterConnection);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerSponsoredAdRespository.UpdateThirdPartyInquiryPushResponse()");
                objErr.SendMail();
            }
        }

        public IEnumerable<DealerInquiryDetails> GetLeadDetailsByLeadId(int leadId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_LeadId", leadId);
                LogLiveSps.LogSpInGrayLog("GetLeadDetailsOnLeadId");
                using (var con = NewCarMySqlMasterConnection)
                    return con.Query<DealerInquiryDetails>("GetLeadDetailsOnLeadId_v18_10_11", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerSponsoredAdRespository.GetLeadDetailsByLeadId()");
                objErr.SendMail();
                return null;
            }

        }

        public void SaveBlockedLeads(ulong pqDealerLeadId, int reasonId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_PqDealerLeadId", Convert.ToInt64(pqDealerLeadId));
                param.Add("v_ReasonId", reasonId);
                LogLiveSps.LogSpInGrayLog("[dbo].[PQDealerBlockedLeads]");
                using (var con = NewCarMySqlReadConnection)
                    con.Execute("PQDealerBlockedLeads_v16_11_7", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                ErrorClass objErr = new ErrorClass(e, "DealerSponsoredAdRespository.SaveBlockedLeads()");
                objErr.SendMail();
            }
        }

        public int GetLeadCountForCurrentDayOnMobile(string mobile)
        {
            int leadCount = 0;
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    leadCount = con.Query<int>("GetLeadCountForCurrentDay_v16_11_7", new { Mobile = mobile }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("[dbo].[GetLeadCountForCurrentDay]");
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return leadCount;
        }

        public int GetLeadCountForCurrentDayOnCwCookie(int platformId)
        {
            var nonAppsPlatform = platformId == (int)Platform.CarwaleDesktop || platformId == (int)Platform.CarwaleMobile;
            var cwcCookieVal = nonAppsPlatform ? UserTracker.GetSessionCookie() : HttpContext.Current.Request.Headers["IMEI"];
            int leadCount = 0;
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    leadCount = con.Query<int>("GetLeadCountForCurrentDayOnCwCookie_v16_11_7", new { cwcCookieVal = cwcCookieVal }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("[dbo].[GetLeadCountForCurrentDayOnCwCookie]");
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return leadCount;
        }

        public void SaveLeadSponsoredBanner(ulong pqDealerLeadId, int targetModel, int targetVersion, int featuredVersion)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_PqDealerLeadId", Convert.ToInt64(pqDealerLeadId));
                param.Add("v_targetModelId", targetModel);
                param.Add("v_targetVersionId", targetVersion);
                param.Add("v_featureVersionId", featuredVersion);

                using (var con = NewCarMySqlReadConnection)
                    con.Execute("PQDealerSaveLeadsSponsoredBanner", param, commandType: CommandType.StoredProcedure);
                LogLiveSps.LogSpInGrayLog("[dbo].[PQDealerSaveLeadsSponsoredBanner]");
            }
            catch (Exception e)
            {
                ErrorClass objErr = new ErrorClass(e, "DealerSponsoredAdRespository.SaveLeadSponsoredBanner()");
                objErr.SendMail();
            }
        }
    }
}
