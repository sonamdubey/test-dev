using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Finance
{
    [Serializable]
    public class FinanceLinkData
    {
        public int PlatformId { get; set; }
        public int ScreenId { get; set; }
        public int ClientId { get; set; }
        public string LinkText { get; set; }
        public string Url { get; set; }
        public bool AppendQS { get; set; }
        public bool IsAd { get; set; }
        public string AdText { get; set; }
    }
}
