using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 8 Nov 2016
    /// Summay     : get and bind data for upcoming bikes for content pages 
    /// </summary>
    public class UpcomingBikesCMS : System.Web.UI.UserControl
    {
        public Repeater rptUpcomingBikes;
        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpcomingBikes();
        }

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
    }
}