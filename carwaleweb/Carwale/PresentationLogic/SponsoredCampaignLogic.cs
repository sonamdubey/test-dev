using Carwale.Entity.CarData;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.UI.PresentationLogic
{
    public static class SponsoredCampaignLogic
    {
        private static string _testDriveCampaignIds = ConfigurationManager.AppSettings["TestDriveCampaignIds"];
        public static int[] TestDriveCampaignIds = string.IsNullOrEmpty(_testDriveCampaignIds) ?
                                                    new int[0] : _testDriveCampaignIds.Split(',').Select(x => int.Parse(x)).ToArray();
        public static Sponsored_Car GetSponsoredBackground(int categoryId, int platformId, int categorySection, string param, int applicationId)
        {
            ISponsoredCarCache _sponsoredCache = UnityBootstrapper.Resolve<ISponsoredCarCache>();
            var sponsoredBackground = _sponsoredCache.GetSponsoredCampaigns(categoryId, platformId, categorySection, param, applicationId);
            if (sponsoredBackground != null && sponsoredBackground.Count > 0)
            {
                return sponsoredBackground.First();
            }   

            return null;
        }
    }
}