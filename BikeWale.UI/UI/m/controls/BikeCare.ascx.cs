using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By:- Subodh Jain 08 nov 2016
    /// Summart :- Tips And Advice widget
    /// </summary>
    public class BikeCare : System.Web.UI.UserControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public IEnumerable<ArticleSummary> objArticleList;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindBikeCareControl objTipsAndAdvice = new BindBikeCareControl();
            if (objTipsAndAdvice != null)
            {
                objTipsAndAdvice.TotalRecords = TotalRecords;
                objTipsAndAdvice.MakeId = MakeId;
                objTipsAndAdvice.ModelId = ModelId;
                objArticleList = objTipsAndAdvice.MaintainanceTips();
                if (objArticleList != null)
                    FetchedRecordsCount = objArticleList.Count();
            }
        }
    }
}