using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Models.BikeSeries
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 11th Sep 2017
    /// Summary : VM for bike series management page
    /// Modified by : Rajan Chauhan on 12th Dec 2017
    /// Summary : Added bike bodystyles list
    /// </summary>
    public class BikeSeriesPageVM
    {
        public IEnumerable<BikeSeriesEntity> BikeSeriesList { get; set; }
        public IEnumerable<BikeMakeEntityBase> BikeMakesList { get; set; }
        public IEnumerable<BikeBodyStyleEntity> BikeBodyStylesList { get; set; }
        public string PageTitle { get; set; }
        public uint UpdatedBy { get; set; }
    }
}
