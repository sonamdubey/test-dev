using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class PQOutputEntity
    {
        public ulong PQId { get; set; }
        public uint DealerId { get; set; }
        public uint VersionId { get; set; }
    }
}
