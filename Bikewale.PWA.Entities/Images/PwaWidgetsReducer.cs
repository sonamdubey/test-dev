using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.PWA.Entities.Images
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 14 Feb 2018
    /// Description : Reducer for all widgets.
    /// </summary>
    public class PwaWidgetsReducer
    {
        public PwaBikeImagesCarouselReducer BikeImagesCarouselReducer { get; set; }
        public PwaWidgetsReducer()
        {
            BikeImagesCarouselReducer = new PwaBikeImagesCarouselReducer();
        }
    }
}
