using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Manage Availability By Color Operations
    /// Author  :   Sushil Kumar Kanojia
    ///  Created On : 20th Nov 2015
    /// </summary>
    public class ManageBikeAvailbilityByColor
    {
        /// <summary>
        ///  To get ModelId based on selected VersionId
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns>ModelId</returns>
        public int GetModelIdForVersion(int versionId)
        {
            int modelId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("select BikeModelId AS ModelId  from bikeversions where id = @versionid and isdeleted = 0"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(DbFactory.GetDbParam("@versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd,ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                modelId = Convert.ToInt32(reader["ModelId"]);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageBikeAvailbilityByColor.GetModelIdForVersion");
                objErr.SendMail();
            }

            return modelId;
        }


        /// <summary>
        /// Retrieves the Version colors list
        /// </summary>
        /// <param name="versionId">Model Id</param>
        /// <returns>Version Color List</returns>
        public IEnumerable<VersionColorWithAvailability> FetchVersionColorsWithAvailability(int versionId,int dealerId)
        {
            IList<VersionColorWithAvailability> versionColors = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeavailabilitybycolor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            versionColors = new List<VersionColorWithAvailability>();
                            while (reader.Read())
                            {                                            
                                versionColors.Add(new VersionColorWithAvailability()
                                {
                                    ModelColorID = Convert.ToUInt32(reader["ID"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    ModelColorName = Convert.ToString(reader["ColorName"]),
                                    NoOfDays = (String.IsNullOrEmpty(Convert.ToString(reader["NoOfDays"])))?"NA":Convert.ToString(reader["NoOfDays"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageBikeAvailbilityByColor.FetchVersionColorsWithAvailability");
                objErr.SendMail();
            }
            return versionColors;
        }

        /// <summary>
        /// Saves Version Color
        /// </summary>
        /// <param name="versionColor">Version Color Entity</param>
        /// <param name="userId">User Id</param>
        /// <returns>Success/Failure</returns>
        public bool UpdateBikeAvailabilityByColor(VersionColorWithAvailability versionColor, int versionId, int userId,int dealerId)
        {
            bool isSaved = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatebikeavailabilitybycolor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Convert.ToInt32(versionColor.ModelColorID)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_noofdays", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 5, versionColor.NoOfDays));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],100, userId));

                    isSaved = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageBikeAvailbilityByColor.UpdateBikeAvailabilityByColor");
                objErr.SendMail();
            }
            return isSaved;
        }
    }


}