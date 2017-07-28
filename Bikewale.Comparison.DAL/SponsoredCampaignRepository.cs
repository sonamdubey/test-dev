using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using Bikewale.DAL.CoreDAL;
using System.Data;
using Dapper;
using Bikewale.Notifications;

namespace Bikewale.Comparison.DAL
{
    /// <summary>
    /// Modified by :- Sangram Nandkhile on 26 july 2017
    /// summary :- Sponsored Campaign Repository
    /// </summary>
    /// <returns></returns>
    public class SponsoredCampaignRepository : ISponsoredCampaignRepository
    {
        public bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_camparisonid", camparisonId);
                    connection.Query<dynamic>("DeleteSponsoredComparisonBikeAllRules", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.DeleteSponsoredComparisonBikeAllRules: -> campaignId : {0}", camparisonId));
            }
            return isSaved;
        }

        public bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredModelId)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_camparisonid", camparisonId);
                    param.Add("par_sponsoredmodelid", SponsoredModelId);
                    connection.Query<dynamic>("DeleteSponsoredComparisonBikeSponsoredModelRules", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.DeleteSponsoredComparisonBikeSponsoredModelRules: -> campaignId : {0},SponsoredModelId : {1} ", camparisonId, SponsoredModelId));
            }
            return isSaved;
        }

        public bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint sponsoredVersionId)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_camparisonid", camparisonId);
                    param.Add("par_sponsoredversionid", sponsoredVersionId);
                    connection.Query<dynamic>("DeleteSponsoredComparisonBikeSponsoredVersionRules", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.DeleteSponsoredComparisonBikeSponsoredModelRules: -> camparisonId : {0},sponsoredVersionId : {1} ", camparisonId, sponsoredVersionId));
            }
            return isSaved;
        }

        public bool DeleteSponsoredComparisonBikeTargetVersionRules(uint camparisonId, uint targetversionId)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_camparisonid", camparisonId);
                    param.Add("par_sponsoredversionid", targetversionId);
                    connection.Query<dynamic>("deletesponsoredComparisonbiketargetVersionRules", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.DeleteSponsoredComparisonBikeTargetVersionRules: -> camparisonId : {0},targetversionId : {1} ", camparisonId, targetversionId));
            }
            return isSaved;
        }

        public SponsoredComparison GetSponsoredComparison()
        {
            throw new NotImplementedException();
        }

        public SponsoredComparison GetSponsoredComparison(uint campaignId)
        {
            SponsoredComparison objCampaign = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    objCampaign = connection.Query<SponsoredComparison>("getsponsoredcomparison", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();


                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.GetSponsoredComparison: -> campaignId : {0}", campaignId));
            }


            return objCampaign;
        }

        public IEnumerable<SponsoredComparison> GetSponsoredComparisons(string statuses)
        {
            IEnumerable<SponsoredComparison> comparisonCampaigns = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_statuses", statuses);
                    comparisonCampaigns = connection.Query<SponsoredComparison>("getsponsoredcomparisons", param: param, commandType: CommandType.StoredProcedure);


                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Comparison.DAL.SponsoredCampaignRepository.GetSponsoredComparisons");
            }

            return comparisonCampaigns;
        }

        public TargetedModel GetSponsoredComparisonSponsoredBike(uint camparisonId)
        {
            throw new NotImplementedException();
        }
        
        public SponsoredVersionMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint sponsoredModelId)
        {
            throw new NotImplementedException();
        }

        public uint SaveSponsoredComparison(SponsoredComparison campaign)
        {
            uint comparisonId = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_name", campaign.Name);
                    param.Add("par_startdate", campaign.StartDate);
                    param.Add("par_enddate", campaign.EndDate);
                    param.Add("par_linktext", campaign.LinkText);
                    param.Add("par_linkurl", campaign.LinkUrl);
                    param.Add("par_impressionurl", campaign.NameImpressionUrl);
                    param.Add("par_imgimpressionurl", campaign.ImgImpressionUrl);
                    param.Add("par_updatedby", campaign.UpdatedBy);
                    param.Add("par_id", campaign.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Query<dynamic>("savesponsoredcomparisons", param: param, commandType: CommandType.StoredProcedure);

                    comparisonId = param.Get<uint>("par_id");

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Comparison.DAL.SponsoredCampaignRepository.SaveSponsoredComparison");
            }

            return comparisonId;
        }

        public bool SaveSponsoredComparisonBikeRules()
        {
            throw new NotImplementedException();
        }
    }
}
