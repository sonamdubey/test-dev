using Carwale.Utility;
using Dapper;
using System;
using System.Data;
using System.Linq;
using Carwale.Interfaces.CustomerVerification;
using Carwale.Entity.Enum;
using Carwale.Notifications.Logs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Carwale.DAL.CustomerVerification
{
    public class CustomerVerificationRepository : RepositoryBase, ICustomerVerificationRepository
    {
        public bool VerifyMobile(string mobileNumber, string emailId, Platform platformId, out string otpCode, string cvid, int source)
        {
            bool isMobileVerified=false;
            string cwiCode;
            try
            {
                var param = GetVerifyMobileDynamicParams(mobileNumber, emailId, platformId, cvid, source);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.CV_VerifyMobile_v17_10_3", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("cwmasterdb.CV_VerifyMobile_v17_10_3");
                    isMobileVerified = param.Get<Int16>("v_IsMobileVer") == 1;
                    cwiCode = param.Get<int>("v_NewCVID").ToString().PadLeft(5, '0');
                }
            }
            catch (Exception ex)
            {
                cwiCode = "-1";
                Logger.LogException(ex);
            }
            
            otpCode = cwiCode;

            return isMobileVerified;
        }

        public bool VerifyMobile(string mobileNumber, string emailId, Platform platformId, out string otpCode, string cvid, int source, string clientTokenId)
        {
            bool isMobileVerified = false;
            string cwiCode;

            try
            {
                var param = GetVerifyMobileDynamicParams(mobileNumber, emailId, platformId, cvid, source);
                param.Add("v_clientTokenId", clientTokenId);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.CV_VerifyMobileAndToken", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("cwmasterdb.CV_VerifyMobileAndToken");
                    isMobileVerified = param.Get<Int16>("v_IsMobileVer") == 1;
                    cwiCode = param.Get<int>("v_NewCVID").ToString().PadLeft(5, '0');
                }
            }
            catch (Exception ex)
            {
                cwiCode = "-1";
                Logger.LogException(ex);
            }

            otpCode = cwiCode;

            return isMobileVerified;
        }

        public bool IsVerified(string mobile, string clientTokenId)
        {
            bool isVerified = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_mobile", mobile, DbType.String);
                param.Add("v_clientTokenId", clientTokenId, DbType.String);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    isVerified = con.Query<bool>("IsMobileAndTokenVerified", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isVerified;
        }

        public bool ProcessTollFreeNumber(string mobileNumber, string emailId, Platform platformId, int source, string clientTokenId)
        {
            if (!emailId.Contains("@"))
            {
                emailId = mobileNumber + "@unknown.com";
            }
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MobileNumber", mobileNumber);
                param.Add("v_Email", emailId);
                param.Add("v_Source", source);
                param.Add("v_PlatformId", platformId);
                param.Add("v_clientTokenId", clientTokenId);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    LogLiveSps.LogSpInGrayLog("cwmasterdb.InsertRecordMissCallVerification_v18_5_22");
                    return con.Execute("cwmasterdb.InsertRecordMissCallVerification_v18_5_22", param, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        public string GetOtpCode(string email, string mobile)
        {
            string otp = string.Empty;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_Mobile", mobile, DbType.String);
                param.Add("v_Email", email, DbType.String);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    otp = con.Query<string>("cwmasterdb.GetOtpCode", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("cwmasterdb.GetOtpCode");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return otp;
        }

        public bool CheckVerification(string mobile, string cwiCode, string cuiCode, string email, string clientTokenId, int sourceId = 0)
        {
            bool verified = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MobileNo", mobile, DbType.String);
                param.Add("v_CWICode", cwiCode, DbType.String);
                param.Add("v_CUICode", cuiCode, DbType.String);
                param.Add("v_SourceId", sourceId, DbType.Int32);
                param.Add("v_clientTokenId", clientTokenId ?? "", DbType.String);
                param.Add("v_IsVerified", dbType: DbType.Int16, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.CV_CheckVerification_18_5_17", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("cwmasterdb.CV_CheckVerification_18_5_17");
                }
                verified = param.Get<short>("v_IsVerified") == 1;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return verified;
        }

        public void VerifyByMissedCall(string mobile, string transToken, string toCall, out string email, int sourceId = 0)
        {
            email = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_TransactionToken", transToken, DbType.String);
                param.Add("v_ToCall", toCall, DbType.String);
                param.Add("v_Mobile", mobile, DbType.String);
                param.Add("v_Source", sourceId, DbType.Int32);
                param.Add("v_Email", dbType: DbType.String, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.ClientMissedCallVerification_18_5_23", param, commandType: CommandType.StoredProcedure);
                    email = param.Get<string>("v_Email");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public bool CheckMissedCallVerification(string mobileNo, string email)
        {
            bool isVerified = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MobileNumber", mobileNo, DbType.String);
                param.Add("v_EmailId", email, DbType.String);
                param.Add("v_Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.CheckIfUserIsVerified_ZipDial", param, commandType: CommandType.StoredProcedure);
                }
                isVerified = param.Get<int>("v_Status") > 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isVerified;
        }

        public bool IsMobileVerified(string mobileNumber)
        {
            bool isVerified = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_mobilenumber", mobileNumber);
                param.Add("v_isverified", direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("cwmasterdb.ismobileverified_v17_10_3", param, commandType: CommandType.StoredProcedure);
                    isVerified = param.Get<int>("v_isverified") == 1;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isVerified;
        }

        public void DeleteVerifiedMobileNos(IEnumerable<string> mobiles)
        {
            var param = new DynamicParameters();
            param.Add("v_mobiles", string.Join(",", mobiles), DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("cwmasterdb.DeleteVerifiedMobileNos", param, commandType: CommandType.StoredProcedure);
            }
        }

        private DynamicParameters GetVerifyMobileDynamicParams(string mobileNumber, string emailId, Platform platformId, string cvid, int source)
        {
            Random rnd = new Random();
            Random rnd1 = new Random(rnd.Next());
            string cwiCode = Miscellaneous.GetRandomCode(rnd, 5);
            string cuiCode = Miscellaneous.GetRandomCode(rnd1, 5);

            if (!emailId.Contains("@"))
            {
                emailId = mobileNumber + "@unknown.com";
            }

            var param = new DynamicParameters();
            param.Add("v_EmailId", emailId);
            param.Add("v_MobileNo", mobileNumber);
            param.Add("v_CVID", cvid);
            param.Add("v_CWICode", cwiCode);
            param.Add("v_CUICode", cuiCode);
            param.Add("v_EntryDateTime", DateTime.Now);
            param.Add("v_IsMobileVer", dbType: DbType.Int16, direction: ParameterDirection.Output);
            param.Add("v_SourceId", source);
            param.Add("v_PlatformId", platformId);
            param.Add("v_NewCVID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            return param;
        }
    }
}
