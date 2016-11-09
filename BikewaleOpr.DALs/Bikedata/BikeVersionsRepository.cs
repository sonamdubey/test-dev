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
    public class BikeVersionsRepository : IBikeVersions
    {

        /// <summary>
        /// Created By : Sushil Kumar on  25th Oct 2016
        /// Description : Getting Versions only by providing ModelId and request type
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionEntityBase> GetVersions(uint modelId, string requestType)
        {

            IList<BikeVersionEntityBase> _objBikeVersions = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeversions"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeVersions = new List<BikeVersionEntityBase>();
                            while (dr.Read())
                            {
                                BikeVersionEntityBase _objModel = new BikeVersionEntityBase();
                                _objModel.VersionName = Convert.ToString(dr["Text"]);
                                _objModel.VersionId = SqlReaderConvertor.ToInt32(dr["Value"]);
                                _objBikeVersions.Add(_objModel);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetVersions_Model_{0}_RequestType_{1}", modelId, requestType));
                objErr.SendMail();
            }
            return _objBikeVersions;
        }
    }
}
