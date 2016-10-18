﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class AlternativeBikes : System.Web.UI.UserControl
    {
        public Repeater rptAlternateBikes;
        public int VersionId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int PQSourceId { get; set; }
        public int cityId { get; set; }
        private int _topCount = 6;
    
        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public int? Deviation { get; set; }

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
            objAlt.cityId = cityId > 0 ? cityId : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
             objAlt.BindAlternativeBikes(rptAlternateBikes);

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