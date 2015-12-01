using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    public class VersionColorWithAvailability : VersionColorBase
    {
        public string  NoOfDays { get; set; }
        public string Hexcode { get; set; }
    }
}