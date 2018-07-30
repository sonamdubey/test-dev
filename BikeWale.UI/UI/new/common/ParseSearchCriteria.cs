// C# Document

using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Bikewale.Common;
using Bikewale.Notifications.MySqlUtility;

namespace Bikewale.New
{
    public class ParseSearchCriterias : search_result
    {
        // Public SqlCommand object to pass values of the parameters in parameterised format;
        public SqlCommand sqlCmdParams;

        NameValueCollection qsColl;

        // global StringBuilder to form sql query based on the selected parameters by the users
        //StringBuilder sbClause;

        // Constructor of the class which initializes 'command parameters' and 'search criterias'
        public ParseSearchCriterias(NameValueCollection qsCollection)
        {
            qsColl = qsCollection;

            Init_CommandParameters();

            if (CityId != string.Empty && CommonOpn.IsNumeric(CityId))
                Init_SearchCriteria();
            else
                IsCriteriasParsed = false;
        }

        string _whereClause = " MA.IsDeleted=0 AND MA.New=1 AND MO.IsDeleted = 0 AND MO.New=1 AND MO.Futuristic=0 AND "
		                    + " BV.New = 1 AND BV.IsDeleted = 0 ";

        string _cityId = "1";
        public string CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        bool _IsCriteriasParsed = true;
        public bool IsCriteriasParsed
        {
            get { return _IsCriteriasParsed; }
            set { _IsCriteriasParsed = value; }
        }

        int _GetCurrentPageIndex = 0;
        public int GetCurrentPageIndex
        {
            get { return _GetCurrentPageIndex; }
            set { _GetCurrentPageIndex = value; }
        }
        
        // Select parameters of sql query used to fatch user results 
        // modified By : Suresh on 24 july 2014 Retrived price from query
        public string GetSelectClause()
        {
            return " BV.ID  AS VersionId, MO.ID AS ModelId, MA.Name + ' ' + MO.Name AS BikeModel, MO.MaskingName AS ModelMappingName, MA.ID AS MakeId, " 
		            + " Ma.Name MakeName,MA.MaskingName AS MakeMaskingName, Mo.Name ModelName, BV.Name VersionName, MA.Name + ' ' + MO.Name + ' ' + BV.Name AS Bike, "
                    + " MO.HostUrl, MO.SmallPic, SD.Displacement, SD.FuelType, SD.MaxPower AS Power, SD.FuelEfficiencyOverall, MO.OriginalImagePath,"
		            + " SD.TransmissionType, "
		            + " ROW_NUMBER() OVER( PARTITION BY MO.ID ORDER BY SP.Price ) AS ModelRank, "
		            + " COUNT(MO.ID) OVER( PARTITION BY MO.ID ) AS ModelCount, "
		            + " MIN(ISNULL(SP.Price , 0)) OVER( PARTITION BY MO.ID ) AS MinPrice, "
		            + " MAX(ISNULL(SP.Price ,0)) OVER( PARTITION BY MO.ID ) AS MaxPrice, "
                    + " SP.Price, "
		            + " ISNULL(MO.ReviewRate, 0) MoReviewRate, "
		            + " ISNULL(MO.ReviewCount,0) MoReviewCount, "
		            + " ISNULL(BV.ReviewRate, 0) VsReviewRate, "
		            + " ISNULL(BV.ReviewCount,0) VsReviewCount ";
        }


        // FROM clause of sql query used to fatch user results
        public string GetFromClause()
        {
            return " BikeVersions AS BV With(NoLock) "
                    + " INNER JOIN BikeModels AS MO With(NoLock) ON MO.ID = BV.BikeModelId "
                    + " INNER JOIN BikeMakes AS MA With(NoLock) ON MA.ID = MO.BikeMakeId "
                    + " LEFT JOIN NewBikeSpecifications AS SD With(NoLock) ON SD.BikeVersionId = BV.ID "
                    + " LEFT JOIN NewBikeShowroomPrices AS SP With(NoLock) ON SP.BikeVersionId = BV.ID AND SP.CityId = " + Configuration.GetDefaultCityId;
        }

        /// <summary>
        ///      Where clause of sql query used to fatch user results 
        /// </summary>
        /// <returns></returns>
        public string GetWhereClause()
        {
            return _whereClause;// + GetSearchCriteria();
        }

