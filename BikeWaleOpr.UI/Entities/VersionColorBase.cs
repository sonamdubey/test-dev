using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    public class VersionColorBase
    {
        public uint ModelColorID { get; set; }
        public string ModelColorName { get; set; }
        public bool IsActive { get; set; }
    }
}