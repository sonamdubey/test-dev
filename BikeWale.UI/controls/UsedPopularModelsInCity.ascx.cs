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
    /// Created Date : 06 Feb 2016
    /// Desc : recently uploaded used models
    /// </summary>
    public class UsedPopularModelsInCity : UserControl
    {
        public IEnumerable<MostRecentBikes> UsedBikeModelInCityList;
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public bool IsAd { get; set; }
        public string MakeName { get; set; }
        public string CityName { get; set; }
        public string MakeMaskingName { get; set; }
        public string AdId { get; set; }

        public int FetchedRecordsCount;
        public string modelMaskingName = string.Empty;
        public string CityMaskingName { get; set; }
        protected short masterGrid = 12;
        protected short childGrid = 4;

        protected IEnumerable<MostRecentBikes> objUsedBikes = null;

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
            if (isValidData())
                BindUsedBikes();
            if (IsAd)
            {
                masterGrid = 8;
                childGrid = 6;
            }
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;
            if (MakeId < 0 || ModelId < 0)
            {
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 15/09/2016
        /// Description : Function to bind used bikes for the makes/models.          
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added page heading null check if null add default heading
        /// </summary>
        /// <returns></returns>
        protected void BindUsedBikes()
        {
            BindUsedBikeModelInCity objUsedBikeModelCity = new BindUsedBikeModelInCity();
            if (CityId > 0)
            {
                UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, (uint)CityId, TopCount);
            }
            if (UsedBikeModelInCityList != null && UsedBikeModelInCityList.Count() > 0)
                FetchedRecordsCount = UsedBikeModelInCityList.Count();

            //BindUsedBikesControl objUsed = new BindUsedBikesControl();
            //objUsed.MakeId = MakeId;
            //objUsed.ModelId = ModelId;
            //objUsed.TopCount = TopCount;
            //objUsed.CityId = CityId;
            //objUsedBikes = objUsed.FetchUsedBikes(TopCount, MakeId, ModelId, CityId);
            //if (objUsedBikes != null)
            //{
            //    MostRecentBikes firstBike = objUsedBikes.FirstOrDefault();
            //    if (firstBike != null)
            //    {
            //        makeName = firstBike.MakeName;
            //        makeMaskingName = firstBike.MakeMaskingName;
            //    }
            //    FetchedRecordsCount = Convert.ToUInt16(objUsedBikes.Count());
            //}
            //if (string.IsNullOrEmpty(pageHeading))
            //    pageHeading = string.Format("Used {0} bikes in {1}", makeName, cityName);
        }
    }
}