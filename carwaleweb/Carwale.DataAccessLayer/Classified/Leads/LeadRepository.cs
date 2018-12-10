using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified.Leads
{
    public class LeadRepository : RepositoryBase, ILeadRepository
    {
        public LeadStatus CheckLeadStatus(string mobile, string ip, int inquiryId, bool isDealer, out int leadId)
        {
            var param = new DynamicParameters();
            param.Add("v_mobile", mobile, DbType.String);
            param.Add("v_ip", ip, DbType.String);
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            param.Add("v_isdealer", isDealer, DbType.Boolean);
            param.Add("v_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("v_leadid", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var con = ClassifiedMySqlReadConnection)
            {
                con.Execute("CheckLeadStatus_v18_7_1", param, commandType: CommandType.StoredProcedure);
            }
            leadId = param.Get<int>("v_leadid");
            return param.Get<LeadStatus>("v_status");
        }

        public int InsertLead(LeadDetail lead, int buyerCity, BuyerInfo buyerInfo, bool isDuplicate)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", lead.Stock.InquiryId, DbType.Int32);
            param.Add("v_isdealer", lead.Stock.IsDealer, DbType.Boolean);
            param.Add("v_buyername", lead.Buyer.Name, DbType.String);
            param.Add("v_buyeremail", lead.Buyer.Email, DbType.String);
            param.Add("v_buyermobile", lead.Buyer.Mobile, DbType.String);
            param.Add("v_sourceid", lead.LeadSource, DbType.Int32);
            param.Add("v_appversion", lead.AppVersion, DbType.Int16);
            param.Add("v_originid", lead.LeadTrackingParams.OriginId, DbType.Int32);
            param.Add("v_ipaddress", lead.IPAddress, DbType.String);
            param.Add("v_imeicode", lead.IMEICode, DbType.String);
            param.Add("v_ltsrc", lead.LTSrc, DbType.String);
            param.Add("v_cwc", lead.Cwc, DbType.String);
            param.Add("v_utmacookie", lead.UtmaCookie, DbType.String);
            param.Add("v_utmzcookie", lead.UtmzCookie, DbType.String);
            param.Add("v_cwutmzcookie", lead.CWUtmzCookie, DbType.String);
            param.Add("v_buyercityid", buyerCity, DbType.Int32);
            param.Add("v_leadtype", lead.LeadTrackingParams.LeadType, DbType.SByte);
            param.Add("v_isduplicate", isDuplicate, dbType: DbType.Boolean);
            param.Add("v_userid", buyerInfo.UserId, DbType.String, direction: ParameterDirection.InputOutput);
            param.Add("v_accesstoken", buyerInfo.AccessToken, DbType.String, direction: ParameterDirection.InputOutput);
            param.Add("v_leadid", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("v_ischatleadgiven", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("InsertUsedLead_v18_7_3", param, commandType: CommandType.StoredProcedure);
            }
            buyerInfo.IsChatLeadGiven = param.Get<int>("v_ischatleadgiven") == (int)LeadType.ChatLead;
            buyerInfo.UserId = param.Get<string>("v_userid");
            buyerInfo.AccessToken = param.Get<string>("v_accesstoken");
            return param.Get<int>("v_leadid");
        }

        public void InsertUnverifiedLead(LeadDetail lead)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", lead.Stock.InquiryId, DbType.Int32);
            param.Add("v_sellertype", lead.Stock.IsDealer ? "1" : "2", DbType.String);
            param.Add("v_buyername", lead.Buyer.Name, DbType.String);
            param.Add("v_buyeremail", lead.Buyer.Email, DbType.String);
            param.Add("v_buyermobile", lead.Buyer.Mobile, DbType.String);
            param.Add("v_sourceid", lead.LeadSource, DbType.Int32);
            param.Add("v_originid", lead.LeadTrackingParams.OriginId, DbType.Int32);
            param.Add("v_ipaddress", lead.IPAddress, DbType.String);
            param.Add("v_utmacookie", lead.UtmaCookie, DbType.String);
            param.Add("v_utmzcookie", lead.UtmzCookie, DbType.String);
            param.Add("v_cwutmzcookie", lead.CWUtmzCookie, DbType.String);
            param.Add("v_leadtype", lead.LeadTrackingParams.LeadType, DbType.SByte);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("insertclassifiedleads_v18_6_1", param, commandType: CommandType.StoredProcedure);
            }
        }

        public bool InsertLeadLog(int leadId, ClassifiedStockSource sourceTable, UsedLeadPushApiSource apiSource)
        {
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("v_leadId", leadId);
                    parameters.Add("v_leadTable", (int)sourceTable);
                    parameters.Add("v_apiSource", (int)apiSource);
                    return con.Execute("verifiedLeadLogger", parameters, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public bool InsertLeadTracking(int leadId, UsedLeadType leadType, int abCookie)
        {
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("v_leadid", leadId, DbType.Int32);
                    parameters.Add("v_leadtype", (Int16)leadType, DbType.Int16);
                    parameters.Add("v_abcookie", abCookie, DbType.Int32);
                    return con.Execute("InsertLeads", parameters, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public bool ShouldResendNotification(int leadId, bool isDealer)
        {
            var param = new DynamicParameters();
            param.Add("v_leadid", leadId, DbType.Int32);
            param.Add("v_isdealer", isDealer, DbType.Int32);
            param.Add("v_resend", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var con = ClassifiedMySqlReadConnection)
            {
                con.Execute("ShouldResendNotification", param, commandType: CommandType.StoredProcedure);
            }
            return param.Get<int>("v_resend") == 1;
        }

        public void InsertLeadNotifications(int leadId, bool isDealer)
        {
            var param = new DynamicParameters();
            param.Add("v_leadid", leadId, DbType.Int32);
            param.Add("v_isdealer", isDealer, DbType.Int32);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("InsertLeadNotifications", param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Lead> GetStockLeads(string mobile, int count)
        {
            var param = new DynamicParameters();
            param.Add("v_mobile", mobile, DbType.String);
            param.Add("v_count", count, DbType.Int32);

            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<Lead>("GetStockLeads", param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Lead> GetDealerStockLeads(string mobile, int count, bool isOrderAsc, int dealerId)
        {
            var param = new DynamicParameters();
            param.Add("v_mobile", mobile, DbType.String);
            param.Add("v_count", count, DbType.Int32);
            param.Add("v_isAsc", isOrderAsc, DbType.Byte);
            param.Add("v_dealerid", dealerId, DbType.Int32);

            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<Lead>("GetDealerStockLeads", param, commandType: CommandType.StoredProcedure);
            }
        }

        public List<ClassifiedRequest> GetClassifiedRequests(int inquiryId, int requestDate)
        {
            List<ClassifiedRequest> classifiedRequests = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId, DbType.Int32);
                param.Add("v_requestdate", requestDate, DbType.Int16);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    classifiedRequests = con.Query<ClassifiedRequest>("getclassifiedpurchaserequests", param, commandType: CommandType.StoredProcedure).ToList();
                    LogLiveSps.LogSpInGrayLog("getclassifiedpurchaserequests");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return classifiedRequests;
        }

        public bool UpdateInquiryStatus(int inquiryId, int inquiryType, int customerId, int statusId)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", inquiryId);
            param.Add("v_inquirytype", inquiryType);
            param.Add("v_customerid", customerId);
            param.Add("v_statusid", statusId);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    LogLiveSps.LogSpInGrayLog("updateinquirystatus");
                    return con.Execute("updateinquirystatus", param, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public bool IsLeadGivenToDealer(string mobile, string chatTokenId)
        {
            var param = new DynamicParameters();
            param.Add("v_mobile", mobile, DbType.String);
            param.Add("v_chatTokenId", chatTokenId, DbType.String);

            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<bool>("IsLeadGivenToDealer", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
