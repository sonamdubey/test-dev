﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Controls
{
    public class ExpertReviews : System.Web.UI.UserControl
    {
        protected Repeater rptExpertReviews;
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
            BindExpertReviewsControl.TotalRecords = this.TotalRecords;
            BindExpertReviewsControl.MakeId = this.MakeId;
            BindExpertReviewsControl.ModelId = this.ModelId;

            BindExpertReviewsControl.BindExpertReviews(rptExpertReviews);

            this.FetchedRecordsCount = BindExpertReviewsControl.FetchedRecordsCount;
        }
    }
}