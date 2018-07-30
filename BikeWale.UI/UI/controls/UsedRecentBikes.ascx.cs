
using Bikewale.BindViewModels.Controls;
using Bikewale.Notifications;
using System;
using System.Web.UI.WebControls;
namespace Bikewale.Controls
{
    /// <summary>
    /// Created by : Subodh Jain on 20 oct 2016
    /// Summary: Bind view model for bind recently uploaded bikes
    /// </summary>
    public class UsedRecentBikes : System.Web.UI.UserControl
    {
        public Repeater rptUsedBikes;
        public BindUsedRecentBikes viewModel;
        public ushort TopCount { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string WidgetTitle { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                viewModel = new BindUsedRecentBikes(TopCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Page_Load");
                
            }
        }

    }
}