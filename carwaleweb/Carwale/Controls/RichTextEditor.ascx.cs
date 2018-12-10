using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Carwale.UI.Controls
{
    public class RichTextEditor : UserControl
    {
        protected TextBox txtContent;

        public string Text
        {
            get
            {
                string text = txtContent.Text;
                //replace src="../ with src="/
                text = text.Replace("src=\"../", "src=\"/").Replace("href=\"../", "href=\"/");
                return text;
            }
            set { txtContent.Text = value; }
        }

        public int Rows
        {
            set { txtContent.Rows = value; }
        }

        public int Cols
        {
            set { txtContent.Columns = value; }
        }

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
            Trace.Warn("RichTextEditor: Loading...");

            if (!IsPostBack)
            {

            }
        }

    } //class

}//namespace	