using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 8 Nov 2016
    /// Summay     : get and bind data for upcoming bikes for content pages 
    /// </summary>
    public class UpcomingBikesMinNew : System.Web.UI.UserControl
    {
        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int topCount { get; set; }
        public string upcomingBikesLink;
        public string makeMaskingName, makeName;
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
        /// Created By : Aditi Srivastava on 10 Nov 2016
        /// Description: set properties for upcoming bikes widget
        /// </summary>
        private void UpcomingBikes()
        {
            BindUpcomingBikesControl objUpcoming = new BindUpcomingBikesControl();
            objUpcoming.sortBy = this.sortBy;
            objUpcoming.MakeId = this.MakeId;
            objUpcoming.ModelId = this.ModelId;
            objUpcoming.pageSize = this.topCount;
            if (String.IsNullOrEmpty(makeMaskingName))
            {
                upcomingBikesLink = "/upcoming-bikes/";
            }
            else
            {
                upcomingBikesLink = String.Format("/{0}-bikes/upcoming/", makeMaskingName);
            }

            objUpcoming.BindUpcomingBikes(null);
            if (objUpcoming.FetchedRecordsCount > 0)
            {
                objBikeList = objUpcoming.objUpcomingBikes;
                this.FetchedRecordsCount = objUpcoming.FetchedRecordsCount;
            }

        }
    }
}