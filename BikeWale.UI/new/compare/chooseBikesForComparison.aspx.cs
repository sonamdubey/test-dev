/*******************************************************************************************************
IN THIS CLASS WE GET THE ID OF THE BIKE MAKE FROM THE QUERY STRING, AND FROM IT WE FETCH ALL THE
MODELS FOR THIS MAKE, and the count for this model in the sell inquiry.
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
//using BikeWale.Controls;
using Ajax;
using Bikewale.controls;

namespace Bikewale.New
{
	public class ComparisonChoose : Page
	{        
		protected int featuredBikeIndex  = 0; // this variable not used any where in this page.
		protected HtmlGenericControl spnError;
		protected DropDownList cmbMake, cmbMake1, cmbMake2, cmbMake3;
			
		protected Button btnCompare;
		protected RadioButton optNew, optAll;
		
		public int make1 = 0, model1 = 0, version1 = 0;
		public int make2 = 0, model2 = 0, version2 = 0;
		public int make3 = 0, model3 = 0, version3 = 0;
		public int make4 = 0, model4 = 0, version4 = 0;

        protected string compareBikes = String.Empty;
				 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			this.btnCompare.Click += new EventHandler( btnCompare_Click );
			optNew.CheckedChanged += new EventHandler( CompareStatusChanged );
			optAll.CheckedChanged += new EventHandler( CompareStatusChanged );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            compareBikes = optNew.Checked ? "new" : "all";

            if ( !IsPostBack )
			{
                // check whether CompareAll cookie is set?
				if ( Request.Cookies["CompareAll"] != null )
				{
					optAll.Checked = true;
					optNew.Checked = false;
				}
			}
			else
			{
				Trace.Warn("Posting back...");
			}
			
			Trace.Warn( "New ? " + optNew.Checked + ". All ? " + optAll.Checked );
			
			// fill makes in drop-downs
			FillMakes( optNew.Checked );
				
			// if Bike ids are passed in query-string, fill the appropriate dropdowns.
			for ( int i=1; i<=4; i++ )
                if (Request["bike" + i] != null && CommonOpn.CheckId(Request["bike" + i]))
                    FillExisting(Request["bike" + i], i);
						
			/*string sql, sql1;
			
			sql = " SELECT DISTINCT Mo.ID, Mo.Name, BikeMakeId "
                + " FROM BikeModels Mo, BikeVersions Ve, NewBikeSpecifications N "
                + " WHERE Mo.IsDeleted = 0 "
                + " AND Ve.IsDeleted = 0 "
                + " AND Mo.ID = Ve.BikeModelId "
                + " AND Ve.ID = N.BikeVersionid ";
			
			if ( optNew.Checked )
				sql += " AND Ve.New=1 ";
			
			sql	+= " ORDER BY Mo.NAME";
			
			if ( optNew.Checked )	
				sql1 = "SELECT DISTINCT Ve.ID, Ve.Name, BikeModelId "
                     + "FROM BikeVersions Ve,NewBikeSpecifications N "
                     + "WHERE Ve.IsDeleted <> 1 "
                     + "AND Ve.ID =N.BikeVersionid "
                     + "AND Ve.New=1 "
                     + "ORDER BY Ve.Name";
			else
				sql1 = "SELECT DISTINCT Ve.ID, (Ve.Name + ' ' + (CASE WHEN (SELECT CV.Name FROM BikeVersions CV WHERE CV.Id=Ve.Id AND CV.New=1) IS NULL THEN '*' ELSE '' END)) AS Name, "
                     + "BikeModelId, Ve.New, Ve.Name AS Useless FROM BikeVersions Ve, NewBikeSpecifications  N "
                     + "WHERE Ve.IsDeleted <> 1 AND Ve.ID =N.BikeVersionid "
                     + "ORDER BY Ve.New DESC, Useless";
			*/							
			//string Script = this.GenerateBikeArray( sql, sql1 );
			
            //string Script1 = this.GenerateChainScript( "cmbMake", "cmbModel", "cmbVersion");
            //string Script2 = this.GenerateChainScript( "cmbMake1", "cmbModel1", "cmbVersion1" );
            //string Script3 = this.GenerateChainScript( "cmbMake2", "cmbModel2", "cmbVersion2" );
            //string Script4 = this.GenerateChainScript( "cmbMake3", "cmbModel3", "cmbVersion3" );
			
			//RegisterStartupScript( "ChainScript", Script + Script1 + Script2 + Script3 + Script4 );
			//ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script + Script1 + Script2 + Script3 + Script4 );
			
			Trace.Warn("End PageLoad");
		} // Page_Load
		
		// user is changing his preference for new or all comparison
		void CompareStatusChanged( object sender, EventArgs e )
		{
			Trace.Warn( "Status Changed : New ? " + optNew.Checked + ". All ? " + optAll.Checked );
			
			if ( optAll.Checked )
			{
				if ( Request.Cookies["CompareAll"] == null )
				{
					Trace.Warn("User is interested in All Bikes... setting cookie");
					
					HttpCookie cookie = new HttpCookie( "CompareAll" );
					cookie.Value = "1";
					cookie.Expires = DateTime.Now.AddYears(1);
					Response.Cookies.Add( cookie );
				}
			}
			else
			{
				if ( Request.Cookies["CompareAll"] != null )
					Response.Cookies["CompareAll"].Expires = DateTime.Now.AddYears(-1);
			}
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlyNew"></param>
		void FillMakes( bool onlyNew )
		{
            CommonOpn op = new CommonOpn();
			string sql;
			
            // Modified By : Ashish G. Kamble
            // Query is modified. Futuristic = 0 is added to ensure that only launched Bikes come in the list

            sql = " SELECT DISTINCT Ma.ID, Ma.Name,CAST(Ma.ID  AS varchar(10)) + '_'+  Ma.MaskingName AS Value  FROM BikeMakes Ma, BikeModels Mo, BikeVersions Ve, NewBikeSpecifications Bs With(NoLock) "
                + " WHERE Ma.IsDeleted = 0 AND Ma.Id=Mo.BikeMakeId AND Ma.Futuristic = 0 AND "
                + " Mo.ID=Ve.BikeModelId AND Ve.ID = Bs.BikeVersionId ";
				
			if ( onlyNew )
				sql += " AND Ve.New=1 AND Ma.New = 1 ";
				
			sql += " ORDER BY Ma.NAME ";
			
			Trace.Warn( sql );
			
			try
			{
				op.FillDropDown( sql, cmbMake, "Name", "Value" );
				op.FillDropDown( sql, cmbMake1, "Name", "Value" );
				op.FillDropDown( sql, cmbMake2, "Name", "Value" );
				op.FillDropDown( sql, cmbMake3, "Name", "Value" );
			}
			catch( SqlException err )
			{
				Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
			
			ListItem item = new ListItem( "--Select Make--", "0" );
			cmbMake.Items.Insert( 0, item );
			cmbMake1.Items.Insert( 0, item );
			cmbMake2.Items.Insert( 0, item );
			cmbMake3.Items.Insert( 0, item );
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bike"></param>
        /// <param name="bikeNo"></param>
        void FillExisting(string bike, int bikeNo)
		{
            Trace.Warn("inside fikll existing");
            SqlDataReader dr = null;
			Database db = new Database();
			string sql;

            sql = "SELECT VE.ID Version, MO.ID Model, MA.ID Make, MA.MaskingName AS MakeMaskingName,MO.MaskingName AS ModelMaskingName"
                + " FROM BikeMakes MA, BikeModels MO, BikeVersions VE With(NoLock) "
				+ " WHERE VE.BikeModelId=MO.Id AND MO.BikeMakeId=MA.ID "
				+ " AND VE.ID=@ID";
			
			SqlCommand cmd =  new SqlCommand(sql);
			cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = bike;

            Trace.Warn("sql ::: ", sql);

			try
			{
				dr = db.SelectQry(cmd);
				
				if ( dr.Read() )
				{
				
					switch ( bikeNo )
					{
						case 1:
							make1 = Convert.ToInt16( dr["Make"].ToString() );
							model1 = Convert.ToInt16( dr["Model"].ToString() );
							version1 = Convert.ToInt16( dr["Version"].ToString() );
                          
							break;
						case 2:
							make2 = Convert.ToInt16( dr["Make"].ToString() );
							model2 = Convert.ToInt16( dr["Model"].ToString() );
							version2 = Convert.ToInt16( dr["Version"].ToString() );
                           
							break;
						case 3:
							make3 = Convert.ToInt16( dr["Make"].ToString() );
							model3 = Convert.ToInt16( dr["Model"].ToString() );
							version3 = Convert.ToInt16( dr["Version"].ToString() );
                         
							break;
						case 4:
							make4 = Convert.ToInt16( dr["Make"].ToString() );
							model4 = Convert.ToInt16( dr["Model"].ToString() );
							version4 = Convert.ToInt16( dr["Version"].ToString() );
                           
							break;
					}
				}
			}
			catch( SqlException err )
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
			finally
			{
                if(dr != null)
                    dr.Close();
				db.CloseConnection();
			}
		}
		
		void btnCompare_Click( object sender, EventArgs e )
		{
			Trace.Warn("Comparing bikes...");
			
			string bike1 = "", bike2 = "", bike3 = "", bike4 = "";
            string compString = "", compareUrl = String.Empty;
            
            string makeMaskingName1 = String.Empty, modelMaskingName1 = String.Empty;
            string makeMaskingName2 = String.Empty, modelMaskingName2 = String.Empty;
            string makeMaskingName3 = String.Empty, modelMaskingName3 = String.Empty;
            string makeMaskingName4 = String.Empty, modelMaskingName4 = String.Empty;

            if (Request.Form["cmbMake"] != null && Request.Form["cmbMake"] != "0")
                makeMaskingName1 = Request.Form["cmbMake"].Split('_')[1];
            if (Request.Form["cmbMake1"] != null && Request.Form["cmbMake1"] != "0")
                makeMaskingName2 = Request.Form["cmbMake1"].Split('_')[1];
            if (Request.Form["cmbMake2"] != null && Request.Form["cmbMake2"] != "0")
                makeMaskingName3 = Request.Form["cmbMake2"].Split('_')[1];
            if (Request.Form["cmbMake3"] != null && Request.Form["cmbMake3"] != "0")
                makeMaskingName4 = Request.Form["cmbMake3"].Split('_')[1];


            if (Request.Form["cmbModel"] != null && Request.Form["cmbModel"] != "0")
                modelMaskingName1 = Request.Form["cmbModel"].Split('_')[1];
            if (Request.Form["cmbModel1"] != null && Request.Form["cmbModel1"] != "0")
                modelMaskingName2 = Request.Form["cmbModel1"].Split('_')[1];
            if (Request.Form["cmbModel2"] != null && Request.Form["cmbModel2"] != "0")
                modelMaskingName3 = Request.Form["cmbModel2"].Split('_')[1];
            if (Request.Form["cmbModel3"] != null && Request.Form["cmbModel3"] != "0")
                modelMaskingName4 = Request.Form["cmbModel3"].Split('_')[1];

			
			if ( Request.Form["cmbVersion"] != null && Request.Form["cmbVersion"] != "0" && CommonOpn.CheckId( Request.Form["cmbVersion"] ) )
				bike1 = Request.Form["cmbVersion"];
			if ( Request.Form["cmbVersion1"] != null && Request.Form["cmbVersion1"] != "0" && CommonOpn.CheckId( Request.Form["cmbVersion1"] ) )
				bike2 = Request.Form["cmbVersion1"];
			if ( Request.Form["cmbVersion2"] != null && Request.Form["cmbVersion2"] != "0" && CommonOpn.CheckId( Request.Form["cmbVersion2"] ) )
				bike3 = Request.Form["cmbVersion2"];
			if ( Request.Form["cmbVersion3"] != null && Request.Form["cmbVersion3"] != "0" && CommonOpn.CheckId( Request.Form["cmbVersion3"] ) )
				bike4 = Request.Form["cmbVersion3"];
			
			int bikeCount = 0;
			
			if( bike1 != "" ) bikeCount ++;
			if( bike2 != "" ) bikeCount ++;
			if( bike3 != "" ) bikeCount ++;
			if( bike4 != "" ) bikeCount ++;


			Trace.Warn("bikeCount : " + bikeCount.ToString());

            if (bikeCount < 2)
            {
                Response.Redirect("./",false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
			
			for ( int i = 2; i <= bikeCount; i++ ) 
			{
				switch( i )
				{
					case 2:
						if ( bike1 == "" )
						{
							bike1 = bike2;
							bike2 = bike3;
							bike3 = bike4;

                            makeMaskingName1 = makeMaskingName2;
                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName1 = modelMaskingName2;
                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
						}
						else if ( bike2 == "" )
						{
							bike2 = bike3;
							bike3 = bike4;

                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
						}
												
						if ( bike1 == "" )
						{
							bike1 = bike2;
							bike2 = bike3;

                            makeMaskingName1 = makeMaskingName2;
                            makeMaskingName2 = makeMaskingName3;

                            modelMaskingName1 = modelMaskingName2;
                            modelMaskingName2 = modelMaskingName3;                         
						}
						else if ( bike2 == "" )
						{
							bike2 = bike3;

                            makeMaskingName2 = makeMaskingName3;

                            modelMaskingName2 = modelMaskingName3;
						}
						
						if ( bike2 == "" )
						{
							bike2 = bike3;

                            makeMaskingName2 = makeMaskingName3;
                            modelMaskingName2 = modelMaskingName3;
						}
						
						compString = "bike1=" + bike1 + "&bike2=" + bike2;
                        compareUrl =  makeMaskingName1 + "-" + modelMaskingName1 + "-vs-" + makeMaskingName2 + "-" + modelMaskingName2 ;                 
						break;
					case 3:
						if ( bike1 == "" )
						{
							bike1 = bike2;
							bike2 = bike3;
							bike3 = bike4;

                            makeMaskingName1 = makeMaskingName2;
                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName1 = modelMaskingName2;
                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
						}
						else if ( bike2 == "" )
						{
							bike2 = bike3;
							bike3 = bike4;

                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
						}
						else if ( bike3 == "" )
						{
							bike3 = bike4;
                            makeMaskingName3 = makeMaskingName4;
                            modelMaskingName3 = modelMaskingName4;
						}						
						compString = "bike1=" + bike1 + "&bike2=" + bike2 + "&bike3=" + bike3;
                        compareUrl =  makeMaskingName1 + "-" + modelMaskingName1 + "-vs-" + makeMaskingName2 + "-" + modelMaskingName2 + "-vs-" + makeMaskingName3 + "-" + modelMaskingName3;                     
						break;				
					case 4:
						compString = "bike1=" + bike1 + "&bike2=" + bike2 + "&bike3=" + bike3 + "&bike4=" + bike4;
                        compareUrl =  makeMaskingName1 + "-" + modelMaskingName1 + "-vs-" + makeMaskingName2 + "-" + modelMaskingName2 + "-vs-" + makeMaskingName3 + "-" + modelMaskingName3 + "-vs-" + makeMaskingName4 + "-" + modelMaskingName4;
						break;				
				}				
			}
            compareUrl =  "/comparebikes/" + compareUrl + "/?" + compString;
			Trace.Warn( compString );
            Trace.Warn("+++compare url : ", compareUrl);
            Response.Redirect(compareUrl);
		} // btnSend_Click
		
		// The conventional chain function is broken in two pieces.
		// This function generates array of models and versions.
        //private string GenerateBikeArray( string Query1, string Query2 )
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append( "<script language=\"javascript\" src=\"/src/chains.js?v=1.0\"></script>" );
        //    sb.Append( "<script language=\"javascript\">" );

        //    sb.Append( "var arMo = new Array(); var i = 0;" );
			
        //    SqlDataReader dr;
        //    Database db = new Database();
        //    try
        //    {
        //        dr = db.SelectQry( Query1 );
			
        //        while ( dr.Read() )
        //            sb.Append( "arMo[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;" );
								
        //        dr.Close();
        //    }
        //    catch ( SqlException ex )
        //    {
        //        HttpContext.Current.Trace.Warn( "Inside GenerateChainScript : " + ex.Message );
        //    }
        //    finally
        //    {
        //        db.CloseConnection();
        //    }
			
        //    sb.Append( "var arVe = new Array(); i = 0;" );
			
        //    dr = db.SelectQry( Query2 );
			
        //    while ( dr.Read() )
        //    {
        //        sb.Append( "arVe[i] = [ " + dr[0] + ",\"" + dr[1] + "\"," + dr[2] + " ]; i++;" );
        //    }
			
        //    sb.Append( "</script>" );
			
        //    dr.Close();
        //    db.CloseConnection();
				
        //    return sb.ToString();
			
        //}
		
        //private string GenerateChainScript( string DropDownList1, string DropDownList2, string DropDownList3 )
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append( "<script language=\"javascript\">" );
        //    sb.Append( "document.getElementById('" + DropDownList1 + "').onchange = " + DropDownList1 + "_OnChange; " );
        //    sb.Append( "document.getElementById('" + DropDownList2 + "').onchange = " + DropDownList2 + "_OnChange; " );
        //    sb.Append( "function " + DropDownList1 + "_OnChange( e ) {" );
        //    sb.Append( "var DropDownList1 = document.getElementById('" + DropDownList1 + "');" );

        //    sb.Append( "fillChain( '" + DropDownList2 + "', DropDownList1, arMo, '" + DropDownList3 +  "', '--Select Model--' ); }" );
							
        //    sb.Append( "function " + DropDownList2 + "_OnChange( e ) {" );
        //    sb.Append( "var DropDownList = document.getElementById('" + DropDownList2 + "');" );
			
        //    sb.Append( "fillChain( '" + DropDownList3 + "', DropDownList, arVe, 'NA', '--Select Version--' ); } " );
			
        //    sb.Append( "</script>" );
			
        //    return sb.ToString();
			
        //}
	} // class
} // namespace