using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Binder class for Model Page Gallery
    /// Author  :   Sumit Kate
    /// Created :   30 Sept 2015
    /// </summary>
    public class BindModelGallery
    {
        /// <summary>
        /// Model Id
        /// </summary>
        public int ModelId { get; set; }
        /// <summary>
        /// Fetched Image Count
        /// </summary>
        public int FetchedImageCount { get; set; }
        /// <summary>
        /// Fetched Video Count
        /// </summary>
        public int FetchedVideoCount { get; set; }

        string _cwHostUrl = string.Empty;
        string _requestType = string.Empty;
        public BindModelGallery()
        {
            _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
            _requestType = "application/json";
        }

        /// <summary>
        /// Binds the Video main and navigation JCarousel
        /// It fetches data from BW Video APIs
        /// </summary>
        /// <param name="rptrVideos">Video repeater</param>
        /// <param name="rptrVideoNav">Video navigation repeater</param>
        public void BindVideos(Repeater rptrVideos, Repeater rptrVideoNav)
        {
            try
            {
                FetchedVideoCount = 0;

                List<BikeVideoEntity> objVideos = GetVideos();

                if (objVideos != null && objVideos.Count > 0)
                {
                  var videoList = objVideos.ToList();
                  if (videoList.Count > 0)
                  {
                    FetchedVideoCount = videoList.Count;

                    rptrVideos.DataSource = videoList;
                    rptrVideos.DataBind();

                    rptrVideoNav.DataSource = videoList;
                    rptrVideoNav.DataBind();
                  }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Binds the Video main and navigation JCarousel
        /// It fetches data from BW Video APIs
        /// </summary>
        /// <param name="rptrVideos">Video repeater</param>
        /// <param name="rptrVideoNav">Video navigation repeater</param>
        public void BindVideos(Repeater rptrVideoNav)
        {
          try
          {
            FetchedVideoCount = 0;

            List<BikeVideoEntity> objVideos = GetVideos();

            if (objVideos != null && objVideos.Count > 0)
            {
              var lst = objVideos.ToList();
              if (lst.Count > 0)
              {
                FetchedVideoCount = lst.Count;
                rptrVideoNav.DataSource = lst;
                rptrVideoNav.DataBind();
              }
            }
          }
          catch (Exception ex)
          {
            ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            objErr.SendMail();
          }
        }

        /// <summary>
        /// It binds the Image and image navigation JCarousel
        /// </summary>
        /// <param name="rptrImages">Image repeater</param>
        /// <param name="rptrImageNav">Image navigation repeater</param>
        /// <param name="photos">list of photos</param>
        public void BindImages(Repeater rptrImages, Repeater rptrImageNav, List<ModelImage> photos)
        {
            try
            {
                if (photos != null && photos.Count > 1)
                {
                    FetchedImageCount = photos.Count;
                    rptrImages.DataSource = photos;
                    rptrImages.DataBind();

                    rptrImageNav.DataSource = photos;
                    rptrImageNav.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 16 Dec 2015
        /// Summary : To get videolist usinf carwale video api 
        /// </summary>
        /// <returns></returns>
        private List<BikeVideoEntity> GetVideos()
        {
            string _apiUrl = string.Empty;
            List<BikeVideoEntity> objVideosList = null;
            try
            {
                _apiUrl = String.Format("/api/v1/videos/model/{0}/?appId=2&pageNo=1&pageSize=1000", ModelId);

                objVideosList = BWHttpClient.GetApiResponseSync<List<BikeVideoEntity>>(_cwHostUrl, _requestType, _apiUrl, objVideosList);
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindModelGallery.GetVideos");
                objErr.SendMail();
            }
            return objVideosList;
        }
    }
}