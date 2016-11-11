using Bikewale.BindViewModels.Webforms.ServiceCenter;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Mobile.Controls;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Content.MaintenanceTips
{
    public class BikeCare : System.Web.UI.Page
    {
        BikeCareModels objBikeCare = null;
        public LinkPagerControl ctrlPager;
        protected uint makeId, modelId;
        private const int _pageSize = 10;
        int _curPageNo = 1;
        protected IEnumerable<ArticleSummary> objArticleList;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            BikeCare();

        }
        private void BikeCare()
        {
            objBikeCare = new BikeCareModels();
            if (objBikeCare != null)
            {
                objBikeCare.BindLinkPager(ctrlPager);
            }
        }





    }
}