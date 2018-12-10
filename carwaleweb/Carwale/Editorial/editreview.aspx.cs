using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace Carwale.UI.Editorial
{
    public class EditReview : Page
    {
        protected Label lblMessage, lblTitle, lblPros, lblCons, lblDescription, lblAuthorId, lblIsVerified;
        protected Button butSave;
        protected TextBox txtTitle, txtPros, txtCons, txtMileage, txtUpdateReason;
        public string modelId = "", versionId = "", customerId = "";
        protected HtmlInputHidden hdnRateST, hdnRateCM, hdnRatePE, hdnRateVC, hdnRateFE, hdnRateOA;
        protected DropDownList drpVersions, ddlFamiliar;
        protected RichTextEditor ftbDescription;
        protected HtmlGenericControl divVersions, divEmail;
        protected RadioButton radNot, radNew, radOld;

        protected string display, leftColumnWidth;

        private string authorName = "", authorEmail = "";

        protected bool isModerator = false;

        public string reviewId = "";

        public string CarName
        {
            get
            {
                if (ViewState["CarName"] != null)
                    return ViewState["CarName"].ToString();
                else
                    return "";
            }
            set { ViewState["CarName"] = value; }
        }

        public string CarMake
        {
            get
            {
                if (ViewState["CarMake"] != null)
                    return ViewState["CarMake"].ToString();
                else
                    return "";
            }
            set { ViewState["CarMake"] = value; }
        }

        public string CarModel
        {
            get
            {
                if (ViewState["CarModel"] != null)
                    return ViewState["CarModel"].ToString();
                else
                    return "";
            }
            set { ViewState["CarModel"] = value; }
        }

        public string CarVersion
        {
            get
            {
                if (ViewState["CarVersion"] != null)
                    return ViewState["CarVersion"].ToString();
                else
                    return "";
            }
            set { ViewState["CarVersion"] = value; }
        }

        public string MakeId
        {
            get
            {
                if (ViewState["MakeId"] != null)
                    return ViewState["MakeId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["MakeId"] = value; }
        }

        public string ModelIdVer
        {
            get
            {
                if (ViewState["ModelIdVer"] != null)
                    return ViewState["ModelIdVer"].ToString();
                else
                    return "-1";
            }
            set { ViewState["ModelIdVer"] = value; }
        }

        public string BackUrl
        {
            get
            {
                if (ViewState["BackUrl"] != null)
                    return ViewState["BackUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["BackUrl"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            butSave.Click += new EventHandler(butSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (Request.QueryString["rid"] != null && CommonOpn.CheckId(Request.QueryString["rid"]))
                reviewId = Request.QueryString["rid"];
            else
                Response.Redirect("/research/");

            ForumsCache threadInfo = new ForumsCache();
            isModerator = threadInfo.IsModerator(CurrentUser.Id);

            if (isModerator)
            {
                display = "none;";
                leftColumnWidth = "110";
            }
            else
            {
                display = "display;";
                leftColumnWidth = "250";
            }

            if (!GetLoginStatus(CurrentUser.Id, reviewId) && !(isModerator))
            {
                Trace.Warn("Not a correct user");
                UrlRewrite.Return404();
            }

            if (!IsPostBack)
            {
                ddlFamiliar.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlFamiliar.Items.Insert(1, new ListItem("Haven’t driven it", "1"));
                ddlFamiliar.Items.Insert(2, new ListItem("Have done a short test-drive once", "2"));
                ddlFamiliar.Items.Insert(3, new ListItem("Have driven for a few hundred kilometres", "3"));
                ddlFamiliar.Items.Insert(4, new ListItem("Have driven a few thousands kilometres", "4"));
                ddlFamiliar.Items.Insert(5, new ListItem("It’s my mate since ages", "5"));

                FillData(reviewId);
                GetCar();
            }
        } // Page_Load

        bool GetLoginStatus(string userId, string postId)
        {
            bool userLoggedIn = false;
            try
            {          
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerIdByReviewId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, postId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            if (dr["CustomerId"].ToString() == userId)
                            {
                                userLoggedIn = true;
                            }
                        }
                    }
                }           
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                userLoggedIn = false;
            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return userLoggedIn;
        }


        void FillData(string reviewId)
        {
            if (reviewId != "" && reviewId != null)
            {
                DataSet ds = new DataSet();        
                try 
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("GetUserReviewToEdit_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int64, reviewId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerator", DbType.Boolean, isModerator));
                        using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                        {
                            if (dr.Read())
                            {
                                lblAuthorId.Text = dr["CustomerId"].ToString();

                                if (Convert.ToBoolean(dr["IsVerified"]) == true)
                                {
                                    lblIsVerified.Text = "1";
                                }
                                else
                                {
                                    lblIsVerified.Text = "0";
                                }

                                txtPros.Text = dr["Pros"].ToString();
                                lblPros.Text = dr["Pros"].ToString();
                                txtCons.Text = dr["Cons"].ToString();
                                lblCons.Text = dr["Cons"].ToString();
                                ftbDescription.Text = dr["Comments"].ToString();
                                lblDescription.Text = dr["Comments"].ToString();
                                txtTitle.Text = dr["Title"].ToString();
                                lblTitle.Text = dr["Title"].ToString();
                                hdnRateST.Value = dr["StyleR"].ToString();
                                hdnRateCM.Value = dr["ComfortR"].ToString();
                                hdnRatePE.Value = dr["PerformanceR"].ToString();
                                hdnRateVC.Value = dr["ValueR"].ToString();
                                hdnRateFE.Value = dr["FuelEconomyR"].ToString();
                                hdnRateOA.Value = dr["OverallR"].ToString();
                                versionId = dr["VersionId"].ToString();
                                modelId = dr["ModelId"].ToString();

                                if (dr["Familiarity"].ToString() != "")
                                    ddlFamiliar.SelectedValue = dr["Familiarity"].ToString();
                                if (dr["Mileage"].ToString() != "")
                                    txtMileage.Text = dr["Mileage"].ToString();

                                if (txtMileage.Text == "0")
                                    txtMileage.Text = "";


                                if (dr["IsNewlyPurchased"].ToString() != "" && dr["IsOwned"].ToString() != "")
                                {
                                    if (!(Convert.ToBoolean(dr["IsOwned"])))
                                    {
                                        radNew.Checked = false;
                                        radOld.Checked = false;
                                        radNot.Checked = true;
                                    }
                                    else if (Convert.ToBoolean(dr["IsNewlyPurchased"]) == true)
                                    {
                                        radNew.Checked = true;
                                        radOld.Checked = false;
                                        radNot.Checked = false;
                                    }
                                    else if (Convert.ToBoolean(dr["IsNewlyPurchased"]) == false)
                                    {
                                        radNew.Checked = false;
                                        radOld.Checked = true;
                                        radNot.Checked = false;
                                    }
                                }
                                else
                                {
                                    radNew.Checked = false;
                                    radOld.Checked = false;
                                    radNot.Checked = false;
                                }
                            }
                        }
                    }                  
                }
                catch (Exception err)
                {
                    Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }              
            }
        }

        private string GetThreadIdForReview()
        {
            string returnVal = "-1";    
            try
            {         
                using (DbCommand cmd = DbFactory.GetDBCommand("GetThreadIdForReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId",DbType.Int64,reviewId != "" ? reviewId : "-1"));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            returnVal = dr[0].ToString();
                        }
                    }
                }
               
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                returnVal = "-1";
            }   
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                returnVal = "-1";
            }      
            return returnVal;
        }


        void butSave_Click(object Sender, EventArgs e)
        {
            if (isModerator)
            {
                if (UpdateDetails())
                {
                    AddToLog();
                    SendMailToCustomer();
                    string threadId = GetThreadIdForReview();
                    if (threadId != "-1")
                    {
                        string remoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? null : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? null : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                        PostDAL postDetails = new PostDAL();
                        postDetails.SavePost(CurrentUser.Id, "This review has been edited by moderator", 1, threadId, 1, remoteAddr, clientIp);
                    }
                    Response.Redirect("/research/userreviews/reviewdetails.aspx?rid=" + reviewId);
                }
            }
            else
            {
                SaveUpdatesToReplica();
                Response.Redirect("/research/userreviews/reviewupdatemessage.aspx");
            }
        }

        private void SaveUpdatesToReplica()
        {
            string recordId = "";   
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("EntryCustomerReviewsReplica_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StyleR", DbType.Int16, hdnRateST.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ComfortR", DbType.Int16, hdnRateCM.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PerformanceR", DbType.Int16, hdnRatePE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ValueR", DbType.Int16, hdnRateVC.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelEconomyR", DbType.Int16, hdnRateFE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OverallR", DbType.Decimal, (Convert.ToInt32(hdnRateST.Value) + Convert.ToInt32(hdnRateCM.Value) + Convert.ToInt32(hdnRatePE.Value) + Convert.ToInt32(hdnRateVC.Value) + Convert.ToInt32(hdnRateFE.Value)) / 5));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Pros",DbType.String,100,txtPros.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Cons", DbType.String, 100, txtCons.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comments", DbType.String, 8000, SanitizeHTML.ToSafeHtml(ftbDescription.Text.Trim())));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Title", DbType.String, 100, txtTitle.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsOwned", DbType.Boolean, !(radNot.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsNewlyPurchased", DbType.Boolean, radNew.Checked ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Familiarity", DbType.Int32, ddlFamiliar.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mileage", DbType.Decimal, txtMileage.Text.Trim() != "" ? txtMileage.Text.Trim().Replace("'", "''") : "0"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID",DbType.Int64,ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    recordId = cmd.Parameters["v_ID"].Value.ToString();
                }            
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception         
        }

        private void AddToLog()
        {       
            try
            {     
                using (DbCommand cmd = DbFactory.GetDBCommand("LogCustomerReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId",DbType.Int64,reviewId != "" ? reviewId : "-1"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UpdatedBy",DbType.Int64,CurrentUser.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UpdatedDateTime", DbType.DateTime, DateTime.Now));
                    MySqlDatabase.InsertQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (MySqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }    
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }       
        }

        private void SendMailToCustomer()
        {

            authorName = "";
            authorEmail = "";
            GetAuthorNameAndEmail();
            string subject = "Your " + CarName + " review on CarWale is edited";
            string email = authorEmail;
            StringBuilder sb = new StringBuilder();
            sb.Append("<p>Dear " + authorName + ",</p>");
            sb.Append("<p>Your review on <b>" + CarName + "</b> at CarWale.com has been edited by the moderator for &quot;" + txtUpdateReason.Text.Trim().Replace("<p>", "").Replace("</p>", "") + "&quot;. For your ready reference, we are attaching the previous copy of your review.</p>");
            sb.Append("<p>Your previous review:</p>");
            sb.Append("<p><b>Title : </b> &nbsp; " + lblTitle.Text + "</p>");
            sb.Append("<p><b>Pros : </b> &nbsp; " + lblPros.Text + "</p>");
            sb.Append("<p><b>Cons : </b> &nbsp; " + lblCons.Text + "</p>");
            sb.Append("<p><b>Detailed Review : </b> <br/> " + lblDescription.Text + "</p>");
            sb.Append("<p>Please feel free to write to community@carwale.com for any questions/clarifications you may have. Please do NOT reply to this email.</p>");
            sb.Append("<br>Warm regards,<br>");
            sb.Append("<b>CarWale Community Mods Team</b>");

            CommonOpn op = new CommonOpn();
            op.SendMail(email, subject, sb.ToString(), true);
        }

        private void GetAuthorNameAndEmail()
        {
            try
            {             
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.GetCustomerNameEmailById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID", DbType.Int64, lblAuthorId.Text != "" ? lblAuthorId.Text : "-1"));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                        authorName = dr["Name"].ToString();
                        authorEmail = dr["Email"].ToString();
                        }
                    }                    
                }                
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();             
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }   
      
        }

        bool UpdateDetails()
        {
            bool isUpdated = false;
            string recordId = "";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("EditCustomerReviews_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id",DbType.Int64,reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StyleR",DbType.Int16,hdnRateST.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ComfortR",DbType.Int16,hdnRateCM.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PerformanceR",DbType.Int16,hdnRatePE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ValueR",DbType.Int16,hdnRateVC.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelEconomyR",DbType.Int16,hdnRateFE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OverallR",DbType.Decimal,(Convert.ToInt32(hdnRateST.Value) + Convert.ToInt32(hdnRateCM.Value) + Convert.ToInt32(hdnRatePE.Value) + Convert.ToInt32(hdnRateVC.Value) + Convert.ToInt32(hdnRateFE.Value)) / 5));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Pros",DbType.String,100,txtPros.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Cons",DbType.String,100, txtCons.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comments",DbType.String,8000,SanitizeHTML.ToSafeHtml(ftbDescription.Text.Trim())));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Title",DbType.String,100,txtTitle.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LastUpdatedOn",DbType.DateTime,DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LastUpdatedBy",DbType.Int64,!isModerator ? CurrentUser.Id : lblIsVerified.Text == "1" ? CurrentUser.Id : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsOwned",DbType.Boolean,!(radNot.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsNewlyPurchased",DbType.Boolean,radNew.Checked ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Familiarity",DbType.Int32,ddlFamiliar.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mileage",DbType.Decimal,txtMileage.Text.Trim() != "" ? txtMileage.Text.Trim().Replace("'", "''") : "0" ));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsVerified",DbType.Boolean,!isModerator ? 0 :lblIsVerified.Text == "1" ? 1 : 0 ));               
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Status",DbType.Int32,ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    recordId = cmd.Parameters["v_Status"].Value.ToString();
                }
                if (recordId != "" && recordId != null)
                {
                    isUpdated = true;
                }
            }
            catch (MySqlException err)
            {          
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                isUpdated = false;
            } // catch SqlException
            catch (Exception err)
            {         
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                isUpdated = false;
            } // catch Exception          
            return isUpdated;
        }

        void GetCar()
        {    
            try
            {        
                using (DbCommand cmd = DbFactory.GetDBCommand("GetBasicCarDetailsById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, !string.IsNullOrEmpty(modelId) ? modelId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, !string.IsNullOrEmpty(versionId) ? versionId : Convert.DBNull));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            CarName = dr["Make"].ToString() + " " + dr["Model"].ToString() + " " + dr["Version"].ToString();
                        }
                    }
                }             
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

    } // class
} // namespace