        public string GetOrderByClause()
        {
            string retVal = "";

            string sortCriteria = string.Empty;
            string sortOrder = string.Empty;

            sortCriteria = qsColl.Get("sc");
            sortOrder = qsColl.Get("so");

            switch (sortCriteria)
            {
                case "1":
                    retVal = "BikeModel " + (sortOrder == "1" ? "DESC" : "ASC");
                    break;

                case "2":
                    retVal = "MaxPrice " + (sortOrder == "1" ? "DESC" : "ASC");
                    break;

                case "3":
                    retVal = "MoReviewCount " + (sortOrder == "0" ? "DESC" : "ASC");
                    break;

                default:
                    retVal = "MaxPrice";
                    break;
            }

            return retVal;
        }


        // Sql Query to show number of records matched users criteria
        public string GetRecordCountQry()
        {
            return " Select Count(ModelId) FROM ( SELECT MO.ID AS ModelId, ROW_NUMBER() OVER( PARTITION BY MO.ID ORDER BY SP.Price ) AS ModelRank "
                            + " From " + GetFromClause() + " Where " + GetWhereClause() + " ) tbl WHERE ModelRank = 1 ";            
        }

        // 
        void Init_CommandParameters()
        {
            string _currentPageIndex = qsColl.Get("pn");

            if (_currentPageIndex != null && CommonOpn.IsNumeric(_currentPageIndex))
                    GetCurrentPageIndex = Convert.ToInt32(_currentPageIndex);                     

            sqlCmdParams = new SqlCommand();
                       
            sqlCmdParams.Parameters.Add("@CityId", SqlDbType.BigInt).Value = CityId;
        }

        void Init_SearchCriteria()
        {
            MySqlDbUtilities db = new MySqlDbUtilities();

            //Authenticate budget parameters
            string budget = qsColl.Get("budget");
            if (!String.IsNullOrEmpty(budget))
            {
                string[] _arr = budget.Split(',');
                string tempClause = string.Empty;
                if (_arr.Length > 0)
                {
                    for (int i = 0; i < _arr.Length; i++)
                    {
                        if (ValidRange(Convert.ToInt32(_arr[i]), 1, 9))
                        {
                            string budgetClause = GetBudgetClause(_arr[i]);
                            if (budgetClause != "")
                            {
                                if (tempClause == string.Empty)
                                    tempClause = budgetClause;
                                else
                                    tempClause += " OR " + budgetClause;
                            }
                        }
                    }
                    if (tempClause != "")
                        _whereClause += " AND ( " + tempClause + " ) ";                    
                }
            }

            //Authenticate make parameters
            string makes = qsColl.Get("make");
            if (!String.IsNullOrEmpty(makes) && ValidateInClause(makes))
            {
                if (makes != "" && IsValidQsFormat(makes))
                {
                    _whereClause += " AND MA.Id in ( " + db.GetInClauseValue(makes, "Makes", sqlCmdParams) + " ) ";
                }
            }

            //Authenticate 'Body Type' parameters
            string bodyStyles = qsColl.Get("bs");
            if (!String.IsNullOrEmpty(bodyStyles) && ValidateInClause(bodyStyles))
            {
                if (bodyStyles != "" && IsValidQsFormat(bodyStyles))
                {
                    _whereClause += " AND BV.BodyStyleId IN ( " + db.GetInClauseValue(bodyStyles, "BodyStyles", sqlCmdParams) + " ) ";
                }
            }

            //Authenticate fuel parameters
            string fuel = qsColl.Get("fuel");
            if (!String.IsNullOrEmpty(fuel))
            {
                string[] _arr = fuel.Split(',');
                string tempClause = string.Empty;
                if (_arr.Length > 0)
                {
                    for (int i = 0; i < _arr.Length; i++)
                    {
                        if (ValidRange(Convert.ToInt32(_arr[i]), 1, 4))
                        {
                            string fuelType = GetFuelTypeClause(_arr[i]);
                            if (fuelType != "")
                            {
                                if (tempClause == string.Empty)
                                    tempClause = fuelType;
                                else
                                    tempClause += " OR " + fuelType;
                            }
                        }
                    }
                    if (tempClause != "")
                        _whereClause += " AND ( " + tempClause + " ) ";
                }
            }

            //Authenticate transmission parameters
            string transmission = qsColl.Get("transmission");
            if (!String.IsNullOrEmpty(transmission))
            {
                string[] _arr = transmission.Split(',');
                string tempClause = string.Empty;
                if (_arr.Length > 0)
                {
                    for (int i = 0; i < _arr.Length; i++)
                    {
                        if (ValidRange(Convert.ToInt32(_arr[i]), 1, 2))
                        {
                            string trans = GetTransmissionClause(_arr[i]);
                            if (trans != "")
                            {
                                if (tempClause == string.Empty)
                                    tempClause = trans;
                                else
                                    tempClause += " OR " + trans;
                            }
                        }
                    }
                    if (tempClause != "")
                        _whereClause += " AND ( " + tempClause + " ) ";
                }
            }            

            //Authenticate 'Seating Capacity' parameters
            string seat = qsColl.Get("seat");
            if (!String.IsNullOrEmpty(seat))
            {
                string[] _arr = seat.Split(',');
                string tempClause = string.Empty;
                if (_arr.Length > 0)
                {
                    for (int i = 0; i < _arr.Length; i++)
                    {
                        if (ValidRange(Convert.ToInt32(_arr[i]), 1, 3))
                        {
                            string sc = GetSeatCapacityClause(_arr[i]);
                            if (sc != "")
                            {
                                if (tempClause == string.Empty)
                                    tempClause = sc;
                                else
                                    tempClause += " OR " + sc;
                            }
                        }
                    }
                    if (tempClause != "")
                        _whereClause += " AND ( " + tempClause + " ) ";
                }
            }

            //Authenticate 'Engine Power' parameters
            string ep = qsColl.Get("ep");
            
            if (!String.IsNullOrEmpty(ep))
            {
                string[] _arr = ep.Split(',');                
                string tempClause = string.Empty;
                if (_arr.Length > 0)
                {                    
                    for (int i = 0; i < _arr.Length; i++)
                    {
                        if (ValidRange(Convert.ToInt32(_arr[i]), 1, 6))
                        {
                            string enginePower = GetEnginePowerClause(_arr[i]);
                            if (enginePower != "")
                            {
                                if (tempClause == string.Empty)
                                    tempClause = enginePower;
                                else
                                    tempClause += " OR " + enginePower;
                            }
                        }
                    }
                    if (tempClause != "")
                        _whereClause += " AND ( " + tempClause + " ) ";
                }
            }

            //Authenticate 'Important Features' parameters
            string features = qsColl.Get("feature");
            if (!String.IsNullOrEmpty(features))
            {
                string[] _arr = features.Split(',');
                string tempClause = string.Empty;
                if (_arr.Length > 0)
                {
                    for (int i = 0; i < _arr.Length; i++)
                    {
                        if (ValidRange(Convert.ToInt32(_arr[i]), 1, 6))
                        {
                            string feature = GetFeatures(_arr[i]);
                            if (feature != "")
                            {
                                _whereClause += " AND " + feature + " = 'A'";
                            }
                        }
                    }
                    //if (tempClause != "")
                    //    _whereClause += " AND ( " + tempClause + " ) ";
                }
            }
        }

