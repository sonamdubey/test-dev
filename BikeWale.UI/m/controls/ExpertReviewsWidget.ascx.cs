﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Mobile.Controls
{
    public class ExpertReviewsWidget : System.Web.UI.UserControl
    {
        protected Repeater rptExpertReviews;

        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindExpertReviewsControl ber = new BindExpertReviewsControl();
            ber.TotalRecords = this.TotalRecords;
            ber.MakeId = this.MakeId;
            ber.ModelId = this.ModelId;

            ber.BindExpertReviews(rptExpertReviews);

            this.FetchedRecordsCount = ber.FetchedRecordsCount;
        }
    }
}