using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeSeries
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 25 May 2018
    /// Description : VM for the Series Slug shown on the Model Page. 
    /// </summary>
    public class ModelSeriesSlugVM
    {
        public string SeriesName { get; set; }
        public string MakeName { get; set; }
        public uint SeriesBikesCount { get; set;}
        public long MinimumPrice { get; set; }
        public string MakeMaskingName { get; set; }
        public string SeriesMaskingName { get; set; }
    }
}
