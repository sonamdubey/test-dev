using Carwale.Entity.Dealers;
using Carwale.Interfaces.Campaigns;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.DAL.Campaigns
{
    public class CampaignRepository : RepositoryBase, ICampaignRepository
    {
        /// <summary>
        /// Get Campaign Lead Info
        /// Written By : Sanjay Soni
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DealerInquiryDetails GetCampaignLeadInfo(int leadId)
        {
            var leadDetail = new DealerInquiryDetails();

            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetCampaignLeadDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_leadId", DbType.Int16, leadId));

                    using (var dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            leadDetail.Name = CustomParser.parseStringObject(dr["Name"]);
                            leadDetail.Email = CustomParser.parseStringObject(dr["Email"]);
                            leadDetail.Mobile = CustomParser.parseStringObject(dr["Mobile"]);
                            leadDetail.VersionId = CustomParser.parseIntObject(dr["VersionId"]);
                            leadDetail.CityId = CustomParser.parseIntObject(dr["CityId"]);
                            leadDetail.CityName = dr["CityName"].ToString();
                            leadDetail.ZoneId = CustomParser.parseStringObject(dr["ZoneId"]);
                            leadDetail.DealerId = CustomParser.parseIntObject(dr["DealerId"]);
                            leadDetail.CampaignId = CustomParser.parseIntObject(dr["CampaignId"]);
                            leadDetail.ModelsHistory = CustomParser.parseStringObject(dr["ModelHistory"]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerSponsoredAdRespository.GetCampaignLeadInfo()");
                objErr.SendMail();
            }
            return leadDetail;
        }

        public Dictionary<int,int> GetCampaignTemplateGroups(int templateGroupId, int platformId)
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_AssignedTemplateGroupId", templateGroupId);
                    param.Add("v_PlatformId", platformId);
                    return con.Query<int, int, KeyValuePair<int, int>>("GetCampaignTemplateGroups_v16_11_7",
                        (s, i) => new KeyValuePair<int, int>(s, i), param, commandType: CommandType.StoredProcedure, splitOn: "TemplateId").ToDictionary(kv => kv.Key, kv => kv.Value);
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return null;
            }
        }
    }
}
