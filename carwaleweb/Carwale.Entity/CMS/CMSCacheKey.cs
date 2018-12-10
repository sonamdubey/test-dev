using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS
{
    [Serializable]
    public class CMSCacheKey
    {
        public ushort CategoryId { get; set; }
        public ushort ApplicationId { get; set; }
        public string Key { get; set; }
    }
}
