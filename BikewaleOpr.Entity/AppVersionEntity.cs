using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    public class AppVersionEntity
    {
        public int Id { get; set; }
        public bool IsSupported { get; set; }
        public bool IsLatest { get; set; }
        public string Description { get; set; }
        public AppEnum AppType { get; set; }    
    }
}