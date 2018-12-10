using Carwale.Entity;
using Carwale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Carwale.UI.Controls
{
    public class CommonPager : UserControl
    {
        private string _baseUrl, _pageUrlType, _prevUrl;
        private int _pageNo, _pageSize, _pageSlotSize, _totalResults;
        private string _nextUrl, firstUrl, lastUrl;
        protected HyperLink prevPage;
        protected HyperLink nextPage;
        protected HyperLink firstPage;
        protected HyperLink lastPage;
        protected HtmlGenericControl lastPageSpan, nextPageSpan, firstPageSpan, prevPageSpan;
        protected int pageNo;


        public Repeater rptPagerList;

        // Base Url for the URL (Ideally the part that comes between domain name and the page Url.)
        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        public string PrevUrl
        {
            get { return _prevUrl; }
            set { _prevUrl = value; }
        }

        public string NextUrl
        {
            get { return _nextUrl; }
            set { _nextUrl = value; }
        }


        // Current Page Number.
        public int PageNo
        {
            get { return _pageNo; }
            set { _pageNo = value; }
        }

        //Number of results per page.
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        //Number of page numbers in each slot.
        public int PageSlotSize
        {
            get { return _pageSlotSize; }
            set { _pageSlotSize = value; }
        }

        //Page Url Format.
        public string PageUrlType
        {
            get { return _pageUrlType; }
            set { _pageUrlType = value; }
        }

        // Total number of results in the result set.
        public int TotalResults
        {
            get { return _totalResults; }
            set { _totalResults = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        protected void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        } // Page_Load


        /// <summary>
        /// Get the pager for the listing page.
        /// </summary>
        public void GetPagerForCMS()
        {
            IPager pager = new Carwale.BL.Pager();
            PagerEntity pagerDetails = new PagerEntity();

            // Initialize pager properties.
            pagerDetails.baseUrl = BaseUrl;
            pagerDetails.pageNo = PageNo;
            pagerDetails.pagerSlotSize = PageSlotSize;
            pagerDetails.pageSize = PageSize;
            pagerDetails.pageUrlType = PageUrlType;
            pagerDetails.totalResults = TotalResults;

            pageNo = PageNo;
            //Get the pager.
            PagerOutputEntity pagerlist = pager.GetPager<PagerOutputEntity>(pagerDetails);
            GetPageList(pagerlist);
        }

        protected string ApplyPageClass(string pageNo)
        {
            if (pageNo == PageNo.ToString())
                return "pgSel";
            else
                return "pg";
        }

        /// <summary>
        /// Generate Pevious and Next Url for the first , next label in the pager.
        /// </summary>
        /// <param name="getOtherUrls"></param>
        private void GetPrevNextUrl(PagerOutputEntity getOtherUrls)
        {
            PrevUrl = getOtherUrls.PreviousPageUrl;
            NextUrl = getOtherUrls.NextPageUrl;
            firstUrl = getOtherUrls.FirstPageUrl;
            lastUrl = getOtherUrls.LastPageUrl;
        }

        /// <summary>
        /// Gets the page list out and binds it to the repeater.
        /// </summary>
        /// <param name="pagerdetails"></param>
        private void GetPageList(PagerOutputEntity pagerdetails)
        {
            GetPrevNextUrl(pagerdetails);
            List<PagerUrlList> pagerList = pagerdetails.PagesDetail;

            if (rptPagerList != null)
            {
                rptPagerList.DataSource = pagerList;
                rptPagerList.DataBind();
            }
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            if (string.IsNullOrEmpty(PrevUrl))
            {
                firstPageSpan.Attributes["Class"] = "pgEnd";
                prevPageSpan.Attributes["Class"] = "pgEnd";
                nextPageSpan.Attributes["Class"] = "pgSel";
                lastPageSpan.Attributes["Class"] = "pgSel";
                nextPage.Attributes.Add("href", NextUrl);
                lastPage.Attributes.Add("href", lastUrl);
            }

            else if (string.IsNullOrEmpty(NextUrl))
            {
                firstPageSpan.Attributes["Class"] = "pgSel";
                prevPageSpan.Attributes["Class"] = "pgSel";
                nextPageSpan.Attributes["Class"] = "pgEnd";
                lastPageSpan.Attributes["Class"] = "pgEnd";
                prevPage.Attributes.Add("href", PrevUrl);
                firstPage.Attributes.Add("href", firstUrl);
            }

            else
            {
                firstPageSpan.Attributes["Class"] = "pgSel";
                prevPageSpan.Attributes["Class"] = "pgSel";
                nextPageSpan.Attributes["Class"] = "pgSel";
                lastPageSpan.Attributes["Class"] = "pgSel";
                nextPage.Attributes.Add("href", NextUrl);
                prevPage.Attributes.Add("href", PrevUrl);
                firstPage.Attributes.Add("href", firstUrl);
                lastPage.Attributes.Add("href", lastUrl);

            }
        }
    }
}