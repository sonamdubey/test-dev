using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;
using Bikewale.Common;
using System.Text.RegularExpressions;

namespace AutoExpo
{		
    public class sponsoredContent : UserControl	
    {
        protected string SMainImageSet = string.Empty, SHostURL = string.Empty, SBasicId = "8474", //8474 - Id for sponsored content in live server
            SNewsTitle = string.Empty, SAuthorName = string.Empty, SUrl = string.Empty, SDetails = string.Empty, SKnowm = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }
        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BindData();
        }

        void BindData()
        {
            SqlDataReader dr = null;
            Database db = null;

            try
            {
                db = new Database();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AutoExpo_GetSponsoredContent";

                cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = SBasicId;

                dr = db.SelectQry(cmd);
                while (dr.Read())
                {
                    SNewsTitle = dr["Title"].ToString();
                    SAuthorName = dr["AuthorName"].ToString();                    
                    SUrl = dr["Url"].ToString();
                    SDetails = dr["Description"].ToString();
                    SDetails = SDetails.Substring(0, 60).Substring(0, SDetails.Substring(0, 60).LastIndexOf(' ')) + "...";
                    SMainImageSet = dr["MainImageSet"].ToString();
                    SHostURL = dr["HostURL"].ToString();                    
                }             
            }
            catch (Exception ex)
            {
                Trace.Warn("error : "+ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                dr.Close();
                db.CloseConnection();
            }          
        }
	}
}