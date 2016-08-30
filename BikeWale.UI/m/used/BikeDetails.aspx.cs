using Bikewale.m.controls;
using System;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BikeDetails : System.Web.UI.Page
    {
        protected SimilarUsedBikes similarUsedBikes;

        string profileId = "S42667";
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}