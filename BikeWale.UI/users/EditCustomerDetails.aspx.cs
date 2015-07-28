/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
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
using Bikewale.Controls;
using System.Configuration;
using System.Data.SqlTypes;
using Bikewale.BAL;
using Bikewale.BAL.MobileVerification;
//using Ajax;

namespace Bikewale.MyBikeWale
{
	public class EditCustomerDetails : Page
	{
		protected HtmlGenericControl spnError, divNote;
		protected DropDownList drpState, drpCity, drpPinCode;
		protected TextBox txtName, txtMobile;
        //txtAddress, txtStdCode1,txtPhone1; 
				
		protected Label lblEmail;		
		protected CheckBox chkNewsLetter;
		protected HtmlInputCheckBox chkDOB;
		
		protected Button btnSave;
		
		public string customerId, cityId = "", areaId = "", pinId = "";
		public CustomerDetails cd;
		
		public bool NewCustomer
		{
			get{return Convert.ToBoolean(ViewState["NewCustomer"]);}
			set{ViewState["NewCustomer"] = value;}
		}
		
		public string Comments
		{
			get
			{
				if(ViewState["Comments"] != null)
					return ViewState["Comments"].ToString();
				else
					return "";
			}
			set{ViewState["Comments"] = value;}
		}
		
		public string SelectedCity
		{
			get
			{
				if(Request.Form["drpCity"] != null)
					return Request.Form["drpCity"].ToString();
				else
					return "-1";
			}
		}
		
		public string CityContents
		{
			get
			{
				if(Request.Form["hdn_drpCity"] != null)
					return Request.Form["hdn_drpCity"].ToString();
				else
					return "";
			}
		}
		
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler(btnSave_Click);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			//register the ajax library and emits corresponding javascript code
			//for this page
			//Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
			
			// check for login.
			if ( CurrentUser.Id == "-1" )
				Response.Redirect( "/users/login.aspx?returnUrl=/users/EditCustomerDetails.aspx" );
			
			customerId = CurrentUser.Id;			
			
			if(Request["returnUrl"] != null)
				divNote.Visible = true;
			else
				divNote.Visible = false;
						
			if ( !IsPostBack )
			{
				NewCustomer = false;
				
				FillStates();								
				ShowCustomerDetails();

                Trace.Warn("fill state");
			}
			else
			{
				//in case of post back update contents of the city and the area drop down
                //AjaxFunctions aj = new AjaxFunctions();
				
                ////update the contents for city
                //aj.UpdateContents(drpCity, CityContents, SelectedCity);
                CommonOpn opn = new CommonOpn();                
                opn.UpdateContents(drpCity, CityContents, SelectedCity);
			}

		} // Page_Load
			
		//show the customer details
		void ShowCustomerDetails()
		{
			cd = new CustomerDetails(customerId);
			NewCustomer	= !cd.IsVerified;
			
			//Code Added on 29 Oct 2009 By Sentil
			string phone = cd.Phone1;
			string[] str_PhoneCode = phone.Split('-');
			Trace.Warn(str_PhoneCode.Length.ToString());
				
			if(cd.Exists == true)
			{
				txtName.Text 			= cd.Name; 
				//txtPhone1.Text 			= cd.Phone1;
                //if( str_PhoneCode.Length > 1 )
                //{
                //    //txtStdCode1.Text 		= str_PhoneCode[0];  
                //    txtPhone1.Text 			= str_PhoneCode[1];  
                //}	
                //else
                //{
                //    txtPhone1.Text 			= str_PhoneCode[0];
                //}
				
				txtMobile.Text 			= cd.Mobile;  
				
				//txtAddress.Text 		= cd.Address;  
						
				lblEmail.Text 			= cd.Email; 
				
				//cmbAboutCarwale.SelectedIndex 		= cmbAboutCarwale.Items.IndexOf(cmbAboutCarwale.Items.FindByValue(cd.CarwaleContact));
								
				string stateId = cd.StateId;
				cityId 	= cd.CityId;
				areaId 	= cd.AreaId;

                Trace.Warn("stateid : " + stateId);

				if(stateId != "" && stateId != "-1")
				{
					drpState.SelectedIndex 	= drpState.Items.IndexOf(drpState.Items.FindByValue(stateId));
                    Trace.Warn("drp state value : " + drpState.SelectedValue);

                    StateCity objCity = new StateCity();
                    DataTable dtCities = objCity.GetCities(stateId, "ALL");

                    if (dtCities != null && dtCities.Rows.Count > 0)
                    {
                        drpCity.DataSource = dtCities;
                        drpCity.DataTextField = "Text";
                        drpCity.DataValueField = "Value";
                        drpCity.DataBind();
                        drpCity.Items.Insert(0, new ListItem("Select City", "0"));
                        drpCity.SelectedValue = cityId;
                    }
                    
                    //AjaxFunctions aj = new AjaxFunctions();
                    ////bind the city drop down 
                    //drpCity.DataSource = aj.GetCities(cd.StateId);
                    //drpCity.DataTextField = "Text";
                    //drpCity.DataValueField = "Value";
                    //drpCity.DataBind();
                    //drpCity.Items.Insert(0, new ListItem("Any","0"));
					
					
					Trace.Warn("cityId : " + cityId);
					//if cityid is already there, then make that one selected
					//drpCity.SelectedIndex = drpCity.Items.IndexOf(drpCity.Items.FindByValue(cityId));
					
					/*if(drpCity.SelectedIndex > 0)
					{
						drpArea.Enabled = true;
						//fill the area drop down
						drpArea.DataSource = aj.GetAreas(cityId);
						drpArea.DataTextField = "Text";
						drpArea.DataValueField = "Value";
						drpArea.DataBind();
						drpArea.Items.Insert(0, new ListItem("Any","0"));
					}
					//if areaid is already there, then make that one selected
					drpArea.SelectedIndex = drpArea.Items.IndexOf(drpArea.Items.FindByValue(areaId));*/
				}
								
				//Trace.Warn("areaId : " + areaId);
				pinId	= cd.PinCodeId;
				
				chkNewsLetter.Checked 	= cd.ReceiveNewsletters;
				Comments = cd.Comment;
			}
			else
			{
				spnError.InnerHtml = "No such customer exists";
				btnSave.Enabled = false;
			}
		}
		
