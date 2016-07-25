using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls; 
using System.Drawing.Imaging;
using Ajax;
using System.IO;

namespace BikeWaleOpr.EditCms
{
	public class ShowAlbum : Page
	{
		protected DataList dlstPhoto;
		protected string basicId = "";
		protected HtmlTable tbThumbNail, tbMedium, tbLarge;
        protected Repeater rpt_ShowPhotos;
        //HtmlTableRow objTrTn;
        //HtmlTableCell objTd;
		protected int tnCount = 0, mCount = 0;
		
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
			//CommonOpn op = new CommonOpn();
			Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/fillpages.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/fillpages.aspx");
				
            //int pageId = 53;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
				
			if( Request.QueryString["bid"] != null && Request.QueryString["bid"].ToString() != "")
			{
				basicId = Request.QueryString["bid"].ToString();
			}
			FillAlbum();/**/
			
		}

        protected void FillAlbum()
        {
            throw new Exception("Method not used/commented");

            //Database db = null;
            //DataSet ds = null;

            //string sql = " SELECT CEI.Id, CEI.BasicID, CEI.Caption, CEI.HostURL,CEi.ImagePathThumbnail, CEI.ImagePathLarge "
            //           + " FROM Con_EditCms_Images CEI "
            //           + " WHERE CEI.BasicId = " + basicId + " AND IsActive = 1 AND CEi.ImagePathThumbnail IS NOT NULL";

            //try 
            //{	        
            //    db = new Database();

            //    using(SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = sql;

            //        ds = db.SelectAdaptQry(cmd);

            //        if (ds != null && ds.Tables[0].Rows.Count > 0)
            //        {
            //            rpt_ShowPhotos.DataSource = ds.Tables[0];
            //            rpt_ShowPhotos.DataBind();
            //        }
            //    }
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
        }

        protected string ShowImage(string hostUrl, string imagePath)
        {
            string fullImagePath = String.Empty;

            if (!String.IsNullOrEmpty(hostUrl) && String.IsNullOrEmpty(imagePath))
            {
                fullImagePath = BikeWaleOpr.ImagingOperations.GetPathToShowImages(imagePath,hostUrl);
            }
            else
            {
                fullImagePath = "";
            }
            return fullImagePath;
        }
		

        /*  Code commented By : Ashish G. Kamble on 12/9/2012   */
        //void FillAlbum()
        //{
        //    Trace.Warn("FillAlbum");
        //    CommonOpn op = new CommonOpn();
        //    SqlDataReader dr =  null;
        //    Database db = new Database();
		
        //    string sql;
        //    sql = " SELECT CEI.Id, CEI.BasicID, CEI.Caption, CEI.HostURL"
        //        + " FROM Con_EditCms_Images CEI"
        //        + " WHERE CEI.BasicId = @basicId AND IsActive = 1";
			
        //    SqlParameter [] param = 
        //    {
        //        new SqlParameter("@basicId", basicId)
        //    };
			
        //    try
        //    {                
        //        dr = db.SelectQry( sql, param );
        //        while(dr.Read())
        //        {                    
        //            FillThumbNail(dr["Id"].ToString(), dr["HostURL"].ToString());
        //        }                
        //    }
        //    catch(SqlException err)
        //    {
        //        Trace.Warn(err.Message + err.Source);
        //        ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
        //        objErr.ConsumeError();
        //    }// catch Exception
        //    finally
        //    {
        //        if( dr != null )
        //            dr.Close();
        //        db.CloseConnection();
        //    }
        //}
		
        //protected string GetImagePath()
        //{
        //    if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
        //    {
        //        return "http://img.carwale.com/bikewale/";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
		
        ////Function to display thumbnail images
        //void FillThumbNail(string imgId, string hostUrl)
        //{
        //    objTd = new HtmlTableCell();
        //    HtmlImage img = new HtmlImage();
        //    objTrTn = new HtmlTableRow();
        //    string filePath = "";	
			
        //    filePath = ImagingOperations.GetPathToShowImages("//bikewaleimg//ec//", hostUrl) + basicId + "/img/t/" + imgId + ".jpg";
			
        //    Trace.Warn("filePath: " + filePath);
        //   // Trace.Warn("SERver:" + Server.MapPath(filePath));

        //    HttpWebRequest reqt = (HttpWebRequest)WebRequest.Create(filePath);
        //    HttpWebRequest reql = (HttpWebRequest)WebRequest.Create(filePath.Replace("/t/", "/l/"));
        //    HttpWebResponse respt = null;
        //    HttpWebResponse respl = null;
        //    try
        //    {
        //        respt = (HttpWebResponse)reqt.GetResponse();
        //        respl = (HttpWebResponse)reql.GetResponse();

        //        Trace.Warn("respt.StatusCode: " + respt.StatusCode.ToString());

        //        if (respt.StatusCode == HttpStatusCode.OK)
        //        {
        //            Trace.Warn("FileExists");
        //            img.Attributes.Add("src", ImagingOperations.GetPathToShowImages("//bikewaleimg//ec//", hostUrl) + basicId + "/img/t/" + imgId + ".jpg");
        //            objTd.Controls.Add(img);
        //            objTd.Style.Add("border", "1px solid #DBDBCE");
        //            objTd.Style.Add("padding", "10px");
        //            //if(tnCount % 4 == 0)
        //            //				{ 
        //            //					objTrTn = new HtmlTableRow();
        //            //				}
        //            if (respl.StatusCode == HttpStatusCode.OK)
        //            {
        //                Trace.Warn("LargeFileFileExists");
        //                HtmlAnchor objA = new HtmlAnchor();
        //                objA.InnerHtml = "View Large Image";
        //                objA.Attributes.Add("onClick", "javascript:window.open('ViewImage.aspx?bid=" + basicId + "&img=" + imgId + "','Large_Image','width=600px,height=600px,top=200px,left=600px')");
        //                objA.Style.Add("cursor", "pointer");
        //                objA.Style.Add("Padding-left", "35px");
        //                objTd.Controls.Add(new LiteralControl("</br>"));
        //                objTd.Controls.Add(objA);
        //            }
        //            objTrTn.Cells.Add(objTd);
        //            tbThumbNail.Controls.Add(objTrTn);
        //            //tnCount++;
        //        }
        //        respl.Close();
        //        respt.Close();
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Status != WebExceptionStatus.ProtocolError)
        //        {
        //            ErrorClass objEx = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //            objEx.SendMail();
        //        }
        //        if (respt != null)
        //        {
        //            respt.Close();
        //        }
        //        if (respl != null)
        //        {
        //            respl.Close();
        //        }
        //        Trace.Warn("WebEx: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objEx = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objEx.SendMail();
        //        if (respt != null)
        //        {
        //            respt.Close();
        //        }
        //        if (respl != null)
        //        {
        //            respl.Close();
        //        }

        //        Trace.Warn("Ex: " + ex.Message);
        //    }
		//}
	}
}