        /// <summary>
        /// Check whether the value passed is between the specified range
        /// </summary>
        /// <param name="val">Value to be checked</param>
        /// <param name="minVal">Minimum value in the range</param>
        /// <param name="maxVal">Maximum value in the range</param>
        /// <returns></returns>
        private bool ValidRange(int val, int minVal, int maxVal)
        {
            bool retVal = false;

            if (val >= minVal && val <= maxVal)
                retVal = true;

            return retVal;
        }

        /// <summary>
        /// Vaidate the parameters chosen in the search criteria (dynamic entries)
        /// </summary>
        /// <param name="param">Param that needs to be checked</param>
        /// <returns>true/false</returns>
        bool ValidateInClause(string param)
        {
            Regex reg = new Regex(@"^([0-9]{1,3},?)+$");
            Trace.Warn("match_str : " + param + " status : " + reg.IsMatch(param));
            return reg.IsMatch(param);
        }
        /// <summary>
        /// Vaidate the parameters chosen in the search criteria
        /// </summary>
        /// <param name="param">Parameter to be checked</param>
        /// <param name="indexBoundary">Boundary to which it needs to be checked</param>
        /// <returns>true/false</returns>
        public bool ValidateParamIndex(string param, string indexBoundary)
        {
            HttpContext.Current.Trace.Warn("Values : " + param);
            Regex reg = new Regex(@"^([1-" + indexBoundary + @"],?)+$");
            HttpContext.Current.Trace.Warn("match_str : " + param + " status : " + reg.IsMatch(param));
            return reg.IsMatch(param);
        }

