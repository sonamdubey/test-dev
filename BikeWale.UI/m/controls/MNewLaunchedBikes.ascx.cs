using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.controls
{
    public class MNewLaunchedBikes : System.Web.UI.UserControl
    {
        public Repeater rptNewLaunchedBikes;

        public int pageSize { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            NewLaunchedBikes();
        }

        private void NewLaunchedBikes()
        {
            BindNewLaunchedBikesControl.pageSize = this.pageSize;
            BindNewLaunchedBikesControl.curPageNo = this.curPageNo;
            BindNewLaunchedBikesControl.BindNewlyLaunchedBikes(rptNewLaunchedBikes);
            this.FetchedRecordsCount = BindNewLaunchedBikesControl.FetchedRecordsCount;
        }
    }
}