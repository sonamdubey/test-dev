using Bikewale.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.MyBikeWale
{
    public class NewsSubscription : System.Web.UI.Page
    {
        protected Button btnSave;
        protected CheckBox chkNewsLetter;
        public string customerId = string.Empty;
        protected HtmlGenericControl spnError;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Page.Validate();
            //if (!IsValid)
            //    return;

            if (SaveNewsSubscription() == true)
            {

                if (Request["returnUrl"] != null && Request["returnUrl"].ToString() != "")
                    Response.Redirect(Request["returnUrl"].ToString());
                //else
                //Response.Redirect("/users/newssubscription.aspx");
            }

        }

        private bool SaveNewsSubscription()
        {
            bool returnVal = false;
            SqlParameter prm = null;
            SqlConnection con = null;

            try
            {
                Database db = new Database();
                string conStr = db.GetConString();

                using (con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("SubscribeNewsletter", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
                    prm.Value = customerId;

                    prm = cmd.Parameters.Add("@ReceiveNewsletters", SqlDbType.Bit);
                    prm.Value = chkNewsLetter.Checked;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    returnVal = true;

                    if (returnVal)
                    {
                        if (chkNewsLetter.Checked)
                            spnError.InnerHtml = "Your will recieve bikewale news updates.";
                        else
                            spnError.InnerHtml = "Your will not recieve bikewale news updates.";
                    }

                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                returnVal = false;
            } // catch SqlException
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                returnVal = false;
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return returnVal;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect("/users/login.aspx?returnUrl=/users/newssubscription.aspx");

            customerId = CurrentUser.Id;

            if (!IsPostBack)
                chkNewsLetter.Checked = IsNewsLetterSubscribed();
        }

        private bool IsNewsLetterSubscribed()
        {
            Database db = null;
            SqlDataReader dr = null;
            bool isNewsSubscribed = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetNewsSubscription"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

                    db = new Database();

                    dr = db.SelectQry(cmd);

                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            isNewsSubscribed = Convert.ToBoolean(dr["SubscribeNewsletters"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
            }

            return isNewsSubscribed;
        }
    }
}