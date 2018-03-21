using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    public class AdSlotModel
    {
        public string AdId { get; set; }
        public uint DivId { get; set; }
        public uint Width { get; set; }
        public bool LoadImmediate { get; set; }
    }
}
