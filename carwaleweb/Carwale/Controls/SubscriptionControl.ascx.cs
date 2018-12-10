using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;
using Carwale.UI.Common;

namespace Carwale.UI.Controls
{
    public class SubscriptionControl : UserControl
    {
        //public int _SubscriptionType = -1;
        //public string _SubscriptionHeading = string.Empty;
        //public string _SubscriptionLabel = string.Empty;
        protected Literal subscriptionHeading;
        protected Literal subscriptionLabel;
        protected int SubscriptionType = -1;
        protected int SubscriptionCategory = -1;

        public string SubscriptionHeading { get; set; }
        public string SubscriptionLabel { get; set; }
        public int subscriptionType { get; set; }
        public int subscriptionCategory { get; set; }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }


        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                subscriptionHeading.Text = SubscriptionHeading;
                subscriptionLabel.Text = SubscriptionLabel;
                SubscriptionType = subscriptionType;
                SubscriptionCategory = subscriptionCategory;
            }
        }
    }
}