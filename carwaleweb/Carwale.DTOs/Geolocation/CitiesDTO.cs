using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class CitiesDTO
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public int StdCode { get; set; }
        public bool IsPopular { get; set; }
        public DateTime CityEntryDate { get; set; }
        public string CityMaskingName { get; set; }
        public int BWCityOrder { get; set; }
        public string StateName { get; set; }
    }
}
