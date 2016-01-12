using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class BikeAvailabilityByColor
    {
        
        public uint ColorId { get; set; }
        public uint DealerId {get;set;}
        public uint NoOfDays { get; set; }
        public bool isActive {get;set;}
    }
}