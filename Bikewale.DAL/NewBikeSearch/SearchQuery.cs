using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using Microsoft.Practices.Unity;
using Bikewale.Notifications;
using System.Configuration;
using Bikewale.Utility;

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
        string _whereClause = " MA.IsDeleted = 0 AND MA.New = 1 AND MO.IsDeleted = 0 AND MO.New = 1 "
                            + " AND MO.Futuristic = 0 AND BV.New = 1 AND BV.IsDeleted = 0";


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
                selectClause = @" MA.NAME + ' ' + MO.NAME AS BikeName
		                        ,MA.ID AS MakeId
		                        ,Ma.NAME MakeName
		                        ,MA.MaskingName AS MakeMaskingName
		                        ,MO.ID AS ModelId
		                        ,Mo.NAME ModelName
		                        ,MO.MaskingName AS ModelMappingName
		                        ,MO.HostURL
		                        ,MO.OriginalImagePath AS ImagePath
		                        ,ISNULL(SD.Displacement,0) Displacement
		                        ,SD.FuelType
		                        ,ISNULL(SD.MaxPower,0) AS Power
		                        ,ISNULL(SD.FuelEfficiencyOverall,0) FuelEfficiencyOverall
		                        ,ISNULL(SD.KerbWeight,0) AS [Weight]
		                        ,MIN(ISNULL(SP.Price, 0)) OVER (PARTITION BY MO.ID) AS MinPrice
		                        ,MAX(ISNULL(SP.Price, 0)) OVER (PARTITION BY MO.ID) AS MaxPrice
		                        ,ISNULL(MO.ReviewRate, 0) MoReviewRate
		                        ,ISNULL(MO.ReviewCount, 0) MoReviewCount
		                        ,ISNULL(BV.ReviewRate, 0) VsReviewRate
		                        ,ISNULL(BV.ReviewCount, 0) VsReviewCount
                                ,ISNULL(MPB.ModelwisePQCount, 0) ModelwisePQCount ";
            }
            catch(Exception ex)
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
                fromClause = " BikeVersions AS BV WITH (NOLOCK) "
                            + " INNER JOIN BikeModels AS MO WITH (NOLOCK) ON MO.ID = BV.BikeModelId "
                            + " INNER JOIN BikeMakes AS MA WITH (NOLOCK) ON MA.ID = MO.BikeMakeId "
                            + " LEFT JOIN NewBikeSpecifications AS SD WITH (NOLOCK) ON SD.BikeVersionId = BV.ID "
                            + " LEFT JOIN NewBikeShowroomPrices AS SP WITH (NOLOCK) ON SP.BikeVersionId = BV.ID "
                            + " AND SP.CityId = " + ConfigurationManager.AppSettings["defaultCity"]
                            + " LEFT JOIN MostPopularBikes MPB WITH(NOLOCK) ON MPB.ModelId = MO.ID AND MPB.RowNum = 1 ";
            }
            catch(Exception ex)
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
                        retVal = " MinPrice " + (sortOrder == "1" ? " DESC " : " ASC ");
                        break;

                    case "2":
                        retVal = " MoReviewCount " + (sortOrder == "0" ? " DESC " : " ASC ");
                        break;

                    default:
                        retVal = " ModelwisePQCount DESC ";
                        break;
                }
            }
            catch(Exception ex)
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
                recordCountQuery = " Select Count(*) AS RecordCount FROM ( SELECT DENSE_RANK() OVER (ORDER BY MO.MinPrice) AS DenseRank "
                                    + " ,ROW_NUMBER() OVER (PARTITION BY MO.ID ORDER BY SP.Price) AS ModelRank "
                                    + " From " + GetFromClause() + " Where " + GetWhereClause() + " ) tbl WHERE ModelRank = 1; ";
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetRecordCountQry");
            }
            return recordCountQuery;
        }

        public void InitSearchCriteria(FilterInput filter)
        {
            filterInputs = filter;
            if(!String.IsNullOrEmpty(filterInputs.MinBudget))
                BudgetClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.Displacement))
                DisplacementClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.Mileage))
                MileageClause();

            if (CollectionHelper.IsNotEmpty(filterInputs.RideStyle))
                RideStyleClause();

            if(CollectionHelper.IsNotEmpty(filterInputs.Make) || CollectionHelper.IsNotEmpty(filterInputs.Model))
                MakeModelFilterClause();

            ABSFilterClause();

            WheelFilterClause();

            StartTypeFilterClause();

            BrakeTypeFilterClause();
        }

        private void BrakeTypeFilterClause()
        {
            if (filterInputs.DrumBrake && !filterInputs.DiscBrake)
                _whereClause += " AND SD.FrontDisc = 0 ";
            else if(filterInputs.DiscBrake && !filterInputs.DrumBrake)
                _whereClause += " AND SD.FrontDisc = 0 ";
        }

        private void StartTypeFilterClause()
        {
            if (filterInputs.Electric && !filterInputs.Manual)
                _whereClause += " AND SD.ElectricStart = 1 ";
            else if(!filterInputs.Electric && filterInputs.Manual)
                _whereClause += " AND SD.ElectricStart = 0 ";
        }

        private void WheelFilterClause()
        {
            if (filterInputs.SpokeWheel && !filterInputs.AlloyWheel)
                _whereClause += " AND SD.AlloyWheels = 0 ";
            else if (filterInputs.AlloyWheel && !filterInputs.SpokeWheel)
                _whereClause += " AND SD.AlloyWheels = 1 ";
        }

        private void ABSFilterClause()
        {
            if (filterInputs.ABSAvailable && !filterInputs.ABSNotAvailable)
                _whereClause += " AND SD.AntilockBrakingSystem = 1 ";
            else if(!filterInputs.ABSAvailable && filterInputs.ABSNotAvailable)
                _whereClause += " AND SD.AntilockBrakingSystem = 0 ";
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

                    _whereClause += " AND MA.Id IN ( " + makeList + " ) ";
                 }

                if(filterInputs.Model!=null && filterInputs.Model.Length>0)
                {
                    foreach (string str in filterInputs.Model)
                    {
                        modelList += str + ",";
                    }
                    modelList = modelList.Substring(0, modelList.Length - 1);

                    if (filterInputs.Make != null && filterInputs.Make.Length > 0)
                        _whereClause += " OR ";
                    else
                        _whereClause += " AND ";

                    _whereClause += " MO.Id IN ( " + modelList + " ) ";
                }
            }
            catch(Exception ex)
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

                _whereClause += " AND BV.BodyStyleId IN ( " + rideStyleList + " ) ";
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
                                tempClause += " OR " + mileageClause;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(tempClause))
                    _whereClause += " AND ( " + tempClause + " ) ";
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
                                tempClause += " OR " + displacementClause;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(tempClause))
                    _whereClause += " AND ( " + tempClause + " ) ";

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
                    _whereClause += " AND Sp.Price BETWEEN " + filterInputs.MinBudget + " AND " + filterInputs.MaxBudget;
                else if (!String.IsNullOrEmpty(filterInputs.MinBudget) && String.IsNullOrEmpty(filterInputs.MaxBudget))
                    _whereClause += " AND sp.Price >= " + filterInputs.MinBudget;
            }
            catch(Exception ex)
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
                    clause = " SD.Displacement <= 110 ";
                    break;
                case "2":
                    clause = " SD.Displacement BETWEEN 110 AND 150 ";
                    break;
                case "3":
                    clause = " SD.Displacement BETWEEN 150 AND 200 ";
                    break;
                case "4":
                    clause = " SD.Displacement BETWEEN 200 AND 250 ";
                    break;
                case "5":
                    clause = " SD.Displacement BETWEEN 250 AND 500 ";
                    break;
                case "6":
                    clause = " SD.Displacement >= 500000 ";
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
                    clause = " SD.FuelEfficiencyOverall >= 70 ";
                    break;
                case "2":
                    clause = " SD.FuelEfficiencyOverall BETWEEN 50 AND 70 ";
                    break;
                case "3":
                    clause = " SD.FuelEfficiencyOverall BETWEEN 30 AND 50 ";
                    break;
                case "4":
                    clause = " SD.FuelEfficiencyOverall <= 30 ";
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
                searchResultQuery = " WITH CTE_BikeModels AS ( SELECT DENSE_RANK() OVER (ORDER BY MO.MinPrice) AS DenseRank "
                                    + " ,ROW_NUMBER() OVER (PARTITION BY MO.ID ORDER BY SP.Price) AS ModelRank, "
                                    + GetSelectClause()
                                    + " FROM " + GetFromClause() + " Where " + GetWhereClause() + " ) SELECT * FROM CTE_BikeModels "
                                    + " WHERE DenseRank BETWEEN " + filterInputs.StartIndex + " AND " + filterInputs.EndIndex
                                    + " AND ModelRank = 1 ORDER BY " + GetOrderByClause() + " ; ";
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetSearchResultQuery");
                objError.SendMail();
            }
            return searchResultQuery;
        }
    }
}
