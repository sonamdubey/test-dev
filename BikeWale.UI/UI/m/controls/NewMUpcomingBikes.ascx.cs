using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 June 2016
    /// Description :   New Upcoming Bikes Widget for Make Page
    /// </summary>
    public class NewMUpcomingBikes : System.Web.UI.UserControl
    {
        public Repeater rptUpcomingBikes;

        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpcomingBikes();
        }

        /// <summary>
        ///  Author : Sushil Kumar 
        ///  Created On : 23rd June 2016
        ///  Description : Bind upcoming bikes o the repeater for the widget
        /// </summary>
        private void UpcomingBikes()
        {
            BindUpcomingBikesControl objUpcoming = new BindUpcomingBikesControl();
            objUpcoming.sortBy = this.sortBy;
            objUpcoming.MakeId = this.MakeId;
            objUpcoming.ModelId = this.ModelId;
            objUpcoming.pageSize = this.pageSize;

            objUpcoming.BindUpcomingBikes(rptUpcomingBikes);
            this.FetchedRecordsCount = objUpcoming.FetchedRecordsCount;
        }

        public override void Dispose()
        {
            rptUpcomingBikes.DataSource = null;
            rptUpcomingBikes.Dispose();

            base.Dispose();
        }
    }
}