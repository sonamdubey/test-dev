using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class CollectionHelper
    {
        public static bool IsEmpty(IEnumerable collection)
        {
            if (collection == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsNotEmpty(string[] arr)
        {
            if(arr !=null && arr.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
    }
}
