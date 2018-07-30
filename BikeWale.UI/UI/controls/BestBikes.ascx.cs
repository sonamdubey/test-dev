using Bikewale.Entities.GenericBikes;
using System;
using System.Globalization;
using System.Web.UI;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 23 Dec 2016
    /// Summary: To show best bike widgets
    /// </summary>
    public class BestBikes : System.Web.UI.UserControl
    {
        public EnumBikeBodyStyles? CurrentPage { get; set; }
        public string PrevMonthDate { get; set; }

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
            if (CurrentPage != null)
            {
                Control item = FindControl(Convert.ToString(CurrentPage).ToLower());
                if (item != null)
                    item.Visible = false;
            }
            DateTime prevMonth = DateTime.Now.AddMonths(-1);
            PrevMonthDate = string.Format("{0} {1}", prevMonth.ToString("MMMM", CultureInfo.InvariantCulture), prevMonth.Year);
        }
    }
}