using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Pager;
using Bikewale.BAL.Pager;

namespace Bikewale.Controls
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24 Sept 2014
    /// Commented OnInit and Page_Load Events as they are not more required
    /// </summary>
    
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

        
        //protected override void OnInit(EventArgs e)
        //{
        //    InitializeComponent();
        //}

        //void InitializeComponent()
        //{
        //    base.Load += new EventHandler(Page_Load);
        //}
        //protected void Page_Load(object sender, EventArgs e)
        //{
           
        //    if (TotalPages > 1)
        //    {
        //        firstPageUrl = PagerOutput.FirstPageUrl;
        //        prevPageUrl = PagerOutput.PreviousPageUrl;
        //        nextPageUrl = PagerOutput.NextPageUrl;
        //        lastPageUrl = PagerOutput.LastPageUrl;
        //        BindPagerList();
        //    }
        //}

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 June 2014
        /// Summary : To bind repeater for pager
        /// Modified By : Ashwini Todkar on 24 Sept 2014
        /// Added condition TotalPages should greater than 1 to create pager
        /// </summary>
        public void BindPagerList()
        {
            if (TotalPages > 1)
            {
                firstPageUrl = PagerOutput.FirstPageUrl;
                prevPageUrl = PagerOutput.PreviousPageUrl;
                nextPageUrl = PagerOutput.NextPageUrl;
                lastPageUrl = PagerOutput.LastPageUrl;
                rptPager.DataSource = PagerOutput.PagesDetail;
                rptPager.DataBind();
            }
        }
    }
}