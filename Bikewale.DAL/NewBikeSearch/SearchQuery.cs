using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
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
        IUnityContainer container;
        IProcessFilter processFilter;
        string _whereClause = " ma.isdeleted = 0 and ma.new = 1 and mo.isdeleted = 0 and mo.new = 1 "
                            + " and mo.futuristic = 0 and bv.new = 1 and bv.isdeleted = 0";


        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To get Selected Clause of New Bike Search query
        /// </summary>
        /// <returns></returns>
        public string GetSelectClause()
        {
            string selectClause = string.Empty;
            try
            {
                selectClause = @" concat(ma.name,' ',mo.name) as bikename
		                        ,ma.id as makeid
		                        ,ma.name makename
		                        ,ma.maskingname as makemaskingname
		                        ,mo.id as modelid
		                        ,mo.name modelname
		                        ,mo.maskingname as modelmappingname
		                        ,mo.hosturl
		                        ,mo.originalimagepath as imagepath
		                        ,ifnull(sd.displacement,0) displacement
		                        ,sd.fueltype
		                        ,ifnull(sd.maxpower,0) as power
		                        ,ifnull(sd.fuelefficiencyoverall,0) fuelefficiencyoverall
		                        ,ifnull(sd.kerbweight,0) as weight
		                        ,ifnull(mo.minprice, 0) as minprice
		                        ,ifnull(mo.maxprice, 0) as maxprice
		                        ,ifnull(mo.reviewrate, 0) moreviewrate
		                        ,ifnull(mo.reviewcount, 0) moreviewcount
		                        ,ifnull(bv.reviewrate, 0) vsreviewrate
		                        ,ifnull(bv.reviewcount, 0) vsreviewcount
                                ,ifnull(sd.maximumtorque,0) maximumtorque
                                ,ifnull(mpb.modelwisepqcount, 0) modelwisepqcount ";
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSelectClause");
                objError.SendMail();
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
                            + " inner join bikemakes as ma   on ma.id = mo.bikemakeid "
                            + " left join newbikespecifications as sd  on sd.bikeversionid = bv.id "
                            + " left join mostpopularbikes mpb  on mpb.modelid = mo.id and mpb.rownum = 1 ";
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSelectClause");
                objError.SendMail();
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetOrderByClause");
                objError.SendMail();
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetRecordCountQry");
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

                    _whereClause += " and ma.id in ( " + makeList + " ) ";
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.MileageClause");
                objError.SendMail();
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.MileageClause");
                objError.SendMail();
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.MileageClause");
                objError.SendMail();
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
                    if (CommonValidators.ValidRange(Convert.ToInt32(filterInputs.Displacement[i]), 1, 6))
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.DisplacementClause");
                objError.SendMail();
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.BudgetClause");
                objError.SendMail();
            }
        }

        private string GetDisplacementClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " sd.displacement <= 110 ";
                    break;
                case "2":
                    clause = " sd.displacement between 110 and 150 ";
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
                //searchResultQuery = @" WITH CTE_BikeModels AS ( SELECT DENSE_RANK() OVER (ORDER BY " + GetDenseRankClause() + " , MO.MinPrice ASC, MO.Name ASC) AS DenseRank "
                //                    + " ,ROW_NUMBER() OVER (PARTITION BY MO.ID ORDER BY MO.MinPrice) AS ModelRank, "
                //                    + GetSelectClause()
                //                    + " FROM " + GetFromClause() + " Where " + GetWhereClause() + " ) SELECT * FROM CTE_BikeModels "
                //                    + " WHERE DenseRank BETWEEN " + filterInputs.StartIndex + " AND " + filterInputs.EndIndex
                //                    + " AND ModelRank = 1 ORDER BY " + GetOrderByClause() + " ; ";

                searchResultQuery = string.Format(@"set @row_number:=0;set @curr_id:=0;
                                                    drop temporary table if exists temp_bikes_searched;
                                                    create temporary table temp_bikes_searched
                                                    select *, @row_number:= if(@curr_Id = modelid,@row_number+1,1) as modelrank,@curr_Id := modelid from
                                                    (   select {0}
                                                        from {1}
                                                        where {2}  
                                                        order by mo.id
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSearchResultQuery");
                objError.SendMail();
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetOrderByClause");
                objError.SendMail();
            }
            return retVal;
        }
    }
}
