using System;
using System.Collections.Generic;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class ExteriorColor : ColorEntity
    {
        public int ExtColorId { get; set; }
        public int VersionId { get; set; }
        public List<InteriorColor> InteriorColor { get; set; }
        public string HostUrl { get; set; }        
        public string OriginalImgPath { get; set; }
    }
}
