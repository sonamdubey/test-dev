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
using System.Drawing.Imaging;
using Ajax;
using System.IO;

namespace BikeWaleOpr.EditCms
{
	public class ViewImage : Page
	{
        protected string basicId, imgId, caption, hostUrl = string.Empty, imagePath = string.Empty, showImage = string.Empty;
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();			
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            if (!IsPostBack)
            {
                imagePath = Request.QueryString["imagepath"].ToString();
                hostUrl = Request.QueryString["hostUrl"].ToString();

                showImage = ImagingOperations.GetPathToShowImages(imagePath,hostUrl);
            }
            //basicId = Request.QueryString["bid"].ToString();
            ////hostUrl = GetHostUrl(basicId);
            //imgId = Request.QueryString["img"].ToString();
            //GetCaption();
		}

        private string GetHostUrl(string basicId)
        {
            throw new Exception("Method not used/commented");

            //string retVal = string.Empty, sql = string.Empty;
            //Database db = new Database();
            //SqlDataReader dr = null;
            //SqlCommand cmd = new SqlCommand();

            //sql = "SELECT hostUrl FROM Con_EditCms_Basic Where Id = @BasicId";
            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;

            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    while (dr.Read())
            //    {
            //        retVal = dr["hostUrl"].ToString();
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    Trace.Warn("SqlEX: " + ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn("EX: " + ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //Trace.Warn("retVal: " + retVal);
            //return retVal;
        }

		protected string GetImagePath()
		{
			if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
            {
				return "http://img.carwale.com/bikewale/";
			}
			else
			{
				return "";
			}
		}
		
		void GetCaption()
		{

            throw new Exception("Method not used/commented");

            //CommonOpn op = new CommonOpn();
            //Database db = new Database();
            //SqlDataReader dr = null;
		
            //string sql;
            //sql = " SELECT Caption, hostUrl"
            //    + " FROM Con_EditCms_Images"
            //    + " WHERE BasicId = @basicId AND Id = @imgId";
			
            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId),
            //    new SqlParameter("@imgId", imgId)
            //};
			
            //try
            //{
            //    dr = db.SelectQry(sql, param);
            //    while (dr.Read())
            //    {
            //        caption = dr["Caption"].ToString();
            //        hostUrl = dr["hostUrl"].ToString();
            //    }
                
            //}
            //catch(SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    if (dr != null)
            //    {
            //        dr.Close();
            //    }
            //    db.CloseConnection();
            //}
		}
	}
}