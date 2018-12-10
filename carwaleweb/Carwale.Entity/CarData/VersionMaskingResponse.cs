using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class VersionMaskingResponse
    {
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public string VersionMaskingName { get; set; }
        public bool Redirect { get; set; }
        public bool Valid { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeName { get; set; }
    }
}
