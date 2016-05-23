using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.CMS.Articles;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class News_Widget : System.Web.UI.UserControl
    {
        protected Repeater rptNews;

        public uint TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public ArticleSummary firstPost;
        public string WidgetTitle { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindNews();
        }

        private void BindNews()
        {
            BindNewsWidget objNews = new BindNewsWidget();
            objNews.TotalRecords = this.TotalRecords;
            objNews.MakeId = this.MakeId;
            objNews.ModelId = this.ModelId;
            firstPost = objNews.BindNews(rptNews, 2);
            this.FetchedRecordsCount = objNews.FetchedRecordsCount;
        }

        public override void Dispose()
        {
            rptNews.DataSource = null;
            rptNews.Dispose();

            base.Dispose();
        }
    }
}