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
using Ajax;
using System.Text.RegularExpressions;
using System.Net;

namespace BikeWaleOpr.EditCms
{
	public class Summary : Page
	{
		protected string basicId, heading, isPublished, basicInfo;
        protected string HostUrl = string.Empty, imagePathThumbnail = string.Empty;
        protected bool MainImageSet = false;
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
			basicId = Request.QueryString["bid"];
            GetImageInfo(basicId);
			basicInfo = GetBasicInfo();
		}

        private void GetImageInfo(string basicId)
        {
            throw new Exception("Method not used/commented");
            //string sql = string.Empty;
            //Database db = new Database();
            //SqlDataReader dr = null;
            //SqlCommand cmd = new SqlCommand();

            //sql = " SELECT IMG.HostUrl, IMG.ImagePathThumbnail FROM Con_EditCms_Images AS IMG "
            //    + " INNER JOIN Con_EditCms_Basic AS B ON B.Id = IMG.BasicId AND IMG.IsMainImage = 1 "
            //    + " WHERE B.Id = @BasicId ";
            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;

            //Trace.Warn("Sql: " + sql);
            //Trace.Warn("BasicId: " + basicId);

            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    if (dr != null)
            //    {
            //        while (dr.Read())
            //        {
            //            HostUrl = dr["HostURL"].ToString();
            //            //MainImageSet = Convert.ToBoolean(dr["MainImageSet"]);
            //            imagePathThumbnail = dr["ImagePathThumbnail"].ToString();   // Modified by : Ashish Kamble on 11 Nov 2012
            //            Trace.Warn("imagePathThumbnail : " + imagePathThumbnail);
            //        } 
            //    }
            //    Trace.Warn("HostUrl: " + HostUrl);
            //    //Trace.Warn("MainImageSet: " + MainImageSet.ToString());
            //    Trace.Warn("URL: " + ImagingFunctions.GetImagePath("/ec/", HostUrl) + basicId + "/img/m/" + basicId + ".jpg");
            //}
            //catch (SqlException ex)
            //{
            //    Trace.Warn("SqlEx: " + ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn("Ex: " + ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}       
        }
        
		protected string GetBasicInfo()
		{
            throw new Exception("Method not used/commented");


            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql, ret = "";
			
            //sql = "SELECT CB.Id, Title, AuthorName, Description, CC.Name, CB.DisplayDate, CB.IsPublished FROM Con_EditCms_Basic CB, Con_EditCms_Category CC WHERE CC.Id = CB.CategoryId AND CB.Id = @basicId";

            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if (dr != null)
            //    {
            //        if (dr.Read())
            //        {
            //            isPublished = dr["IsPublished"].ToString().ToUpper() == "FALSE" ? "Not Published" : "Published"; ;
            //            heading = dr["Title"].ToString();
            //            ret = "<tr><td>Category </td><td>:</td><td>" + dr["Name"].ToString() + "</td></tr>";
            //            ret += "<tr><td>Display Date </td><td>:</td><td>" + dr["DisplayDate"].ToString() + "</td></tr>";
            //            ret += "<tr><td>Author </td><td>:</td><td>" + dr["AuthorName"].ToString() + "</td></tr>";
            //            string description = dr["Description"].ToString();
            //            description = StripHTML(description);
            //            if (description.Length > 100) description = description.Substring(0, 100) + " ...";
            //            ret += "<tr><td>Summary </td><td>:</td><td>" + description + "</td></tr>";
            //        } 
            //    }
				
            //    Trace.Warn(" ret = " + ret );
            //}
            //catch( SqlException err )	
            //{
            //    if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
            //    {
            //        ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //return ret;
		}
		
		private string StripHTML(string htmlString)
		{
			string pattern = @"<(.|\n)*?>";
			return Regex.Replace( htmlString, pattern, string.Empty );
		}
		
		protected string GetTaggedBikes()
		{

            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql, ret = string.Empty;
			

            //sql = " SELECT (Select Name FROM BikeMakes WHERE ID = CC.MakeId) As MakeName,"
            //      + " (Select Name FROM BikeModels WHERE ID = CC.ModelId) AS ModelName,"
            //      + " ISNULL((Select Name FROM BikeVersions WHERE ID = CC.VersionId),'') AS VersionName,"
            //      + " CC.Id As Id"
            //      + " FROM Con_EditCms_Bikes CC"
            //      + " WHERE CC.BasicId = @basicId"
            //      + " AND IsActive = 1";
            //Trace.Warn(sql);	
            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if(dr.Read())
            //    {
            //        ret = "<tr><td><b>Bikes:</b> " + dr["MakeName"].ToString() + " " + dr["ModelName"].ToString() + " " + dr["VersionName"].ToString() + "</td></tr>";		
            //    }
            //    dr.Close();
            //    if(ret == "") ret = "<tr><td><b>Bikes:</b> No Bikes Tagged </td></tr>";
            //}
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    db.CloseConnection();
            //}
            //return ret;
		}
		
