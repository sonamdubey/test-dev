
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Bikewale.Notifications;
using System.Data.Common;

namespace Bikewale.DAL.Location
{
    public class StateRepository : IState
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Getting States id and states names
        /// </summary>
        /// <returns></returns>
        public List<StateEntityBase> GetStates()
        {
            List<StateEntityBase> objStateList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getstates"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    objStateList = new List<StateEntityBase>();
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objStateList.Add(new StateEntityBase
                                {
                                    StateId = Convert.ToUInt32(dr["Value"]),
                                    StateName = Convert.ToString(dr["Text"]),
                                    StateMaskingName = Convert.ToString(dr["MaskingName"])
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objStateList;
        }   // End of GetStates method
    }
}
