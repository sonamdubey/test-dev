using Bikewale.Controls;
using Bikewale.Entities.Pager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    public class VideoCategories : System.Web.UI.Page
    {
        protected LinkPagerControl repeaterPager;

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
            // for RepeaterPager
            PagerOutputEntity pagerOutput = new PagerOutputEntity();
            repeaterPager.PagerOutput = pagerOutput;
            repeaterPager.CurrentPageNo = 4;
            repeaterPager.TotalPages = 60;
        }
    }
}