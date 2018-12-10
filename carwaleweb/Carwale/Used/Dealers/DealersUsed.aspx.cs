using System;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using Carwale.Utility;
using Carwale.DAL.Classified.UsedDealers;
using Carwale.Entity.Classified.UsedDealers;
using System.Collections.Generic;
using System.Linq;
using Carwale.Interfaces.Geolocation;
using Microsoft.Practices.Unity;
using Carwale.Service;

namespace Carwale.UI.Used.Dealers
{
    public class DealersUsedNew : System.Web.UI.Page
    {
        protected DataList dlShowroom;
        protected string cityId = string.Empty, cityName = string.Empty;

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["cityid"]))
                {
                    cityId = Request.QueryString["cityid"].ToString();
                    IGeoCitiesCacheRepository geoCitiesCacheRepo;
                    using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
                    {
                        geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
                    }
                    cityName = geoCitiesCacheRepo.GetCityNameById(cityId);
                }
                BindControl();
            }
        }

        private void BindControl()
        {
            try
            {

                long intCityId;
                Int64.TryParse(cityId, out intCityId);
                DealersForCityRepository dealerCity = new DealersForCityRepository();
                List<DealersForCity> dealerList = dealerCity.GetDealerForCity(intCityId);
                //Populating the IsWebSiteAvailable property based on whether url is fetched from DB.
                //This will be used for checking website availability on aspx page since this check,
                // if done on aspx, will have to be done more than once
                dealerList.Where(x => x.WebsiteUrl != null && !string.IsNullOrWhiteSpace(x.WebsiteUrl))
                    .Select(s => { s.IsWebSiteAvailable = true; return s; })
                    .ToList();

                dlShowroom.DataSource = dealerList;
                dlShowroom.DataBind();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Modified By : Sachin Shukla , Aug 6, 2015
        /// Description : Added One Parameter to For OriginalImage Path.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="dealerId"></param>
        /// <param name="hostUrl"></param>
        /// <returns></returns>
        public string GetThumbNail(string img, string dealerId, string hostUrl, string OrignalImgPath)
        {
            string str = "";

            if (!string.IsNullOrWhiteSpace(img))
                //str = "<img class='lazy' src='https://imgd.aeplcdn.com/0x0/statics/grey.gif' data-original='" + ImagingFunctions.GetImagePath("/dealer/img/", hostUrl) + img + "' style='border: 3px solid #F2F6F5;'/>";
                str = "<img class='lazy' src='https://imgd.aeplcdn.com/0x0/dealer/img/noimage.jpg' data-original='" + ImageSizes.CreateImageUrl(hostUrl, ImageSizes._110X61, OrignalImgPath) + "' style='border: 3px solid #F2F6F5;'/>";
            else
                str = "<img class='lazy' src='https://imgd.aeplcdn.com/0x0/dealer/img/noimage.jpg' data-original='https://imgd.aeplcdn.com/0x0/dealer/img/noimage.jpg' border='1' style='border: 3px solid #F2F6F5;' />";

            return str;
        }

    }

}