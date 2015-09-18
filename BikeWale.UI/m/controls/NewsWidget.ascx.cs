using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Mobile.Controls
{
    public class NewsWidget : System.Web.UI.UserControl
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
            BindNewsControl.TotalRecords = this.TotalRecords;
            BindNewsControl.MakeId = this.MakeId;
            BindNewsControl.ModelId = this.ModelId;

            BindNewsControl.BindNews(rptNews);

            this.FetchedRecordsCount = BindNewsControl.FetchedRecordsCount;
        }
    }
}