using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using Ajax;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Serialization;

namespace BikeWaleOpr.EditCms
{
	public class Bike {
		[XmlElement("license")]
		public string License;
		[XmlElement("color")]
		public string BikeColor;

		public Bike() {}

		public Bike(string license, string color) {
			License = license;
			BikeColor = color;
		}
	}
	
	[XmlRoot("bikesRoot")]
	public class BikeArray {
		[XmlArray("bikes")]
		[XmlArrayItem("bike")]
		public Bike[] bikes = new Bike[2];
	}

	public class TestSerial : Page
	{		
		protected Button btnSave;
		protected TextBox txtResult;
							 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			Bike bike1 = new Bike("1234","Black");
			Bike bike2 = new Bike("4321","Blue");
			
			Bike[] bikes = new Bike[2];
			BikeArray bikeArray = new BikeArray();
			bikeArray.bikes[0] = bike1;
			bikeArray.bikes[1] = bike2;
			
			//Serialize
			StringWriter sw = new StringWriter();
			XmlSerializer s = new XmlSerializer(typeof(BikeArray));
			s.Serialize(sw,bikeArray);
			//this.txtOutput.Text = sw.ToString();

			txtResult.Text = sw.ToString();
		}
	}
}			