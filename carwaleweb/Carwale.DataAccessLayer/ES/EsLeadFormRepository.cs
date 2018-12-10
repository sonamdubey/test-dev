using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;
namespace Carwale.DAL.ES
{
    public class EsLeadFormRepository : RepositoryBase, IEsLeadFormRepository
    {
        public int SubmitEsLeadFormData(EsLeadFormResponse customerResponse)
        {
            int customerId = -1;
            try
            {              
                var param = new DynamicParameters();
                param.Add("v_CustomerId", customerResponse.Id != -1 ? customerResponse.Id : -1);
                param.Add("v_LeadType", customerResponse.LeadTypeId != -1 ? customerResponse.LeadTypeId : -1);
                param.Add("v_Email", customerResponse.Email != null ? customerResponse.Email : null);
                param.Add("v_Mobile", customerResponse.MobileNo != null ? customerResponse.MobileNo : null);
                param.Add("v_CustName", customerResponse.Name != null ? customerResponse.Name : null);
                param.Add("v_Power", customerResponse.Power != null ? customerResponse.Power : null);
                param.Add("v_SelectedCityId", customerResponse.CityId);
                param.Add("v_Age", customerResponse.Age != 0 ? customerResponse.Age : 0);
                param.Add("v_Profession", customerResponse.Profession != null ? customerResponse.Profession : null);
                param.Add("v_CurrentCar", customerResponse.CurrentCar != null ? customerResponse.CurrentCar : null);
                param.Add("v_Licence", customerResponse.Licence ? 1 : 0);
                param.Add("v_Facebook", customerResponse.Facebook != null ? customerResponse.Facebook : null);
                param.Add("v_Twitter", customerResponse.Twitter != null ? customerResponse.Twitter : null);
                param.Add("v_LinkdIn", customerResponse.LinkdIn != null ? customerResponse.LinkdIn : null);
                param.Add("v_Instagram", customerResponse.Instagram != null ? customerResponse.Instagram : null);

                using (var con = EsMySqlMasterConnection)
                {
                    var response = con.Query("SaveLeadFormData_v17_4_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("SaveLeadFormData_v17_4_1");
                    customerId = response.AsList()[0].CustomerId;
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "DAL.SubmitSurvey()");
                obj.LogException();
            }
            return customerId;
        }
    }
}
