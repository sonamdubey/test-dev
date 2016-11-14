using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Text;
namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By:-Subodh Jain 12 Nov 2016
    /// Summary :- Detaile Page for TipsAndAdvice
    /// </summary>
    public class ViewBikeCare : System.Web.UI.Page
    {
        DetailPageBikeCare objDetailBikeCare;

        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty, pageDescription = String.Empty, pageKeywords = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected ArticlePageDetails objTipsAndAdvice;
        public StringBuilder bikeTested;
        protected IEnumerable<ModelImage> objImg = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DetailBikeCare();
        }
        private void DetailBikeCare()
        {
            objDetailBikeCare = new DetailPageBikeCare();
            objTipsAndAdvice = objDetailBikeCare.objTipsAndAdvice;
            objImg = objDetailBikeCare.objImg;
            pageTitle = objDetailBikeCare.title;
            pageDescription = objDetailBikeCare.description;
            pageKeywords = objDetailBikeCare.keywords;
            displayDate = objDetailBikeCare.displayDate;
            bikeTested = objDetailBikeCare.bikeTested;
            canonicalUrl = objDetailBikeCare.canonicalUrl;


        }


    }
}