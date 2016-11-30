using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Bikewale.Utility
{
    /// Created by:Sangram Nandkhile on 30 Nov 2016
    /// Desc: Utilities for sponsored campaigns
    public class SponsoredCampaigns
    {
        /// <summary>
        /// Created by:Sangram Nandkhile on 30 Nov 2016
        /// Desc: returns value for model id campaigns
        /// </summary>
        /// <param name="offerText">Individual offer to be checked</param>
        /// <returns></returns>
        public static string FetchValue(string key)
        {
            string displayText = string.Empty;
            try
            {
                NameValueCollection keyValCollection = ConfigurationManager.GetSection("sponsoredModelId") as NameValueCollection;
                if (keyValCollection != null)
                {
                    foreach (var keyp in keyValCollection.AllKeys)
                    {
                        if (key == keyp)
                        {
                            displayText = keyValCollection.GetValues(keyp).First();
                            break;
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
            return displayText;
        }
    }
}
