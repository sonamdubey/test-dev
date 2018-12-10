using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.DeepLinking
{
    public class DeepLinkingEntity
    {
        public string CwApi { get; set; }
        public int CwAppScreenId { get; set; }
        public string cwAppExtraParams { get; set; } // This is used in case the app requires any extra params other than ones in API.
    }
}
