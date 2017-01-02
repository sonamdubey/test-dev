
using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Controls
{
    public class UsedBikeByModels : System.Web.UI.UserControl
    {
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used Bike By Models 
        /// </summary>
        protected IEnumerable<MostRecentBikes> UsedBikeModelInCityList;
        public uint MakeId { get; set; }
        public uint CityId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string CityMaskingName { get; set; }
        public string CityName { get; set; }
        public uint TopCount { get; set; }
        public int FetchCount;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used Bike By Models page_load 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Device Detection
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            Bindwidget();

        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used Bike By Models widget
        /// </summary>
        private void Bindwidget()
        {
            BindUsedBikeModelInCity objUsedBikeModelCity = new BindUsedBikeModelInCity();
            UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, CityId, TopCount);
            if (UsedBikeModelInCityList != null && UsedBikeModelInCityList.Count() > 0)
                FetchCount = UsedBikeModelInCityList.Count();

        }

    }
}