using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.ES
{
    public class SponsoredNavigationRepo : RepositoryBase, ISponsoredNavigationRepository
    {
        public List<SponsoredNavigation> GetSponsoredNavigationData(int sectionId, int platformId, out DateTime nextCampaignStartDate)
        {
            List<SponsoredNavigation> sponsoredNavData = new List<SponsoredNavigation>();
            nextCampaignStartDate = DateTime.Now.AddDays(1);
            try
            {
                var param = new DynamicParameters();
                param.Add("v_SectionId", sectionId);
                param.Add("v_PlatformId", platformId);
                using (var con = EsMySqlReadConnection)
                {
                    var response = con.QueryMultiple("GetSponsoredNavigationData", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetSponsoredNavigationData");
                    sponsoredNavData = response.Read<SponsoredNavigation>().ToList();
                    
                    DateTime.TryParse(response.Read<DateTime>().Single().ToString(), out nextCampaignStartDate);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return sponsoredNavData;
        }
    }
}
