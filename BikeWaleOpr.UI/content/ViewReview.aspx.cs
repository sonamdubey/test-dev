﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using System.Configuration;
using BikeWaleOpr.Controls;

namespace BikeWaleOpr.Content
{
    /// <summary>
    ///     Written By : Ashish G. Kamble on 3 Apr 2013
    ///     Summary : Class to show and edit the customer review information.
    /// </summary>
    public class ViewReview : Page
    {
        // Variable for ASP Controls
        protected Label exterior, Comfort, Performance, mileage, Value, Familiarity, Purchased, Fuel, Detail;
        protected TextBox txtExterior, txtComfort, txtPerformance, txtMileage, txtValue, txtTitle, txtPros, txtCons, txtFamiliarity, txtFuel;
        protected RichTextEditor rteDetail;
        protected Button btnUpdateReview;
        protected Repeater rptReviews;
        
        // Variable for html Control
        protected HtmlGenericControl errMsg, divShowReview;

        protected string reviewId = string.Empty;
        protected bool isVerified = false, isDiscarded = false, isPending = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdateReview.Click += new EventHandler(UpdateReview);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            reviewId = Request.QueryString["id"];
            errMsg.InnerHtml = "";
            errMsg.Visible = false;
            if(!IsPostBack)
                    detailreview();
        }   // End of page load


        /// <summary>
        ///     Written By : Ashish G. Kamble on 3 Apr 2013
        ///     Summary : Method to show the details of the selected review
        ///     Modified By : Suresh Prajapati on 31st July 2014
        ///     Summary : modified query to retrieve IsVerified, IsDiscarded flag
        /// </summary>
        protected void detailreview()
        {
            if (!String.IsNullOrEmpty(reviewId))
            {
                Database db = null;
                DataSet ds = null;

                string sql = " SELECT StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, Pros, Cons, Comments, Title, "
                           + " IsNewlyPurchased, Familiarity, Mileage,IsVerified,IsDiscarded FROM CustomerReviews WHERE ID = " + reviewId;
                try
                {
                    db = new Database();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        ds = db.SelectAdaptQry(cmd);

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            txtExterior.Text = ds.Tables[0].Rows[0]["StyleR"].ToString();
                            exterior.Text = "/5";

                            txtComfort.Text = ds.Tables[0].Rows[0]["ComfortR"].ToString();
                            Comfort.Text = "/5";

                            txtPerformance.Text = ds.Tables[0].Rows[0]["PerformanceR"].ToString();
                            Performance.Text = "/5";

                            txtValue.Text = ds.Tables[0].Rows[0]["ValueR"].ToString();
                            Value.Text = "/5";

                            txtFuel.Text = ds.Tables[0].Rows[0]["FuelEconomyR"].ToString();
                            Fuel.Text = "/5";

                            txtPros.Text = ds.Tables[0].Rows[0]["Pros"].ToString();

                            txtCons.Text = ds.Tables[0].Rows[0]["Cons"].ToString();

                            rteDetail.Text = ds.Tables[0].Rows[0]["Comments"].ToString();

                            txtTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString();

                            Purchased.Text = ds.Tables[0].Rows[0]["IsNewlyPurchased"].ToString() == "True" ? "Yes" : "No";

                            txtFamiliarity.Text = ds.Tables[0].Rows[0]["Familiarity"].ToString();
                            Familiarity.Text = "/5";

                            txtMileage.Text = ds.Tables[0].Rows[0]["Mileage"].ToString();

                            isDiscarded = Convert.ToBoolean( ds.Tables[0].Rows[0]["IsDiscarded"]);
                            isVerified = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsVerified"]);
                            divShowReview.Visible = true;

                        }
                        else
                        {
                            divShowReview.Visible = false;
                            errMsg.InnerHtml = "<h3>Oops !! The review does not exists.</h3>";
                            errMsg.Visible = true;
                        }
                    }                    
                }
                catch (SqlException ex)
                {
                    Trace.Warn("UserReviews.detailreview Sql Ex : ", ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, "UserReviews.detailreview");
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    Trace.Warn("UserReviews.detailreview Ex : ", ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, "UserReviews.detailreview");
                    objErr.SendMail();
                }
            }
        }   // End of detailreview function


        /// <summary>
        ///     Written By : Ashish G. Kamble on 4 Apr 2013
        ///     Summary : Function to update the review
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateReview(object sender, EventArgs e)
        {
            Database db = null;
            float overallRating;

            overallRating = (Convert.ToInt32(txtExterior.Text) + Convert.ToInt32(txtComfort.Text) + Convert.ToInt32(txtPerformance.Text) + Convert.ToInt32(txtFuel.Text) + Convert.ToInt32(txtValue.Text)) / 5;

            try 
	        {
                db = new Database();

		        using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateCustomerReviews";

                    cmd.Parameters.Add("@StyleR", SqlDbType.SmallInt).Value = txtExterior.Text;
                    cmd.Parameters.Add("@ComfortR", SqlDbType.SmallInt).Value = txtComfort.Text;
                    cmd.Parameters.Add("@PerformanceR", SqlDbType.SmallInt).Value = txtPerformance.Text;
                    cmd.Parameters.Add("@FuelEconomyR", SqlDbType.SmallInt).Value = txtFuel.Text;
                    cmd.Parameters.Add("@ValueR", SqlDbType.SmallInt).Value = txtValue.Text;                
                    cmd.Parameters.Add("@OverallR", SqlDbType.SmallInt).Value = overallRating;
                    cmd.Parameters.Add("@Pros", SqlDbType.VarChar, 100).Value = txtPros.Text;
                    cmd.Parameters.Add("@Cons", SqlDbType.VarChar, 100).Value = txtCons.Text;
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 8000).Value = SanitizeHTML.ToSafeHtml(rteDetail.Text);
                    cmd.Parameters.Add("@Title", SqlDbType.VarChar, 100).Value = txtTitle.Text;
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = reviewId;
                    cmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add("@Familiarity", SqlDbType.SmallInt).Value = txtFamiliarity.Text;
                    cmd.Parameters.Add("@Mileage", SqlDbType.Float).Value = txtMileage.Text;
                    cmd.Parameters.Add("@UpdatedBy", SqlDbType.BigInt).Value = CurrentUser.Id;

                    db.UpdateQry(cmd);

                    errMsg.InnerHtml = "<h3>Record Updated Successfully.</h3>";
                    errMsg.Visible = true;
                }
	        }
	        catch (SqlException ex)
            {
                Trace.Warn("UserReviews.UpdateReview Sql Ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "UserReviews.UpdateReview");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("UserReviews.UpdateReview Ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "UserReviews.UpdateReview");
                objErr.SendMail();
            }
            detailreview();
        }   // End of UpdateReview method

    }   // End of class
}   // End of namespace