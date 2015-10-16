using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public class NewLaunchedBikes_new : System.Web.UI.UserControl
    {
        public Repeater rptNewLaunchedBikes;

        public  int pageSize { get; set; }
        public  int? curPageNo { get; set; }
        public  int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }

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
            BindNewLaunchedBikesControl objNewLaunch = new BindNewLaunchedBikesControl();
            objNewLaunch.pageSize = this.pageSize;
            objNewLaunch.curPageNo = this.curPageNo;
            objNewLaunch.BindNewlyLaunchedBikes(rptNewLaunchedBikes);
            this.FetchedRecordsCount = objNewLaunch.FetchedRecordsCount;
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            string price = String.Empty;
            if (estimatedPrice != null)
            {
                price = Bikewale.Utility.Format.FormatPrice(estimatedPrice.ToString());
                if (price == "N/A")
                {
                    price = "Price unavailable";
                }
                else
                {
                    price += " <span class='font16'> Onwards</span>";
                }
            }
            return price;
        }

        public override void Dispose()
        {
            rptNewLaunchedBikes.DataSource = null;
            rptNewLaunchedBikes.Dispose();

            base.Dispose();
        }
    }
}