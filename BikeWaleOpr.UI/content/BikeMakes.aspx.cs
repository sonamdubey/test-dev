﻿/*******************************************************************************************************
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
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using BikeWaleOPR.DAL.CoreDAL;
using System.Data.Common;
using BikeWaleOPR.Utilities;

namespace BikeWaleOpr.Content
{
	public class BikeMakes : Page
	{
		protected HtmlGenericControl spnError;
        protected TextBox txtMake,txtMaskingName;
		protected Button btnSave;
		protected DataGrid dtgrdMembers;
        protected Label lblStatus;
				
		private string SortCriteria
		{
			get { return ViewState["SortCriteria"].ToString(); }
			set { ViewState["SortCriteria"] = value; }
		} // SortCriteria
		
		private string SortDirection
		{
			get { return ViewState["SortDirection"].ToString(); }
			set { ViewState["SortDirection"] = value; }
		} // SortDirection
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			dtgrdMembers.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
			dtgrdMembers.SortCommand += new DataGridSortCommandEventHandler(Sort_Grid);
			dtgrdMembers.EditCommand += new DataGridCommandEventHandler( dtgrdMembers_Edit );
			dtgrdMembers.UpdateCommand += new DataGridCommandEventHandler( dtgrdMembers_Update );
			dtgrdMembers.CancelCommand += new DataGridCommandEventHandler( dtgrdMembers_Cancel );
			dtgrdMembers.DeleteCommand += new DataGridCommandEventHandler( dtgrdMembers_Delete );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            lblStatus.Text = "";
			if ( !IsPostBack )
			{
				SortDirection = "";
				SortCriteria = "Name";
                			
				BindGrid();
			}
		} // Page_Load
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			Page.Validate();
			if ( !Page.IsValid ) return;
			
			string sql;
			
			sql = @"insert into bikemakes( name,maskingname,isdeleted,macreatedon,maupdatedby)
                                   values( @make, @makemaskingname, 0 , now(),@userid)";

			try
			{
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {

                    cmd.Parameters.Add(DbFactory.GetDbParam("@make", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtMake.Text.Trim().Replace("'", "''")));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@makemaskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtMaskingName.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], BikeWaleAuthentication.GetOprUserId()));

                    MySqlDatabase.InsertQuery(cmd); 
                }
			}
			catch( SqlException ex )	
			{
				Trace.Warn("Error", ex.Message + ex.Source);
				ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
				objErr.SendMail();

                // Error code Unique key constraint in the database.
                if (ex.Number == 2627)
                {
                    lblStatus.Text = "Make name or make masking name already exists. Can not insert duplicate name.";
                }
                else
                    lblStatus.Text = "";
			}
            catch(Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
			BindGrid();
		}
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindGrid()
		{
			string sql = "";
			
			int pageSize = dtgrdMembers.PageSize;

            sql = @" select bm.id, bm.name,bm.maskingname,
                        if(bm.used,true,false) as used ,if(bm.new,true,false) as new, if(bm.futuristic,true,false) as futuristic ,
                        bm.macreatedon as createdon,
                        bm.maupdatedon as updatedon,
                        ou.username as updatedby 
                    from bikemakes bm left join oprusers ou on bm.maupdatedby = ou.id where isdeleted=0 ";
			
			if(SortCriteria != "")
                sql += " order by bm.futuristic desc,bm.new desc,bm.used desc," + SortCriteria + " " + SortDirection; 
			
			Trace.Warn(sql);
			CommonOpn objCom = new CommonOpn();			
			try
			{
				objCom.BindGridSet( sql, dtgrdMembers, pageSize);
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			
		}
		
		void dtgrdMembers_Edit( object sender, DataGridCommandEventArgs e )
		{
			dtgrdMembers.EditItemIndex = e.Item.ItemIndex;
			BindGrid();
			//req1.Enabled = false;
			btnSave.Enabled = false;
		}
		
		void dtgrdMembers_Update( object sender, DataGridCommandEventArgs e )
		{
			Page.Validate();
			if ( !Page.IsValid ) return;
			
			string sql;
            CheckBox chkFuturistic = (CheckBox)e.Item.FindControl("chkFut");
            CheckBox chkUsed = (CheckBox)e.Item.FindControl("chkUsed");
            CheckBox chkNew = (CheckBox)e.Item.FindControl("chkNew");

			TextBox txt = (TextBox) e.Item.FindControl( "txtMake" );
			sql = @"update bikemakes set
				name= @make,
                Futuristic=@isfuturistic,
                Used = @isused,
                New = @isnew,
                MaUpdatedOn=now(),
                MaUpdatedBy=@userid
				WHERE Id=@makeid";

            try
            {

                DbParameter[] sqlParams = new[]
                    {
                        DbFactory.GetDbParam("@make", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],100, txt.Text.Trim().Replace("'","''")),
                        DbFactory.GetDbParam("@isfuturistic", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], Convert.ToInt16(chkFuturistic.Checked)),
                         DbFactory.GetDbParam("@isnew", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt],  Convert.ToInt16(chkNew.Checked)),
                          DbFactory.GetDbParam("@isused", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], Convert.ToInt16(chkUsed.Checked)),
                        DbFactory.GetDbParam("@userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], BikeWaleAuthentication.GetOprUserId()),
                        DbFactory.GetDbParam("@makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], dtgrdMembers.DataKeys[ e.Item.ItemIndex ])
                    };

                MySqlDatabase.InsertQuery(sql,sqlParams);
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);                
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();

                Trace.Warn("number : " + ex.Number + " : error code : " + ex.ErrorCode);
                // Error code Unique key constraint in the database.
                if (ex.Number == 2627)
                {
                    lblStatus.Text = "Make name or make masking name already exists. Can not insert duplicate name";
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

			dtgrdMembers.EditItemIndex = -1;
			//req1.Enabled = true;
			btnSave.Enabled = true;
			BindGrid();
		}
		
		void dtgrdMembers_Cancel( object sender, DataGridCommandEventArgs e )
		{
			dtgrdMembers.EditItemIndex = -1;
			BindGrid();
			//req1.Enabled = true;
			btnSave.Enabled = true;
		}
		
		void dtgrdMembers_Delete( object sender, DataGridCommandEventArgs e )
		{
            MakeModelVersion mmv = new MakeModelVersion();
            mmv.DeleteMakeModelVersion(dtgrdMembers.DataKeys[e.Item.ItemIndex].ToString(), BikeWaleAuthentication.GetOprUserId());
			BindGrid();
		}
		
		void Page_Change(object sender,DataGridPageChangedEventArgs e)
		{
			// Set CurrentPageIndex to the page the user clicked.
			dtgrdMembers.CurrentPageIndex = e.NewPageIndex;
			BindGrid();
		}
		
		
		 // <summary>
		/// this function sorts the dataset based on given criteria
		/// </summary>
		/// <paramname="sender"></param>
		/// <paramname="e"></param>
		protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
		{
			if ( SortCriteria == e.SortExpression )
			{
				SortDirection = SortDirection == "desc" ? "asc" : "desc"; 
			}
			else
			{
				SortDirection = "asc";
			}
			SortCriteria = e.SortExpression;			
			BindGrid();		
		} 
	} // class
} // namespace