using BikeWaleOpr.Entities.Pager;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Controls
{
    public class LinkPagerControl : System.Web.UI.UserControl
    {
        protected Repeater rptPager;
        protected string firstPageUrl = string.Empty, prevPageUrl = string.Empty, nextPageUrl = string.Empty, lastPageUrl = string.Empty;
        private bool _hideFirstLastUrl = false;
        public bool HideFirstLastUrl
        {
            get { return _hideFirstLastUrl; }
            set { _hideFirstLastUrl = value; }
        }
        public PagerOutputEntity PagerOutput { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPageNo { get; set; }

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
            if (TotalPages > 1)
            {
                firstPageUrl = PagerOutput.FirstPageUrl;
                prevPageUrl = PagerOutput.PreviousPageUrl;
                nextPageUrl = PagerOutput.NextPageUrl;
                lastPageUrl = PagerOutput.LastPageUrl;
                BindPagerList();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 June 2014
        /// Summary : To bind repeater for pager
        /// </summary>
        protected void BindPagerList()
        {
            rptPager.DataSource = PagerOutput.PagesDetail;
            rptPager.DataBind();
        }
    }
}