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
using log4net;
using Grpc.CMS;
using Bikewale.BAL.GrpcFiles;

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

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BindModelGallery));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);


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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
            ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            
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
                if (photos != null && photos.Count >= 1)
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 16 Dec 2015
        /// Summary : To get videolist usinf carwale video api 
        /// </summary>
        /// <returns></returns>
        private List<BikeVideoEntity> GetVideos()
        {
            return GetVideosByModelIdViaGrpc();
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private List<BikeVideoEntity> GetVideosByModelIdViaGrpc()
        {
            List<BikeVideoEntity> videoDTOList = null;
            try
            {

                int startIndex, endIndex;
                Bikewale.Utility.Paging.GetStartEndIndex(1000, 1, out startIndex, out endIndex);

                var _objVideoList = GrpcMethods.GetVideosByModelId(ModelId, (uint)startIndex, (uint)endIndex);

                if (_objVideoList != null && _objVideoList.LstGrpcVideos.Count > 0)
                {
                    videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                }                

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return videoDTOList;
        }

  

    }
}