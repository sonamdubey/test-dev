using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Mar 2017
    /// Summary    : View model for videos widget
    /// </summary>
    public class RecentVideosVM
    {
        public IEnumerable<BikeVideoEntity> VideosList { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MakeMasking { get; set; }
        public string ModelMasking { get; set; }
        public string LinkTitle { get; set; }
        public string MoreVideoUrl { get; set; }
        public string BikeName { get; set; }
    }
}
