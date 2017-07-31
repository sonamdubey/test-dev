using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bikewale.Comparison.DAL
{
    /// <summary>
    /// Modified by :- Sangram Nandkhile on 26 july 2017
    /// summary :- Sponsored Comparison Repository
    /// </summary>
    /// <returns></returns>
    public class SponsoredComparisonRepository : ISponsoredComparisonRepository
    {
        /// <summary>
        /// Deletes the sponsored comparison bike all rules.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes the sponsored comparison bike sponsored model rules.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <param name="SponsoredModelId">The sponsored model identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes the sponsored comparison bike sponsored version rules.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <param name="sponsoredVersionId">The sponsored version identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes the sponsored comparison bike target version rules.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <param name="targetversionId">The targetversion identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Jul 2017
        /// Description :   Returns Sponsored version by given list of Target Version Ids.
        /// </summary>
        /// <param name="versionIds">Comma delimited version ids</param>
        /// <returns></returns>
        public SponsoredVersionEntityBase GetSponsoredBike(string versionIds)
        {
            SponsoredVersionEntityBase sponsoredVersion = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_versions", versionIds);
                    sponsoredVersion = connection.Query<SponsoredVersionEntityBase>("getsponsoredcomparisonbyversions", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();


                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("SponsoredCampaignRepository.GetSponsoredBike() {0}:", versionIds));
            }

            return sponsoredVersion;
        }

        /// <summary>
        /// Gets the sponsored comparison.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns></returns>
        public SponsoredComparison GetSponsoredComparison()
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Comparison.DAL.SponsoredCampaignRepository.GetSponsoredComparison()");
            }
            return objCampaign;
        }

        /// <summary>
        /// Gets the sponsored comparisons.
        /// </summary>
        /// <param name="statuses">The statuses.</param>
        /// <returns></returns>
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.GetSponsoredComparison() {0}:", statuses));
            }

            return comparisonCampaigns;
        }

        /// <summary>
        /// Gets the sponsored comparison sponsored bike.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public dynamic GetSponsoredComparisonSponsoredBike(uint camparisonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the sponsored comparison version mapping.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <param name="sponsoredModelId">The sponsored model identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TargetSponsoredMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint targetModelId, uint sponsoredModelId)
        {
            TargetSponsoredMapping objResult = default(TargetSponsoredMapping);
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    objResult = new TargetSponsoredMapping();
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_comparisonid", camparisonId);
                    param.Add("par_targetmodelid", targetModelId);
                    param.Add("par_sponsoredmodelid", sponsoredModelId);

                    using (var results = connection.QueryMultiple("getsponsoredcomparisonversionmapping", param: param, commandType: CommandType.StoredProcedure))
                    {
                        objResult.SponsoredModelVersion = results.Read<BikeModel>();
                        objResult.SponsoredVersionsMapping = results.Read<BikeModelVersionmapping>();
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.GetSponsoredComparisonVersionMapping, camparisonId: {0}, targetModelId: {1}, sponsoredModelId: {2} ", camparisonId, targetModelId, sponsoredModelId));
            }
            return objResult;
        }


        /// <summary>
        /// Saves the sponsored comparison.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Saves the sponsored comparison bike rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public bool SaveSponsoredComparisonBikeRules(VersionTargetMapping rules)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_comparisonid", rules.Comparisonid);
                    param.Add("par_isversionmapping", rules.IsVersionMapping);
                    param.Add("par_targetsponsoredids", rules.TargetSponsoredIds);
                    param.Add("par_impressionurls", rules.ImpressionUrl);
                    connection.Query<dynamic>("savesponsoredcomparisonsbikerules", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Comparison.DAL.SponsoredCampaignRepository.SaveSponsoredComparisonBikeRules");
            }
            return isSaved;
        }

        /// <summary>
        /// Changes the sponsored comparison status.
        /// </summary>
        /// <param name="camparisonId">The camparison identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public bool ChangeSponsoredComparisonStatus(uint camparisonId, ushort status)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_comparisonid", camparisonId);
                    param.Add("par_status", status);

                    connection.Query<dynamic>("changesponsoredcomparisonstatus", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Comparison.DAL.SponsoredCampaignRepository.ChangeSponsoredComparisonStatus, camparisonId: {0}, Status: {1}", camparisonId, status));
            }
            return isSaved;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Jul 2017
        /// Description :   Returns Active Sponsored Comparisons
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ActiveSponsoredCampaign> GetActiveSponsoredComparisons()
        {
            IEnumerable<ActiveSponsoredCampaign> sponsoredComparison = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {
                    connection.Open();
                    sponsoredComparison = connection.Query<ActiveSponsoredCampaign>("getactivesponsoredcomparisons", commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Comparison.DAL.SponsoredCampaignRepository.GetActiveSponsoredComparisons");
            }
            return sponsoredComparison;

        }
    }
}
