﻿using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class NewLaunchedBikes_new : System.Web.UI.UserControl
    {
        public Repeater rptNewLaunchedBikes;

        public  int pageSize { get; set; }
        public  int? curPageNo { get; set; }
        public  int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }
        public int PQSourceId { get; set; }

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
            objNewLaunch.currentPageNo = this.curPageNo;
            objNewLaunch.BindNewlyLaunchedBikes(rptNewLaunchedBikes);
            this.FetchedRecordsCount = objNewLaunch.FetchedRecordsCount;
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            if (estimatedPrice != null && Convert.ToInt32(estimatedPrice) > 0)
            {
                return String.Format("<span class='bwsprite inr-xl'></span> <span class='font22'>{0}</span><span class='font16'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font22'>Price Unavailable</span>";
            }
        }

        public override void Dispose()
        {
            rptNewLaunchedBikes.DataSource = null;
            rptNewLaunchedBikes.Dispose();

            base.Dispose();
        }
    }
}