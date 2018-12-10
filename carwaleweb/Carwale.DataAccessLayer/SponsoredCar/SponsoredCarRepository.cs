using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.CarData;
using Carwale.Entity.Sponsored;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Carwale.DAL.SponsoredCar
{
    public class SponsoredCarRepository : RepositoryBase, ISponsoredCar
    {
        /// <summary>
        /// Created Date : 19/8/2014
        /// Desc : Function to get sponsored car for versionids passed according to categoryid passed
        /// </summary>
        /// <param name="versionIds"></param>
        /// <param name="categoryId">Use Enum SponsoredCarEnum to pass categoryId</param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public int GetFeaturedCar(string versionIds, int categoryId, int platformId)
        {
            int featuredVersionId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_Versions",versionIds);
                param.Add("v_CategoryId",categoryId > 0 ? categoryId : 1);
                param.Add("v_PlatformId",platformId > 0 ? platformId : 1);
                param.Add("v_FeaturedVersionId",DbType.Int32,direction:ParameterDirection.Output);
                using(var con = CarDataMySqlReadConnection)
                {
                    con.Execute("GetFeaturedVersionIDByVersionID_v16_11_7", param, commandType: CommandType.StoredProcedure);
                    featuredVersionId = param.Get<int?>("v_FeaturedVersionId") != null ? param.Get<int>("v_FeaturedVersionId") : -1;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return featuredVersionId;
        }

        /// <summary>
        /// Returns the SponsoredCampaigns based on categoryId and platformId passed 
        /// </summary>
        /// <param name="categoryId">CampaignCategoryId for eg: 4 for TopSellingcars</param>
        /// <param name="platformId"></param>
        public List<Sponsored_Car> GetSponsoredCampaigns(int categoryId, int platformId, int categorySection, out DateTime nextCampaignStartDate, string param = "", int applicationId = 1)
        {
            List<Sponsored_Car> sponsoredCampaigns = new List<Sponsored_Car>();
            nextCampaignStartDate = DateTime.Now.AddDays(1);

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("FetchSponsoredCampaigns_v17_12_8"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CampaignCategoryId", DbType.Int32, categoryId > 0 ? categoryId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int32, platformId > 0 ? platformId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CategorySection", DbType.Int32, categorySection > 0 ? categorySection : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Param", DbType.Int32, !String.IsNullOrEmpty(param) ? param : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ApplicationId", DbType.Int32, applicationId > 0 ? applicationId : Convert.DBNull));

                    using (var dr = MySqlDatabase.SelectQuery(cmd,DbConnections.EsMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            bool isSponsored, isDefault;

                            isSponsored = String.IsNullOrEmpty(dr["IsSponsored"].ToString()) ? false : Convert.ToBoolean(dr["IsSponsored"]);
                            isDefault = String.IsNullOrEmpty(dr["IsDefault"].ToString()) ? false : Convert.ToBoolean(dr["IsDefault"]);

                            sponsoredCampaigns.Add(new Sponsored_Car()
                            {
                                //Core members
                                Id = CustomParser.parseIntObject(dr["Id"]),
                                IsSponsored = isSponsored,
                                CampaignCategoryId = CustomParser.parseIntObject(dr["CampaignCategoryId"]),
                                CategorySection = CustomParser.parseIntObject(dr["CategorySection"]),
                                PlatformId = CustomParser.parseIntObject(dr["PlatformId"]),
                                StartDate = CustomParser.parseDateObject(dr["StartDate"]),
                                EndDate = CustomParser.parseDateObject(dr["EndDate"]),
                                IsDafault = isDefault,
                                //End of Core members

                                // Start of common members, which are common across multiple categories
                                Ad_Html = CustomParser.parseStringObject(dr["AdScript"]),
                                ImageUrl = CustomParser.parseStringObject(dr["ImageUrl"]),
                                HorizontalPosition = CustomParser.parseStringObject(dr["HPosition"]),
                                WidgetPosition = CustomParser.parseStringObject(dr["WidgetPosition"]),
                                VerticalPosition = CustomParser.parseStringObject(dr["VPosition"]),
                                JumbotronPosition = CustomParser.parseStringObject(dr["JumbotronPos"]),
                                BackgroundColor = CustomParser.parseStringObject(dr["BackGroundColor"]),
                                // End of common members, which are common across multiple categories

                                //AppAds  members start here
                                MakeId = CustomParser.parseIntObject(dr["MakeId"]),
                                ModelId = CustomParser.parseIntObject(dr["ModelId"]),
                                MakeName = CustomParser.parseStringObject(dr["MakeName"]),
                                ModelName = CustomParser.parseStringObject(dr["ModelName"]),
                                ModelMaskingName = CustomParser.parseStringObject(dr["ModelMaskingName"]),
                                SponsoredTitle = CustomParser.parseStringObject(dr["SponsoredTitle"]),
                                CardHeader = CustomParser.parseStringObject(dr["CardHeader"]),
                                Subtitle = CustomParser.parseStringObject(dr["Subtitle"]),
                                Postion = string.IsNullOrWhiteSpace(dr["Position"].ToString()) ? new List<int>() : dr["Position"].ToString().Split(',').Where(i => RegExValidations.IsNumeric(i)).Select(i=>int.Parse(i)).ToList()
                                //AppAds  members ends here
                            });
                        }

                        if (dr.NextResult())
                        {
                            int index = 0;
                            while (dr.Read() && CustomParser.parseIntObject(dr["CampaignId"])>0)
                            {
                                index = sponsoredCampaigns.FindIndex(item => item.Id == CustomParser.parseIntObject(dr["CampaignId"]));

                                object payload = null;
                                if (!string.IsNullOrWhiteSpace(CustomParser.parseStringObject(dr["PayLoad"])))
                                {
                                    var dict = HttpUtility.ParseQueryString(CustomParser.parseStringObject(dr["PayLoad"]));
                                    payload = dict.AllKeys.ToDictionary(k => k, k => dict[k]);

                                }

                                bool isInsideApp, isUpcoming;
                                isInsideApp = String.IsNullOrEmpty(dr["IsInsideApp"].ToString()) ? false : Convert.ToBoolean(dr["IsInsideApp"]);
                                isUpcoming = String.IsNullOrEmpty(dr["IsUpcoming"].ToString()) ? false : Convert.ToBoolean(dr["IsUpcoming"]);
                                
                                sponsoredCampaigns[index].Links = sponsoredCampaigns[index].Links ?? new List<Sponsored_CarLink>();
                                sponsoredCampaigns[index].Links.Add(new Sponsored_CarLink()
                                {
                                    Name = CustomParser.parseStringObject(dr["Name"]),
                                    IsInsideApp = isInsideApp,
                                    Url = CustomParser.parseStringObject(dr["Url"]),
                                    IsUpcoming = isUpcoming,
                                    PayLoad = payload
                                });
                            }
                        }

                        if (dr.NextResult())
                        {
                            if(dr.Read())
                            {
                                DateTime.TryParse(dr["NextCampaignStartDate"].ToString(), out nextCampaignStartDate);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return sponsoredCampaigns;
        }

        /// <summary>
        /// Author:Piyush Sahu
        /// Created Date : 08/03/2017  (ddMMyy)
        /// Returns all the active and upcoming SponsoredHistyoryModels based on platformId passed
        /// </summary>
        /// <param name="platformId">eg: 1 for desktop</param>        
        public List<SponsoredHistoryModels> GetAllSponsoredHistoryModels(int platformId)
        {            
            try
            {
                using (var con = EsMySqlReadConnection)
                {
                    var sponsoredHistoryAdData = con.Query<SponsoredHistoryModels>("FetchAllSponsoredHistoryModels", new { v_PlatformId = platformId }, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("FetchAllSponsoredHistoryModels");

                    if (sponsoredHistoryAdData != null && sponsoredHistoryAdData.AsList().Count > 0)
                        return sponsoredHistoryAdData.AsList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public List<GlobalSearchSponsoredModelEntity> GetAllSponsoredTrendingCars(int platformId)
        {
            List<GlobalSearchSponsoredModelEntity> trendingAdModelsList = new List<GlobalSearchSponsoredModelEntity>();
            try
            {
                using (var con = EsMySqlReadConnection)
                {
                    trendingAdModelsList = con.Query<GlobalSearchSponsoredModelEntity>("GetSponsoredTrendingModels", new { v_PlatforId = platformId }, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return trendingAdModelsList;
        }
    }
}
