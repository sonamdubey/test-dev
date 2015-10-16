﻿using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
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
    public static class BindModelGallery
    {
        /// <summary>
        /// Model Id
        /// </summary>
        public static int ModelId { get; set; }
        /// <summary>
        /// Fetched Image Count
        /// </summary>
        public static int FetchedImageCount { get; set; }
        /// <summary>
        /// Fetched Video Count
        /// </summary>
        public static int FetchedVideoCount { get; set; }

        static string _cwHostUrl;
        static BindModelGallery()
        {
            _cwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        }

        /// <summary>
        /// Binds the Video main and navigation JCarousel
        /// It fetches data from BW Video APIs
        /// </summary>
        /// <param name="rptrVideos">Video repeater</param>
        /// <param name="rptrVideoNav">Video navigation repeater</param>
        public static void BindVideos(Repeater rptrVideos, Repeater rptrVideoNav)
        {
            try
            {
                FetchedVideoCount = 0;
                VideosList objVideos = null;

                
                string _requestType = "application/json";

                string _apiUrl = String.Format("/api/videos/pn/1/ps/1000/model/{0}/", ModelId);

                objVideos = BWHttpClient.GetApiResponseSync<VideosList>(_cwHostUrl, _requestType, _apiUrl, objVideos);

                if (objVideos != null)
                {
                  var videoList = objVideos.Videos.ToList();
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
        public static void BindVideos(Repeater rptrVideoNav)
        {
          try
          {
            FetchedVideoCount = 0;
            VideosList objVideos = null;

            string _requestType = "application/json";

            string _apiUrl = String.Format("/api/videos/pn/1/ps/1000/model/{0}/", ModelId);

            objVideos = BWHttpClient.GetApiResponseSync<VideosList>(_cwHostUrl, _requestType, _apiUrl, objVideos);

            if (objVideos != null)
            {
              var lst = objVideos.Videos.ToList();
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
        public static void BindImages(Repeater rptrImages, Repeater rptrImageNav, List<Bikewale.DTO.CMS.Photos.CMSModelImageBase> photos)
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
    }
}