using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Written By : Snehal Dange on 7th August 2017
    /// Description : Dto for dealer facility
    /// </summary>
    public class DealerFacilityDTO
    {
      
        public string Facility { get; set; }

      
        public bool IsActive { get; set; }

       
        public uint Id { get; set; }

        public uint FacilityId { get; set; }

        public UInt16 LastUpdatedById { get; set; }
    }
}
