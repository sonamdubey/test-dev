using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace Carwale.UI.m.used
{
    public partial class Finance : System.Web.UI.Page
    {
        protected string iframeUrl;
        private const string profileIdKey = "profileId";
        protected static string ctFinanceCWVisitedURL = ConfigurationManager.AppSettings["CTFinanceCWVisitedURL"];
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection nvc = Request.QueryString;
            StringBuilder param = new StringBuilder();
            foreach (string key in nvc.Keys)
            {
                if (key != profileIdKey)
                {
                    param.AppendFormat("&{0}={1}", key, nvc[key]); 
                }
            }
            iframeUrl = string.Format("{0}{1}/step1?{2}", ConfigurationManager.AppSettings["CTFinanceMsite"], nvc[profileIdKey], param.ToString());
        }
    }
}