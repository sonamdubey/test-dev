using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class CarData
    {
       public ComponentType CarDataType { get; set; }
       public string CategoryName { get; set; }
       public int SortOrder { get; set; }
       public List<CategoryItem> Items { get; set; }

    }
}
