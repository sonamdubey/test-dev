using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.ServiceCenters;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.ServiceCenters
{
    /// <summary>
    /// Created By : Sajal Gupta on 07/11/2016
    /// Description: Class for fetching service center data. 
    /// </summary>
    public class ServiceCentersRepository : IServiceCentersRepository
    {

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: DAL layer Function for fetching service center data.
        /// </summary>     
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData serviceCenters = new ServiceCenterData();
            IList<ServiceCenterDetails> objServiceCenterList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getservicecentersbycity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objServiceCenterList = new List<ServiceCenterDetails>();

                            while (dr.Read())
                            {
                                ServiceCenterDetails objServiceCenterDetails = new ServiceCenterDetails();

                                objServiceCenterDetails.ServiceCenterId = SqlReaderConvertor.ToUInt32(dr["id"]);
                                objServiceCenterDetails.Name = Convert.ToString(dr["name"]);
                                objServiceCenterDetails.Address = Convert.ToString(dr["address"]);
                                objServiceCenterDetails.Phone = Convert.ToString(dr["phone"]);
                                objServiceCenterDetails.Mobile = Convert.ToString(dr["mobile"]);

                                objServiceCenterList.Add(objServiceCenterDetails);
                            }

                            serviceCenters.ServiceCenters = objServiceCenterList;

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    serviceCenters.Count = SqlReaderConvertor.ToUInt32(dr["totalServiceCenters"]);
                                }
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersRepository.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return serviceCenters;
        }
    }
}
