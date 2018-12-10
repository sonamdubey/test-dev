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
    public class ModelColors
    {
        public int ColorId { get; set; }
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostUrl { get; set; }
    }
}
