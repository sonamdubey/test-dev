using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public class SortByCustomPriority : IComparer<int>
    {
        private Dictionary<int, int> sortDict = null;

        private SortByCustomPriority() { }

        public SortByCustomPriority(string commaSeparatedIds)
        {
            if (RegExValidations.ValidateCommaSeperatedNumbers(commaSeparatedIds))
            {
                sortDict = new Dictionary<int, int>();
                var cats = commaSeparatedIds.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < cats.Count; i++)
                {
                    sortDict.Add(Convert.ToInt32(cats[i]), i);
                }
            }
        }

        int IComparer<int>.Compare(int a, int b) //implement Compare
        {
            try
            {
                if (sortDict[a] > sortDict[b])
                {
                    return -1;
                }
                else if (sortDict[a] < sortDict[b])
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0; // equal
            }
        }
    }
}
