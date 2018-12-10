using System;
using System.Web;
using System.Web.UI;
using System.IO;
using Carwale.UI.Common;

namespace Uploadify
{
    public class Upload_ : Page 
	{       
     	protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler(Page_Load);
		}
		
		void Page_Load(object Sender, EventArgs e)
		{
			if(!IsPostBack)
			{
								
			}
		}
	}
}
