using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class CategoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string UnitTypeName { get; set; }
        public int DataTypeId { get; set; }
        public string Description { get; set; }
        public string Tip { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }
        public int CustomTypeId { get; set; }
    }
}
