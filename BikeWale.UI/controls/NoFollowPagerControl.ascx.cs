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
    /// Written By : Ashwini Todkar on 24 Sept 2014
    /// To bind repeater for pager
    /// </summary>

    public class NoFollowPagerControl : System.Web.UI.UserControl
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

        /// <summary>
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