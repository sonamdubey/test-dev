using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class Paging
    {
        /// <summary>
        /// Function to the start and end index for the current page number.
        /// </summary>
        /// <param name="pageSize">Total number of records per page.</param>
        /// <param name="currentPageNo">Current page number</param>
        /// <param name="startIndex">start index for records</param>
        /// <param name="endIndex">End index for records</param>
        public static void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex)
        {
            startIndex = 0;
            endIndex = 0;

            endIndex = currentPageNo * pageSize;

            startIndex = (endIndex - pageSize) + 1;
        }

        /// <summary>
        /// Function to get the number of pages for the given number of records and page size
        /// </summary>
        /// <param name="totalRecords">Total number of records.</param>
        /// <param name="pageSize">Size of the page data.</param>
        /// <returns>Returns count of the pages exist.</returns>
        public static int GetTotalPages(int totalRecords, int pageSize)
        {
            int pageCount = (int)Math.Ceiling((double)totalRecords / (double)pageSize);// Total number of pages in the pager.

            return pageCount;
        }
    }
}
