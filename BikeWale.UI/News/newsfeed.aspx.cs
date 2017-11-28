using Bikewale.Common;
using System;
using System.Web;
using System.Xml;
namespace Bikewale.News
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 16 Nov 2012
    ///     Summary : Class for creating the news feed.
    /// </summary>
    public class NewsFeed : System.Web.UI.Page
    {
        private int CurrentPageIndex = 1;//, PageSize = 10;

        protected string _title = "";
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }
        void Page_Load(object Sender, EventArgs e)
        {
            if (Request["pn"] != null && Request.QueryString["pn"].ToString().Trim() != string.Empty)
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    CurrentPageIndex = Convert.ToInt32(Request.QueryString["pn"]);
            }
            BindNews();
        }

        private void BindNews()
        {
            try
            {
                //sets the base URI for HTTP requests
                string _apiUrl = string.Format("{0}/news/feed/?application=2", Utility.BWConfiguration.Instance.CwApiHostUrl);

                if (CurrentPageIndex > 1)
                {
                    _apiUrl = string.Format("{0}/news/page/{1}/feed/?application=2", Utility.BWConfiguration.Instance.CwApiHostUrl, CurrentPageIndex);
                }

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(_apiUrl) as System.Net.HttpWebRequest;
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "text/xml";
                Response.Cache.SetCacheability(HttpCacheability.Public);

                Response.Write(xmlDoc.InnerXml);

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

            finally
            {
                Response.End();
            }
        }
    }   // End of class
}   // End of namespace