		protected string GetTagNames()
		{
            throw new Exception("Method not used/commented");

            //string tagNames = string.Empty;
            //string tagNamesRetVal = string.Empty;
            //string sqlTagNames = string.Empty;
            //Database db = new Database();
            //SqlDataReader dr = null;
            //string[] tagNameList = new string[5]; // Since we are pulling only the Top 5
            //int arrCount = 0;		
			
            //try
            //{
            //    sqlTagNames = "Select Top 5 Tag From Con_EditCms_Tags T "
            //                + "Left Join Con_EditCms_BasicTags BT On BT.TagId = T.Id "
            //                + "Where BT.BasicId = @BasicId ";
							
            //    SqlParameter[] param = { new SqlParameter("@BasicId", basicId) };
				
            //    dr = db.SelectQry(sqlTagNames, param);
				
            //    while ( dr.Read() )
            //    {
            //        tagNameList[arrCount] = dr["Tag"].ToString();
            //        Trace.Warn("tagNameList : " + tagNameList[arrCount]);
            //        ++arrCount;
            //    }
            //    dr.Close();
				
            //    tagNamesRetVal = "<tr><td><b>Tags:</b> ";
            //    Trace.Warn("arrCount = " + arrCount.ToString() );
            //    if( arrCount != 0 )
            //    {
            //        tagNames = tagNameList[0];
					
					
            //        for( int i=1; i<arrCount; ++i)
            //        {
            //            tagNames += ", " + tagNameList[i];
            //            Trace.Warn("tagNames(for) = " + tagNames);
            //        }
					
            //        if( arrCount == 5 )
            //            tagNames += "...";		
            //    }
            //    else
            //    {
            //        tagNames = " No Tags Available ";
            //    }
				
            //    tagNamesRetVal += tagNames + "</td></tr>";
            //    Trace.Warn("tagNamesRetVal : " + tagNamesRetVal);
            //}
            //catch( SqlException ex )
            //{
            //    tagNamesRetVal = string.Empty;
            //    Trace.Warn(ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch( Exception ex )
            //{
            //    tagNamesRetVal = string.Empty;
            //    Trace.Warn(ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if( dr != null ){
            //        dr.Close();
            //    }
            //    db.CloseConnection();
            //}
			
            //return tagNamesRetVal;
		}		
		
		protected string GetOtherInfo()
		{

            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql, ret = "";
			
            //sql = " SELECT (SELECT FieldName FROM Con_EditCms_CategoryFields CF WHERE CF.ID = CO.CategoryFieldId) AS FieldName,"
            //    + " (CASE (ValueType) WHEN '1' THEN Convert(Varchar(50),BooleanValue) WHEN '2' THEN Convert(Varchar(50),NumericValue) WHEN '3' THEN Convert(Varchar(50),DecimalValue) WHEN '4' THEN TextValue WHEN '5' THEN Convert(Varchar(50),DateTimeValue) END) AS FieldValue"
            //    + " FROM Con_EditCms_OtherInfo CO"
            //    + " WHERE BasicId = @basicId ";

            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if (dr != null)
            //    {
            //        while (dr.Read())
            //        {
            //            ret += "<tr><td>" + dr["FieldName"].ToString() + "</td><td>:</td><td>" + dr["FieldValue"].ToString() + "</td></tr>";
            //        } 
            //    }
            //}
            //catch( SqlException err )	
            //{
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if( dr != null )
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //return ret;
		}
		
		protected string GetPages()
		{

            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql;
            //string ret = "<tr><td>@@Count Pages: ";
            //int count = 0;
			
            //sql = " SELECT PageName, Id FROM Con_EditCms_Pages WHERE BasicId = @basicId AND IsActive = 1 ORDER BY Priority ASC ";

            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if (dr != null)
            //    {
            //        while (dr.Read())
            //        {
            //            count++;
            //            ret += "<a href='FillPages.aspx?bid=" + basicId + "&pid=" + dr["Id"].ToString() + "' >" + dr["PageName"].ToString() + "</a>, ";
            //        } 
            //    }
            //    ret = ret.Substring(0,ret.Length - 2);
            //    ret += "</td></tr>";
            //    ret = ret.Replace("@@Count", count.ToString());
				
            //}
            //catch( SqlException err )	
            //{
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //return ret;
		}
		
		protected string GetImageCount()
		{
            throw new Exception("Method not used/commented");


            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql, ret = "";
			
            //sql = "SELECT COUNT(*) AS ImageCount FROM Con_EditCms_Images WHERE BasicId = @basicId AND IsActive = 1";

            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if (dr != null)
            //    {
            //        while (dr.Read())
            //        {
            //            ret = dr["ImageCount"].ToString();
            //        } 
            //    }
            //}
            //catch( SqlException err )	
            //{
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //return ret;
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

        protected bool MainImageExist(string filePath)
        {
            bool retVal = false;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(filePath);
            HttpWebResponse resp = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                Trace.Warn("resp.StatusCode: " + resp.StatusCode.ToString());
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    retVal = true;
                }                
                else
                {
                    retVal = false;
                }
                resp.Close();
            }
            catch (WebException ex)
            {
                Trace.Warn("Error: " + ex.Status.ToString());
                if ( ex.Status != WebExceptionStatus.ProtocolError)
                {
                    ErrorClass objEx = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objEx.SendMail();
                }
                retVal = false;
                if (resp != null)
                {
                    resp.Close();
                }
            }
            return retVal;
        }
	}
}