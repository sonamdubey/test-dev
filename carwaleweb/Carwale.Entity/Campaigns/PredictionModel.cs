using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class PredictionModelRequest
    {
        public string Name { get; set; }
        public string CookieId { get; set; }
        public string PageUrl { get; set; }
        public string Platform { get; set; }
        public string CarModelName { get; set; }
        public string ReferrerUrl { get; set; }
        public PredictionModelRequestLocation Global { get; set; }
        public PredictionModelRequestLocation User { get; set; }
        public bool IsUserLoggedIn { get; set; }
        public int CampaignId { get; set; }
        public ulong LeadId { get; set; }
    }

    [Serializable]
    public class PredictionCampaignRequest
    {
        public string Name { get; set; }
        public string CookieId { get; set; }
        public string PageUrl { get; set; }
        public string Platform { get; set; }
        public int ModelId { get; set; }
        public string ReferrerUrl { get; set; }
        public PredictionModelRequestLocation Global { get; set; }
        public PredictionModelRequestLocation User { get; set; }
        public bool IsUserLoggedIn { get; set; }
        public string Source { get; set; }
    }

    [Serializable]
    public class PredictionModelRequestLocation
    {
        public string CityName { get; set; }
        public string ZoneName { get; set; }
    }

    [Serializable]
    public class PredictionModelResponse
    {
        public double Score { get; set; }
        public string Label { get; set; }
    }
}
