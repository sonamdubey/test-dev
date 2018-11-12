using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description : To bind desktop generic bike info control 
    /// Modified By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details
    /// </summary>
    public class GenericBikeInfoControl : System.Web.UI.UserControl
    {
        public uint ModelId { get; set; }
        protected GenericBikeInfo bikeInfo { get; set; }
        protected string bikeUrl = string.Empty, bikeName = string.Empty;
        protected PQSourceEnum pqSource;
        protected bool IsUpcoming { get; set; }
        protected bool IsDiscontinued { get; set; }
        public uint CityId { get; set; }
        public BikeInfoTabType PageId { get; set; }
        protected CityEntityBase cityDetails;
        public uint TabCount { get; set; }
        protected float Rating { get; set; }
        protected UInt16 RatingCount { get; set; }
        protected UInt16 UserReviewCount { get; set; }
        protected BikeSeriesEntity Series { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Modified  By :- Sajal Gupta 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// Modified by : Snehal Dange on 6th Nov 2018
        /// Desc : Binded series data
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CityId > 0)
                cityDetails = new CityHelper().GetCityById(CityId);
            if (ModelId > 0)
            {
                BindBikeInfo genericBikeInfo = new BindBikeInfo();
                genericBikeInfo.ModelId = ModelId;
                genericBikeInfo.CityId = CityId;
                genericBikeInfo.PageId = PageId;
                genericBikeInfo.TabCount = TabCount;
                genericBikeInfo.cityDetails = cityDetails;
                bikeInfo = genericBikeInfo.GetBikeInfo();
                if (bikeInfo != null)
                {
                    if (bikeInfo.Make != null && bikeInfo.Model != null)
                    {
                        bikeUrl = string.Format("{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName));
                        bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                    }
                    pqSource = PQSourceEnum.Mobile_GenricBikeInfo_Widget;
                    IsUpcoming = genericBikeInfo.IsUpcoming;
                    IsDiscontinued = genericBikeInfo.IsDiscontinued;
                    Rating = genericBikeInfo.Rating;
                    RatingCount = genericBikeInfo.RatingCount;
                    UserReviewCount = genericBikeInfo.UserReviewCount;
                    if (bikeInfo.Make != null && bikeInfo.Make.MakeId > 0)
                    {
                        Series = genericBikeInfo.BindSeriesData(Convert.ToUInt32(bikeInfo.Make.MakeId), ModelId);
                    }

                };

            }
        }



    }
}