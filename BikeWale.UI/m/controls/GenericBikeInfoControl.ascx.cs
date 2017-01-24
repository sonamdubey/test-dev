using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using System;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description : To bind mobile generic bike info control 
    /// Modified By : Aditi Srivastava on 23 Jan 2017
    /// Summary     : Added flags for upcoming and discontinued bikes
    /// </summary>
    public class GenericBikeInfoControl : System.Web.UI.UserControl
    {
        public uint ModelId { get; set; }
        protected GenericBikeInfo bikeInfo { get; set; }
        protected string bikeUrl = string.Empty, bikeName = string.Empty;
        protected PQSourceEnum pqSource;
        protected bool IsUpcoming { get; set; }
        protected bool IsDiscontinued { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModelId > 0)
            {
                BindGenericBikeInfo genericBikeInfo = new BindGenericBikeInfo();
                genericBikeInfo.ModelId = ModelId;
                bikeInfo = genericBikeInfo.GetGenericBikeInfo();
                if (bikeInfo != null)
                {
                    if (bikeInfo.Make != null)
                        bikeUrl = string.Format("/m/{0}-bikes/{1}/", bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName);
                    if (bikeInfo.Model != null)
                        bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                    pqSource = PQSourceEnum.Mobile_GenricBikeInfo_Widget;
                    // for photos page
                    bikeInfo.PhotosCount = 0; bikeInfo.VideosCount = 0; 
                    IsUpcoming = genericBikeInfo.IsUpcoming;
                    IsDiscontinued = genericBikeInfo.IsDiscontinued;
                };
            }

        }
    }
}