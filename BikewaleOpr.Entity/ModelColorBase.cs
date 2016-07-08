using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    public class ModelColorBase
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ColorCodeBase> ColorCodes { get; set; }
    }
}