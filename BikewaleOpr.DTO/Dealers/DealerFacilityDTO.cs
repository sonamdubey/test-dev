using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.Dealers
{
   public class DealerFacilityDTO
    {
      
        public string Facility { get; set; }

      
        public bool IsActive { get; set; }

       
        public uint Id { get; set; }

        public uint FacilityId { get; set; }
    }
}
