using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models.Photos
{
    public class SimilarBikesWithPhotosVM
    {
        public ushort TotalRecords { get; set; }
        public int ModelId { get; set; }
        public string SimilarMakeName { get; set; }
        public string SimilarModelName { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string WidgetHeading { get; set; }
        public uint CityId { get; set; }
        public string City { get; set; }
        public IEnumerable<SimilarBikesWithPhotos> Bikes { get; set; }
    }
}
