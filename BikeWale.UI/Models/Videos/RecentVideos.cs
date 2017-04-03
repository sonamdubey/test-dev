using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Mar 2017
    /// Summary    : Model to get list of videos for partial view
    /// </summary>
    public class RecentVideos
    {
        private readonly IVideos _videos = null;
        private readonly ushort _pageNo;
        private readonly ushort _pageSize;
        private readonly uint _makeId;
        private readonly string _makeName;
        private readonly string _makeMasking;
        private readonly uint _modelId;
        private readonly string _modelName;
        private readonly string _modelMasking;

        #region Constructor
        public RecentVideos(ushort pageNo, ushort pageSize, IVideos videos)
        {
            _pageNo = pageNo;
            _pageSize = pageSize;
            _videos = videos;
        }

        public RecentVideos(ushort pageNo, ushort pageSize, uint makeId, string makeName, string makeMasking, IVideos videos)
        {
            _pageNo = pageNo;
            _pageSize = pageSize;
            _makeId = makeId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _videos = videos;
        }

        public RecentVideos(ushort pageNo, ushort pageSize, uint makeId, string makeName, string makeMasking, uint modelId, string modelName, string modelMasking, IVideos videos)
        {
            _pageNo = pageNo;
            _pageSize = pageSize;
            _makeId = makeId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _modelId = modelId;
            _modelName = modelName;
            _modelMasking = modelMasking;
            _videos = videos;
        }
        #endregion

        #region Functions to get data
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Mar 2017
        /// Summary    : To get list of news articles
        /// </summary>
        public RecentVideosVM GetData()
        {
            RecentVideosVM recentVideos = new RecentVideosVM();
            try
            {
                recentVideos.VideosList = _videos.GetVideosByMakeModel(_pageNo, _pageSize, _makeId, _modelId);
                if (recentVideos.VideosList != null)
                {
                    if (string.IsNullOrEmpty(_makeMasking) && string.IsNullOrEmpty(_modelMasking))
                    {
                        recentVideos.MoreVideoUrl = string.Format("/bike-videos/");
                        recentVideos.LinkTitle = "Bikes Videos";
                    }

                    else if (!String.IsNullOrEmpty(_makeMasking) && String.IsNullOrEmpty(_modelMasking))
                    {
                        recentVideos.MoreVideoUrl = string.Format("/{0}-bikes/videos/", _makeMasking);
                        recentVideos.LinkTitle = string.Format("{0} Videos", _makeName);
                    }
                    else if (!String.IsNullOrEmpty(_makeMasking) && !String.IsNullOrEmpty(_modelMasking))
                    {
                        recentVideos.MoreVideoUrl = string.Format("/{0}-bikes/{1}/videos/", _makeMasking, _modelMasking);
                        recentVideos.LinkTitle = string.Format("{0} {1} Videos", _makeName, _modelName);
                        recentVideos.BikeName = string.Format("{0} {1}", _makeName, _modelName);
                    }
                    recentVideos.FetchedCount = recentVideos.VideosList.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.RecentVideos.GetData: PageNo {0},PageSize {1}, MakeId {2}, ModelId {3}", _pageNo, _pageSize, _makeId, _modelId));
            }
            return recentVideos;
        }
        #endregion
    }
}