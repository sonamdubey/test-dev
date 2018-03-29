using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Location;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

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
                    objStateList = connection.Query<StateEntityBase>("getstates_26052017", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Location.GetStates");
                
            }

            return objStateList;
        }
        #endregion
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 01 Aug 2017
        /// Description :   Gets all dealer cities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CityNameEntity> GetDealerCities()
        {
            IEnumerable<CityNameEntity> cities = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    
                    cities = connection.Query<CityNameEntity>("bw_getbikedealercities_01082017", commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerCitites");
            }

            return cities;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 01 Aug 2017
        /// Description :   Fetches all the cities belonging to the mentioned state from database.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public IEnumerable<CityNameEntity> GetCitiesByState(uint stateId)
        {
            IEnumerable<CityNameEntity> stateCities = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_stateid", stateId);

                    stateCities = connection.Query<CityNameEntity>("getcitiesbystate", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetCitiesByState stateId={0}", stateId));
            }

            return stateCities;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 01 Aug 2017
        /// Description :   Fetches all the cities from database.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public IEnumerable<CityNameEntity> GetAllCities()
        {
            IEnumerable<CityNameEntity> cities = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    cities = connection.Query<CityNameEntity>("getallcities", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetAllCities"));
            }

            return cities;
        }

    }   // class
}   // namespace
