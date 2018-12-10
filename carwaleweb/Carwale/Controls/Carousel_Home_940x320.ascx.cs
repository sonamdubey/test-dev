using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.CoreDAL.MySql;
using System.Net.Http;
using System.Net.Http.Headers;
using Carwale.Entity.XmlFeed;
using Carwale.Entity.UsedCarsDealer;

namespace Carwale.UI.Controls
{
    public class Carousel_Home_940x320 : UserControl
    {
        protected Repeater rptImges; //Repeater  
        protected HtmlGenericControl bannerfull;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private bool _roundedcor = true;
        public bool isRoundedCorner
        {
            get { return _roundedcor; }
            set { _roundedcor = value; }
        }

        private string _dealer = string.Empty;
        public string DealerId
        {
            get { return _dealer; }
            set { _dealer = value; }
        }

        private bool _banner = true;
        public bool IsBanner
        {
            get { return _banner; }
            set { _banner = value; }
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //GetImages();
            }
        }

        private static readonly HttpClient _httpClient = new HttpClient();

        public void GetImages(string DealerId)
        {
            try
            {
                var response = _httpClient.GetAsync(new Uri(ConfigurationManager.AppSettings["BannerImageUrl"] + "?dealerId=" + DealerId)).Result;
                List <ShowroomImage> lstShowroomImages = null;
                if (response.IsSuccessStatusCode)
                {
                    lstShowroomImages = response.Content.ReadAsAsync<List<ShowroomImage>>().Result;
                }

                if (lstShowroomImages != null)
                {
                    rptImges.DataSource = lstShowroomImages;
                    rptImges.DataBind();
                }
                else
                {
                    IsBanner = false;
                    bannerfull.Visible = false;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected string IsRoundedCorner()
        {
            if (isRoundedCorner)
                return "class='rounded-corner'";
            else
                return "";
        }
    }//class
}//namespace