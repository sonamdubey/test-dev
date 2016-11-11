using Bikewale.BindViewModels.Webforms.ServiceCenter;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Mobile.Controls;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Content
{
    public class BikeCare : System.Web.UI.Page
    {
        BikeCareModels objBikeCare = null;
        public LinkPagerControl ctrlPager;
        protected uint makeId, modelId;

        protected IEnumerable<ArticleSummary> objArticleList;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            BikeCareTips();

        }
        private void BikeCareTips()
        {
            objBikeCare = new BikeCareModels();
            if (objBikeCare != null)
            {
                objArticleList = objBikeCare.BikeCare();
                objBikeCare.BindLinkPager(ctrlPager);

            }
        }
    }
}