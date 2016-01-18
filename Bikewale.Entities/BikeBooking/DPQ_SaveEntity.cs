using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Created By : Sumit Kate on 29 Dec 2015
    /// </summary>
    public class DPQ_SaveEntity
    {
        public uint DealerId { get; set; }
        public uint PQId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public uint? ColorId { get; set; }
        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        public UInt16? LeadSourceId { get; set; }
        public string UTMA { get; set; }
        public string UTMZ { get; set; }
        public string DeviceId { get; set; }
    }
}