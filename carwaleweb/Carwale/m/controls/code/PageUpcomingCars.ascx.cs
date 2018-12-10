using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using MobileWeb.DataLayer;
using Carwale.Interfaces.CarData;
using Carwale.Service;

namespace MobileWeb.Controls
{
	public class PageUpcomingCars : UserControl
	{
		private int _pageSize = 10, _pageNo = 1;
        private int _totalRecords = 0;
		protected Repeater rptUpcomingCars;
		
		public int PageSize 
		{ 
			get {return _pageSize;} 
			set {_pageSize=value; } 
		}
		
		public int PageNo 
		{ 
			get {return _pageNo;} 
			set {_pageNo=value; } 
		}

        public int TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value; }
        }
        protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
		}
		
		public void BindPage()
		{
            ICarModels _carModelsBL = UnityBootstrapper.Resolve<ICarModels>();
            var results = _carModelsBL.GetUpcomingCarModels(new Carwale.Entity.Pagination() { PageNo = (ushort)PageNo, PageSize = (ushort)PageSize });
            if (results != null && results.Count > 0) _totalRecords = results[0].RecordCount;
            rptUpcomingCars.DataSource = results;
            rptUpcomingCars.DataBind();
		}
    }
}		