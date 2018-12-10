using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Carwale.UI.Common;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Common
{
    public class SPCallMethod
    {
        public static DataSet GetRecomCars(int index, int pageSize, int ModelCount, string Budget, string Range, string MakeIDs, string mUsage, string DimensionAndSpace,
            string Comfort, string Performance, string Convenience, string Safety, string Entertainment, string Aesthetics, string SalesAndSupport, string FuelEconomy,
            string FuelType, string TransMissonType, string ChildSafety, string Powerwindows, string ABS, string CentralLocking, string AirBags, int preset, out int Count, out string MakeidsOut)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("NewRecommendCars_v17_11_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Index", DbType.Int32,index));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PageSize", DbType.Int32, pageSize));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelCount", DbType.Int32, ModelCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Budget", DbType.Int32,Budget));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Range", DbType.Int32,Range));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeIDs", DbType.String,MakeIDs));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MonthlyUsage", DbType.Int32,mUsage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DimensionAndSpace", DbType.Int16,DimensionAndSpace));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comfort", DbType.Int16,Comfort));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Performance", DbType.Int16,Performance));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Convenience", DbType.Int16, Convenience));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Safety", DbType.Int16,Safety));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Entertainment", DbType.Int16, Entertainment));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Aesthetics", DbType.Int16, Aesthetics));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SalesAndSupport", DbType.Int16, SalesAndSupport));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelEconomy", DbType.Int16, FuelEconomy));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelType",DbType.String, FuelType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_TransMissonType", DbType.Int16, TransMissonType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ChildSafety", DbType.Int16, ChildSafety));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Powerwindows", DbType.Int16, Powerwindows));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ABS", DbType.Int16, ABS));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CentralLocking", DbType.Int16, CentralLocking));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AirBags", DbType.Int16, AirBags));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Preset", DbType.Int16, preset));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarCount", DbType.Int32,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeidsOut", DbType.String, ParameterDirection.Output));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                    if (!int.TryParse(cmd.Parameters["v_CarCount"].Value.ToString(), out Count))
                        Count = 0;
                    MakeidsOut = cmd.Parameters["v_MakeidsOut"].Value!=null?cmd.Parameters["v_MakeidsOut"].Value.ToString():string.Empty;
                }
            }
            catch (Exception err)
            {
                Count = 0;
                MakeidsOut = "";
                ErrorClass objErr = new ErrorClass(err, "RC_dal.cs GetRecomCars() dal exception");
                objErr.SendMail();
            }
            return ds;
        }

    }
}
