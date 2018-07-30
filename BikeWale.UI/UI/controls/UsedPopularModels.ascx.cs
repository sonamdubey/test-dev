using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Bikewale.Controls
{
    /// <summary>
    /// Author : Sangram Nandkhile
    /// Created Date : 02 Feb 2017
    /// Desc : recently uploaded used bikes control
    /// </summary>
    public class UsedPopularModels : UserControl
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
                UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, CityId, TopCount);
            }
            else
            {
                UsedBikeModelInCityList = objUsedBikeModelCity.GetPopularUsedModelsByMake(MakeId, TopCount);
                CityName = "India";
            }
            if (UsedBikeModelInCityList != null && UsedBikeModelInCityList.Any())
                FetchedRecordsCount = UsedBikeModelInCityList.Count();
        }
    }
}