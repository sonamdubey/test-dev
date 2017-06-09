using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entities;

namespace BikewaleOpr.Entity.DealerCampaign
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 11 May 2017
    /// Summary : Entity to hold areas data
    /// </summary>
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAutoAssigned { get; set; }
    }

    /// <summary>
    /// Written By : Ashish G. Kamble on 11 May 2017
    /// Summary : Entity to hold cities data
    /// </summary>
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }        
    }

    /// <summary>
    /// Written By : Ashish G. Kamble on 11 May 2017    
    /// Summary : Entity to hold the data for city and its assigned areas
    /// </summary>
    public class CityArea
    {
        public City City { get; set; }
        public IEnumerable<Area> Areas { get; set; }
        public IEnumerable<Area> AutoAssignedAreas { get { return Areas!=null && Areas.Count() > 0 ? Areas.Where(area=> area.IsAutoAssigned) : null; } }
        public IEnumerable<Area> AdditionalAreas { get { return Areas != null && Areas.Count() > 0 ? Areas.Where(area => !area.IsAutoAssigned) : null; } }
    }

    /// <summary>
    /// Written By : Ashish G. Kamble on 11 May 2017
    /// Summary : Entity to hold the dealers location to areas list
    /// </summary>
    public class CampaignAreas
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public bool IsAutoAssigned { get; set; }
    }

    /// <summary>
    /// Entity to hold the dealers distance from the mapped areas
    /// </summary>
    public class DealerAreaDistance
    {
        public GeoLocationEntity DealerLocation { get; set; }
        public IEnumerable<GeoLocationEntity> Areas { get; set; }
    }

    public class DealerCampaignArea
    {
        public string DealerName { get; set; }
        public IEnumerable<CampaignAreas> Areas { get; set; }
    }
}
