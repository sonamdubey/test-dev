﻿
namespace BikewaleOpr.Entities.BikePricing
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 23 Sept 2016
	/// Class to hold the bike price 
	/// Modified by : Ashutosh Sharma on 27 Nov 2017
	/// Description : Added BikeSeriesId.
	/// </summary>
	public class BikePrice
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public uint VersionId { get; set; }
		public uint BikeSeriesId { get; set; }
		public string Price { get; set; }
        public string RTO { get; set; }
        public string Insurance { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public uint BikeModelId { get; set; }
    }
}
