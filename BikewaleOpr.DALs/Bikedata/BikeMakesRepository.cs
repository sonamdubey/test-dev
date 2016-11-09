
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace BikewaleOpr.DALs.Bikedata
{
    /// <summary>
    /// 
    /// </summary>
    public class BikeMakesRepository : IBikeMakes
    {
        /// <summary>
        /// Created By : Sushil Kumar on  25th Oct 2016
        /// Description :  Getting makes only by providing only request type
        /// </summary>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote or ALL</param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetMakes(string RequestType)
        {
            IList<BikeMakeEntityBase> _objBikeMakes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeMakes = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                BikeMakeEntityBase _objMake = new BikeMakeEntityBase();
                                _objMake.MakeName = Convert.ToString(dr["Text"]);
                                _objMake.MakeId = SqlReaderConvertor.ToInt32(dr["Value"]);
                                _objBikeMakes.Add(_objMake);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.GetMakes_" + RequestType);
                objErr.SendMail();
            }
            return _objBikeMakes;
        }
    }
}
