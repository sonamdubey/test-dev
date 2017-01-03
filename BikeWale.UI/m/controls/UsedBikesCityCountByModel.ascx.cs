﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using System;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    public class UsedBikesCityCountByModel : System.Web.UI.UserControl
    {
        public BindUsedBikesInCityCount viewModel = null;
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public uint fetchedCount;


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
                viewModel.BindUsedBikesInCityCountByModel(ModelId);

                if (viewModel.bikesCountCityList != null)
                {
                    fetchedCount = (uint)viewModel.bikesCountCityList.Count();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Mobile.Controls.UsedBikesCityCountByModel.BindCountList()");
            }
        }
    }
}
