using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Notifications;
using System.Web;

namespace Carwale.BL
{
    public class Pager : IPager 
    {
        /// <summary>
        /// This method gets the list of page Urls for the pager.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagerDetails"></param>
        /// <returns></returns>
        public T GetPager<T>(PagerEntity pagerDetails) where T : PagerOutputEntity , new()
        {
            
            var results = new List<PagerUrlList>();
            T t = new T();
            try
            {
                bool firstPage = false, lastPage = false;
                string pageUrl;
                int startIndex , endIndex;
                int pageCount = (int)Math.Ceiling((double)pagerDetails.totalResults / (double)pagerDetails.pageSize);// Total number of pages in the pager.
                if (pageCount > 1)
                {
                    int totalSlots = (int)Math.Ceiling((double)pageCount / (double)pagerDetails.pagerSlotSize); // The total number of slots for the pager.
                    int curSlot = ((int)Math.Floor((double)(pagerDetails.pageNo - 1) / (double)pagerDetails.pagerSlotSize)) + 1; // Current page slot.

                    if (pagerDetails.pageNo == 1)// check for if the current page is the first page or the last page.
                        firstPage = true;
                    else if (pagerDetails.pageNo == pageCount)
                        lastPage = true;
                    // Get Start and End Index.
                    GetStartEndIndex(pagerDetails, pageCount, curSlot, out startIndex, out endIndex);
                    // Set the first and last page Urls.
                    if (firstPage == false)
                    {
                        t.FirstPageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + "1/";
                    }
                  
                    if (lastPage == false)
                    {
                        t.LastPageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + pageCount + "/";
                    } 
                    //Get the list of page Urls.
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        PagerUrlList pagerUrlList = new PagerUrlList();
                        pageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + i.ToString() + "/";
                        pagerUrlList.PageNo = i;
                        pagerUrlList.PageUrl = pageUrl;

                        results.Add(pagerUrlList);

                    }

                    //Set previous and next page Url.

                    //startPage
                    if (pagerDetails.pageNo == 1)
                    {
                        if (endIndex == 1)
                        {
                            t.RecordRange = "1-"+ pagerDetails.totalResults.ToString();
                            t.RecordCount = pagerDetails.totalResults;
                        }
                        else
                        {
                            t.RecordRange = "1-" + pagerDetails.pageSize.ToString();
                            t.RecordCount = pagerDetails.pageSize;
                        }
                        t.PreviousPageUrl = string.Empty;
                        t.NextPageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + (pagerDetails.pageNo + 1) + "/";
                    }
                    //endpage
                    else if (endIndex == pagerDetails.pageNo)
                    {
                        t.RecordCount = (pagerDetails.totalResults % pagerDetails.pageSize) == 0 ? pagerDetails.pageSize : (pagerDetails.totalResults % pagerDetails.pageSize);
                        t.RecordRange = (((pagerDetails.pageNo - 1) * pagerDetails.pageSize) + 1).ToString() + "-" + (((pagerDetails.pageNo - 1) * pagerDetails.pageSize) + t.RecordCount).ToString();
                        t.NextPageUrl = "";
                        t.PreviousPageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + (pagerDetails.pageNo - 1) + "/";
                    }
                    //middlepage
                    else
                    {
                        t.RecordCount = pagerDetails.pageSize;
                        t.RecordRange = (((pagerDetails.pageNo - 1) * pagerDetails.pageSize) + 1).ToString() + "-" + (pagerDetails.pageNo * pagerDetails.pageSize).ToString();
                        t.NextPageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + (pagerDetails.pageNo + 1) + "/";
                        t.PreviousPageUrl = pagerDetails.baseUrl + pagerDetails.pageUrlType + (pagerDetails.pageNo - 1) + "/";
                    }
                    
                }
                else
                {
                  if((int)Math.Ceiling((double)pagerDetails.totalResults) > 0)
                  {
                      t.RecordRange = "1-" + pagerDetails.totalResults.ToString();
                      t.RecordCount = pagerDetails.totalResults;
                  }
                    PagerUrlList pagerUrlList = new PagerUrlList();
                    pagerUrlList.PageNo = -1;
                    results.Add(pagerUrlList); //No pager created if the number of pages is equal to 1.
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Pager Class Error.");
                objErr.LogException();
            }
            
            t.TotalCount = pagerDetails.totalResults;
            t.PagesDetail = results;
            return t;
        }

        private void GetStartEndIndex(PagerEntity pagerDetails, int pageCount ,int curSlot,out int startIndex , out int endIndex)
        {
            bool isPagerSlotEven = false;
           startIndex = (curSlot - 1) * pagerDetails.pagerSlotSize + 1;//Calculate the start index.
           endIndex = curSlot * pagerDetails.pagerSlotSize;//Calculate the end Index.
           if (pagerDetails.pagerSlotSize % 2 == 0)// Check if pager slot size is even or odd to calculate the start index and the end index.
           {                                       // This is required to keep the selected page in the centre of the pager whilst the pager size is maintained.
              isPagerSlotEven = true;
           }
           int pagerSlotHalf = (int)Math.Floor((double)pagerDetails.pagerSlotSize / 2);
           //This sets the start index and end index such that the current page is in always in the centre, whilst the pager slot size is maintained.
           if (pagerSlotHalf < pagerDetails.pageNo)
           {
              if (isPagerSlotEven)
                 {
                     startIndex = pagerDetails.pageNo - (pagerSlotHalf - 1);
                     endIndex = pagerDetails.pageNo + (pagerSlotHalf);
                 }
              else
                 {
                     startIndex = pagerDetails.pageNo - pagerSlotHalf;
                     endIndex = pagerDetails.pageNo + pagerSlotHalf;
                 }
         }
         endIndex = endIndex <= pageCount ? endIndex : pageCount;
        }
    }
}
