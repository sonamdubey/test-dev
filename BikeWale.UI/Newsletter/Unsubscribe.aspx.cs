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
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Newsletter
{
	public partial class Unsubscribe: Page
	{
        protected TextBox txtEmail;
        protected Button butUnsubscribe;
        protected HtmlGenericControl dReq, dMes;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            butUnsubscribe.Click += new EventHandler(butUnsubscribe_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("PAGE_LOAD");
        }

        void butUnsubscribe_Click(object Sender, EventArgs e)
		{
			//string sql = "";
            //Database db = new Database();
            //CommonOpn op = new CommonOpn();
            //string conStr = db.GetConString();

            //con = new SqlConnection(conStr);
            CommonOpn op = new CommonOpn();
            
			
			try
			{
                //sql = "UPDATE Con_IN_Users SET ReceiveNewsletter = 0 WHERE Email = @Email";
				
                //SqlParameter[] param = { new SqlParameter("@Email", txtEmail.Text.Trim())};
				
                //Trace.Warn(sql);
				
                //db.UpdateQry(sql, param);

				dReq.Visible = false;
				dMes.Visible = true;

                using (DbCommand cmd = DbFactory.GetDBCommand("insertdonotsendemail"))

                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, txtEmail.Text));

                    //run the command
                    MySqlDatabase.ExecuteNonQuery(cmd); 
                }
                
			}
			catch(SqlException err)
			{
				//catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch SqlException
			catch( Exception err )
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
	}//class
}//namespace