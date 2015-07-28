using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace BikeWaleOpr.Controls
{		
	public class RichTextEditor : UserControl 
	{
		protected TextBox txtContent;
		
		public string Text 
		{
			get { return txtContent.Text; }
			set { txtContent.Text = value; }
		}
		
		public int Rows
		{
			set {txtContent.Rows = value;}
		}
		
		public int Cols
		{
			set{txtContent.Columns = value;}
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
			Trace.Warn("RichTextEditor: Loading..." );
			
			if( !IsPostBack )
			{
				
			}			
		}

	} //class
	
}//namespace	