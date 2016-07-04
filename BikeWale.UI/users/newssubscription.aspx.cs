using Bikewale.Common;
using Bikewale.Notifications.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("SubscribeNewsletter"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_receivenewsletters", DbType.Boolean, chkNewsLetter.Checked));

                    MySqlDatabase.ExecuteNonQuery(cmd);
                    Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);

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
            bool isNewsSubscribed = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewssubscription"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                isNewsSubscribed = Convert.ToBoolean(dr["SubscribeNewsletters"]);
                            }
                            dr.Close();
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

            return isNewsSubscribed;
        }
    }
}