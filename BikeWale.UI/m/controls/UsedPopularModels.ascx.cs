using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by Sangram Nandkhile on 03 Feb 2016
    /// Dec:- Used bike widget to show popular models by Make
    /// </summary>
    public class UsedPopularModels : System.Web.UI.UserControl
    {
        public IEnumerable<MostRecentBikes> UsedBikeModelInCityList;

        public uint MakeId { get; set; }
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public int FetchedRecordsCount;
        public string MakeName { get; set; }
        public string modelName = string.Empty;
        public string CityName { get; set; }
        public string MakeMaskingName { get; set; }
        public string modelMaskingName = string.Empty;
        public string CityMaskingName { get; set; }
        public string pageHeading = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindUsedBikes();
        }

        /// <summary>
        /// Created By : Sangram Nandkhile 
        /// Description : Function to bind used bikes for the makes/models.          
        /// </summary>
        protected void BindUsedBikes()
        {
            BindUsedBikeModelInCity objUsedBikeModelCity = new BindUsedBikeModelInCity();
            if (CityId > 0)
            {
                UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, (uint)CityId, TopCount);
            }
            else
            {
                UsedBikeModelInCityList = objUsedBikeModelCity.GetPopularUsedModelsByMake(MakeId, TopCount);
                CityName = "India";
            }
            if (UsedBikeModelInCityList != null && UsedBikeModelInCityList.Count() > 0)
                FetchedRecordsCount = UsedBikeModelInCityList.Count();
        }
    }
}