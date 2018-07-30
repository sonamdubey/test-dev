using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;

namespace Bikewale.DAL.NewBikeSearch
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 31 Aug 2015
    /// Summary : To create Sql Query For new bike search
    /// </summary>
    public class SearchQuery : ISearchQuery
    {
        FilterInput filterInputs;
        string _whereClause = " bv.isnewmake = 1 and mo.isdeleted = 0 and mo.new = 1 "
                            + " and mo.futuristic = 0 and bv.new = 1 and bv.isdeleted = 0";


        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To get Selected Clause of New Bike Search query
        /// Modified by : SAJAL GUPTA on 02-01-2017
        /// Desc : Added UnitSoldDate in select clause
        /// Modified by : Sajal Gupta on 05-01-2017
        /// Desc : Added NewsCount
        /// Modified by : Ashutosh Sharma on 12-Aug-2017
        /// Description : Added moratingscount
        /// </summary>
        /// <returns></returns>
        public string GetSelectClause()
        {
            string selectClause = string.Empty;
            try
            {
                selectClause = @" concat(bv.makename,' ',bv.modelname) as bikename
		                        ,bv.bikemakeid as makeid
		                        ,bv.makename makename
		                        ,bv.makemaskingname as makemaskingname
		                        ,bv.bikemodelid as modelid
		                        ,bv.modelname modelname
		                        ,bv.modelmaskingname as modelmappingname
		                        ,bv.modelhosturl as hosturl
		                        ,bv.modeloriginalimagepath as imagepath
		                        ,ifnull(sd.displacement,0) displacement
		                        ,sd.fueltype
		                        ,ifnull(sd.maxpower,0) as power
		                        ,ifnull(sd.fuelefficiencyoverall,0) fuelefficiencyoverall
		                        ,ifnull(sd.kerbweight,0) as weight
		                        ,ifnull(mo.minprice, 0) as minprice
		                        ,ifnull(mo.maxprice, 0) as maxprice
                                ,ifnull(mo.ratingscount, 0) as moratingscount
		                        ,ifnull(round(mo.reviewrate, 1), 0) moreviewrate
		                        ,ifnull(mo.reviewcount, 0) moreviewcount
		                        ,ifnull(bv.reviewrate, 0) vsreviewrate
		                        ,ifnull(bv.reviewcount, 0) vsreviewcount
                                ,ifnull(sd.maximumtorque,0) maximumtorque
                                ,ifnull(mpb.modelwisepqcount, 0) modelwisepqcount
                                ,ifnull(bs.smalldescription,'') as smalldescription
                                ,ifnull(bs.fulldescription,'') as fulldescription
                                ,ifnull(mo.PhotosCount,0) as PhotoCount
                                ,ifnull(mo.VideosCount,0) as VideoCount
                                ,ifnull(mo.UnitsSold,0) as UnitsSold
                                ,ifnull(mo.NewsCount, 0) as NewsCount
                                ,mo.launchdate as launchdate
                                ,mo.UnitSoldDate 
                                ,ifnull((select count(1) from bikeversions ibv where ibv.bikemodelid = mo.id and ibv.isdeleted = 0 and ibv.new = 1 group by mo.id),0) as VersionCount
                                ,ifnull((select count(1) from bikemodelcolors ibc where ibc.modelid = bv.bikemodelid and ibc.isactive = 1 group by ibc.modelid),0) as ColorCount";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSelectClause");
                
            }
            return selectClause;
        }

        public string GetFromClause()
        {
            string fromClause = string.Empty;
            try
            {
                fromClause = " bikeversions as bv "
                            + " inner join bikemodels as mo on mo.id = bv.bikemodelid "
                            + " left join bikesynopsis bs on bs.modelid = mo.id and bs.isactive = 1"
                            + " left join newbikespecifications as sd  on sd.bikeversionid = bv.id "
                            + " left join mostpopularbikes mpb  on mpb.modelid = mo.id and mpb.rownum = 1 ";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSelectClause");
                
            }
            return fromClause;
        }

        public string GetWhereClause()
        {
            return _whereClause;
        }

        public string GetOrderByClause()
        {
            string retVal = "";
            string sortCriteria = string.Empty, sortOrder = string.Empty;
            try
            {
                sortCriteria = filterInputs.sc;
                sortOrder = filterInputs.so;

                switch (sortCriteria)
                {
                    case "1":
                        retVal = " minprice " + (sortOrder == "1" ? " desc " : " asc ");
                        break;

                    case "2":
                        retVal = " fuelefficiencyoverall " + (string.IsNullOrEmpty(sortOrder) || sortOrder == "0" ? " DESC " : " ASC ");
                        break;

                    default:
                        retVal = " modelwisepqcount desc ";
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetOrderByClause");
                
            }
            return retVal;
        }

        public string GetRecordCountQry()
        {
            string recordCountQuery = string.Empty;
            try
            {
                recordCountQuery = " select count(*) as recordcount  from temp_bikes_searched   where modelrank = 1 ; drop temporary table if exists temp_bikes_searched;";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetRecordCountQry");
            }
            return recordCountQuery;
        }

        public void InitSearchCriteria(FilterInput filter)
        {
            filterInputs = filter;
            if (!String.IsNullOrEmpty(filterInputs.MinBudget))
                BudgetClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.Displacement))
                DisplacementClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.Mileage))
                MileageClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.RideStyle))
                RideStyleClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.Make) || CollectionHelper.IsNotEmpty(filterInputs.Model))
                MakeModelFilterClause();

            ABSFilterClause();

            WheelFilterClause();

            StartTypeFilterClause();

            BrakeTypeFilterClause();
        }

        private void BrakeTypeFilterClause()
        {
            if (filterInputs.DrumBrake && !filterInputs.DiscBrake)
                _whereClause += " and sd.frontdisc = 0 ";
            else if (filterInputs.DiscBrake && !filterInputs.DrumBrake)
                _whereClause += " and sd.frontdisc = 1 ";
        }

        private void StartTypeFilterClause()
        {
            if (filterInputs.Electric && !filterInputs.Manual)
                _whereClause += " and sd.electricstart = 1 ";
            else if (!filterInputs.Electric && filterInputs.Manual)
                _whereClause += " and sd.electricstart = 0 ";
        }

        private void WheelFilterClause()
        {
            if (filterInputs.SpokeWheel && !filterInputs.AlloyWheel)
                _whereClause += " and sd.alloywheels = 0 ";
            else if (filterInputs.AlloyWheel && !filterInputs.SpokeWheel)
                _whereClause += " and sd.alloywheels = 1 ";
        }

        private void ABSFilterClause()
        {
            if (filterInputs.ABSAvailable && !filterInputs.ABSNotAvailable)
                _whereClause += " and sd.antilockbrakingsystem = 1 ";
            else if (!filterInputs.ABSAvailable && filterInputs.ABSNotAvailable)
                _whereClause += " and sd.antilockbrakingsystem = 0 ";
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To Create Make Model filter clause
        /// </summary>
        private void MakeModelFilterClause()
        {
            string makeList = string.Empty, modelList = string.Empty;
            try
            {
                if (filterInputs.Make != null && filterInputs.Make.Length > 0)
                {
                    foreach (string str in filterInputs.Make)
                    {
                        makeList += str + ",";
                    }
                    makeList = makeList.Substring(0, makeList.Length - 1);

                    _whereClause += " and bv.bikemakeid in ( " + makeList + " ) ";
                }

                if (filterInputs.Model != null && filterInputs.Model.Length > 0)
                {
                    foreach (string str in filterInputs.Model)
                    {
                        modelList += str + ",";
                    }
                    modelList = modelList.Substring(0, modelList.Length - 1);

                    if (filterInputs.Make != null && filterInputs.Make.Length > 0)
                        _whereClause += " or ";
                    else
                        _whereClause += " and ";

                    _whereClause += " mo.id in ( " + modelList + " ) ";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.MileageClause");
                
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To Create ride style filter clause
        /// </summary>
        private void RideStyleClause()
        {
            string rideStyleList = string.Empty;
            try
            {
                foreach (string str in filterInputs.RideStyle)
                    rideStyleList += str + ",";

                rideStyleList = rideStyleList.Substring(0, rideStyleList.Length - 1);

                _whereClause += " and bv.bodystyleid in ( " + rideStyleList + " ) ";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.MileageClause");
                
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To Create Mileage clause
        /// </summary>
        private void MileageClause()
        {
            string tempClause = string.Empty;
            try
            {
                for (int i = 0; i < filterInputs.Mileage.Length; i++)
                {
                    if (CommonValidators.ValidRange(Convert.ToInt32(filterInputs.Mileage[i]), 1, 4))
                    {
                        string mileageClause = GetMileageClause(filterInputs.Mileage[i]);
                        if (!String.IsNullOrEmpty(mileageClause))
                        {
                            if (tempClause == string.Empty)
                                tempClause = mileageClause;
                            else
                                tempClause += " or " + mileageClause;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(tempClause))
                    _whereClause += " and ( " + tempClause + " ) ";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.MileageClause");
                
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To Create Displacement clause
        /// </summary>
        private void DisplacementClause()
        {
            string tempClause = string.Empty;
            try
            {
                for (int i = 0; i < filterInputs.Displacement.Length; i++)
                {
                    if (CommonValidators.ValidRange(Convert.ToInt32(filterInputs.Displacement[i]), 1, 8))
                    {
                        string displacementClause = GetDisplacementClause(filterInputs.Displacement[i]);
                        if (!String.IsNullOrEmpty(displacementClause))
                        {
                            if (tempClause == string.Empty)
                                tempClause = displacementClause;
                            else
                                tempClause += " or " + displacementClause;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(tempClause))
                    _whereClause += " and ( " + tempClause + " ) ";

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.DisplacementClause");
                
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To Create Budget clause
        /// </summary>
        private void BudgetClause()
        {
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.MinBudget) && !String.IsNullOrEmpty(filterInputs.MaxBudget))
                    _whereClause += " and mo.minprice between " + filterInputs.MinBudget + " and " + filterInputs.MaxBudget;
                else if (!String.IsNullOrEmpty(filterInputs.MinBudget) && String.IsNullOrEmpty(filterInputs.MaxBudget))
                    _whereClause += " and mo.minprice >= " + filterInputs.MinBudget;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.BudgetClause");
                
            }
        }

        /// <summary>
        /// Modified By :- Subodh Jain 14 March 2017
        /// Summary :- Added displacement 110-125
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetDisplacementClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " sd.displacement <= 110 ";
                    break;
                case "7":
                    clause = " sd.displacement between 110 and 125 ";
                    break;
                case "8":
                    clause = " sd.displacement between 125 and 150 ";
                    break;
                case "3":
                    clause = " sd.displacement between 150 and 200 ";
                    break;
                case "4":
                    clause = " sd.displacement between 200 and 250 ";
                    break;
                case "5":
                    clause = " sd.displacement between 250 and 500 ";
                    break;
                case "6":
                    clause = " sd.displacement >= 500 ";
                    break;
                case "2":
                    clause = " sd.displacement between 110 and 150 ";
                    break;
                default:
                    break;

            }
            return clause;
        }

        private string GetMileageClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " sd.fuelefficiencyoverall >= 70 ";
                    break;
                case "2":
                    clause = " sd.fuelefficiencyoverall <= 70 and sd.fuelefficiencyoverall >= 50 ";
                    break;
                case "3":
                    clause = " sd.fuelefficiencyoverall <= 50 and sd.fuelefficiencyoverall >= 30 ";
                    break;
                case "4":
                    clause = " sd.fuelefficiencyoverall <= 30 ";
                    break;
                default:
                    break;
            }
            return clause;
        }

        public string GetSearchResultQuery()
        {
            string searchResultQuery = string.Empty;
            try
            {

                searchResultQuery = string.Format(@"set @row_number:=0;set @curr_id:=0;
                                                    drop temporary table if exists temp_bikes_searched;
                                                    create temporary table temp_bikes_searched
                                                    select *, @row_number:= if(@curr_Id = modelid,@row_number+1,1) as modelrank,@curr_Id := modelid from
                                                    (   select {0}
                                                        from {1}
                                                        where {2}  
                                                        order by bv.bikemodelid
                                                    ) as t order by minprice ;

                                                    set @row_number:=0;

                                                    select * from 
                                                    (
                                                        select *,@row_number:= @row_number+1 as denserank
                                                        from temp_bikes_searched
	                                                    where modelrank = 1
                                                        order by  {3},  minprice asc,  modelname asc
                                                    ) as t
                                                    where denserank between {4} and {5};
                                                 ", GetSelectClause(), GetFromClause(), GetWhereClause(), GetOrderByClause(), filterInputs.StartIndex, filterInputs.EndIndex);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSearchResultQuery");
                
            }
            return searchResultQuery;
        }

        public string GetDenseRankClause()
        {
            string retVal = "";
            string sortCriteria = string.Empty, sortOrder = string.Empty;
            try
            {
                sortCriteria = filterInputs.sc;
                sortOrder = filterInputs.so;

                switch (sortCriteria)
                {
                    case "1":
                        retVal = " mo.minprice " + (sortOrder == "1" ? " desc " : " asc ");
                        break;

                    case "2":
                        retVal = " sd.fuelefficiencyoverall " + (string.IsNullOrEmpty(sortOrder) || sortOrder == "0" ? " DESC " : " ASC ");
                        break;

                    default:
                        retVal = " mpb.modelwisepqcount desc ";
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetOrderByClause");
            }
            return retVal;
        }
    }
}