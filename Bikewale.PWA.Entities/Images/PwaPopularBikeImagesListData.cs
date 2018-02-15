using Bikewale.PWA.Entities.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.PWA.Entities.Images
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 14 Feb 2018
    /// Description : Reducer for popular bike models images.
    /// </summary>
    public class PwaPopularBikeImagesListData
    {
        [DataMember]
        public IEnumerable<PwaModelImages> BikeImagesList { get; set; }

    }
}
