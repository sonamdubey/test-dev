using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
namespace BikewaleOpr.DALs.Bikedata
{
    public class BikeBodyStyleRepository:IBikeBodyStyles
    {
        /// <summary>
        /// Created By : Rajan Chauhan on  12th Dec 2017
        /// Description :  Getting bodystyle by providing body style id
        /// </summary>
        /// <param name="bodyStyleId">Pass value as 1 2 or 3 etc</param>
        /// <returns></returns>
        public IEnumerable<BikeBodyStyleEntity> GetBodyStyles(int bodyStyleId)
        {
            IList<BikeBodyStyleEntity> _objBikeBodyStyles = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbodystyle"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bodystyleid", DbType.String, 20, bodyStyleId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeBodyStyles = new List<BikeBodyStyleEntity>();
                            while (dr.Read())
                            {
                                BikeBodyStyleEntity _objBodyStyle = new BikeBodyStyleEntity();
                                _objBodyStyle.BodyStyleId = SqlReaderConvertor.ToInt32(dr["id"]);
                                _objBodyStyle.BodyStyleName = Convert.ToString(dr["Name"]);
                                _objBikeBodyStyles.Add(_objBodyStyle);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.GetBodyStyles" + bodyStyleId);
            }
            return _objBikeBodyStyles;
        }


        /// <summary>
        /// Function to get the bike bodystyles list
        /// </summary>
        public IEnumerable<BikeBodyStyleEntity> GetBodyStylesList()
        {
            IList<BikeBodyStyleEntity> _objBikeBodyStyles = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbodystylelist"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeBodyStyles = new List<BikeBodyStyleEntity>();
                            while (dr.Read())
                            {
                                BikeBodyStyleEntity _objBodyStyle = new BikeBodyStyleEntity();
                                _objBodyStyle.BodyStyleId = SqlReaderConvertor.ToInt32(dr["id"]);
                                _objBodyStyle.BodyStyleName = Convert.ToString(dr["Name"]);
                                _objBikeBodyStyles.Add(_objBodyStyle);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.GetBodyStylesList");
            }
            return _objBikeBodyStyles;
        }

    }
}
