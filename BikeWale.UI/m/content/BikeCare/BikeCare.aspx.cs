using Bikewale.BindViewModels.Webforms.ServiceCenter;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Mobile.Controls;
using System;

namespace Bikewale.Mobile.Content
{
    public class BikeCare : System.Web.UI.Page
    {
        BikeCareModels objBikeCare = null;
        public LinkPagerControl ctrlPager;
        protected uint makeId, modelId;
        public string pgPrevUrl = string.Empty, pgNextUrl = string.Empty, pgTitle = string.Empty, pgDescription = string.Empty, pgKeywords = string.Empty;
        protected CMSContent objArticleList;
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

                objBikeCare.BindLinkPager(ctrlPager);
                objArticleList = objBikeCare.objArticleList;
                pgTitle = objBikeCare.title;
                pgDescription = objBikeCare.description;
                pgKeywords = objBikeCare.keywords;
                pgPrevUrl = objBikeCare.prevUrl;
                pgNextUrl = objBikeCare.nextUrl;

            }
        }
    }
}