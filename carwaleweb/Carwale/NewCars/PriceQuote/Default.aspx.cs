using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Cache.SponsoredData;
using Carwale.DAL.SponsoredCar;
using Carwale.Interfaces;
using Carwale.Interfaces.SponsoredCar;
using Carwale.UI.Common;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.UI;

namespace Carwale.UI.NewCars.PriceQuote
{
    public partial class Default : Page
    {
        protected string CompleteQS { get; set; }
        protected string PQPlaceHolder { get; set; }

        protected override void OnInit(EventArgs e)
        {

            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object o, EventArgs e)
        {
            int versionId = CustomParser.parseIntObject(Request.QueryString["version"]);
            int modelId = CustomParser.parseIntObject(Request.QueryString["model"]);
            int cityId = CustomParser.parseIntObject(Request.QueryString["city"]);
            int zoneId = CustomParser.parseIntObject(Request.QueryString["zone"]);
            int pageId = CustomParser.parseIntObject(Request.QueryString["pageid"]);

            DeviceDetection dd = new DeviceDetection(string.Format("/quotation/landing/", versionId, modelId, cityId, zoneId));
            dd.DetectDevice();

            IUnityContainer container = new UnityContainer();
            container.RegisterType<ISponsoredCar, SponsoredCarRepository>();
            container.RegisterType<ICacheManager, CacheManager>();
            container.RegisterType<ISponsoredCarCache, SponsoredCarCacheRepository>();

            ISponsoredCarCache _sponsoredCars = container.Resolve<ISponsoredCarCache>();
            var sponsorPlaceHolder = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.CarSearchPlaceHolder, (int)Carwale.Entity.Enum.Platform.CarwaleDesktop, (int)Carwale.Entity.Enum.PlaceHolderCategory.PQWidget);

            if (sponsorPlaceHolder != null && sponsorPlaceHolder.Count > 0)
            {
                PQPlaceHolder = sponsorPlaceHolder.First().Ad_Html;
            }

            if ((versionId > 0 || modelId > 0) && cityId > 0)
            {
                Response.Cookies["_CustCityId"].Value = cityId.ToString();
                Response.Cookies["_CustCityId"].Domain = CookiesCustomers.CookieDomain;

                Response.Cookies["_PQZoneId"].Value = zoneId > 0 ? zoneId.ToString() : "";
                Response.Cookies["_PQZoneId"].Domain = CookiesCustomers.CookieDomain;

                Response.Cookies["_PQModelId"].Value = modelId.ToString();
                Response.Cookies["_PQModelId"].Domain = CookiesCustomers.CookieDomain;

                Response.Cookies["_PQVersionId"].Value = versionId.ToString();
                Response.Cookies["_PQVersionId"].Domain = CookiesCustomers.CookieDomain;

                if (pageId > 0)
                {
                    Response.Cookies["_PQPageId"].Value = pageId.ToString();
                    Response.Cookies["_PQPageId"].Domain = CookiesCustomers.CookieDomain;
                }

                Response.Redirect("/new/quotation.aspx");
            }
        }
    }
}