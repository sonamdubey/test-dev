using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using Bikewale.Utility;
using Dapper;
using System;
using System.Data;

namespace Bikewale.DAL.Finance.CapitalFirst
{
    public class FinanceRepository : IFinanceRepository
    {


        public bool SavePersonalDetails(PersonalDetails objDetails)
        {
            bool success = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
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

                    success = Convert.ToBoolean(connection.Execute("savepersonaldetailsfinance", param: param, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "FinanceRepository.SavePersonalDetails");
            }
            return success;
        }
        public bool SaveEmployeDetails(EmployeDetails objDetails)
        {
            bool success = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_status", objDetails.Status);
                    param.Add("par_companyName", objDetails.CompanyName);
                    param.Add("par_officalAddressLine1", objDetails.OfficalAddressLine1);
                    param.Add("par_officalAddressLine2", objDetails.OfficalAddressLine2);
                    param.Add("par_pincode", objDetails.Pincode);
                    param.Add("par_annualIncome", objDetails.AnnualIncome);
                    success = Convert.ToBoolean(connection.Execute("saveemployedetailsfinance", param: param, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "FinanceRepository.SavePersonalDetails");
            }
            return success;
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
                    conn.Open();
                    conn.Execute("isvalidcapitalfirstlead", param: param, commandType: CommandType.StoredProcedure);
                    isValid = SqlReaderConvertor.ToBoolean(param.Get<Int16>("par_isvalid"));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("FinanceRepository.IsValidLead({0})", leadId));
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
                    conn.Open();
                    conn.Execute("savecapitalfirstvoucherdetails", param: param, commandType: CommandType.StoredProcedure);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("FinanceRepository.SaveVaoucherDetails({0},{1})", leadIdCarTrade, Newtonsoft.Json.JsonConvert.SerializeObject(voucherDetails)));
            }
            return isSaved;
        }
    }
}
