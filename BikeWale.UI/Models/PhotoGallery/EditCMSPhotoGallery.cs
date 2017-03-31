using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 31 Mar 2017
    /// Summary    : Model to fetch photos and videos for gallery
    /// </summary>
    public class EditCMSPhotoGallery
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IVideos _videos = null;
        #endregion

        #region Constructor
        public EditCMSPhotoGallery(ICMSCacheContent cmsCache,IVideos videos)
        {
            _cmsCache = cmsCache;
            _videos = videos;
        }
        #endregion

        #region Public properties
        public uint BasicId { get; set; }
        public uint ModelId { get; set; }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get photos and videos for photo gallery
        /// </summary>
        public EditCMSPhotoGalleryVM GetData()
        {
            EditCMSPhotoGalleryVM PhotoGallery = new EditCMSPhotoGalleryVM();
            try
            {
                PhotoGallery.Images = _cmsCache.GetArticlePhotos(Convert.ToInt32(BasicId));
                if (PhotoGallery.Images != null && PhotoGallery.Images.Count() > 0)
                {
                   PhotoGallery.ImageCount = PhotoGallery.Images.Count();
                }
                if (ModelId > 0)
                {
                    PhotoGallery.Videos = _videos.GetVideosByModelId(ModelId);
                    if (PhotoGallery.Videos != null && PhotoGallery.Videos.Count() > 0)
                        PhotoGallery.VideoCount = PhotoGallery.Videos.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.EditCMSPhotoGalleryVM.GetData");
            }
            return PhotoGallery;
        }
        #endregion
    }
}