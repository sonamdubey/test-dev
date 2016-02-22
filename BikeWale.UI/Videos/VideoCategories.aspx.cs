using Bikewale.Controls;
using System;

namespace Bikewale.Videos
{
    public class VideoCategories : System.Web.UI.Page
    {
        protected LinkPagerControl repeaterPager;
        protected uint categoryId = 0;
        protected string make = string.Empty;
        protected string model = string.Empty;

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
            //PagerOutputEntity pagerOutput = new PagerOutputEntity();
            //repeaterPager.PagerOutput = pagerOutput;
            //repeaterPager.CurrentPageNo = 4;
            //repeaterPager.TotalPages = 60;

        }
    }
}