using System;

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


        /// <summary>
        /// Created By : Sajal Gupta on 17-02-2017
        /// Desc: Function to make next url and prev url.
        /// </summary>
        /// <param name="totalPages"></param>
        /// <param name="baseUrl"></param>
        public static void CreatePrevNextUrl(int totalPages, string baseUrl, int pageNumber, ref string nextPageUrl, ref string prevPageUrl)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrlForJs, baseUrl);

            if (pageNumber == 1)    //if page is first page
            {
                nextPageUrl = string.Format("{0}2/", _mainUrl);
            }
            else if (pageNumber == totalPages)    //if page is last page
            {
                prevPageUrl = string.Format("{0}{1}/", _mainUrl, pageNumber - 1);
            }
            else
            {          //for middle pages
                prevPageUrl = string.Format("{0}{1}/", _mainUrl, pageNumber - 1);
                nextPageUrl = string.Format("{0}{1}/", _mainUrl, pageNumber + 1);
            }
        }
    }
}
