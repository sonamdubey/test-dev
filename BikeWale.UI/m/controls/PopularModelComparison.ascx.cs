﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sajal Gupta on 12/09/2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// </summary>
    public class PopularModelComparison : System.Web.UI.UserControl
    {

        public uint fetchedCount { get; set; }
        protected ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;
        public uint versionId { get; set; }
        public string versionName { get; set; }
        public int? cityid { get; set; }
        public ushort TopCount { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPopularCompareBikes();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 12/09/2016
        /// Desc       : To bind similar bikes
        /// </summary>
        private void BindPopularCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
            objAlt.cityid = cityid.HasValue && cityid > 0 ? cityid.Value : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
            objSimilarBikes = objAlt.BindPopularCompareBikes(versionId.ToString(), TopCount);
            fetchedCount = objAlt.FetchedRecordsCount;
        }

    }
}