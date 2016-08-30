using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace BikewaleOpr.DALs.ManufactureCampaign
{
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        public List<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerId)
        {
            List<ManufactureDealerCampaign> dtManufactureCampaigns = null;
            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("searchmanufacturercampaign"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId > 0 ? dealerId : Convert.DBNull));
                        dtManufactureCampaigns = new List<ManufactureDealerCampaign>();
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    dtManufactureCampaigns.Add(new ManufactureDealerCampaign
                                    {
                                        id = Convert.ToInt32(dr["id"]),
                                        dealerid = Convert.ToInt32(dr["dealerid"]),
                                        description = Convert.ToString(dr["description"]),
                                        isactive = Convert.ToInt32(dr["isactive"])

                                    });
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.searchmanufacturercampaign");
                objErr.SendMail();
            }
            return dtManufactureCampaigns;
        }
        public bool statuschangeCampaigns(uint id, uint isactive)
        {

            bool isSuccess = false;
            try
            {


                using (DbCommand cmd = DbFactory.GetDBCommand("statuschangeCampaigns"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.UInt32, isactive));
                    //var k = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at UpdateDealerDisclaimer : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.statuschangeCampaigns");
                objErr.SendMail();
            }
            return isSuccess;
        }



    }
}
