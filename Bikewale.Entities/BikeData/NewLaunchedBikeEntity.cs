using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class NewLaunchedBikeEntity : BikeModelEntity
    {
        public uint BikeLaunchId { get; set; }
        public DateTime LaunchDate { get; set; }
        public ulong BasicId { get; set; }
        public string RoadTestUrl { get; set; }
    }
}