		void FillStates()
		{
			string sql = "";
			CommonOpn op = new CommonOpn();

            sql = " SELECT ID, Name FROM States With(NoLock) WHERE IsDeleted = 0 ORDER BY Name ";
			try
			{
				op.FillDropDown(sql, drpState, "Name", "ID");
				drpState.Items.Insert(0, new ListItem("Select State", "0"));
			}
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
			catch(Exception err)
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		
		void btnSave_Click(object sender, EventArgs e)
		{
			Page.Validate();
			if(!IsValid)
				return;
				
			if(SaveCustomerDetails() == true)
			{
				
				if(Request["returnUrl"] != null && Request["returnUrl"].ToString() != "")
					Response.Redirect(Request["returnUrl"].ToString());
				else
					Response.Redirect("/users/MyContactDetails.aspx");
					
				spnError.InnerHtml = "Your Details has been updated successfully.";
			}
			else
				spnError.InnerHtml = "Your Details could not be updated. Please try again.";
		}
		
		bool SaveCustomerDetails()
		{
			bool returnVal = false;
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
						
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );
			
            //string address = txtAddress.Text.Trim();
            //if(address.Length > 250)
            //    address = address.Substring(0, 249);
				
			string stateId = "0";
			
			if(drpState.SelectedIndex > -1)	
				stateId = drpState.SelectedItem.Value;
			
            //string phoneNo = "";

            //if(txtStdCode1.Text != "")
            //    phoneNo = txtStdCode1.Text.Trim() + "-" + txtPhone1.Text.Trim();
            //else
            //    phoneNo = txtPhone1.Text.Trim();
				
			try
			{
				cmd = new SqlCommand("UpdateCustomerDetails", con);
				cmd.CommandType = CommandType.StoredProcedure;
			
				prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
				prm.Value = customerId;
								
				prm = cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100);
				prm.Value = txtName.Text.Trim();
				
				prm = cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100);
				prm.Value = lblEmail.Text.Trim();
				
                //prm = cmd.Parameters.Add("@Address", SqlDbType.VarChar, 250);
                //prm.Value = address;
				
				prm = cmd.Parameters.Add("@CityId", SqlDbType.BigInt);
				prm.Value = SelectedCity;
								
				prm = cmd.Parameters.Add("@AreaId", SqlDbType.BigInt);
				prm.Value = 0;
								
                //prm = cmd.Parameters.Add("@Phone1", SqlDbType.VarChar, 50);
                //prm.Value = phoneNo;
				
				prm = cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50);
				prm.Value = txtMobile.Text.Trim();
								
				prm = cmd.Parameters.Add("@ReceiveNewsletters", SqlDbType.Bit);
				prm.Value = chkNewsLetter.Checked;

                //Modified By : Ashwini Todkar on 3 Sep 2014
                //Added isverfied condition while updating mobile no.
                MobileVerification objMV = new MobileVerification();
               
				prm = cmd.Parameters.Add("@IsVerified", SqlDbType.Bit);
				//prm.Value = true;
                prm.Value = objMV.IsMobileVerified(txtMobile.Text.Trim(), lblEmail.Text.Trim());

				con.Open();
				//run the command
    			cmd.ExecuteNonQuery();
				
				returnVal = true;			
			}
			catch(SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
				returnVal = false;
			} // catch SqlException
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
				returnVal = false;
			} // catch Exception
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			
			return returnVal;
		}
		
		
		public int PrimaryPhone
		{
			get
			{
				int primary = 1;
				primary = txtMobile.Text.Trim() == "" ? 1 : 3;
				
				return primary;
			}
		}
		
	} // class
} // namespace