using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DALs.ManufactureCampaign
{
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        /// <summary>
        /// Created by : Sajal Gupta on 30/08/2016
        /// Description : This function inserts campaigns data and returns campaign id.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaign"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 45, description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingNumber", DbType.String, 10, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newId", DbType.Int32, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    int campaignId = Convert.ToInt32(cmd.Parameters["par_newId"].Value);
                    return campaignId;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
                return 0;
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 30/08/2016
        /// Description : This function updates template data in database.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerId"></param>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        /// <param name="templateHtml1"></param>
        /// <param name="templateId1"></param>
        /// <param name="templateHtml2"></param>
        /// <param name="templateId2"></param>
        /// <param name="templateHtml3"></param>
        /// <param name="templateId3"></param>
        /// <param name="templateHtml4"></param>
        /// <param name="templateId4"></param>
        public void UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, string templateHtml1, int templateId1, string templateHtml2, int templateId2, string templateHtml3, int templateId3, string templateHtml4, int templateId4)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatemanufacturercampaign"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 45, description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingNumber", DbType.String, 10, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml1", DbType.String, 150, templateHtml1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId1", DbType.Int32, templateId1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml2", DbType.String, 150, templateHtml2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId2", DbType.Int32, templateId2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml3", DbType.String, 150, templateHtml3));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId3", DbType.Int32, templateId3));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml4", DbType.String, 150, templateHtml4));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId4", DbType.Int32, templateId4));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.UpdateBWDealerCampaign");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 30/08/2016
        /// Description : This function saves template html and also maps them in database.
        /// </summary>
        /// <param name="templateHtml1"></param>
        /// <param name="templateId1"></param>
        /// <param name="templateHtml2"></param>
        /// <param name="templateId2"></param>
        /// <param name="templateHtml3"></param>
        /// <param name="templateId3"></param>
        /// <param name="templateHtml4"></param>
        /// <param name="templateId4"></param>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        public void SaveManufacturerCampaignTemplate(string templateHtml1, int templateId1, string templateHtml2, int templateId2, string templateHtml3, int templateId3, string templateHtml4, int templateId4, int userId, int campaignId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturecampaigntemplate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml1", DbType.String, 150, templateHtml1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId1", DbType.Int32, templateId1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml2", DbType.String, 150, templateHtml2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId2", DbType.Int32, templateId2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml3", DbType.String, 150, templateHtml3));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId3", DbType.Int32, templateId3));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml4", DbType.String, 150, templateHtml4));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId4", DbType.Int32, templateId4));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.SaveManufacturerCampaignTemplate");
                objErr.SendMail();
            }
        }
        
        //public void SaveManufacturerCampaignTemplateMapping(int campaignId, int templateId, int pageId, int isActive, int userId)
        //{
        //    try
        //    {
        //        using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaigntemplatemapping"))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId", DbType.Int32, templateId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_pageId", DbType.Int32, pageId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
        //            MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.SaveManufacturerCampaignTemplateMapping");
        //        objErr.SendMail();
        //    }
        //}

        /// <summary>
        /// Created by :Sajal Gupta on 30/08/2016
        /// Description : This method fetches campaign and template data from database.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId)
        {
            List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> objManufacturerCampaignDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchcampaigndetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objManufacturerCampaignDetails = new List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity>();
                            while (dr.Read())
                            {
                                objManufacturerCampaignDetails.Add(new BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity() { CampaignDescription = dr["Description"].ToString(), CampaignMaskingNumber = dr["maskingnumber"].ToString(), IsActive = Convert.ToInt32(dr["isactive"]), IsDefault = Convert.ToInt32(dr["isdefault"]), PageId = Convert.ToInt32(dr["pageid"]), TemplateHtml = dr["templatehtml"].ToString(), TemplateId = Convert.ToInt32(dr["id"]) });
                            }
                        }
                    }
                    return objManufacturerCampaignDetails;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.FetchCampaignDetails");
                objErr.SendMail();

                return objManufacturerCampaignDetails;
            }
        }
        
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Return all the campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerId)
        {
            IList<ManufactureDealerCampaign> dtManufactureCampaigns = null;
            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("searchmanufacturercampaign"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));


                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                        {
                            if (dr != null)
                            {
                                dtManufactureCampaigns = new List<ManufactureDealerCampaign>();
                                while (dr.Read())
                                {
                                    dtManufactureCampaigns.Add(new ManufactureDealerCampaign
                                    {
                                        id = Convert.ToInt32(dr["id"]),
                                        dealerid = Convert.ToInt32(dr["dealerid"]),
                                        description = Convert.ToString(dr["description"]),
                                        isactive = Convert.ToInt32(dr["isactive"])

                                    });
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.searchmanufacturercampaign");
                objErr.SendMail();
            }
            return dtManufactureCampaigns;
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Change Status of the campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public bool statuschangeCampaigns(uint id, uint isactive)
        {

            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("statuschangeCampaigns"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.UInt32, isactive));
                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.statuschangeCampaigns");
                objErr.SendMail();
            }
            return isSuccess;
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description :To fetch all the manufacturer in dropdown
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufacturerEntity> GetDealerAsManuFacturer()
        {
            IList<ManufacturerEntity> manufacturers = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerasmanufacturer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            manufacturers = new List<ManufacturerEntity>();
                            while (reader.Read())
                            {
                                manufacturers.Add(
                                    new ManufacturerEntity()
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Name = Convert.ToString(reader["Name"]),
                                        Organization = Convert.ToString(reader["Organization"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageManufacturerCampaign.GetDealerAsManuFacturer");
                objErr.SendMail();
            }

            return manufacturers;
        }
        /// <summary>
        /// Created By : Sajal Gupta on 31/08/2016
        /// Description : THis method will relese masking number from db against given campaign id;
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public bool ReleaseCampaignMaskingNumber(int campaignId)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("releasemaskingnumber"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));

                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.statuschangeCampaigns");
                objErr.SendMail();
            }
            return isSuccess;
        }
    }
}