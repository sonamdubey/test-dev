using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Location
{
    public class StateMaskingResponse
    {
        public uint StateId { get; set; }
        public string MaskingName { get; set; }
        public ushort StatusCode { get; set; }
    }
}
