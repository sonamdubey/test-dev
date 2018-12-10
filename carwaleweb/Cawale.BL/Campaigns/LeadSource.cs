using System.Collections.Generic;
using System;
using Carwale.Notifications;

namespace Carwale.BL.Campaigns
{
    public static class LeadSource
    {
        static Dictionary<string, int> LeadSources = new Dictionary<string, int>();

        static LeadSource()
        {
            LeadSourcesDict();  
        }

        public static int Get(string key)
        {
            int leadSource;
            try
            {
                if(LeadSources.TryGetValue(key, out leadSource))
                    return leadSource;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Campaigns.BL.LeadSource.Get()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return 0;
        }

        private static Dictionary<string,int> LeadSourcesDict()
        {
            try
            {
                var leadSources = new DAL.Campaigns.LeadSource().GetAllLeadSources();

                foreach (var source in leadSources)
                {
                    string leadClickSourceKey = string.Format("leadsource-adtype-platform-leadclicksource-{0}-{1}-{2}", source.LeadClickSourceDesc, source.AdType, source.PlatformId);
                    string inquirySourceKey = string.Format("leadsource-adtype-platform-inquirysource-{0}-{1}-{2}", source.LeadClickSourceDesc, source.AdType, source.PlatformId);

                    LeadSources[leadClickSourceKey] = source.LeadClickSourceId;
                    LeadSources[inquirySourceKey] = source.InquirySourceId;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Campaigns.BL.LeadSource.LeadSourcesDict()\n Exception : " + ex.Message);
                objErr.LogException(); 
            }
            return LeadSources;
        }

        /// <summary>
        /// This function returns leadsources list
        /// </summary>
        /// <param name="leadSourceDesc"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public static Entity.PriceQuote.LeadSource GetLeadSource(string leadSourceDesc, int leadSourceId, int platformId)
        {
            var leadSource = new Entity.PriceQuote.LeadSource
            {
                LeadClickSourceId = BL.Campaigns.LeadSource.Get(string.Format("leadsource-adtype-platform-leadclicksource-{0}-{1}-{2}", leadSourceDesc, leadSourceId, platformId)),
                InquirySourceId = BL.Campaigns.LeadSource.Get(string.Format("leadsource-adtype-platform-inquirysource-{0}-{1}-{2}", leadSourceDesc, leadSourceId, platformId)),
                LeadClickSourceDesc = leadSourceDesc,
            };
            return leadSource;
        }
    }
}
