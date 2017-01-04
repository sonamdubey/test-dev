﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using System;
using System.Linq;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by Sajal Gupta on 20-01-2017
    /// Desc : Class to bind brand india widget.
    /// </summary>
    public class UsedBikesCityCountByBrand : System.Web.UI.UserControl
    {
        public BindUsedBikesInCityCount viewModel = null;
        public uint MakeId { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public uint FetchedCount { get; set; }
        public ushort TopCount { get; set; }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MakeId > 0)
                BindCountList();
        }

        /// <summary>
        /// Created by Sajal Gupta on 20-01-2017
        /// Desc : Function to bind brand india widget.
        /// </summary>
        private void BindCountList()
        {
            try
            {
                viewModel = new BindUsedBikesInCityCount();

                viewModel.BindUsedBikesInCityCountByMake(MakeId, TopCount);

                if (viewModel.bikesCountCityList != null)
                {
                    FetchedCount = (uint)viewModel.bikesCountCityList.Count();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Controls.UsedBikesCityCountByBrand.BindCountList()");
            }
        }
    }
}