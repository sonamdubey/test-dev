using Bhrigu;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.Bhrigu;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.Tracking
{
    public class BhriguTracker
    {
        /// <summary>
        /// Sends tracking request to Bhrigu
        /// Uses current http request data for request params
        /// </summary>
        /// <param name="category">Event category (mandatory)</param>
        /// <param name="action">Event action (mandatory)</param>
        /// <param name="label">Label to send extra info in key value pairs</param>
        public void Track(string category, string action, Dictionary<string, string> label)
        {
            string lbl = FormatLabel(label);
            Track(category, action, lbl);
        }

        public void Track(string category, string action, string label)
        {
            try
            {
                IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
                apiGatewayCaller.AddTrackEvent(category, action, label);
                Task.Run(() =>
                {
                    apiGatewayCaller.CallAsync();
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "BhriguTracker.Track()");
            }
        }

        private static string FormatLabel(Dictionary<string, string> label)
        {
            return label != null && label.Count > 0 ? string.Join("|", label.Select(l => string.Format("{0}={1}", l.Key, l.Value))) : "NA";
        }
    }
}

