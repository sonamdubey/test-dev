using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Videos
{
    public class BikeVideosListEntity
    {
        public int TotalRecords { get; set; }
        public IEnumerable<BikeVideoEntity> Videos { get; set; }
        public string NextPageUrl { get; set; }
        public string PrevPageUrl { get; set; }

    }
}
