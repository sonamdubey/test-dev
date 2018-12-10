using Carwale.BL.Videos;
using Carwale.Cache.CMS;
using AEPLCore.Cache;
using Carwale.Entity.CMS;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Carwale.UI.Editorial
{
    public class VideoDetails : System.Web.UI.Page
    {
        //YouTubeVideos obj = new YouTubeVideos();
        IUnityContainer uContainer = new UnityContainer();

        //
        protected string views = string.Empty, likes = string.Empty, videoUrl = string.Empty, maskingName = string.Empty,
                        description = string.Empty, videoId = string.Empty, basicId = string.Empty, videoTitle = string.Empty, tags = string.Empty, makeName = string.Empty, modelName = string.Empty, orgMakeName = string.Empty, orgModelName = string.Empty, videoTitleUrl = string.Empty, subCatName = string.Empty;

        protected Label lblViews, lblLikes, lblDescription, lblTags;
        protected Repeater rptRelatedVid;
        protected HtmlGenericControl divTags, divRelatedVid;
        protected string metaTitle = string.Empty, metaDescription = string.Empty, subCatId = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (DeviceDetectionManager.IsMobile(new HttpContextWrapper(HttpContext.Current)))
                Response.Redirect("/m" + Request.ServerVariables["HTTP_X_REWRITE_URL"]);
            if (!String.IsNullOrEmpty(Request.QueryString["basicId"].ToString()))
            {
                basicId = Request.QueryString["basicId"].ToString();
            }
            if (!IsPostBack)
            {
                //Trace.Warn("string is : " + FindSubString(videoUrl, "/embed/", "?"));
                BindData();
                BindRelatedVideos();
                RetrieveMetaTags(Convert.ToInt32(subCatId), orgMakeName, orgModelName);
                //Trace.Warn("subcat2 is:" +FormatSubCat("Car Review "));
            }
        }

        void BindData()
        {
            try
            {
                var repoObj = UnityBootstrapper.Resolve<IVideosBL>();
                Carwale.Entity.CMS.Video vidObj = repoObj.GetVideoByBasicId(Convert.ToInt32(basicId), Entity.CMS.CMSAppId.Carwale);
                videoTitle = vidObj.VideoTitle;
                videoUrl = vidObj.VideoUrl + "&autoplay=1";
                views = vidObj.Views.ToString(); ;
                likes = vidObj.Likes.ToString(); ;
                description = vidObj.Description;
                tags = vidObj.Tags;
                orgMakeName = vidObj.MakeName;
                orgModelName = vidObj.ModelName;
                videoTitleUrl = vidObj.VideoTitleUrl;
                makeName = UrlRewrite.FormatSpecial(vidObj.MakeName);
                modelName = UrlRewrite.FormatSpecial(vidObj.ModelName);
                maskingName = vidObj.MaskingName;
                videoId = vidObj.VideoId;
                lblViews.Text = Convert.ToInt32(views).ToString("#,##0");
                lblLikes.Text = Convert.ToInt32(likes).ToString("#,##0");
                lblDescription.Text = description;
                subCatId = vidObj.SubCatId;
                lblTags.Text = tags;
                subCatName = UrlRewrite.FormatSpecial(vidObj.SubCatName);
                Trace.Warn("subcatname is : " + subCatName);
                if (tags == string.Empty)
                {
                    divTags.Visible = false;
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

        public void BindRelatedVideos()
        {
            try
            {
                IVideosBL blObj = UnityBootstrapper.Resolve<VideosBL>();

                List<Carwale.Entity.CMS.Video> lstobj = blObj.GetSimilarVideos(Convert.ToInt32(basicId), CMSAppId.Carwale, 3);

                if (lstobj.Count > 0)
                {
                    rptRelatedVid.DataSource = lstobj;
                    rptRelatedVid.DataBind();
                }
                else
                {
                    divRelatedVid.Visible = false;
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


        protected string FindSubString(string str, string strFrom, string strTo)
        {
            int pFrom = str.IndexOf(strFrom) + strFrom.Length; ;
            int pTo = str.IndexOf(strTo);
            return str.Substring(pFrom, pTo - pFrom);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string UpdateParameters(string basicId, string views, string likes)
        {
            IVideosBL obj = UnityBootstrapper.Resolve<VideosBL>();

            string IsUpdated = string.Empty;
            try
            {
                int BasicId;
                int Likes;
                int Views;
                int.TryParse(basicId, out BasicId);
                int.TryParse(likes, out Likes);
                int.TryParse(views, out Views);
                IsUpdated = obj.UpdateViewsAndLikes(BasicId,Likes,Views).ToString();
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
            return IsUpdated;
        }


        public void RetrieveMetaTags(int subcategoryId, string makeName, string modelName)
        {
            if (subcategoryId == 47 || subcategoryId == 48 || subcategoryId == 50)
            {
                metaTitle = "Video Review - " + makeName + " " + modelName + " - " + basicId;
                metaDescription = makeName + " " + modelName + " Video Review - Watch Carwale Expert's Take on " + makeName + " " + modelName + "- Features, performance, price, fuel economy, handling and more.";
            }
            else if (subcategoryId == 49)//49-Car Interiors
            {
                metaTitle = "Interior Review Video - " + makeName + " " + modelName + " - " + basicId;
                metaDescription = "Watch " + makeName + " " + modelName + "Interior Video Review - Carwale Expert's take on Interior and Exterior Features, Colours and More";
            }
            else
            {
                metaTitle = videoTitle;
                metaDescription = videoTitle;
            }

        }

        protected string FormatSubCat(string subCat)
        {
            return subCat.Trim().ToLower().Replace(" ", "-");
        }

    }//class
}//namespace