using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    [Serializable]
    public class VersionEntityBase
    {
        public UInt32 VersionId { get; set; }
        public string VersionName { get; set; }
    }
}