        /// <summary>
        /// This function will return price band preffered by the user, later it will be appended to WHERE clause.
        /// These price band refrenced form AutoBild India magazine.
        /// </summary>
        /// <param name="id">Index of the selected price band</param>
        /// <returns>sql query for selected price band</returns>
        string GetBudgetClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " Sp.Price <= 55000 ";
                    break;
                case "2":
                    clause = " Sp.Price BETWEEN 55000 AND 70000 ";
                    break;
                case "3":
                    clause = " Sp.Price BETWEEN 70000 AND 80000 ";
                    break;
                case "4":
                    clause = " Sp.Price BETWEEN 80000 AND 140000 ";
                    break;
                case "5":
                    clause = " Sp.Price BETWEEN 140000 AND 250000 ";
                    break;
                case "6":
                    clause = " Sp.Price BETWEEN 250000 AND 500000  ";
                    break;
                case "7":
                    clause = " Sp.Price >= 500000 ";
                    break;
                default:
                    break;

            }

            return clause;
        }

        /// <summary>
        /// This function will return sql query on the basis of selected fuel type index.
        /// </summary>
        /// <param name="id">Index of the selected Fuel Type</param>
        /// <returns>Sql Query for selected fuel type index</returns>
        string GetFuelTypeClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " FuelType = 'Petrol' ";
                    break;
                case "2":
                    clause = " FuelType = 'Diesel' ";
                    break;
                case "3":
                    clause = " FuelType = 'CNG' ";
                    break;
                case "4":
                    clause = " FuelType = 'Electric' ";
                    break;
                default:
                    break;
            }

            return clause;
        }

        /// <summary>
        /// Returns Sql query for selected seating capacity by the user
        /// </summary>
        /// <param name="id">index of the selected Seating Capacity Parameter</param>
        /// <returns></returns>
        string GetSeatCapacityClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " SD.SeatingCapacity BETWEEN 0 AND 5  ";
                    break;
                case "2":
                    clause = " SD.SeatingCapacity BETWEEN 6 AND 8 ";
                    break;
                case "3":
                    clause = " SD.SeatingCapacity >= 9 ";
                    break;
                default:
                    break;
            }

            return clause;
        }

        /// <summary>
        /// There will be two possibilities for Transmission type that is
        /// 1. Automatic 2. Manual
        /// </summary>
        /// <param name="id">Index of the selected transmission type</param>
        /// <returns>Sql Query for selected index</returns>
        string GetTransmissionClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " BV.BikeTransmission = '1' ";    // 1 for automatic
                    break;
                case "2":
                    clause = " BV.BikeTransmission = '2' ";   // 2 for manual
                    break;
                default:
                    break;
            }

            return clause;
        }

        /// <summary>
        /// Sql query for selected engine power
        /// </summary>
        /// <param name="id">index of the selected engine power</param>
        /// <returns>Sql query for selected engine power</returns>
        string GetEnginePowerClause(string id)
        {
            string clause = string.Empty;

            switch (id)
            {
                case "1":
                    clause = " SD.Displacement BETWEEN 0 And 110 ";
                    break;

                case "2":
                    clause = " SD.Displacement BETWEEN 110 And 150 ";
                    break;

                case "3":
                    clause = " SD.Displacement BETWEEN 150 And 200 ";
                    break;

                case "4":
                    clause = " SD.Displacement BETWEEN 200 And 250";
                    break;

                case "5":
                    clause = " SD.Displacement BETWEEN 250 And 500";
                    break;

                case "6":
                    clause = " SD.Displacement >= 500 ";
                    break;

                default:
                    break;
            }

            return clause;
        }
        /// <summary>
        /// Sql query for selected features
        /// </summary>
        /// <param name="id">index of the selected features</param>
        /// <returns>Sql query for selected features</returns>
        string GetFeatures(string id)
        {
            string feature = "";

            switch (id)
            {
                case "1":
                    feature = "airconditioner";
                    break;

                case "2":
                    feature = "powersteering";
                    break;

                case "3":
                    feature = "powerwindows";
                    break;

                case "4":
                    feature = "abs";
                    break;

                case "5":
                    feature = "driverairbags";
                    break;

                case "6":
                    feature = "centrallocking";
                    break;

                default:
                    break;
            }

            return feature;
        }        

        bool IsValidQsFormat(string qsVal)
        {
            Regex regEx = new Regex("^[0-9]+(,[0-9]+)*$");

            Trace.Warn("is matched : " + qsVal + " : " + regEx.Match(qsVal, 0));

            return regEx.IsMatch(qsVal, 0);
        }
    }
}