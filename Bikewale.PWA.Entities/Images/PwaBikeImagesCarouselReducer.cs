using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.PWA.Entities.Images
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 14 Feb 2018
    /// Description : Reducer for popular bike models images.
    /// </summary>
    public class PwaBikeImagesCarouselReducer
    {
        public PwaPopularBikeImagesListData PopularBikeImagesListData { get; set; }

        public PwaBikeImagesCarouselReducer()
        {
            PopularBikeImagesListData = new PwaPopularBikeImagesListData();
        }
    }
}
