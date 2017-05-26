using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.Location;
using Dapper;

namespace BikewaleOpr.DALs.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 26 May 2017
    /// Summary : Class have function implementation related to location interface
    /// </summary>
    public class LocationRepository : ILocation
    {
        #region GetStates function
        /// <summary>
        /// Written By : Ashish G. Kamble on 26 May 2017
        /// Summary : Function to get the all states
        /// </summary>
        /// <returns>Returns list of all states</returns>
        public IEnumerable<StateEntityBase> GetStates()
        {
            IEnumerable<StateEntityBase> objStateList = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    objStateList = connection.Query<StateEntityBase>("getstates_26052017", commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Location.GetStates");
                objErr.SendMail();
            }

            return objStateList;
        } 
        #endregion


    }   // class
}   // namespace
