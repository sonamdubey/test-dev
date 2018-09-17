
namespace Bikewale.Entities.Tracking
{
    /// <summary>
    /// Created By : Sanjay George on 16th August 2018
    /// Description : Entity for Bhrigu Tracking
    /// </summary>
    public class BhriguTrackEntity
    {
        public string Category { get; set; }
        public string Action { get; set; }
        public string Label { get; set; }
        public string CookieId { get; set; }
        public string SessionId { get; set; }
        public string PageUrl { get; set; }
        public string QueryString { get; set; }
        public string Cookie { get; set;}
        public string ClientIP { get; set;}
        public string UserAgent { get; set;}
        public string Referrer { get; set; }
    }
}
