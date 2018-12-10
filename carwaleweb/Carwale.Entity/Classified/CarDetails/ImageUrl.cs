using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class ImageUrl
    {
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public bool IsMain { get; set; }
    }
}
