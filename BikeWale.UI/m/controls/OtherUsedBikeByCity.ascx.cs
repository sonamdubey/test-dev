using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 30th Aug 2016
    /// Summary: Bind view model for binding similar used bikes
    /// </summary>
    public class OtherUsedBikeByCity : System.Web.UI.UserControl
    {
        public Repeater rptUsedBikes;
        public IEnumerable<OtherUsedBikeDetails> otherBikesinCity = null;
        public BindOtherUsedBikesForCity viewModel;
        public ushort TopCount { get; set; }
        public ushort FetchedRecordsCount { get; set; }

        //public string CityName { get; set; }
        //public string CityMaskingName { get; set; }

        public uint ModelId { get; set; }
        //public string ModelName { get; set; }
        //public string ModelMaskingName { get; set; }

        //public string MakeName { get; set; }
        //public string MakeMaskingName { get; set; }

        public uint CityId { get; set; }
        public uint InquiryId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSimilarBikes();
        }
        /// <summary>
        /// Bind similar bikes widget
        /// </summary>
        private void BindSimilarBikes()
        {
            try
            {
                viewModel = new BindOtherUsedBikesForCity();
                otherBikesinCity = viewModel.GetOtherBikesByCityId(InquiryId, CityId, TopCount);
                FetchedRecordsCount = Convert.ToUInt16(otherBikesinCity.Count());
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " OtherUsedBikeByCity.BindSimilarBikes");
                objErr.SendMail();
            }
        }
    }
}