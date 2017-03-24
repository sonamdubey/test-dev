using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.BAL.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Mar 2017
    /// Summary    : Model to get list of videos for partial view
    /// </summary>
    public class RecentVideos
    {
         private readonly IVideos _videos = null;

        #region Constructor
         public RecentVideos(IVideos videos)
        {
            _videos = videos;
        }
        #endregion

        #region Functions to get data
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Mar 2017
        /// Summary    : To get list of news articles
        /// </summary>
        //public RecentNewsVM GetData(uint totalRecords, uint makeId, uint modelId, string makeName, string makeMasking, string modelName, string modelMasking)
        //{
        //    RecentNewsVM recentNews = new RecentNewsVM();
        //    try
        //    {
        //        recentNews.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), totalRecords, makeId, modelId);
        //        if (makeId > 0)
        //        {
        //            recentNews.MakeName = makeName;
        //            recentNews.MakeMasking = makeMasking;
        //        }

        //        if (modelId > 0)
        //        {
        //            recentNews.ModelName = modelName;
        //            recentNews.ModelMasking = modelMasking;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.News.RecentNews.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", totalRecords, makeId, modelId));
        //    }
        //    return recentNews;
        //}

         public RecentVideosVM GetData(ushort pageNo, ushort pageSize, uint makeId, string makeName,string makeMasking, uint modelId, string modelName,string modelMasking)
        {
             RecentVideosVM recentVideos=new RecentVideosVM();
             try
             {
                 recentVideos.VideosList = _videos.GetVideosByMakeModel(pageNo, pageSize, makeId, modelId);
                 if (string.IsNullOrEmpty(makeMasking) && string.IsNullOrEmpty(modelMasking))
                 {
                     recentVideos.MoreVideoUrl = string.Format("/bike-videos/");
                     recentVideos.LinkTitle = "Bikes Videos";
                 }

                 else if (!String.IsNullOrEmpty(makeMasking) && String.IsNullOrEmpty(modelMasking))
                 {
                     recentVideos.MoreVideoUrl = string.Format("/{0}-bikes/videos/", makeMasking);
                     recentVideos.LinkTitle = string.Format("{0} Videos", makeName);
                 }
                 else if (!String.IsNullOrEmpty(makeMasking) && !String.IsNullOrEmpty(modelMasking))
                 {
                     recentVideos.MoreVideoUrl = string.Format("/{0}-bikes/{1}/videos/", makeMasking, modelMasking);
                     recentVideos.LinkTitle = string.Format("{0} {1} Videos", makeName, modelName);
                     recentVideos.BikeName = string.Format("{0} {1}",makeName,modelName);
                 }
             }
             catch (Exception ex)
             {
                 ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.RecentVideos.GetData: PageNo {0},PageSize {1}, MakeId {2}, ModelId {3}", pageNo, pageSize,makeId, modelId));
             }
             return recentVideos;
        }
        #endregion
    }
}