using System;
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
using System.Data.Common;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;

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
            if (!IsPostBack)
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

                string sql = @" SELECT StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, Pros, Cons, Comments, Title, 
                            IsNewlyPurchased, Familiarity, Mileage,IsVerified,IsDiscarded from customerreviews where id = " + reviewId;
                try
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
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

                                isDiscarded = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDiscarded"]);
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
            float overallRating;

            overallRating = (Convert.ToInt32(txtExterior.Text) + Convert.ToInt32(txtComfort.Text) + Convert.ToInt32(txtPerformance.Text) + Convert.ToInt32(txtFuel.Text) + Convert.ToInt32(txtValue.Text)) / 5;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "updatecustomerreviews";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_styler", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtExterior.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comfortr", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtComfort.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performancer", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtPerformance.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueleconomyr", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtFuel.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_valuer", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtValue.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallr", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], overallRating));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pros", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtPros.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cons", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtCons.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 8000, SanitizeHTML.ToSafeHtml(rteDetail.Text)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_title", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtTitle.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_familiarity", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtFamiliarity.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mileage", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtMileage.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], CurrentUser.Id));

                    if (MySqlDatabase.UpdateQuery(cmd))
                    {
                        errMsg.InnerHtml = "<h3>Record Updated Successfully.</h3>";
                        errMsg.Visible = true;
                    }

                    
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