using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Carwale.DAL.PriceQuote
{
    public class PricesRepository<T1, T2> : RepositoryBase, IPricesRepository<T1, T2>
        where T1 : CarPriceQuote
        where T2 : VersionPriceQuote
    {
        //type = 1 -> form itemid-itemvalue string
        //type = 2 -> form itemids string
        private string FormKeyString(T2 pricesInput, int type)
        {
            try
            {
                string keyString = "";

                if (type == 1)
                {
                    foreach (var item in pricesInput.PricesList)
                    {
                        keyString = string.Format("{0}{1}-{2},", keyString, item.PQItemId, item.PQItemValue);
                    }
                }
                else
                {
                    foreach (var item in pricesInput.PricesList)
                    {
                        keyString = string.Format("{0}{1},", keyString, item.PQItemId);
                    }
                }

                keyString = keyString.TrimEnd(',');

                return keyString;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return null;
            }
        }

        public bool InsertPriceQuoteInDB(T2 pricesInput, int cityId, int updatedBy, DynamicParameters param)
        {
            string keyValuePairs = FormKeyString(pricesInput, 1);
            if (keyValuePairs == null)
                return false;

            param.Add("v_VersionId", pricesInput.VersionId, DbType.Int32, direction: ParameterDirection.Input);
            param.Add("v_IsMetallic", pricesInput.IsMetallic, DbType.Boolean, direction: ParameterDirection.Input);
            param.Add("v_PricesKeyValuePairs", keyValuePairs, DbType.String, direction: ParameterDirection.Input);
            param.Add("v_IsPriceInserted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            using (var con = NewCarMySqlMasterConnection)
            {
                con.Execute("InsertPrices_v18_2_3", param, null, commandType: CommandType.StoredProcedure);
                LogLiveSps.LogSpInGrayLog("[dbo].[InsertPrices]");
            }

            return true;
        }

        public bool InsertPriceQuote(List<T2> pricesInput, int cityId, int updatedBy, ref List<int> priceAddedVersions)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CityId", cityId, DbType.Int32, direction: ParameterDirection.Input);
                param.Add("v_LastUpdated", DateTime.Now, DbType.DateTime, direction: ParameterDirection.Input);
                param.Add("v_UpdatedBy", updatedBy, DbType.Int16, direction: ParameterDirection.Input);

                foreach (var item in pricesInput)
                {
                    if (!InsertPriceQuoteInDB(item, cityId, updatedBy, param))
                    {
                        return false;
                    }
                    else
                    {
                        if (CustomParser.parseBoolObject(param.Get<byte?>("v_IsPriceInserted")) && priceAddedVersions.IndexOf(item.VersionId) < 0)
                        {
                            priceAddedVersions.Add(item.VersionId);
                        }
                    }
                }
                if (cityId == 10)
                {
                    var modelParam = new DynamicParameters();
                    modelParam.Add("v_CityId", cityId, DbType.Int32, direction: ParameterDirection.Input);
                    modelParam.Add("v_UpdatedBy", updatedBy, DbType.Int16, direction: ParameterDirection.Input);
                    foreach (var item in pricesInput)
                    {
                        using (var conn = NewCarMySqlMasterConnection)
                        {
                            modelParam.Add("v_CarVersionId", item.VersionId, DbType.Int32, direction: ParameterDirection.Input);
                            conn.Execute("UpdateModelPrices_v16_11_7", modelParam, null, commandType: CommandType.StoredProcedure);
                            LogLiveSps.LogSpInGrayLog("[dbo].[UpdateModelPrices_v16_6_1]");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string errString = HttpContext.Current.Request.ServerVariables["URL"] + "pricesInput: " + JsonConvert.SerializeObject(pricesInput) + ", cityId: " + cityId.ToString() + ", updatedBy: " + updatedBy.ToString();
                ErrorClass objErr = new ErrorClass(e, errString);
                objErr.SendMail();
                return false;
            }

            return true;
        }

        public bool DeletePriceQuoteFromDB(T2 pricesInput, int cityId, int updatedBy, DynamicParameters param)
        {
            string categoryItems = FormKeyString(pricesInput, 2);
            if (categoryItems == null)
                return false;

            param.Add("v_VersionId", pricesInput.VersionId, DbType.Int32, direction: ParameterDirection.Input);
            param.Add("v_IsMetallic", pricesInput.IsMetallic, DbType.Boolean, direction: ParameterDirection.Input);
            param.Add("v_CategoryItems", categoryItems, DbType.String, direction: ParameterDirection.Input);
            param.Add("v_IsPriceDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            using (var con = NewCarMySqlMasterConnection)
            {
                con.Execute("DeletePrices_v18_1_4", param, null, commandType: CommandType.StoredProcedure);
                LogLiveSps.LogSpInGrayLog("[dbo].[DeletePrices]");
            }

            return true;
        }


        public bool DeletePriceQuote(List<T2> pricesInput, int cityId, int updatedBy, ref List<int> PriceDeletedVersions)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CityId", cityId, DbType.Int32, direction: ParameterDirection.Input);
                param.Add("v_LastUpdated", DateTime.Now, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                param.Add("v_UpdatedBy", updatedBy, DbType.Int16, direction: ParameterDirection.Input);

                foreach (var item in pricesInput)
                {
                    if (!DeletePriceQuoteFromDB(item, cityId, updatedBy, param))
                    {
                        return false;
                    }
                    else
                    {
                        if (CustomParser.parseBoolObject(param.Get<byte?>("v_IsPriceDeleted")))
                        {
                            PriceDeletedVersions.Add(item.VersionId);
                        }
                    }
                }
                if (cityId == 10)
                {
                    var modelParam = new DynamicParameters();
                    modelParam.Add("v_CityId", cityId, DbType.Int32, direction: ParameterDirection.Input);
                    modelParam.Add("v_UpdatedBy", updatedBy, DbType.Int16, direction: ParameterDirection.Input);
                    foreach (var item in pricesInput)
                    {
                        using (var conn = NewCarMySqlMasterConnection)
                        {
                            modelParam.Add("v_CarVersionId", item.VersionId, DbType.Int32, direction: ParameterDirection.Input);
                            conn.Execute("UpdateModelPrices_v16_11_7", modelParam, null, commandType: CommandType.StoredProcedure);
                            LogLiveSps.LogSpInGrayLog("[dbo].[UpdateModelPrices_v16_6_1]");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string errString = HttpContext.Current.Request.ServerVariables["URL"] + "pricesInput: " + JsonConvert.SerializeObject(pricesInput) + ", cityId: " + cityId.ToString() + ", updatedBy: " + updatedBy.ToString();
                ErrorClass objErr = new ErrorClass(e, errString);
                objErr.SendMail();
                return false;
            }

            return true;
        }
    }
}
