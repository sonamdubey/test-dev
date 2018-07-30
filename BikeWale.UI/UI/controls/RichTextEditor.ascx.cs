using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class RichTextEditor : System.Web.UI.UserControl
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

        /*
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        } */
    }
}