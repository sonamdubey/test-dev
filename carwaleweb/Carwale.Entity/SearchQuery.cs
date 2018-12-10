using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class SearchQuery<T>
    {
        //------------------------------------------------------------
        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchQuery()
        {
            Filters = new List<Expression<Func<T, bool>>>();
            //SortCriterias = new List<ISortCriteria<T>>();
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Contains a list of filters to be applied to the query.
        /// </summary>
        public List<Expression<Func<T, bool>>> Filters { get; protected set; }

        //-----------------------------------------------------------
        /// <summary>
        /// Adds a new filter to the list
        /// </summary>
        /// <param name="filter"></param>
        public void AddFilter(Expression<Func<T, Boolean>> filter)
        {
            Filters.Add(filter);
        }

        //-----------------------------------------------------------
        /// <summary>
        /// Contains a list of criterias that would be used for sorting.
        /// </summary>
        //public List<ISortCriteria<T>> SortCriterias
        //{
        //    get;
        //    protected set;
        //}

        //-------------------------------------------------------------
        /// <summary>
        /// Adds a Sort Criteria to the list.
        /// </summary>
        //public void AddSortCriteria(ISortCriteria<T> sortCriteria)
        //{
        //    SortCriterias.Add(sortCriteria);
        //}

        //-------------------------------------------------------------
        /// <summary>
        /// Contains a list of properties that would be eagerly loaded 
        /// with he query.
        /// </summary>
        public string IncludeProperties { get; set; }

        //-------------------------------------------------------------
        /// <summary>
        /// Number of items to be skipped. Useful for paging.
        /// </summary>
        public int Skip { get; set; }

        //-------------------------------------------------------------
        /// <summary>
        /// Represents the number of items to be returned by the query.
        /// </summary>
        public int Take { get; set; }
    }
}
