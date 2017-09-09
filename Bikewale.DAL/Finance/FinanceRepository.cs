using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
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
    }
}
