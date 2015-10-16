using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.CMS;
using Bikewale.Entity.CMS.Articles;
using Bikewale.Notifications;
using Bikewale.Utility;

namespace Bikewale.Controls
{
    public class News_new : System.Web.UI.UserControl
    {
        protected Repeater rptNews;
        
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

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
            BindNewsControl objNews = new BindNewsControl();
            objNews.TotalRecords = this.TotalRecords;
            objNews.MakeId = this.MakeId;
            objNews.ModelId = this.ModelId;

            objNews.BindNews(rptNews);

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