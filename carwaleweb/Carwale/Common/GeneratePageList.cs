using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for GeneratePageList
/// </summary>
/// 
namespace Carwale.UI.Common
{
    public class GeneratePageList
    {


        /// <summary>
        /// No of records per page
        /// </summary>
        public int PageSize { get; set; }

        public int CurrentPageNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int _PagesPerSlot = 10;
        public int PagesPerSlot
        {
            get { return _PagesPerSlot; }
            set { _PagesPerSlot = value; }
        }

        int _SlotCount = 0;
        public int SlotCount
        {
            get { return _SlotCount; }
            set { _SlotCount = value; }
        }

        int _CurrentSlot = 0;
        public int CurrentSlot
        {
            get { return _CurrentSlot; }
            set { _CurrentSlot = value; }
        }


        /// <summary>
        /// Total no of records matching user's criteria
        /// </summary>
        public int TotalRecordCount { get; set; }

        public List<int> GetPageList()
        {
            List<int> pagerList = new List<int>();

            // Get required information to generate pagination
            int pageCount = GetPageCount();
            SlotCount = GetSlotCount(pageCount);
            CurrentSlot = GetCurrentSlot(pageCount);
            if (pageCount > 1)
            {
                int startIndex = (CurrentSlot - 1) * PagesPerSlot + 1;
                int endIndex = CurrentSlot * PagesPerSlot;
                endIndex = endIndex <= pageCount ? endIndex : pageCount;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    pagerList.Add(i);
                }
            }
            return pagerList;
        }

        int GetPageCount()
        {
            return (int)Math.Ceiling((double)TotalRecordCount / (double)PageSize);
        }

        int GetSlotCount(int pageCount)
        {
            return (int)Math.Ceiling((double)pageCount / (double)PagesPerSlot);
        }

        int GetCurrentSlot(int pageCount)
        {
            return ((int)Math.Ceiling((double)(CurrentPageNumber) / (double)PagesPerSlot));
        }

    }
}