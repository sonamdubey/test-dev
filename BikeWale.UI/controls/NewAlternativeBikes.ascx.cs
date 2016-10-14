﻿using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Written By : Sangram Nandkhile on 24 May 2016
    /// Summary : Control to show alternative bikes with new UI
    /// </summary>
    public class NewAlternativeBikes : System.Web.UI.UserControl
    {
        public Repeater rptAlternateBikes;
        public int VersionId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int PQSourceId { get; set; }
        public string model { get; set; }
        private int _topCount = 6;
        protected string heading = string.Empty;
        public string cityName { get; set; }
        public int cityId { get; set; }
        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public int? Deviation { get; set; }
        public string WidgetTitle { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindAlternativeBikes();
        }

        private void BindAlternativeBikes()
        {
            BindAlternativeBikesControl objAlt = new BindAlternativeBikesControl();
            objAlt.VersionId = VersionId;
            objAlt.TopCount = TopCount;
            objAlt.Deviation = Deviation;
            objAlt.BindAlternativeBikes(rptAlternateBikes);
            if (PQSourceId == 47)
                heading = string.Format("Prices of {0} alternative bikes in {1}", model, cityName);
            else
                heading = string.Format("Alternative Bikes for {0}", model);
            FetchedRecordsCount = objAlt.FetchedRecordsCount;
        }

        public override void Dispose()
        {
            rptAlternateBikes.DataSource = null;
            rptAlternateBikes.Dispose();
            base.Dispose();
        }
    }
}