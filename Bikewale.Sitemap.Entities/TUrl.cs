using System;

namespace Bikewale.Sitemap.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Oct 2017
    /// Description :   Base class for Sitemap Url entity
    /// </summary>
    public class TUrl
    {
        public string Location { get; set; }
        public DateTime LastModified { get; set; }
        public String ChangeFreq
        {
            get
            {
                switch (ChangeFrequency)
                {
                    case ChangeFreqVal.Always:
                        return "always";
                    case ChangeFreqVal.Hourly:
                        return "hourly";
                    case ChangeFreqVal.Daily:
                        return "daily";
                    case ChangeFreqVal.Weekly:
                        return "weekly";
                    case ChangeFreqVal.Monthly:
                        return "monthly";
                    case ChangeFreqVal.Yearly:
                        return "yearly";
                    case ChangeFreqVal.Never:
                        return "Never";
                    default:
                        return "";
                }
            }
        }
        public ChangeFreqVal ChangeFrequency { get; set; }
        public decimal Priority { get; set; }
    }

    public enum ChangeFreqVal
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
}
