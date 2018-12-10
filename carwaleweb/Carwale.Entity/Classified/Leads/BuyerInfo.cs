
using System;
namespace Carwale.Entity.Classified.Leads
{
    [Serializable]
    public class BuyerInfo
    {
        public string Mobile { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public bool IsChatLeadGiven { get; set; }
    }
}