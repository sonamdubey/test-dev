using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using Bikewale.Utility;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Finance.CapitalFirst
{
    public class FinanceRepository : IFinanceRepository
    {


        public uint SavePersonalDetails(PersonalDetails objDetails)
        {
            uint id = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_id", objDetails.Id, DbType.Int32, ParameterDirection.InputOutput);
                    param.Add("par_leadid", objDetails.LeadId);
                    param.Add("par_ctleadid", objDetails.CtLeadId);
                    param.Add("par_firstname", objDetails.FirstName);
                    param.Add("par_lastname", objDetails.LastName);
                    param.Add("par_mobilenumber", objDetails.MobileNumber);
                    param.Add("par_emailid", objDetails.EmailId);
                    param.Add("par_dateofbirth", objDetails.DateOfBirth);
                    param.Add("par_gender", objDetails.Gender);
                    param.Add("par_maritalStatus", objDetails.MaritalStatus);
                    param.Add("par_addressLine1", objDetails.AddressLine1);
                    param.Add("par_addressLine2", objDetails.AddressLine2);
                    param.Add("par_pincode", objDetails.Pincode);
                    param.Add("par_pancard", objDetails.Pancard);

                    param.Add("par_status", objDetails.Status);
                    param.Add("par_companyname", objDetails.CompanyName);
                    param.Add("par_officialaddressline1", objDetails.OfficialAddressLine1);
                    param.Add("par_officialaddressline2", objDetails.OfficialAddressLine2);
                    param.Add("par_pincodeoffice", objDetails.PincodeOffice);
                    param.Add("par_annualincome", objDetails.AnnualIncome);
                    param.Add("par_cityId", objDetails.objLead.CityId);
                    param.Add("par_versionid", objDetails.objLead.VersionId);
                    param.Add("par_loanAmount", objDetails.LoanAmount);
                    connection.Execute("savecapitalfirstleaddetails_25102017", param: param, commandType: CommandType.StoredProcedure);
                    id = SqlReaderConvertor.ToUInt32(param.Get<Int32>("par_id"));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "FinanceRepository.SavePersonalDetails");
            }
            return id;
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Checks if it is valid Lead Id
        /// </summary>
        /// <param name="leadId">CarTrade Lead Id</param>
        /// <returns></returns>
        public bool IsValidLead(string leadId)
        {
            Boolean isValid = false;
            try
            {
                using (IDbConnection conn = DatabaseHelper.GetReadonlyConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_leadId", leadId, dbType: DbType.Int32);
                    param.Add("par_isvalid", dbType: DbType.Int16, direction: ParameterDirection.Output);
                    conn.Execute("isvalidcapitalfirstlead", param: param, commandType: CommandType.StoredProcedure);
                    isValid = SqlReaderConvertor.ToBoolean(param.Get<Int16>("par_isvalid"));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("FinanceRepository.IsValidLead({0})", leadId));
            }
            return isValid;
        }

        public bool SaveVoucherDetails(string leadIdCarTrade, CapitalFirstVoucherEntityBase voucherDetails)
        {
            Boolean isSaved = false;
            try
            {
                using (IDbConnection conn = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_CTLeadId", leadIdCarTrade, dbType: DbType.Int32);
                    param.Add("par_voucherCode", voucherDetails.VoucherCode);
                    param.Add("par_voucherExpiryDate", voucherDetails.ExpiryDate, dbType: DbType.Date);
                    param.Add("par_agentName", voucherDetails.AgentName, dbType: DbType.String);
                    param.Add("par_agentNumber", voucherDetails.AgentContactNumber, dbType: DbType.String);
                    param.Add("par_leadStatus", (int)voucherDetails.Status, dbType: DbType.Int32);
                    conn.Execute("savecapitalfirstvoucherdetails", param: param, commandType: CommandType.StoredProcedure);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("FinanceRepository.SaveVaoucherDetails({0},{1})", leadIdCarTrade, Newtonsoft.Json.JsonConvert.SerializeObject(voucherDetails)));
            }
            return isSaved;
        }

        public CapitalFirstBikeEntity GetCapitalFirstBikeMapping(uint versionId)
        {
            CapitalFirstBikeEntity bike = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getcapitalfirstbikemapping";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            bike = new CapitalFirstBikeEntity();
                            bike.MakeBase = new CapitalFirstMakeEntityBase() { Make = Convert.ToString(dr["Make"]), MakeId = Convert.ToString(dr["MakeId"]) };
                            bike.ModelBase = new CapitalFirstModelEntityBase()
                            {
                                ModelId = Convert.ToString(dr["ModelId"]),
                                ModelNo = Convert.ToString(dr["ModelNo"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("FinanceRepository.GetCapitalFirstBikeMapping({0})", versionId));
            }
            return bike;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 14 Sep 2017
        /// Description :   Save CTApi Response and lead status
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="status"></param>
        /// <param name="responseText"></param>
        /// <returns></returns>
        public bool SaveCTApiResponse(uint leadId, uint ctleadid, ushort status, string responseText)
        {
            Boolean isSaved = false;
            try
            {
                using (IDbConnection conn = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_leadid", leadId, dbType: DbType.Int32);
                    param.Add("par_ctleadid", ctleadid, dbType: DbType.Int32);
                    param.Add("par_status", status);
                    param.Add("par_apiresponse", responseText, dbType: DbType.String);
                    conn.Execute("updatecapitalfirstleadresponse", param: param, commandType: CommandType.StoredProcedure);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("FinanceRepository.SaveVaoucherDetails({0},{1})", leadId, responseText));
            }
            return isSaved;
        }
    }
}
