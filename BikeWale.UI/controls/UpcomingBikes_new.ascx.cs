using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public partial class UpcomingBikes_new : System.Web.UI.UserControl
    {
        public Repeater rptUpcomingBikes;

        public  int sortBy { get; set; }
        public  int pageSize { get; set; }
        public  int? MakeId { get; set; }
        public  int? ModelId { get; set; }
        public  int? curPageNo { get; set; }
        public  int FetchedRecordsCount { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpcomingBikes();
        }

        private void UpcomingBikes()
        {
            BindUpcomingBikesControl.sortBy = this.sortBy;
            BindUpcomingBikesControl.MakeId = this.MakeId;
            BindUpcomingBikesControl.ModelId = this.ModelId;
            BindUpcomingBikesControl.pageSize = this.pageSize;

            BindUpcomingBikesControl.BindUpcomingBikes(rptUpcomingBikes); 
            this.FetchedRecordsCount = BindUpcomingBikesControl.FetchedRecordsCount;
        }
    }
}