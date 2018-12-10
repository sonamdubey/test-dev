
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
    public class ReviewRating : UserControl
    {

        protected Label lblImageRating;
        protected HtmlGenericControl reviewRating;
        protected HtmlGenericControl notRatedYet;

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                double _tempDouble;
                lblImageRating.Text = CommonOpn.GetRateImage(Convert.ToDouble(ReviewRate));
                double.TryParse(ReviewCount, out _tempDouble);
                if (_tempDouble > 0)
                {
                    reviewRating.Visible = true;
                    notRatedYet.Visible = false;
                }
                else
                {
                    reviewRating.Visible = false;
                    notRatedYet.Visible = true;
                }
            }
        }

        string _reviewRate = "0";
        public string ReviewRate
        {
            get { return _reviewRate; }
            set { _reviewRate = value; }
        }

        string _reviewCount = "0";
        public string ReviewCount
        {
            get { return _reviewCount; }
            set { _reviewCount = value; }
        }

        string _modelLink = string.Empty;
        public string ModelLink
        {
            get { return _modelLink; }
            set { _modelLink = value; }
        }

        string _modelId;
        public string ModelId
        {
            get { return _modelId; }
            set { _modelId = value; }
        }

    }//class
}//namespace

