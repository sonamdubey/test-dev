using System;
using System.Collections.Generic;

namespace Carwale.Utility
{
    /// <summary>
    /// Author  :   Sachin Bharti(9/2/15)
    /// </summary>
    public class PageList
    {
        public static List<int> GetPages(uint recordCount, int pageSize)
        {
            try
            {
                var pagesList = new List<int>();

                if (pageSize != 0)
                {
                    var pageCount = (recordCount / pageSize);
                    if (recordCount % pageSize != 0)
                        pageCount++;

                    for (int i = 0; i < pageCount; i++)
                    {
                        pagesList.Add(i);
                    }
                    return pagesList;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }
    }
}
