using AutoMapper;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Carwale.UI.Editorial
{
    public class FeaturesDefault : System.Web.UI.Page
    {
        protected CommonPager pagerDetails;
        protected Repeater rptFeatures, rptUCL;
        protected DropDownList ddlUsedCarLocations, ddlMake;

        protected string SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
                             OrderByClause = string.Empty, RecordCntQry = string.Empty, BaseUrl = string.Empty;

        protected int pageNumber = 1, pageSize = 10;
        protected string prevUrl = string.Empty, nextUrl = string.Empty;
        public string subCategoryId { get; set; }
        public string subCategoryName { get; set; }
        public PlaceHolder headMetaTags = null;
        protected string canonicalUrl = string.Empty;
        protected int totalPages;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());//device detection code
            dd.DetectDevice();

            CommonOpn op = new CommonOpn();
            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    int.TryParse(Request.QueryString["pn"], out pageNumber);
            }

            subCategoryId = Request.QueryString["catid"];

            FillComparisonTests();

            FormCanonical();
            CreatePrevNextUrl();

            if (nextUrl != "")
            {
                HtmlLink linkNext = new HtmlLink();
                linkNext.Href = nextUrl;
                linkNext.Attributes.Add("rel", "next");
                //headMetaTags.Controls.Add(linkNext);
            }

            if (prevUrl != "")
            {
                HtmlLink linkPrev = new HtmlLink();
                linkPrev.Href = prevUrl;
                linkPrev.Attributes.Add("rel", "prev");
                //headMetaTags.Controls.Add(linkPrev);
            }
        }


        private void FillComparisonTests()
        {
            ushort startIndex = (ushort)((pageNumber - 1) * pageSize + 1);
            ushort endIndex = (ushort)((pageNumber) * pageSize);
            string categoryIds = "6";

            var featuresList = GetFeaturesByAsync(new ArticleByCatURI() { ApplicationId = 1, CategoryIdList = categoryIds, StartIndex = startIndex, EndIndex = endIndex });// Bind news data by async call


            rptFeatures.DataSource = featuresList.Articles;
            rptFeatures.DataBind();

            pagerDetails.BaseUrl = "/features/";
            pagerDetails.PageNo = Convert.ToInt32(pageNumber);
            pagerDetails.PageSlotSize = Convert.ToInt32(pageSize);
            pagerDetails.PageSize = 10;
            pagerDetails.PageUrlType = "page/";
            pagerDetails.TotalResults = Convert.ToInt32(featuresList.RecordCount);
            pagerDetails.GetPagerForCMS();

            totalPages = pagerDetails.TotalResults / pagerDetails.PageSlotSize;
            if (pagerDetails.TotalResults % pagerDetails.PageSlotSize != 0)
            {
                totalPages++;
            }
        }


        protected CMSContent GetFeaturesByAsync(ArticleByCatURI querystringPopular)
        {
            var cmsCacheRepository = UnityBootstrapper.Resolve<ICMSContent>();
            return Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, CMSContent>(cmsCacheRepository.GetContentListByCategory(querystringPopular));
        }


        private void CreatePrevNextUrl()
        {
            string mainUrl = "/features/";

            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;

            if (pageNumber <= 1)    //if page is first page
            {
                if (pagerDetails.TotalResults > pagerDetails.PageSlotSize)
                {
                    nextPageNumber = "2";
                    nextUrl = mainUrl + "page" + "/" + nextPageNumber + "/";
                }
            }
            else if (pageNumber == totalPages)    //if page is last page
            {
                prevPageNumber = (pageNumber - 1).ToString();
                prevUrl = mainUrl + "page" + "/" + prevPageNumber + "/";
            }
            else
            {          //for middle pages
                prevPageNumber = (pageNumber - 1).ToString();
                prevUrl = mainUrl + "page" + "/" + prevPageNumber + "/";
                nextPageNumber = (pageNumber + 1).ToString();
                nextUrl = mainUrl + "page" + "/" + nextPageNumber + "/";
            }
        }

        protected string FormCanonical()
        {
            if (pageNumber <= 1)
                canonicalUrl = "https://www.carwale.com/features/";
            else
                canonicalUrl = "https://www.carwale.com/features/" + "page" + "/" + pageNumber + "/";
            return canonicalUrl;
        }
    }
}