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
using Bikewale.Common;

namespace Bikewale.Controls
{
	public class BikesInBudget : UserControl
	{
		protected DataList dlHighlights;
		private string profileNo = "";
		private int records = 10, price = 0;
		private string cityId = "1", selectedCities = "";
		private int cityDistance = 0;

		protected Label lblCities; 
		
		public string ProfileNo
		{
			set{profileNo = value;}
		}
		
		public int Records
		{
			set{records = value;}
		}

        private int _recordCount = 0;
        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }
		
		public int Price
		{
			set{price = value;}
		}

        private string _headerText = "No Worries... There are many to choose from";
        public string HeaderText 
        {
            get { return _headerText; }
            set { _headerText = value; }
        }
			
		protected override void OnInit( EventArgs e )
		{
			InitializeComponents();
		}
		
		void InitializeComponents()
		{
			this.Load += new EventHandler( this.Page_Load );
		}
			
		void Page_Load( object sender, EventArgs e )
		{
			if(!IsPostBack)		
			{	
				CommonOpn op = new CommonOpn();
				
				cityId = op.GetCityId();
				cityDistance = Convert.ToInt16(op.GetCityDistance());
		
				ShowHighlights();
			}
		} // Page_Load
		
		
		//function to show more Bikes(10) at the same budget
		void ShowHighlights()
        {
            throw new Exception("Method not used/commented");

            //string sql;
			
            //double lattDiff = CommonOpn.GetLattitude(cityDistance);
            //double longDiff = CommonOpn.GetLongitude(cityDistance);

            //Trace.Warn("cityDistance : ", cityDistance.ToString());
            //Trace.Warn("lattDiff : ", lattDiff.ToString());
            //Trace.Warn("longDiff : ", longDiff.ToString());
			
            //Trace.Warn("ShowHighlights : price : " + price.ToString());			
            //if ( price != 0 )
            //{
            //    sql = " SELECT TOP " + records + " LL.ProfileId, "
            //        + " ( MakeName + ' ' + ModelName + ' ' + VersionName ) BikeMake, MakeName, ModelName, LC.Name AS CityName,LC.MaskingName as CityMaskingName, "
            //        + " MakeYear, LL.Price, LL.Color, LL.Kilometers, BM.MaskingName AS MakeMaskingName, BMO.MaskingName AS ModelMaskingName FROM LiveListings AS LL With(NoLock) INNER JOIN BikeMakes BM With(NoLock) ON  LL.MakeId = BM.ID INNER JOIN BikeModels BMO With(NoLock) ON LL.ModelId = BMO.ID, BWCities AS LC With(NoLock) WHERE "
            //        + " LL.ProfileId <> @ProfileNo AND "
            //        + " LC.Id = @CityId AND " 
            //        + " LL.Lattitude BETWEEN LC.Lattitude - @LattDiff AND LC.Lattitude + @LattDiff AND "
            //        + " LL.Longitude BETWEEN LC.Longitude - @LongDiff AND LC.Longitude + @LongDiff "
            //        + " ORDER BY ABS(LL.Price - @Price) ASC ";
				
            //    Trace.Warn(sql);
            //    Trace.Warn("profileNo : " + profileNo + " cityId : " + cityId + " lattDiff : " + lattDiff + " longDiff : " + longDiff + " price : " + price);
            //    SqlParameter [] param = 
            //    {
            //        new SqlParameter("@ProfileNo", profileNo),
            //        new SqlParameter("@CityId", cityId),
            //        new SqlParameter("@LattDiff", lattDiff),
            //        new SqlParameter("@LongDiff", longDiff),
            //        new SqlParameter("@Price", price)
            //    };
				
            //    Database db = new Database();
            //    DataSet ds = null;				
            //    try
            //    {
            //        ds = db.SelectAdaptQry( sql, param );
            //        _recordCount = ds.Tables[0].Rows.Count;

            //        if (ds != null && ds.Tables[0].Rows.Count > 0)
            //        {
            //            dlHighlights.DataSource = ds.Tables[0];
            //            dlHighlights.DataBind();
            //        }
            //    }
            //    catch( SqlException err )
            //    {
            //        Trace.Warn(err.Message);
            //        ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //    catch (Exception err)
            //    {
            //        Trace.Warn(err.Message);
            //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //}
			
            //lblCities.Text = selectedCities;
		} // ShowHighlights

        
		
	}
}