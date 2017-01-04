﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using System;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by Sajal Gupta on 20-01-2017
    /// Desc : Class to bind mpdel india widget.
    /// </summary>
    public class UsedBikesCityCountByModel : System.Web.UI.UserControl
    {
        public BindUsedBikesInCityCount viewModel = null;
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public uint FetchedCount { get; set; }
        public uint Count { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModelId > 0)
                BindCountList();
        }

        /// <summary>
        /// Created by Sajal Gupta on 20-01-2017
        /// Desc : Function to bind model india widget.
        /// </summary>
        private void BindCountList()
        {
            try
            {
                viewModel = new BindUsedBikesInCityCount();

                if (Count == 0)
                    Count = 6;

                viewModel.BindUsedBikesInCityCountByModel(ModelId, Count);

                if (viewModel.bikesCountCityList != null)
                {
                    FetchedCount = (uint)viewModel.bikesCountCityList.Count();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Mobile.Controls.UsedBikesCityCountByModel.BindCountList()");
            }
        }
    }
}
