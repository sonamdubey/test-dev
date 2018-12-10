using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// Created By : Shalini Nair
    /// </summary>
    [Serializable]
    public class GoogleKeywords
    {
        public string Make { get; set; }
        public string SubSegment { get; set; }
        public string CarBodyStyle { get; set; }
    }
}
