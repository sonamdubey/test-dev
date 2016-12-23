
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
    public class BikeModelsRepository : IBikeModels
    {
        /// <summary>
        /// Created By : Sushil Kumar on  25th Oct 2016
        /// Description :  Getting Models only by providing MakeId and request type
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModels(uint makeId, string requestType)
        {
            IList<BikeModelEntityBase> _objBikeModels = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeModels = new List<BikeModelEntityBase>();
                            while (dr.Read())
                            {
                                BikeModelEntityBase _objModel = new BikeModelEntityBase();
                                _objModel.ModelName = Convert.ToString(dr["Text"]);
                                _objModel.ModelId = SqlReaderConvertor.ToInt32(dr["Value"]);
                                _objBikeModels.Add(_objModel);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetModels_Make_{0}_RequestType_{1}", makeId, requestType));
                objErr.SendMail();
            }
            return _objBikeModels;
        }
    }
}
