using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 16 Nov 2016
    /// Summary    : To inject upcoming bikes widget for cms pages
    /// </summary>
    public class UpcomingBikesMin : System.Web.UI.UserControl
    {

        public Repeater rptUpcomingBikes;

        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string makeMaskingName, upcomingBikesLink, makeName;
        public IEnumerable<UpcomingBikeEntity> objBikeList = null;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpcomingBikes();
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 16 Nov 2016
        /// Summary    : To inject upcoming bikes widget for cms pages
        /// </summary>
        private void UpcomingBikes()
        {
            try
            {
                BindUpcomingBikesControl objUpcoming = new BindUpcomingBikesControl();
                objUpcoming.sortBy = sortBy;
                objUpcoming.MakeId = MakeId;
                objUpcoming.ModelId = ModelId;
                objUpcoming.pageSize = pageSize;
                if (String.IsNullOrEmpty(makeMaskingName))
                {
                    upcomingBikesLink = "/m/upcoming-bikes/";
                }
                else
                {
                    upcomingBikesLink = String.Format("/m/{0}-bikes/upcoming/", makeMaskingName);
                }
                objUpcoming.BindUpcomingBikes(null);
                if (objUpcoming.FetchedRecordsCount > 0)
                {
                    objBikeList = objUpcoming.objUpcomingBikes;
                    FetchedRecordsCount = objUpcoming.FetchedRecordsCount;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpcomingBikesMin.UpcomingBikes");
                
            }
        }


    }
}