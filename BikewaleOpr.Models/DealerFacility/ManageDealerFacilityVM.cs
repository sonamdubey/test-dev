using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.DealerFacility
{
   public class ManageDealerFacilityVM
    {
        public List<FacilityEntity> FacilityList { get; set; }
        public uint DealerId { get; set; }

    }
}
