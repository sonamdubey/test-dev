using Carwale.Entity;
using Carwale.Entity.Geolocation;
using Carwale.Entity.LandingPage;
using Carwale.Interfaces.LandingPage;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.LandingPage
{
    public class LandingPageRepository : RepositoryBase, ILandingPageRepository
    {

        public Tuple<LandingPageDetails, IEnumerable<MakeModelIdsEntity>, IEnumerable<Cities>> GetLandingPageDetails(int CampaignId)
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_CampaignId", CampaignId);
                    LogLiveSps.LogSpInGrayLog("[dbo].[GetLandingPageDetailsByCampaignId]");
                    var campaignData = con.QueryMultiple("GetLandingPageDetailsByCampaignId_v16_11_7", param, null, null, CommandType.StoredProcedure);

                    var campaignConfigData = campaignData.Read<LandingPageDetails>().Single();
                    var modelRulesData = campaignData.Read<MakeModelIdsEntity>();
                    var cityRulesData = campaignData.Read<Cities>();

                    return new Tuple<LandingPageDetails, IEnumerable<MakeModelIdsEntity>, IEnumerable<Cities>>(campaignConfigData, modelRulesData, cityRulesData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LandingPageRepository.GetLandingPageDetails()");
                objErr.SendMail();
            }
            return null;
        }

    }//class
}//namespace
