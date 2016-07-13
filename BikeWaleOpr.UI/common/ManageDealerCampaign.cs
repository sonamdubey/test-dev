using BikewaleOpr.Entity;
using BikeWaleOpr.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Mar 2016
    /// Description :   Manage Dealer Campaign
    /// </summary>
    public class ManageDealerCampaign
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Fetch the Dealer Campaign details
        ///                 SP Called : BW_FetchBWDealerCampaign
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <returns></returns>
        public DataTable FetchBWDealerCampaign(int campaignId)
        {
            DataTable dtDealerCampaign = null;

            try
            {
                if (campaignId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwdealercampaign"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                dtDealerCampaign = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.FetchBWDealerCampaign");
                objErr.SendMail();
            }

            return dtDealerCampaign;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Save the new Dealer Campaign
        ///                 SP Called : BW_InsertBWDealerCampaign
        /// Updated by  :   Sangram Nandkhile on 31st March 2016
        /// Description :   Used out parameter 'NewCampaignId' and changed return type
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public int InsertBWDealerCampaign(bool isActive, int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, bool isBookingAvailable = false)
        {
            int newCampaignId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_insertbwdealercampaign"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbParamTypeMapper.GetInstance[SqlDbType.Int], contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerleadservingradius", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerLeadServingRadius));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isBookingAvailable));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newcampaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    newCampaignId = Convert.ToInt32(cmd.Parameters["par_newcampaignid"].Value);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
            }

            return newCampaignId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Save the new Dealer Campaign
        ///                 SP Called : BW_UpdateBWDealerCampaign
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public bool UpdateBWDealerCampaign(bool isActive, int campaignId, int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, bool isBookingAvailable = false)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatebwdealercampaign"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerleadservingradius", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerLeadServingRadius));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isBookingAvailable));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbParamTypeMapper.GetInstance[SqlDbType.Int], contractId));

                    isSuccess = MySqlDatabase.UpdateQuery(cmd,ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 22 Mar 2016
        /// Description :   Fetch the Dealer Campaigns for contacts
        ///                 SP Called : BW_FetchBWCampaigns
        /// Modified by :   Sumit Kate on 11 July 2016
        /// Description :   new SP called
        /// </summary>
        /// <param name="dealerId">dealer Id</param>
        /// <returns></returns>
        public DealerCampaignBase FetchBWCampaigns(int dealerId)
        {
            IList<DealerCampaignEntity> lstDealerCampaign = null;
            DealerCampaignBase dealerCampaigns = null;
            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealercampaigns"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerId));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (reader != null)
                            {
                                dealerCampaigns = new DealerCampaignBase();

                                if (reader.Read())
                                {
                                    dealerCampaigns.DealerName = Convert.ToString(reader["organization"]);
                                    dealerCampaigns.DealerNumber = Convert.ToString(reader["MobileNo"]);
                                }


                                if (reader.NextResult())
                                {
                                    lstDealerCampaign = new List<DealerCampaignEntity>();
                                    while (reader.Read())
                                    {
                                        lstDealerCampaign.Add(
                                            new DealerCampaignEntity()
                                            {
                                                CampaignId = !Convert.IsDBNull(reader["campaignId"]) ? Convert.ToInt32(reader["campaignId"]) : default(int),
                                                CampaignName = !Convert.IsDBNull(reader["CampaignName"]) ? Convert.ToString(reader["CampaignName"]) : string.Empty,
                                                EmailId = !Convert.IsDBNull(reader["EmailId"]) ? Convert.ToString(reader["EmailId"]) : string.Empty,
                                                MaskingNumber = !Convert.IsDBNull(reader["MaskingNumber"]) ? Convert.ToString(reader["MaskingNumber"]) : string.Empty,
                                                ServingRadius = !Convert.IsDBNull(reader["ServingRadius"]) ? Convert.ToInt32(reader["ServingRadius"]) : default(int),
                                            }
                                            );
                                    }
                                    dealerCampaigns.DealerCampaigns = lstDealerCampaign;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.FetchBWCampaigns");
                objErr.SendMail();
            }

            return dealerCampaigns;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 04 Apr 2016
        /// Description :   Fetch the Dealer Campaigns for contacts
        ///                 SP Called : GetMaskingNumbers
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataTable BindMaskingNumbers(int dealerId)
        {

            DataTable dtb = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getmaskingnumbers"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dtb = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindMaskingNumbers");
                objErr.SendMail();
            }

            return dtb;
        }


        /// <summary>
        ///  Created By : Sushil Kumar
        ///  Created On : 18th April 2016
        ///  Description : To get dealer camapigns and contracts mapping based on dealerId
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public DataTable GetDealerCampaigns(uint dealerId)
        {
            DataTable dtDealerCampaigns = null;

            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getdealercampaigns"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId > 0 ? dealerId : Convert.DBNull));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                dtDealerCampaigns = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.GetDealerCampaigns");
                objErr.SendMail();
            }
            return dtDealerCampaigns;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 12 July 2016
        /// Description :   Saves the Dealer Contract to ContractCampaignMapping table
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public bool SaveDealerContract(DealerContractEntity contract)
        {
            int rowsAffected = 0;
            try
            {
                if (contract != null)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_savecontractdetails"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_contractId", DbParamTypeMapper.GetInstance[SqlDbType.Int], contract.ContractId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbParamTypeMapper.GetInstance[SqlDbType.Int], 200, contract.DealerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_packageId", DbParamTypeMapper.GetInstance[SqlDbType.Int], 50, contract.PackageId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_packageName", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, contract.PackageName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_startDate", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], contract.StartDate));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_endDate", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], contract.EndDate));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_contractStatus", DbParamTypeMapper.GetInstance[SqlDbType.Int], contract.ContractStatus));
                        rowsAffected = MySqlDatabase.ExecuteNonQuery(cmd);

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ManageDealerCampaign.SaveDealerContract({0})", Newtonsoft.Json.JsonConvert.SerializeObject(contract)));
                objErr.SendMail();
            }

            return rowsAffected > 0 ? true : false;
        }

        public bool MapContractCampaign(int contractId, int campaignId)
        {
            int rowsAffected = 0;
            try
            {
                if (contractId > 0 && campaignId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatebwdealercontractcampaign"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbParamTypeMapper.GetInstance[SqlDbType.Int], contractId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], campaignId));
                        rowsAffected = MySqlDatabase.ExecuteNonQuery(cmd);

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ManageDealerCampaign.MapContractCampaign({0},{1})", contractId, campaignId));
                objErr.SendMail();
            }
            return rowsAffected > 0 ? true : false;
        }
    }
}
