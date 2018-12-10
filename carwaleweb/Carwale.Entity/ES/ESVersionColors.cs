using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class ESVersionColors
    {        
        public CarVersionEntity Version { get; set; }        
        public List<ExteriorColor> ExteriorColor { get; set; }        
    }
}
