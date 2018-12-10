using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace Carwale.Utility
{
    public static class Calculation
    {
        public static void GetStartEndIndex(ushort pageNo, ushort pageSize, out ushort startIndex, out ushort endIndex)
        {
            try
            {
                startIndex = (UInt16)(1 + pageSize * (pageNo - 1));
                endIndex = (UInt16)(pageSize * pageNo);
            }
            catch (Exception)
            {
                throw;
            }
        }
      
    }//class
}//namespace
