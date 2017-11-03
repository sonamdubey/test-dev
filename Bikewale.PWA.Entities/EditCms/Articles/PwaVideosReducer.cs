using System.Collections.Generic;

namespace Bikewale.Entities.PWA.Articles
{

    public class PwaVideosReducer
    {
        public PwaAllVideos VideosLanding { get; private set; }
        public PwaVideosBySubcategory VideosByCategory { get; private set; }
        public PwaVideoDetailReducer VideoDetail { get; private set; }

        public PwaVideosReducer()
        {
            VideosLanding = new PwaAllVideos();
            VideosByCategory = new PwaVideosBySubcategory();
            VideoDetail = new PwaVideoDetailReducer();
        }
    }

}
