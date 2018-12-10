//comment added by Ashish
//more comments added...
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using WURFL;
using WURFL.Config;
using MobileWeb.DataLayer;
using Carwale.UI.Controls;
using Carwale.Cache.SponsoredData;
using System.Configuration;
using MobileWeb.Common;

namespace MobileWeb
{
	public class Default : Page
	{
		public String WurflManagerCacheKey = "__WurflManager";
   		public String WurflDataFile = MobileWeb.Common.CommonOpn.GetRelativeLocation() + "wurfl/wurfl.xml";
    	public String WurflPatchFile = MobileWeb.Common.CommonOpn.GetRelativeLocation() + "wurfl/web_browsers_patch.xml";

        protected string PlatformId = ConfigurationManager.AppSettings.Get("MobilePlatformId");
        DataView dv;

        protected Repeater rptNews, rptHotDiscussions, rptSponsoredCars;

	
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{	//Response.Cookies["DeviceDetected"].Expires = DateTime.Now.AddDays(-1); Response.End();
			if (!Page.IsPostBack)
			{
				/*
				// if cookie not present it means device is not detected 
				if(Request.Cookies["DeviceDetected"] == null)
				{
					IWURFLManager wurflManager;
					if (Cache[WurflManagerCacheKey] == null)
					{
						string WurflDataFilePath = Server.MapPath(WurflDataFile);
						string WurflPatchFilePath = Server.MapPath(WurflPatchFile);
						
						CacheDependency dependency = new CacheDependency(WurflDataFilePath);
						var configurer = new InMemoryConfigurer().MainFile(WurflDataFilePath).PatchFile(WurflPatchFilePath);
						var manager = WURFLManagerBuilder.Build(configurer);
						Cache.Insert(WurflManagerCacheKey, manager, dependency);
						wurflManager = manager as IWURFLManager;
						//Response.Write("new cache created<br/>");
					}
					else
					{
						wurflManager = Cache[WurflManagerCacheKey] as IWURFLManager;
						//Response.Write("cache found");
					}	
					string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
					//Response.Write("<br/><b>User Agent : </b><br/> " + userAgent);
					IDevice device = wurflManager.GetDeviceForRequest(userAgent);
					string bname = device.GetCapability("brand_name");
					//Response.Write("<br/><b>brand_name : </b><br/> " + bname);
					string device_os = device.GetCapability("device_os").ToString().Trim().ToLower();
					//Response.Write("<br/><b>device_os : </b><br/> " + device_os);
					string mobile_browser = device.GetCapability("mobile_browser").ToString().Trim().ToLower();
					//Response.Write("<br/><b>mobile_browser : </b><br/> " + mobile_browser);
					string ajax_support_javascript = device.GetCapability("ajax_support_javascript").ToString().Trim();
					
					//string is_wireless_device = device.GetCapability("is_wireless_device").ToString().Trim();
					//Response.Write("<br/><b>is_wireless_device : </b><br/> " + is_wireless_device);
					//Response.End();
					
					Response.Cookies["DeviceDetected"].Value = "1";
					
					if (!SmartPhoneOs(device_os) && !(SmartPhoneBrowser(mobile_browser)) && (ajax_support_javascript == "" || ajax_support_javascript == "false"))
					{
						Response.Redirect("NoSite.html");
					}	
				}
				*/
			}
				
			LoadTopNews("3");
			//LoadHotDiscussions(); commented because it's repeater in UI has been removed.
            FetchSponsoredCampaigns();
			//_pagetitle = "New Cars, Used Cars, Car Prices, News & Forums in India - CarWale";
		}
		
        protected void FetchSponsoredCampaigns()
        {
            try
            {
                FrequentlyRequestedQueries frqs = new FrequentlyRequestedQueries();
                dv = frqs.GetSponsoredCampaigns().Tables[0].DefaultView;
                dv.RowFilter = "CampaignCategoryId = 1 AND PlatformId=" + PlatformId;

                rptSponsoredCars.DataSource = dv;
                rptSponsoredCars.DataBind();

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
		private void LoadTopNews(string noOfNews)
		{
            
            NewsLayer obj = new NewsLayer();
            obj.GetRepeater = true;
            obj.Rpt = rptNews;
            obj.LoadTopNews(noOfNews);
            
		}
		
        // removed by sanjay soni because it is not used.
        //private void LoadHotDiscussions()
        //{
        //    Forum obj = new Forum();
        //    obj.GetRepeater = true;
        //    obj.Rpt = rptHotDiscussions;
        //    obj.GetHotDiscussions();
        //}
		
		private bool SmartPhoneBrowser(string mobile_browser)
		{
			bool retVal = false;
			
			string[] browserlist = {"safari", "webkit", "mango", "firefox", "blackberry", "android", "gecko"};	
			
			for (int i=0; i<browserlist.Length; i++)
			{
				if (mobile_browser.IndexOf(browserlist[i]) != -1)
					retVal = true;
			}
			
			return retVal;
		}
		
		private bool SmartPhoneOs(string device_os)
		{
			bool retVal = false;
			
			string[] oslist = {"android",  "iphone", "ios", "rim", "symbian", "blackberry", "palm", "bada", "apple", "windows", "webos", "meego", "phone 7", "os6", "qnx"};	
			
			for (int i=0; i<oslist.Length; i++)
			{
				if (device_os.IndexOf(oslist[i]) != -1)
					retVal = true;
			}
			
			return retVal;
		}
		
		protected string GetDisplayDate(string _displayDate)
		{
			string retVal = "";
			TimeSpan tsDiff = DateTime.Now.Subtract(Convert.ToDateTime(_displayDate));
	
			if (tsDiff.Days > 0)
				retVal =  tsDiff.Days.ToString() + " days ago";	
			else if (tsDiff.Hours > 0)
				retVal =  tsDiff.Hours.ToString() + " hours ago";	
			else if (tsDiff.Minutes > 0)
				retVal =  tsDiff.Minutes.ToString() + " minutes ago";
			else if (tsDiff.Seconds > 0)
				retVal =  tsDiff.Seconds.ToString() + " seconds ago";	
				
			if (tsDiff.Days > 360)
				retVal =  Convert.ToString(tsDiff.Days / 360) + " years ago";	
			else if (tsDiff.Days > 30)
				retVal =  Convert.ToString(tsDiff.Days / 30) + " months ago";							
					
			return retVal;
		}
	}
}			