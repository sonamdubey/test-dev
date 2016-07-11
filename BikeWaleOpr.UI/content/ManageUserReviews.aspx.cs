﻿using BikeWaleOpr.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    /// <summary>
    ///     Written By : Ashish G. Kamble on 1 Apr 2013
    ///     Summary : Class to manage the user reviews. User can view, edit, approve or discard user reviews.
    /// </summary>
    public class ManageUserReviews : Page
    {
        protected Repeater rptReviews;
        protected DropDownList ddlMakes, ddlModels;
        protected RadioButton rdoPending, rdoApproved, rdoDiscarded;
        protected Button btnShowReviews;
        protected HiddenField hdnSelectedModel = null;

        protected HtmlGenericControl errMsg;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnShowReviews.Click += new EventHandler(ShowReviews);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            errMsg.InnerHtml = "";
            errMsg.Visible = false;

            if (!IsPostBack)
            {
                FillMakes();
                FillModels();

                rdoPending.Checked = true;

                GetReviews();
            }

        }


        /// <summary>
        ///     Writtten By : Ashish G. Kamble on 1 Apr 2013
        ///     Summary : Function to fill the dropdownlist for makes.
        /// </summary>
        protected void FillMakes()
        {
            string sql = "select Id,Name from bikemakes where isdeleted=0 order by name";

            try
            {
                CommonOpn op = new CommonOpn();
                op.FillDropDown(sql, ddlMakes, "Name", "Id");
                ddlMakes.Items.Insert(0, new ListItem("--All Makes--", "0"));
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of FillMakes method


        /// <summary>
        ///     Writtten By : Ashish G. Kamble on 1 Apr 2013
        ///     Summary : Function to fill the dropdownlist for models.
        /// </summary>
        protected void FillModels()
        {
            ddlModels.Items.Insert(0, new ListItem("--Select Model--", "0"));
        }


        /// <summary>
        ///     Written By : Ashish G. Kamble on 1 Apr 2013
        ///     Summary : Method is used to show user reviews as per user make, model, approved or discarded reviews.
        /// </summary>
        void ShowReviews(object sender, EventArgs e)
        {
            GetReviews();
        }   // End of showreviews method

        protected void GetReviews()
        {
            string makeId = string.Empty, modelId = string.Empty;
            bool isPending = true, isVerified = false, isDiscarded = false;

            GetReviewCriteria(ref makeId, ref modelId, ref isPending, ref isVerified, ref isDiscarded);

            try
            {

                if (!string.IsNullOrEmpty(makeId) && makeId != "0")
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "getcustomerreviews_sp";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, String.IsNullOrEmpty(modelId) ? "0" : modelId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, isVerified));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isdiscarded", DbType.Boolean, isDiscarded));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_ispending", DbType.Boolean, isPending));


                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            {
                                rptReviews.DataSource = ds.Tables[0];
                                rptReviews.DataBind();
                                rptReviews.Visible = true;
                            }
                            else
                            {
                                rptReviews.Visible = false;
                                errMsg.InnerHtml = "<h3>Oops !! The records does not exists.</h3>";
                                errMsg.Visible = true;
                            }
                        }

                    }

                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("ManageUserReviews.GetReviews sql ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "ManageUserReviews.GetReviews");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("ManageUserReviews.GetReviews ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "ManageUserReviews.GetReviews");
                objErr.SendMail();
            }
        }   // End of GetReviews method

        /// <summary>
        ///     Written By : Ashish G. Kamble on 3 Apr 2013
        ///     Summary : Function will read the review parameters selected by the current user
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="isPending"></param>
        /// <param name="isVerified"></param>
        /// <param name="isDiscarded"></param>
        protected void GetReviewCriteria(ref string makeId, ref string modelId, ref bool isPending, ref bool isVerified, ref bool isDiscarded)
        {
            makeId = ddlMakes.SelectedValue;
            modelId = hdnSelectedModel.Value;

            isPending = rdoPending.Checked;
            isVerified = rdoApproved.Checked;
            isDiscarded = rdoDiscarded.Checked;

        }   // End of GetReviewCriteria method

    }   // End of Class
}   // End of Namespace