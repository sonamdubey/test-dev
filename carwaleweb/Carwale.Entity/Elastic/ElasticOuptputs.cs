using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Elastic
{
    public class ElasticOuptputs
    {
        /// <summary>
        /// Class for Filters Properties ||A user can select in Used Cars Search Page 
        /// Will be fetching FromUri || Added By Jugal
        /// </summary>

        public string[] cities { get; set; }
        public string[] cars { get; set; }
        public string[] roots { get; set; }
        //Added By : Sadhana Upadhyay on 24 March 2015
        //Contains only those make whose roots are not selected
        public string[] NewMakes { get; set; }
        public string[] NewRoots { get; set; }
        public bool IsCarTradeCertifiedCars { get; set; }
        public string carsWithPhotos = string.Empty;
        public string[] fuels;
        public string[] bodytypes { get; set; }
        public string[] sellers { get; set; }
        public string[] owners { get; set; }
        public string[] transmissions { get; set; }
        public string[] colors { get; set; }
        public string yearMin = string.Empty;
        public string yearMax = string.Empty;
        public string budgetMin = string.Empty;
        public string budgetMax = string.Empty;
        public string kmMin = string.Empty;
        public string kmMax = string.Empty;
        public string subSegmentID { get; set; }
        public string profileId { get; set; }
        public string packageType { get; set; }
        public string sessionId { get; set; }
        public int multiCityId { get; set; }
        public string multiCityName { get; set; }

        public string sc { get; set; }
        public string so { get; set; }
        public int pn { get; set; }
        public string ps { get; set; }
        public bool bestmatch { get; set; }
        public int lcr { get; set; }
        public int ldr { get; set; }
        public int lir { get; set; }
        public int nearbyCityId { get; set; }
        public string nearbyCityIds { get; set; }
        public string nearbyCityIdsStockCount { get; set; }
        public int userPreferredRootId { get; set; }
        public bool IsFranchiseCars { get; set; }
        public string[] ExcludeStocks { get; set; }
        // Nearby cars
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool ShouldFetchNearbyCars { get; set; }
        public int FeaturedSlotsCount { get; set; }

    }
}
