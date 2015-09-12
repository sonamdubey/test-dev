﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data.SqlClient;

namespace Bikewale.Content
{
    public class DefaultUR : System.Web.UI.Page
    {
        protected DropDownList drpMake, drpModel;
        protected Button btnWrite;
        protected Repeater rptMakes, rptMostReviewed;

        private DataSet dsMain;    

        CommonOpn op = new CommonOpn();

        public string SelectedModel
        {
            get
            {
                if (Request.Form["drpModel"] != null && Request.Form["drpModel"].ToString() != "")
                    return Request.Form["drpModel"].ToString();
                else
                    return "-1";
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_drpModel"] != null && Request.Form["hdn_drpModel"].ToString() != "")
                    return Request.Form["hdn_drpModel"].ToString();
                else
                    return "";
            }
        }
		
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnWrite.Click += new EventHandler(btnWrite_Click);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection deviceDetection = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            deviceDetection.DetectDevice();


            if (!IsPostBack)
            {
                BindControl();
                FillMake();
            }
        }

        private void btnWrite_Click(object Sender, EventArgs e)
        {          
            Response.Redirect("/content/userreviews/writereviews.aspx?bikem=" + Request.Form["drpModel"]);           
        }

        //Modified By : Ashwini Todkar on 12nd Feb 2014
        //Description : Replaced inline query by method
        private void FillMake()
        {
            try
            {
                DataTable dt;
                MakeModelVersion mmv = new MakeModelVersion();
                dt = mmv.GetMakes("NEW");

                if(dt.Rows.Count > 0 )
                {
                    drpMake.DataSource = dt;
                    drpMake.DataTextField = "Text";
                    drpMake.DataValueField = "Value";
                    drpMake.DataBind();

                    ListItem item = new ListItem("--Select--", "0");
                    drpMake.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        //Modified By : Ashwini Todkar on 12nd feb 2014
        //Description : replaced inline query by stored procedure 
        private void BindControl()
        {
            try
            {
                DataSet ds = null;

                using (SqlCommand cmd = new SqlCommand("GetUserReviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    Database db = new Database();
                    ds = new DataSet();

                    ds = db.SelectAdaptQry(cmd);             
                }

                dsMain = new DataSet();
                dsMain = ds;

                rptMakes.DataSource = ds.Tables[0];
                rptMakes.DataBind();

                rptMostReviewed.DataSource = ds.Tables[2];
                rptMostReviewed.DataBind();
                Trace.Warn("++dsmain rows count ", dsMain.Tables.Count.ToString());
            }
            catch (SqlException sqlEx)
            {
                Trace.Warn(sqlEx.Message + sqlEx.Source);
                ErrorClass objErr = new ErrorClass(sqlEx, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }   
        }

        public DataSet GetDataSource(string makeId)
        {
            Trace.Warn("Binding List...");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Wallpapers");

            dt.Columns.Add("MakeId", typeof(string));
            dt.Columns.Add("ModelId", typeof(string));
            dt.Columns.Add("ModelName", typeof(string));
            dt.Columns.Add("TotalReviews", typeof(string));
            dt.Columns.Add("BikeMake", typeof(string));
            dt.Columns.Add("BikeModel", typeof(string));
            dt.Columns.Add("ModelMaskingName", typeof(string));
            dt.Columns.Add("MakeMaskingName", typeof(string));

            try
            {
                Trace.Warn("Total Models..." + dsMain.Tables[1].Rows.Count);

                DataRow[] rows = dsMain.Tables[1].Select("MakeId=" + makeId);

                DataRow dr;
                Trace.Warn("Current Make contains..." + rows.Length);

                foreach (DataRow row in rows)
                {
                    dr = dt.NewRow();

                    dr[0] = row["MakeId"];
                    dr[1] = row["ModelId"];
                    dr[2] = row["ModelName"];
                    dr[3] = row["TotalReviews"];
                    dr[4] = row["BikeMake"];
                    dr[5] = row["BikeModel"];
                    dr[6] = row["ModelMaskingName"];
                    dr[7] = row["MakeMaskingName"];

                    dt.Rows.Add(dr);
                }

                Trace.Warn("Current Table contains..." + dt.Rows[0]["ModelMaskingName"].ToString());
                Trace.Warn("current table contain..." + dt.Rows[0]["MakeMaskingName"].ToString());
                ds.Tables.Add(dt);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return ds;
        }
    }
}