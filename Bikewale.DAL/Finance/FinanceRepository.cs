using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Finance.CapitalFirst
{
    public class FinanceRepository : IFinanceRepository
    {
        public CapitalFirstBikeEntity GetCapitalFirstBikeMapping(uint versionId)
        {
            CapitalFirstBikeEntity bike = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getcapitalfirstbikemapping";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            bike = new CapitalFirstBikeEntity();
                            bike.MakeBase = new CapitalFirstMakeEntityBase() { Make = Convert.ToString(dr["Make"]), MakeId = Convert.ToString(dr["MakeId"]) };
                            bike.ModelBase = new CapitalFirstModelEntityBase()
                            {
                                ModelId = Convert.ToString(dr["ModelId"]),
                                ModelNo = Convert.ToString(dr["ModelNo"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("FinanceRepository.GetCapitalFirstBikeMapping({0})", versionId));
            }
            return bike;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 May 2018
        /// Description :   Calls savecapitalfirstleaddetails_24052018 to save the lead data to Capital First Lead table
        /// </summary>
        /// <param name="objDetails"></param>
        /// <returns></returns>
        public uint SaveCapitalFirstLeadData(PersonalDetails objDetails, CTFormResponse formResponse)
        {
            uint id = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savecapitalfirstleaddetails_24052018";

                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, ));
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, objDetails.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadid", DbType.Int32, objDetails.LeadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, objDetails.objLead.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, objDetails.objLead.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ctleadid", DbType.Int32, objDetails.CtLeadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_firstname", DbType.String, objDetails.FirstName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastname", DbType.String, objDetails.LastName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobilenumber", DbType.String, objDetails.MobileNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, objDetails.EmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincode", DbType.String, objDetails.Pincode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pancard", DbType.String, objDetails.Pancard));

                    if (formResponse != null)
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Int32, formResponse.Status));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_agentName", DbType.String, formResponse.SalesOfficer));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_agentNumber", DbType.String, formResponse.SalaesOfficerMobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_apiResponse", DbType.String, formResponse.Message));
                    }
                    else
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Int32, DBNull.Value));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_agentName", DbType.String, DBNull.Value));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_agentNumber", DbType.String, DBNull.Value));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_apiResponse", DbType.String, DBNull.Value));
                    }
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, ParameterDirection.InputOutput, objDetails.Id));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    id = SqlReaderConvertor.ToUInt32(cmd.Parameters["par_id"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("FinanceRepository.SaveCapitalFirstLeadData({0})", JsonConvert.SerializeObject(objDetails)));
            }
            return id;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 May 2018
        /// Description :   Returns the Lead data by CT Lead Id
        /// </summary>
        /// <param name="ctLeadId"></param>
        /// <returns></returns>
        public CapitalFirstLeadEntity GetLeadDetails(string ctLeadId)
        {
            CapitalFirstLeadEntity lead = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcapitalfirstleaddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ctleadid", DbType.Int32, ctLeadId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        lead = new CapitalFirstLeadEntity();
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                lead.AgentName = Convert.ToString(dr["AgentName"]);
                                lead.AgentNumber = Convert.ToString(dr["AgentNumber"]);
                                lead.BikeName = Convert.ToString(dr["BikeName"]);
                                lead.CtLeadId = ctLeadId;
                                lead.EmailId = Convert.ToString(dr["EmailId"]);
                                lead.Exshowroom = SqlReaderConvertor.ToUInt32(dr["Exshowroom"]);
                                lead.Rto = SqlReaderConvertor.ToUInt32(dr["RTO"]);
                                lead.Insurance = SqlReaderConvertor.ToUInt32(dr["Insurance"]);
                                lead.FirstName = Convert.ToString(dr["FirstName"]);
                                lead.LastName = Convert.ToString(dr["LastName"]);
                                lead.MobileNo = Convert.ToString(dr["MobileNo"]);
                                lead.EmailId = Convert.ToString(dr["EmailId"]);
                                lead.VoucherNumber = Convert.ToString(dr["voucherNumber"]);
                                lead.VoucherExpiryDate = SqlReaderConvertor.ToDateTime(dr["VoucherExpiryDate"]);

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Error in GetLeadDetails({0})", ctLeadId));
            }
            return lead;
        }
    }
}
