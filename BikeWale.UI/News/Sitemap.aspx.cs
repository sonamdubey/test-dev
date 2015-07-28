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

namespace Bikewale.News
{

    public partial class Sitemap : System.Web.UI.Page
    {
        private string mydomain = "http://www.bikewale.com/news/";
       
        Database db = new Database();
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlTextWriter writer = new XmlTextWriter(Server.MapPath("google-news-sitemap\\google-news-sitemap.xml"), Encoding.UTF8);
            try
            {
                DataSet ds = new DataSet();
                string constr = db.GetConString();
                CommonOpn op = new CommonOpn();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("GoogleSiteMapDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                    }
                }
                // Creating the SiteMap XML using XMLTextWriter
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "news", null, "http://www.google.com/schemas/sitemap-news/0.9");
                writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dtr;
                    int i = 0;
                    while (i < ds.Tables[0].Rows.Count)
                    {

                        dtr = ds.Tables[0].Rows[i];
                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", mydomain + dtr["BasicId"].ToString() + "-" + dtr["Url"].ToString() + ".html");
                        writer.WriteStartElement("news:news");
                        writer.WriteStartElement("news:publication");
                        writer.WriteElementString("news:name", "BikeWale");
                        writer.WriteElementString("news:language", "en");
                        writer.WriteEndElement();
                        writer.WriteElementString("news:genres", "PressRelease, Blog");
                        writer.WriteElementString("news:geo_locations", "India");
                        writer.WriteElementString("news:publication_date", Convert.ToDateTime(dtr["DisplayDate"]).ToString("yyyy-MM-ddThh:mm:sszzz"));
                        writer.WriteElementString("news:keywords", dtr["Tag"].ToString());
                        writer.WriteElementString("news:title", dtr["Title"].ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("image:image");
                        writer.WriteElementString("image:loc", "http://" + dtr["HostUrl"].ToString() + dtr["ImagePathLarge"].ToString());
                        writer.WriteElementString("image:title", dtr["Caption"].ToString());
                        writer.WriteElementString("image:caption", dtr["Caption"].ToString());
                        writer.WriteElementString("image:geo_location", "India");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        i++;
                    }
                }
                 writer.WriteEndDocument();


            }
            catch (Exception ex)
            {
                // Trace.Warn(ex.Message);
                throw ex;
            }
            finally
            {
               writer.Close();
            }

            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.ContentType = "text/html";
            //Response.Cache.SetCacheability(HttpCacheability.Public);           
        }
        
    }//class
}//namespace
                                  