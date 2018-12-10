using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{

    public class CompareAllDTO
    {
        public List<HotCarComparison> CompareList { get; set; }
        public ushort CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public PageMetaTags MetaData { get; set; }
    }
	
}
