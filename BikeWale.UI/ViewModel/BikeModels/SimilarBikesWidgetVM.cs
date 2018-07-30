using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Mar 2017
    /// Description :   SimilarBikesWidgetVM view model
    /// Modified by: Vivek Singh Tomar on 23 Aug 2017
    /// Description: Added bodystyle and Page to hold page id and page name
    /// Modified by : Vivek Singh Tomar on 27th Oct 2017
    /// Description: Added city and return url for redirection from popups on amp pages
    /// </summary>
    public class SimilarBikesWidgetVM
    {
        public PQSourceEnum PQSourceId { get; set; }
        public bool ShowCheckOnRoadCTA { get; set; }
        public bool ShowPriceInCityCTA { get; set; }
        public IEnumerable<SimilarBikeEntity> Bikes { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public uint VersionId { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public GAPages Page { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
        public uint CityId { get; set; }
        public string ReturnUrlForAmpPages { get; set; }
    }
}
