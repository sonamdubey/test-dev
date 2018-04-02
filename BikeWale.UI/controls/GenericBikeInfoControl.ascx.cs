using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;

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

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Modified  By :- Sajal Gupta 10 Feb 2017
        /// Summary :- BikeInfo Slug details
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
                    var versionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(new List<int> { bikeInfo.VersionId }).GetEnumerator();
                    if (versionMinSpecs.MoveNext())
                    {
                        bikeInfo.MinSpecsList = versionMinSpecs.Current.MinSpecsList;
                    }
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
                };
            }
        }
    }
}