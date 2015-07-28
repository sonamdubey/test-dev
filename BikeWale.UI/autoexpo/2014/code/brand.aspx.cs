using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Text.RegularExpressions;


namespace AutoExpo
{
    public class BrandDetails : Page
    {

        protected Repeater rptNews, rptUpcoming, rptModels;
        protected string[] vidArr = new string[3], imgArr = new string[8], vidIdArr = new string[3], vidUrl=new string[3];
        protected Label lblDesc;
        protected HtmlGenericControl divNews, divGallery, divArticle, video1, video2, image1, image2, divUpcoming, divModels,errorMsg;
        protected string makeMaskingName = string.Empty, articleTitle = string.Empty, articleDesc = string.Empty, articleImg = string.Empty, makeDesc = string.Empty, articleUrl=string.Empty;
        protected string makeId = "-1", MakeMappingName = string.Empty,makeName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            //rptNews = (Repeater)rpgNews.FindControl("rptNews");
            InitializeComponent();
        }

        void InitializeComponent()
        {

            base.Load += new EventHandler(Page_Load);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["mId"] != null && Request.QueryString["mId"].ToString() != "")
            {
               //Trace.Warn("makeid is:" + makeId);
                makeId = Request.QueryString["mId"].ToString();
                //Trace.Warn("makeid1 is:" + makeId);
            }
            else
            {
                //Trace.Warn("makeid2 is:" + makeId);
                Response.Redirect("http://bikewale.com/autoexpo/2014/");
            }
            errorMsg.Visible=false;
            BindData(makeId);
            BindImageGalleryData(makeId);
            ShowMsg();
            GetMakeMaskingName();
        }


        void BindData(string makeId)
        {
            DataSet ds = new DataSet();
            Database db = new Database();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "cw.AutoExpo_GetBrandDetails";
                cmd.Parameters.Add("@MakeId", SqlDbType.Decimal).Value = makeId;
                ds = db.SelectAdaptQry(cmd);

               
                //Trace.Warn("1");
                if (ds.Tables[0].Rows.Count > 0) //news
                {
                    rptNews.DataSource = ds.Tables[0];
                    rptNews.DataBind();
                }
                else
                {
                    divNews.Visible = false;
                }
                //Trace.Warn("2");
                if (ds.Tables[1].Rows.Count > 0) //description
                {
                    makeDesc = ds.Tables[1].Rows[0]["MDescription"].ToString();
                }
                
                //Trace.Warn("3");
                if (ds.Tables[2].Rows.Count > 0) //upcoming
                {
                    rptUpcoming.DataSource = ds.Tables[2];
                    rptUpcoming.DataBind();
                }
                else
                {
                    divUpcoming.Visible = false;
                }

                //Trace.Warn("4");
                if (ds.Tables[3].Rows.Count > 0) //article
                {
                    articleDesc = ds.Tables[3].Rows[0]["Description"].ToString();
                    articleTitle = ds.Tables[3].Rows[0]["Title"].ToString();
                    articleImg = ds.Tables[3].Rows[0]["HostUrl"].ToString() + ds.Tables[3].Rows[0]["ImagePathLarge"].ToString();
                    articleUrl = "/autoexpo/2014/" + ds.Tables[3].Rows[0]["BasicId"].ToString() + "-" + ds.Tables[3].Rows[0]["Url"].ToString() + ".html";
                }
                else
                {
                    divArticle.Visible = false;
                }
                //Trace.Warn("5");
                if (ds.Tables[4].Rows.Count > 0) //models
                {
                    makeMaskingName = ds.Tables[4].Rows[0]["MakeMaskingName"].ToString();

                    makeName = ds.Tables[4].Rows[0]["Make"].ToString();
                    rptModels.DataSource = ds.Tables[4];
                    rptModels.DataBind();
                }
                else
                {
                    divModels.Visible = false;
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

        }

        /// <summary>
        /// 
        /// </summary>
        void BindImageGalleryData(string makeId)
        {
            
            DataSet ds = new DataSet();
            Database db = new Database();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "cw.AutoExpo_ImageGallery_V2";
                cmd.Parameters.Add("@MakeId", SqlDbType.Decimal).Value = makeId;
                ds = db.SelectAdaptQry(cmd);


                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            vidUrl[i] = ds.Tables[0].Rows[i]["videoUrl"].ToString();
                            vidIdArr[i] = ds.Tables[0].Rows[i]["videoId"].ToString(); //FindSubString(ds.Tables[0].Rows[i]["videoUrl"].ToString(), "/embed/","?");
                            vidArr[i]   = "http://www.youtube.com/embed/" + vidIdArr[i] + "?rel=0&amp;wmode=transparent";

                        }
                    }
                    //show all images
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        imgArr[i] = "http://" + ds.Tables[1].Rows[i]["hosturl"].ToString() + ds.Tables[1].Rows[i]["imagepathlarge"].ToString();

                    }


                    if (!string.IsNullOrEmpty(vidUrl[0]))
                    {
                        //Trace.Warn("a");
                        video1.Visible = true;
                        image1.Visible = false;
                    }
                    else
                    {
                        //Trace.Warn("b");
                        video1.Visible = false;
                        image1.Visible = true;
                    }


                    if (!string.IsNullOrEmpty(vidUrl[1]))
                    {
                        video2.Visible = true;
                        image2.Visible = false;
                    }
                    else 
                    {
                        video2.Visible = false;
                        image2.Visible = true;
                    }

                }
                else
                {
                    //hide div
                    divGallery.Visible = false;

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
        }

        public string TruncateDesc(string _desc,string len)
        {
            _desc = Regex.Replace(_desc, @"<[^>]+>", string.Empty);

            if (_desc.Length < Convert.ToInt32(len))
                return _desc;
            else
            {
                _desc = _desc.Substring(0, Convert.ToInt32(len));
                _desc = _desc.Substring(0, _desc.LastIndexOf(" "));
                return _desc + " ...";
            }
        }

        public string FormatSpecial(string url)
        {
            string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }

        protected string FindSubString(string str, string strFrom, string strTo)
        {
            int pFrom = str.IndexOf(strFrom) + strFrom.Length; ;
            int pTo = str.IndexOf(strTo);
            return str.Substring(pFrom, pTo - pFrom);
        }

        void ShowMsg()
        {
            if ((divArticle.Visible == false) && (divGallery.Visible == false) && (divNews.Visible == false))
            {
                errorMsg.Visible = true;
            }
        }

        public void GetMakeMaskingName()
        {
            try
            {
                Database db = new Database();
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetMakeMappingNameUsingMakeId";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = makeId;
                        cmd.Parameters.Add("@MakeMappingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MakeMappingName = cmd.Parameters["@MakeMappingName"].Value.ToString();
                        Trace.Warn("Make Mapping Name : ",MakeMappingName);
                        
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
        }
    }//class
}//namespace