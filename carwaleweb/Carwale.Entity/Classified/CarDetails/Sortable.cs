using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class Sortable : IComparable
    {
        public int SortOrder { get; set; }

        public int CompareTo(Object obj)
        {
            Sortable item = (Sortable)obj;
            int retval = 0;
            if (item.SortOrder < SortOrder && item.SortOrder != 0)
            {
                retval = 1;
            }
            else if (item.SortOrder > SortOrder && SortOrder != 0)
            {
                retval = -1;
            }

            return retval;
        }
    }
}
