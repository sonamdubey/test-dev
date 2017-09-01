using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    public class PoupCityAreaVM
    {
        public uint ModelId { get; set; }

        public string MakeName { get; set; }

        public string ModelName { get; set; }

        public bool IsPersistent { get; set; }
        public uint PQSourceId { get; set; }

        public uint PageCategoryId { get; set; }

        public uint PreSelectedCity { get; set; }

        public bool IsReload { get; set; }


    }
}
