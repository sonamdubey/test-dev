using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by Sangram Nandkhile on 06 Feb 2017
    /// Dec:- Used bike widget for popular models
    /// </summary>
    public class UsedPopularModelsInCity : System.Web.UI.UserControl
    {
        public IEnumerable<MostRecentBikes> UsedBikeModelInCityList;
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public string header { get; set; }
        public int fetchedCount { get; set; }
        public string MakeName { get; set; }
        public string CityName { get; set; }
        public string MakeMaskingName { get; set; }
        public string CityMaskingName { get; set; }

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
            if (MakeId > 0)
                BindUsedBikes();
        }

        /// <summary>
        /// Created By :subodh jain on 14 sep 2016
        /// Description : Function to bind used bikes for the makes          
        /// </summary>
        protected void BindUsedBikes()
        {
            BindUsedBikeModelInCity objUsedBikeModelCity = new BindUsedBikeModelInCity();
            if (CityId > 0)
            {
                UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, CityId, TopCount);
                if (UsedBikeModelInCityList != null)
                {
                    fetchedCount = UsedBikeModelInCityList.Count();
                }
            }
        }
    }
}