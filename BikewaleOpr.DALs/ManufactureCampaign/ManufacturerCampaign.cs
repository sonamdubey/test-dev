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
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Return all the campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerId)
        {
            List<ManufactureDealerCampaign> dtManufactureCampaigns = null;
            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("searchmanufacturercampaign"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
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
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Change Status of the campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
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
