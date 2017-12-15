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
    public class BikeBodyStyleRepository:IBikeBodyStylesRepository
    {
        /// <summary>
        /// Function to get the bike bodystyles list
        /// </summary>
        public IEnumerable<BikeBodyStyleEntity> GetBodyStylesList()
        {
            IList<BikeBodyStyleEntity> _objBikeBodyStyles = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbodystyles"))
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
                                _objBodyStyle.BodyStyleId = SqlReaderConvertor.ToUInt32(dr["id"]);
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
