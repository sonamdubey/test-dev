using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.DealerFacility
{
    /// <summary>
    /// Written By : Snehal Dange on 7th August 2017
    /// Description : Model for dealer facility
    /// </summary>
    public class ManageDealerFacilityVM
    {
        public IEnumerable<FacilityEntity> FacilityList { get; set; }
        public uint DealerId { get; set; }

    }
}
