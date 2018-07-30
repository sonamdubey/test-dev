using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.Pager
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 8 May 2014
    /// </summary>
    public class Pager : IPager
    {
        /// <summary>
        /// This method gets the list of page Urls for the pager.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagerDetails"></param>
        /// <returns></returns>
        public T GetPager<T>(PagerEntity pagerDetails) where T : PagerOutputEntity, new()
        {
            var results = new List<PagerUrlList>();
            T t = new T();
            try
            {
                bool firstPage = false, lastPage = false;
                string pageUrl;
                int startIndex, endIndex;
                int pageCount = (int)Math.Ceiling((double)pagerDetails.TotalResults / (double)pagerDetails.PageSize);// Total number of pages in the pager.

                if (pageCount > 1)
                {
                    int totalSlots = (int)Math.Ceiling((double)pageCount / (double)pagerDetails.PagerSlotSize); // The total number of slots for the pager.
                    int curSlot = ((int)Math.Floor((double)(pagerDetails.PageNo - 1) / (double)pagerDetails.PagerSlotSize)) + 1; // Current page slot.

                    if (pagerDetails.PageNo == 1)// check for if the current page is the first page or the last page.
                        firstPage = true;
                    else if (pagerDetails.PageNo == pageCount)
                        lastPage = true;

                    // Get Start and End Index.
                    GetStartEndIndex(pagerDetails, pageCount, curSlot, out startIndex, out endIndex);

                    // Set the first and last page Urls.
                    if (firstPage == false) t.FirstPageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + "1/";
                    if (lastPage == false) t.LastPageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + pageCount + "/";

                    //Get the list of page Urls.
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        PagerUrlList pagerUrlList = new PagerUrlList();
                        pageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + i.ToString() + "/";
                        pagerUrlList.PageNo = i;
                        pagerUrlList.PageUrl = pageUrl;

                        results.Add(pagerUrlList);

                    }

                    //Set previous and next page Url.
                    if (pagerDetails.PageNo == 1)
                    {
                        t.PreviousPageUrl = "";
                        t.NextPageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + (pagerDetails.PageNo + 1) + "/";
                    }
                    else if (endIndex == pagerDetails.PageNo)
                    {
                        t.NextPageUrl = "";
                        t.PreviousPageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + (pagerDetails.PageNo - 1) + "/";
                    }
                    else
                    {
                        t.NextPageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + (pagerDetails.PageNo + 1) + "/";
                        t.PreviousPageUrl = pagerDetails.BaseUrl + pagerDetails.PageUrlType + (pagerDetails.PageNo - 1) + "/";
                    }

                }
                else
                {
                    PagerUrlList pagerUrlList = new PagerUrlList();
                    pagerUrlList.PageNo = -1;
                    results.Add(pagerUrlList); //No pager created if the number of pages is equal to 1.
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Pager Class Error. GetPager");
                
            }

            t.PagesDetail = results;
            return t;
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 15 Sep 2016
        /// Summary: Fetch used bike pagers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagerDetails"></param>
        /// <returns></returns>
        public T GetUsedBikePager<T>(PagerEntity pagerDetails) where T : PagerOutputEntity, new()
        {
            var results = new List<PagerUrlList>();
            T t = new T();

            try
            {
                bool firstPage = false, lastPage = false;
                string pageUrl;
                int startIndex, endIndex;
                int pageCount = (int)Math.Ceiling((double)pagerDetails.TotalResults / (double)pagerDetails.PageSize);// Total number of pages in the pager.

                if (pageCount > 1)
                {
                    int totalSlots = (int)Math.Ceiling((double)pageCount / (double)pagerDetails.PagerSlotSize); // The total number of slots for the pager.
                    int curSlot = ((int)Math.Floor((double)(pagerDetails.PageNo - 1) / (double)pagerDetails.PagerSlotSize)) + 1; // Current page slot.

                    if (pagerDetails.PageNo == 1)// check for if the current page is the first page or the last page.
                        firstPage = true;
                    else if (pagerDetails.PageNo == pageCount)
                        lastPage = true;

                    // Get Start and End Index.
                    GetStartEndIndex(pagerDetails, pageCount, curSlot, out startIndex, out endIndex);

                    // Set the first and last page Urls.
                    if (firstPage == false) t.FirstPageUrl = string.Format("{0}-1/", pagerDetails.BaseUrl);
                    if (lastPage == false) t.LastPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, pageCount);

                    //Get the list of page Urls.
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        PagerUrlList pagerUrlList = new PagerUrlList();
                        pageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, i);
                        pagerUrlList.PageNo = i;
                        pagerUrlList.PageUrl = pageUrl;
                        results.Add(pagerUrlList);
                    }

                    //Set previous and next page Url.
                    if (pagerDetails.PageNo == 1)
                    {
                        t.PreviousPageUrl = "";
                        t.NextPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo + 1));
                    }
                    else if (endIndex == pagerDetails.PageNo)
                    {
                        t.NextPageUrl = "";
                        t.PreviousPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo - 1));
                    }
                    else
                    {
                        t.NextPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo + 1));
                        t.PreviousPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo - 1));
                    }
                }
                else
                {
                    PagerUrlList pagerUrlList = new PagerUrlList();
                    pagerUrlList.PageNo = -1;
                    results.Add(pagerUrlList); //No pager created if the number of pages is equal to 1.
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Pager Class Error. GetPager");
                
            }

            t.PagesDetail = results;
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagerDetails"></param>
        /// <param name="pageCount"></param>
        /// <param name="curSlot"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        private void GetStartEndIndex(PagerEntity pagerDetails, int pageCount, int curSlot, out int startIndex, out int endIndex)
        {
            bool isPagerSlotEven = false;

            startIndex = (curSlot - 1) * pagerDetails.PagerSlotSize + 1;//Calculate the start index.
            endIndex = curSlot * pagerDetails.PagerSlotSize;//Calculate the end Index.

            if (pagerDetails.PagerSlotSize != pageCount)
            {
                if (pagerDetails.PagerSlotSize % 2 == 0)// Check if pager slot size is even or odd to calculate the start index and the end index.
                {                                       // This is required to keep the selected page in the centre of the pager whilst the pager size is maintained.
                    isPagerSlotEven = true;
                }

                int pagerSlotHalf = (int)Math.Floor((double)pagerDetails.PagerSlotSize / 2);

                //This sets the start index and end index such that the current page is in always in the centre, whilst the pager slot size is maintained.
                if (pagerSlotHalf < pagerDetails.PageNo)
                {
                    if (isPagerSlotEven)
                    {
                        startIndex = pagerDetails.PageNo - (pagerSlotHalf - 1);
                        endIndex = pagerDetails.PageNo + (pagerSlotHalf);
                    }
                    else
                    {
                        startIndex = pagerDetails.PageNo - pagerSlotHalf;
                        endIndex = pagerDetails.PageNo + pagerSlotHalf;
                    }
                }
                endIndex = endIndex <= pageCount ? endIndex : pageCount;
            }
        }

        /// <summary>
        /// Function to the start and end index for the current page number.
        /// </summary>
        /// <param name="pageSize">Total number of records per page.</param>
        /// <param name="currentPageNo">Current page number</param>
        /// <param name="startIndex">start index for records</param>
        /// <param name="endIndex">End index for records</param>
        public void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex)
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
        public int GetTotalPages(int totalRecords, int pageSize)
        {
            int pageCount = (int)Math.Ceiling((double)totalRecords / (double)pageSize);// Total number of pages in the pager.

            return pageCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPage"></param>
        /// <returns></returns>
        public Bikewale.Models.Shared.Pager GetPagerControl(PagerEntity objPage)
        {
            Bikewale.Models.Shared.Pager objPager = null;
            int _startIndex = 0, _endIndex = 0;
            try
            {
                GetStartEndIndex(objPage.PageSize, objPage.PageNo, out _startIndex, out _endIndex);

                objPager = new Models.Shared.Pager();
                objPager.PagerOutput = GetPager<PagerOutputEntity>(objPage);
                objPager.CurrentPageNo = objPage.PageNo;
                objPager.TotalPages = GetTotalPages(objPage.TotalResults, objPage.PageSize);
                objPager.StartIndex = _startIndex;
                objPager.EndIndex = _endIndex;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Pager.Pager.GetPagerControl()");
            }

            return objPager;
        }
    }   // class
}   // namespace
