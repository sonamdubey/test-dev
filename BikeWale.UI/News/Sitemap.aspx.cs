using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using Bikewale.Common;
using System.Data.Sql;
using System.Data.SqlTypes;
using Bikewale.Utility;

namespace Bikewale.News
{
    public class Sitemap : System.Web.UI.Page
    {
        private string mydomain = "http://www.bikewale.com/news/";
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlTextWriter writer = new XmlTextWriter(Server.MapPath("google-news-sitemap\\google-news-sitemap.xml"), Encoding.UTF8);
            DataTable ds = null;
            Database db = null;
            SqlConnection con = null;
            SqlDataAdapter da = null;
            DataRow dtr = null;
            try
            {
                string constr = db.GetConString();
                CommonOpn op = new CommonOpn();
                using (con = new SqlConnection(BWConfiguration.CWConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("cw.GoogleSiteMapDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ApplicationId", SqlDbType.Int).Value = Convert.ToInt32(BWConfiguration.ApplicationId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        da = new SqlDataAdapter(cmd);
                        if (da != null)
                        {
                            da.Fill(ds);
                        }
                    }
                }
                // Creating the SiteMap XML using XMLTextWriter
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "news", null, "http://www.google.com/schemas/sitemap-news/0.9");
                writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                if (ds != null && ds.Rows.Count > 0)
                {
                    int i = 0;
                    while (i < ds.Rows.Count)
                    {

                        dtr = ds.Rows[i];
                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", String.Format("{0}{1}-{2}.html", mydomain, dtr["BasicId"].ToString(), dtr["Url"].ToString()));
                        writer.WriteStartElement("news:news");
                        writer.WriteStartElement("news:publication");
                        writer.WriteElementString("news:name", "BikeWale");
                        writer.WriteElementString("news:language", "en");
                        writer.WriteEndElement();
                        writer.WriteElementString("news:genres", "PressRelease, Blog");
                        writer.WriteElementString("news:geo_locations", "India");
                        writer.WriteElementString("news:publication_date", Convert.ToDateTime(dtr["DisplayDate"]).ToString("yyyy-MM-ddThh:mm:sszzz"));
                        writer.WriteElementString("news:keywords", Convert.IsDBNull(dtr["Tag"]) ? "" : dtr["Tag"].ToString());
                        writer.WriteElementString("news:title", dtr["Title"].ToString());
                        writer.WriteEndElement();
                        if (!Convert.IsDBNull(dtr["HostUrl"]))
                        {
                            writer.WriteStartElement("image:image");
                            writer.WriteElementString("image:loc", dtr["HostUrl"].ToString() + ImageSize._174x98 + dtr["OriginalImgPath"].ToString());
                            writer.WriteElementString("image:title", dtr["Caption"].ToString());
                            writer.WriteElementString("image:caption", dtr["Caption"].ToString());
                            writer.WriteElementString("image:geo_location", "India");
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        i++;
                    }
                }
                writer.WriteEndDocument();
            }

            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                writer.Close();
                if (con != null)
                {
                    con.Close();
                }
            }
        }
    }//class
}//namespace
