﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 30th Aug 2016
    /// Summary: User control for binding similar used bikes
    /// </summary>
    public class SimilarUsedBikes : System.Web.UI.UserControl
    {
        public Repeater rptUsedBikes;
        public IEnumerable<BikeDetailsMin> similarBikeList = null;
        public BindSimilarUsedBikes usedBikeViewModel;
        public BindOtherUsedBikesForCity otherUsedBikeViewModel;
        public ushort TopCount { get; set; }
        public ushort FetchedRecordsCount { get; set; }
        public bool ShowOtherBikes { get; set; }

        public string CityName { get; set; }
        public string CityMaskingName { get; set; }

        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }

        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }

        public string BikeName { get; set; }

        public uint InquiryId { get; set; }
        public uint CityId { get; set; }
        public uint ModelId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindOtherUsedBikes();
        }
        /// <summary>
        /// Bind similar bikes widget
        /// </summary>
        private void BindOtherUsedBikes()
        {
            try
            {
                usedBikeViewModel = new BindSimilarUsedBikes();
                similarBikeList = usedBikeViewModel.BindUsedSimilarBikes(InquiryId, CityId, ModelId, TopCount);
                FetchedRecordsCount = Convert.ToUInt16(similarBikeList.Count());
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " SimilarUsedBikes.BindSimilarBikes");
                objErr.SendMail();
            }
        }
    }
}