using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using BikeWaleOpr.Common;
using RabbitMqPublishing;
using BikeWaleOpr.RabbitMQ;
using System.Collections.Specialized;
using System.IO;

namespace BikeWaleOpr.Content
{
    public class SeriesPhotos : System.Web.UI.Page
    {

        #region Variable Declaration
        protected Button btnSave;
        protected HtmlInputFile filPhoto;
        protected string seriesId = string.Empty, seriesName = string.Empty, hostUrl = string.Empty, originalImagePath = string.Empty, makeMaskingName = string.Empty;
        protected string seriesMaskingName = string.Empty, isReplicated = string.Empty;
        #endregion

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(SavePhoto);
        }

        #region Event Defination
        protected void Page_Load(object sender, EventArgs e)
        {
            seriesId = Request.QueryString["series"].ToString();

            GetSeriesImageInfo(seriesId);
        }

        void SavePhoto(object sender, EventArgs e)
        {
            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();
                
                string imgName = makeMaskingName + '-' + seriesMaskingName;
                string imgPath = ImagingOperations.GetPathToSaveImages("\\bw\\series\\");

                //check for directory ,if not exists then it create series directory
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }

                ImagingOperations.SaveImageContent(filPhoto, "/bw/series/" + imgName + "-" + seriesId + ".jpg");
                //string imageName = makeMaskingName + '-' + seriesMaskingName ;
                ms.SaveSeriesPhoto(seriesId, imgName);
                //UploadPhoto(seriesId);
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SavePhoto sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SavePhoto ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Method defination
        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoId"></param>
        void UploadPhoto(string seriesId)
        {
            HttpContext.Current.Trace.Warn("PhotoId" + seriesId);

            try
            {
                // Trace.Warn("Saving Path : " + galleryPath + drpVersion.SelectedValue + "_" + drpVersion1.SelectedValue + ".jpg");
                string imageName = makeMaskingName + '-' + seriesMaskingName + "_temp.jpg";
                // upload file for temporary purpose

                //rabbitmq code here 

                ImagingOperations.SaveImageContent(filPhoto, "/bw/series/" + imageName);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetSeriesImageInfo  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
    

        /// <summary>
        /// Written By : Ashwini Todkar on 6 March 2014 
        /// summary    :  function to get details of series like name,series image and masking name
        /// <param name="seriesId"></param>
        protected void GetSeriesImageInfo(string seriesId)
        {
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();
                ds = ms.GetSeriesDetails(seriesId);
                dt = ds.Tables[0];

                if(dt.Rows.Count > 0)
                {
                    seriesName = dt.Rows[0]["Name"].ToString();
                    hostUrl = dt.Rows[0]["HostUrl"].ToString();
                    originalImagePath = dt.Rows[0]["OriginalImagePath"].ToString();
                    isReplicated = dt.Rows[0]["IsReplicated"].ToString();
                    makeMaskingName = dt.Rows[0]["MakeMaskingName"].ToString();
                    seriesMaskingName = dt.Rows[0]["SeriesMaskingName"].ToString();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetSeriesImageInfo  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        #endregion
    }
}