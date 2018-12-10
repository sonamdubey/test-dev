using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Common;
using Carwale.Entity.Finance;
using Carwale.Interfaces.Finance;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Carwale.DAL.Finance
{
    public class FinanceOperations : RepositoryBase, IFinanceOperations
    {
        public LoanParams IsEligibleForLoan(LoanEligibilityRequestEntity input)
        {
            LoanParams loanParams = new LoanParams();
            
            try
            {
                using (var cmd = DbFactory.GetDBCommand("CW_GetUserEligibleAmount_v16_12_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FinanceLeadId", DbType.Int32, input.FinanceLeadId > 0 ? input.FinanceLeadId : -1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IncomeTypeId", DbType.Int32, input.IncomeTypeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AnnualIncome", DbType.Int64, input.AnnualIncome));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, input.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UserDOB", DbType.DateTime, input.CustomerDOB));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CompanyId", DbType.Int32, input.CompanyId > 0 ? input.CompanyId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsExistingHdfcCustomer", DbType.Boolean, input.IsExistingCustomer));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, input.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ResidenceTypeId", DbType.Int16, input.ResidenceTypeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StabilityTime", DbType.Int16, input.StabilityTime));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerExp", DbType.Int16, input.CustomerExp != null ? input.CustomerExp : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MaxEligibleAmount", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Tenor", DbType.Int32, ParameterDirection.InputOutput, input.Tenor == 0 ? Convert.DBNull : input.Tenor));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MaxTenor", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LTV", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ROI", DbType.Decimal, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ProcessingFees", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsPermitted", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarPrice", DbType.Int64, input.ExShowroomPrice));

                    LogLiveSps.LogMySqlSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.EsMySqlMasterConnection);
                    loanParams.LoanAmount = Convert.ToInt32(cmd.Parameters["v_MaxEligibleAmount"].Value.ToString());
                    loanParams.Tenor = Convert.ToInt32(cmd.Parameters["v_Tenor"].Value.ToString());
                    loanParams.MaxTenor = Convert.ToInt32(cmd.Parameters["v_MaxTenor"].Value.ToString());
                    loanParams.LTV = Convert.ToDecimal(cmd.Parameters["v_LTV"].Value.ToString());
                    loanParams.ROI = Convert.ToDecimal(cmd.Parameters["v_ROI"].Value.ToString());
                    loanParams.IsPermitted = Convert.ToBoolean(cmd.Parameters["v_IsPermitted"].Value);
                    loanParams.ExShowroomPrice = input.ExShowroomPrice;
                    loanParams.ProcessingFees = Convert.ToInt32(cmd.Parameters["v_ProcessingFees"].Value.ToString());
                }
                
            }
            catch (SqlException ex)
            {
                var obj = new ExceptionHandler(ex, "IsEligibleForLoan()");
                obj.LogException();
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "IsEligibleForLoan()");
                obj.LogException();
            }
            return loanParams;
        }

        public List<IdName> GetFinanceCompanyListRepo(int clientId)
        {
            try
            {
                using (var con = EsMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_ClientId", clientId);

                    IEnumerable<IdName> results = con.Query<IdName>("CW_GetFinanceCompanyList_v16_12_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("CW_GetFinanceCompanyList_v16_12_1");
                    if (results != null)
                        return results.AsList();
                }
            }            
            catch (SqlException ex)
            {
                var obj = new ExceptionHandler(ex, "SaveFinanceLead()");
                obj.LogException();
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "SaveFinanceLead()");
                obj.LogException();
            }
            return null;
        }        

        public List<int> SaveLead(FinanceLead objFinanceLead)
        {
            List<int> leadIdHdfc = new List<int>();
            try
            {
                using (var con = EsMySqlMasterConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_FinanceLeadId", objFinanceLead.FinanceLeadId);
                    param.Add("v_First_Name", objFinanceLead.First_Name);
                    param.Add("v_Last_Name", objFinanceLead.Last_Name);
                    param.Add("v_Email", objFinanceLead.Email);
                    param.Add("v_Pan_No", objFinanceLead.Pan_No);

                    param.Add("v_Res_address", objFinanceLead.Res_address);
                    param.Add("v_Res_address2", objFinanceLead.Res_address2);
                    param.Add("v_Res_address3", objFinanceLead.Res_address3);
                    param.Add("v_Resi_type", objFinanceLead.Resi_type);
                    param.Add("v_CityId", objFinanceLead.CityId);

                    param.Add("v_Mobile", objFinanceLead.Mobile);
                    param.Add("v_LeadClickSource", objFinanceLead.LeadClickSource);
                    param.Add("v_Res_City", objFinanceLead.Res_City);
                    param.Add("v_Resi_City_other", objFinanceLead.Resi_City_other);
                    param.Add("v_Resi_City_other1", objFinanceLead.Resi_City_other1);

                    param.Add("v_Res_Pin", objFinanceLead.Res_Pin);
                    param.Add("v_Company_Name", objFinanceLead.Company_Name);
                    param.Add("v_DateOfBirth", objFinanceLead.DateOfBirth);
                    param.Add("v_Designation", objFinanceLead.Designation);
                    param.Add("v_Emp_Type", objFinanceLead.Emp_Type);

                    param.Add("v_Monthly_Income", objFinanceLead.Monthly_Income);
                    param.Add("v_Card_Held", objFinanceLead.Card_Held);
                    param.Add("v_Source_Code", objFinanceLead.Source_Code);
                    param.Add("v_Promo_Code", objFinanceLead.Promo_Code);
                    param.Add("v_Lead_Date_Time", !String.IsNullOrEmpty(objFinanceLead.Lead_Date_Time) ? Convert.ToDateTime(objFinanceLead.Lead_Date_Time) : (DateTime?)null);

                    param.Add("v_Product_Applied_For", objFinanceLead.Product_Applied_For);
                    param.Add("v_ExistingCust", objFinanceLead.ExistingCust);
                    param.Add("v_YearsInEmp", objFinanceLead.YearsInEmp);
                    param.Add("v_Emi_Paid", objFinanceLead.Emi_Paid);
                    param.Add("v_Car_Make", objFinanceLead.Car_Make);

                    param.Add("v_Car_Model", objFinanceLead.Car_Model);
                    param.Add("v_TypeOfLoan", objFinanceLead.TypeOfLoan);
                    param.Add("v_IP_Address", objFinanceLead.IP_Address);
                    param.Add("v_Indigo_UniqueKey", objFinanceLead.Indigo_UniqueKey);
                    param.Add("v_Indigo_RequestFromYesNo", objFinanceLead.Indigo_RequestFromYesNo);

                    param.Add("v_VersionId", objFinanceLead.VersionId);
                    param.Add("v_LoanAmount", objFinanceLead.LoanAmount);
                    param.Add("v_ROI", objFinanceLead.ROI);
                    param.Add("v_LTV", objFinanceLead.LTV);
                    param.Add("v_CarPrice", objFinanceLead.CarPrice);

                    param.Add("v_IsPermitted", objFinanceLead.IsPermitted);
                    param.Add("v_IsPushSuccess", objFinanceLead.IsPushSuccess);
                    param.Add("v_BankTypeId", objFinanceLead.BankTypeId);
                    param.Add("v_ApiResponse", objFinanceLead.ApiResponse);
                    param.Add("v_BuyingPeriod", objFinanceLead.BuyingPeriod);

                    param.Add("v_IncomeTypeId", objFinanceLead.IncomeTypeId);
                    param.Add("v_YearsInRes", objFinanceLead.YearsInRes);
                    param.Add("v_FailureReason", objFinanceLead.FailureReason);
                    param.Add("v_ClientId", objFinanceLead.ClientId);
                    param.Add("v_PlatformId", objFinanceLead.PlatformId);

                    param.Add("v_UtmCode", objFinanceLead.UtmCode);
                    param.Add("v_ClientCustomerId", objFinanceLead.ClientCustomerId);

                    var response = con.Query("CW_SaveFinanceLeads_v17_9_1", param, commandType: CommandType.StoredProcedure) as IEnumerable<IDictionary<string, object>>;
                    LogLiveSps.LogSpInGrayLog("CW_SaveFinanceLeads_v17_9_1");
                    var outData = response.Select(r => r.ToDictionary(d => d.Key, d => d.Value.ToString()));
                    leadIdHdfc.Add(CustomParser.parseIntObject(outData.FirstOrDefault()["FinanceLeadId"]));
                    leadIdHdfc.Add(CustomParser.parseIntObject(outData.FirstOrDefault()["PushToClient"]));
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "SaveFinanceLead()");
                obj.LogException();
            }
            return leadIdHdfc;
        }

        public bool UpdateLeadResponse(FinanceLead objFinanceLead)
        {
            bool success = false;
            try
            {
                using (var con = EsMySqlMasterConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_FinanceLeadId", objFinanceLead.FinanceLeadId);
                    param.Add("v_First_Name", objFinanceLead.First_Name);
                    param.Add("v_Last_Name", objFinanceLead.Last_Name);
                    param.Add("v_Email", objFinanceLead.Email);
                    param.Add("v_Pan_No", objFinanceLead.Pan_No);

                    param.Add("v_Res_address", objFinanceLead.Res_address);
                    param.Add("v_Res_address2", objFinanceLead.Res_address2);
                    param.Add("v_Res_address3", objFinanceLead.Res_address3);
                    param.Add("v_Resi_type", objFinanceLead.Resi_type);
                    param.Add("v_CityId", objFinanceLead.CityId);

                    param.Add("v_Mobile", objFinanceLead.Mobile);
                    param.Add("v_LeadClickSource", objFinanceLead.LeadClickSource);
                    param.Add("v_Res_City", objFinanceLead.Res_City);
                    param.Add("v_Resi_City_other", objFinanceLead.Resi_City_other);
                    param.Add("v_Resi_City_other1", objFinanceLead.Resi_City_other1);

                    param.Add("v_Res_Pin", objFinanceLead.Res_Pin);
                    param.Add("v_Company_Name", objFinanceLead.Company_Name);
                    param.Add("v_DateOfBirth", objFinanceLead.DateOfBirth);
                    param.Add("v_Designation", objFinanceLead.Designation);
                    param.Add("v_Emp_Type", objFinanceLead.Emp_Type);

                    param.Add("v_Monthly_Income", objFinanceLead.Monthly_Income);
                    param.Add("v_Card_Held", objFinanceLead.Card_Held);
                    param.Add("v_Source_Code", objFinanceLead.Source_Code);
                    param.Add("v_Promo_Code", objFinanceLead.Promo_Code);
                    param.Add("v_Lead_Date_Time", !String.IsNullOrEmpty(objFinanceLead.Lead_Date_Time) ? Convert.ToDateTime(objFinanceLead.Lead_Date_Time) : (DateTime?)null);

                    param.Add("v_Product_Applied_For", objFinanceLead.Product_Applied_For);
                    param.Add("v_ExistingCust", objFinanceLead.ExistingCust);
                    param.Add("v_YearsInEmp", objFinanceLead.YearsInEmp);
                    param.Add("v_Emi_Paid", objFinanceLead.Emi_Paid);
                    param.Add("v_Car_Make", objFinanceLead.Car_Make);

                    param.Add("v_Car_Model", objFinanceLead.Car_Model);
                    param.Add("v_TypeOfLoan", objFinanceLead.TypeOfLoan);
                    param.Add("v_IP_Address", objFinanceLead.IP_Address);
                    param.Add("v_Indigo_UniqueKey", objFinanceLead.Indigo_UniqueKey);
                    param.Add("v_Indigo_RequestFromYesNo", objFinanceLead.Indigo_RequestFromYesNo);

                    param.Add("v_VersionId", objFinanceLead.VersionId);
                    param.Add("v_LoanAmount", objFinanceLead.LoanAmount);
                    param.Add("v_ROI", objFinanceLead.ROI);
                    param.Add("v_LTV", objFinanceLead.LTV);
                    param.Add("v_CarPrice", objFinanceLead.CarPrice);

                    param.Add("v_IsPermitted", objFinanceLead.IsPermitted);
                    param.Add("v_IsPushSuccess", objFinanceLead.IsPushSuccess);
                    param.Add("v_BankTypeId", objFinanceLead.BankTypeId);
                    param.Add("v_ApiResponse", objFinanceLead.ApiResponse);
                    param.Add("v_BuyingPeriod", objFinanceLead.BuyingPeriod);

                    param.Add("v_IncomeTypeId", objFinanceLead.IncomeTypeId);
                    param.Add("v_YearsInRes", objFinanceLead.YearsInRes);
                    param.Add("v_FailureReason", objFinanceLead.FailureReason);
                    param.Add("v_ClientId", objFinanceLead.ClientId);
                    param.Add("v_PlatformId", objFinanceLead.PlatformId);

                    param.Add("v_UtmCode", objFinanceLead.UtmCode);
                    param.Add("v_ClientCustomerId", objFinanceLead.ClientCustomerId);

                    con.Execute("CW_SaveFinanceLeads_v17_9_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("CW_SaveFinanceLeads_v17_9_1");
                    success = true;
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "CW_UpdateFinanceLeads()");
                obj.LogException();
            }
            return success;
        }

        public bool CheckEligibility(LoanEligibilityRequestEntity eligibilityInput)
        {
            bool isPermitted = false;
            try
            {
                using (var con = EsMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_StabilityTime", eligibilityInput.StabilityTime);
                    param.Add("v_ResidenceTypeId", eligibilityInput.ResidenceTypeId);
                    param.Add("v_IncomeTypeId", eligibilityInput.IncomeTypeId);
                    param.Add("v_UserDOB", eligibilityInput.CustomerDOB);
                    param.Add("v_AnnualIncome", eligibilityInput.AnnualIncome);
                    param.Add("v_CustomerExp", eligibilityInput.CustomerExp);

                    var result = con.Query("CW_GetUserEligibilityCriteria_v16_12_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("CW_GetUserEligibilityCriteria_v16_12_1");
                    if (result != null) 
                        isPermitted = Convert.ToBoolean(result.First().IsPermitted);
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "CheckEligibility()");
                obj.LogException();
            }
            return isPermitted;
        }
    }
}