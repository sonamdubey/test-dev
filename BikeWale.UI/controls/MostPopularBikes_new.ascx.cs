﻿using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public partial class MostPopularBikes_new : System.Web.UI.UserControl
    {
        public Repeater rptMostPopularBikes;

        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MostPopularBikes();
        }

        private void MostPopularBikes()
        {
            BindMostPopularBikesControl objPop = new BindMostPopularBikesControl();
            objPop.totalCount = this.totalCount;
            objPop.makeId = this.makeId;
            objPop.BindMostPopularBikes(rptMostPopularBikes);
            this.FetchedRecordsCount = objPop.FetchedRecordsCount;
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
            rptMostPopularBikes.DataSource = null;
            rptMostPopularBikes.Dispose();

            base.Dispose();
        }
    }
}