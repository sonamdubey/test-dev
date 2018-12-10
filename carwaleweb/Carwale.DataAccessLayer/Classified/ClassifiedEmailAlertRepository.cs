using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.Classified
{
    public class ClassifiedEmailAlertRepository : RepositoryBase, IClassifiedEmailAlertRepository
    {
        public bool SaveNdUsedCarAlertCustomerList(NdUsedCarAlert alertData)
        {
            bool inserted = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_customerid", alertData.CustomerId, DbType.Int32);
                parameters.Add("v_email", alertData.Email, DbType.String);
                parameters.Add("v_cityid", alertData.CityId == Int32.MinValue ? Convert.DBNull : alertData.CityId, DbType.Int32);
                parameters.Add("v_makeid", alertData.MakeId, DbType.String);
                parameters.Add("v_modelid", alertData.ModelId, DbType.String);
                parameters.Add("v_fueltypeid", alertData.FuelTypeId, DbType.String);
                parameters.Add("v_bodystyleid", alertData.BodyStyleId, DbType.String);
                parameters.Add("v_transmissionid", alertData.TransmissionId, DbType.String);
                parameters.Add("v_sellerid", alertData.SellerId, DbType.String);
                parameters.Add("v_minbudget", alertData.MinBudget, DbType.Single);
                parameters.Add("v_maxbudget", alertData.MaxBudget, DbType.Single);
                parameters.Add("v_minkms", alertData.MinKms == Int32.MinValue ? Convert.DBNull : alertData.MinKms, DbType.Int32);
                parameters.Add("v_maxkms", alertData.MaxKms == Int32.MinValue ? Convert.DBNull : alertData.MaxKms, DbType.Int32);
                parameters.Add("v_mincarage", alertData.MinCarAge == Int32.MinValue ? Convert.DBNull : alertData.MinCarAge, DbType.Int32);
                parameters.Add("v_maxcarage", alertData.MaxCarAge == Int32.MinValue ? Convert.DBNull : alertData.MaxCarAge, DbType.Int32);
                parameters.Add("v_needonlycertifiedcars", alertData.NeedOnlyCertifiedCars, DbType.Boolean);
                parameters.Add("v_needcarwithphotos", alertData.NeedCarWithPhotos, DbType.Boolean);
                parameters.Add("v_ownertypeid", alertData.OwnerTypeId, DbType.String);
                parameters.Add("v_alertfrequency", alertData.AlertFrequency == Int32.MinValue ? Convert.DBNull : alertData.AlertFrequency, DbType.Int32);
                parameters.Add("v_alerturl", alertData.AlertUrl, DbType.String);
                parameters.Add("v_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("sendusersearchcriteria_15_7_1", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("sendusersearchcriteria_15_7_1");
                }
                inserted = parameters.Get<int>("v_status") == 1;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ClassifiedEmailAlertRepository.SaveNdUsedCarAlertCustomerList");
                objErr.SendMail();
            }
            return inserted;
        }

        public bool UnsubscribeNdUsedCarAlertCustomer(int ucAlertid, string email, out int cityId)
        {
            bool ret = false;
            cityId = 0;
            DynamicParameters param = new DynamicParameters();
            param.Add("v_usedcaralertid", ucAlertid, DbType.Int32);
            param.Add("v_customeremail", email, DbType.String);
            param.Add("v_customercity", DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    ret = con.Execute("setusedcarcustomerinactive", param, commandType: CommandType.StoredProcedure) > 0;
                    LogLiveSps.LogSpInGrayLog("setusedcarcustomerinactive");
                    cityId = param.Get<int?>("v_customercity") != null ? param.Get<int>("v_customercity") : 0;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ClassifiedEmailAlertRepository.UnsubscribeNdUsedCarAlertCustomer");
                objErr.SendMail();
            }
            return ret;
        }
    }
}
