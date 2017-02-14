using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;

namespace Bikewale.Controls
{

    /// <summary>
    /// Created By : Sajal Gupta on 10-02-2017
    /// Description : To bind mobile generic bike info control 
    /// </summary>
    public class MinGenericBikeInfoControl : System.Web.UI.UserControl
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
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Created By :- Sajal Gupta 10 Feb 2017
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
                    if (bikeInfo.Make != null && bikeInfo.Model != null)
                    {
                        bikeUrl = string.Format(Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName));
                        bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                    }
                    pqSource = PQSourceEnum.Mobile_GenricBikeInfo_Widget;
                    IsUpcoming = genericBikeInfo.IsUpcoming;
                    IsDiscontinued = genericBikeInfo.IsDiscontinued;
                };

            }

        }

    }
}