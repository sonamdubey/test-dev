using AutoMapper;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Carwale.UI.Editorial
{
    public class PitStop : System.Web.UI.Page
    {
        protected Repeater rptPitstop;
        protected CommonPager pagerDetails;
        protected string BaseUrl = string.Empty, NewsTitle = string.Empty, NewsBreadCrumb = string.Empty, prevUrl = string.Empty, nextUrl = string.Empty;
        private string canonicalUrl, altURL;
        protected int pageNumber = 1;        

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
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();  

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    int.TryParse(Request.QueryString["pn"], out pageNumber);
            }
            Trace.Warn("page number is:" + pageNumber);
            FillList();// Bind news data
            FormCanonical();
            FormAltUrl();
        } // Page_Load


        protected void FillList()
        {
            NewsTitle = "Pit Stop";

            pagerDetails.PageSize = 10;
            ushort startIndex = (ushort)((pageNumber - 1) * pagerDetails.PageSize + 1);
            ushort endIndex = (ushort)((pageNumber) * pagerDetails.PageSize);
            string contentCategoryId = "12";

            var pitstopList = GetPitstopAsync(new ArticleByCatURI() { ApplicationId = 1, CategoryIdList = contentCategoryId, StartIndex = startIndex, EndIndex = endIndex });// Bind news data by async call
           
            rptPitstop.DataSource = pitstopList.Articles;
            rptPitstop.DataBind();

            pagerDetails.BaseUrl = "/pitstop/";
            pagerDetails.PageNo = Convert.ToInt32(pageNumber);
            pagerDetails.PageSlotSize = 10;
            pagerDetails.PageUrlType = "page/";
            pagerDetails.TotalResults = Convert.ToInt32(pitstopList.RecordCount);
            pagerDetails.GetPagerForCMS();

            BaseUrl = "/pitstop/";
            NewsBreadCrumb = "<li class=\"current\">&rsaquo; <strong>Pit Stop</strong></li>";
            if (!(string.IsNullOrEmpty(pagerDetails.PrevUrl)))
                prevUrl = "https://www.carwale.com" + pagerDetails.PrevUrl;

            if (!(string.IsNullOrEmpty(pagerDetails.NextUrl)))
                nextUrl = "https://www.carwale.com" + pagerDetails.NextUrl;

        }

        protected CMSContent GetPitstopAsync(ArticleByCatURI querystringPopular)
        {
            var cmsCacheRepository = UnityBootstrapper.Resolve<ICMSContent>();
            return Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, CMSContent>(cmsCacheRepository.GetContentListByCategory(querystringPopular));             
        }

        //form page url with page no.
        protected string FormCanonical()
        {
            if (pageNumber<= 1)
                canonicalUrl = "https://www.carwale.com/pitstop/";
            else
                canonicalUrl = "https://www.carwale.com/pitstop/page/" + pageNumber.ToString() + "/";
            return canonicalUrl;
        }
        protected string FormAltUrl()
        {
            if (pageNumber <= 1)
                altURL = "pitstop/";
            else
                altURL = "pitstop/page/" + pageNumber.ToString() + "/";
            return altURL;
        }


    } // class
} // namespace




