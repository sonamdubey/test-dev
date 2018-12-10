using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Notifications.Logs;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Data;

namespace Carwale.DAL.Classified.SellCar
{
    public class CarDetailsRepository : RepositoryBase, ICarDetailsRepository
    {
        public bool UpdateCarDetails(SellCarInfo sellCarInfo, int inquiryId)
        {
            bool ret = false;
            if (sellCarInfo != null)
            {
                try
                {
                    var param = GetUpdateCarParameters(sellCarInfo, inquiryId);
                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        ret = con.Execute("updatecustomersellinquiry_v1", param, commandType: CommandType.StoredProcedure) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw;
                } 
            }
            return ret;
        }
        public bool UpdateCarDetailsV1(SellCarInfo sellCarInfo, int inquiryId)
        {
            bool ret = false;
            if (sellCarInfo != null)
            {
                try
                {
                    var param = GetUpdateCarParameters(sellCarInfo, inquiryId);
                    param.Add("v_regtype", sellCarInfo.RegType.ToString());
                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        ret = con.Execute("updatecustomersellinquiry_v2", param, commandType: CommandType.StoredProcedure) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw;
                }
            }
            return ret;
        }

        private static DynamicParameters GetUpdateCarParameters(SellCarInfo sellCarInfo, int inquiryId)
        {
            DateTime? insuranceExpiryDate = null;
            if (sellCarInfo.InsuranceExpiryYear.HasValue && sellCarInfo.InsuranceExpiryMonth.HasValue)
            {
                insuranceExpiryDate = new DateTime(sellCarInfo.InsuranceExpiryYear.Value, sellCarInfo.InsuranceExpiryMonth.Value, 01);
            }
            var param = new DynamicParameters();
            param.Add("v_carversionid", sellCarInfo.VersionId, DbType.Int32);
            param.Add("v_makeyear", new DateTime(sellCarInfo.ManufactureYear, sellCarInfo.ManufactureMonth, 1), DbType.DateTime);
            param.Add("v_kms", sellCarInfo.KmsDriven, DbType.Int32);
            param.Add("v_price", sellCarInfo.ExpectedPrice, DbType.Int32);
            param.Add("v_owners", sellCarInfo.Owners, DbType.Int16);
            param.Add("v_regno", sellCarInfo.RegistrationNumber, DbType.String);
            param.Add("v_color", sellCarInfo.Color, DbType.String);
            param.Add("v_additionalfuel", sellCarInfo.AlternateFuel, DbType.String);
            param.Add("v_insurance", Convert.ToString(sellCarInfo.Insurance), DbType.String);
            param.Add("v_insuranceexpiry", insuranceExpiryDate, DbType.DateTime);
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            return param;
        }
    }
}
