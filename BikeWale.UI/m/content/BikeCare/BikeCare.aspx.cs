using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Mobile.Controls;
using System;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By:- Subodh jain 11 Nov 2016
    /// Summary :- Bike Care Landing page
    /// </summary>
    public class BikeCare : System.Web.UI.Page
    {
        BikeCareModels objBikeCare = null;
        public LinkPagerControl ctrlPager;
        protected uint makeId, modelId, totalArticles;
        public string pgPrevUrl = string.Empty, pgNextUrl = string.Empty, pgTitle = string.Empty, pgDescription = string.Empty, pgKeywords = string.Empty;
        protected CMSContent objArticleList;
        public int startIndex = 0, endIndex = 0;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BikeCareTips();
        }
        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page Binding
        /// </summary>
        private void BikeCareTips()
        {
            objBikeCare = new BikeCareModels();
            if (objBikeCare != null)
            {
                try
                {

                    objBikeCare.BindLinkPager(ctrlPager);
                    objArticleList = objBikeCare.objArticleList;
                    pgTitle = objBikeCare.title;
                    pgDescription = objBikeCare.description;
                    pgKeywords = objBikeCare.keywords;
                    pgPrevUrl = objBikeCare.prevUrl;
                    pgNextUrl = objBikeCare.nextUrl;
                    totalArticles = objBikeCare.totalRecords;
                    startIndex = objBikeCare.startIndex;
                    endIndex = objBikeCare.endIndex;
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikeCare.BikeCareTips");
                    objErr.SendMail();
                }
            }
        }
    }
}