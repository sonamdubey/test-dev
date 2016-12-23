using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Generic
{
    public class BikeListing : System.Web.UI.Page
    {
        protected BestBikes ctrlBestBikes;
        protected EnumBestBikeArticleType thisPage;

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
            ParseQueryString();
            if (thisPage != null)
                ctrlBestBikes.CurrentPage = thisPage;
        }

        private void ParseQueryString()
        {
            string page = Request.QueryString["page"];
            if (!string.IsNullOrEmpty(page))
            {
                try
                {
                    thisPage = (EnumBestBikeArticleType)System.Enum.Parse(typeof(EnumBestBikeArticleType), page, true);
                }
                catch
                {
                    // 404
                }
            }
        }
    }
}