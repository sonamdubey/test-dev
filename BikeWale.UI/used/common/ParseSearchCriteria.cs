using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Bikewale.Common;

/// <summary>
/// Summary description for ParseSearchCriteria
/// </summary>
namespace Bikewale.Used
{
    public class ParseSearchCriteria
    {
        public SqlCommand sqlCmdParams = null;

        NameValueCollection qsColl;

        // global StringBuilder to form sql query based on the selected parameters by the users
        StringBuilder sbClause;
        

        // Constructor of the class which initializes 'command parameters' and 'search criterias'
        public ParseSearchCriteria(NameValueCollection qsCollection)
        {
            qsColl = qsCollection;
            sqlCmdParams = new SqlCommand();

            string _currentPageIndex = qsColl.Get("pn");
            HttpContext.Current.Trace.Warn("current page index : +", _currentPageIndex);
            if (_currentPageIndex != null && CommonOpn.IsNumeric(_currentPageIndex))
                GetCurrentPageIndex = Convert.ToInt32(_currentPageIndex);

            if (!String.IsNullOrEmpty(qsColl.Get("city")))
                Init_CommandParameters();

            Init_SearchCriterias();
        }

        // Get lattitude on the basis of city distance
        public double Lattitude
        {
            get { return CommonOpn.GetLattitude(CityDistance); }
        }

        // Get longitude on the basis of city distance
        public double Longitude
        {
            get { return CommonOpn.GetLongitude(CityDistance); }
        }

        // Search in kms around selected city
        int _cityDistance = 50;
        public int CityDistance
        {
            get { return _cityDistance; }
            set { _cityDistance = value; }
        }

        // Prefered city by the user
        string _cityId = string.Empty;
        public string CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        /*// if user want to searc
        string _cityId = string.Empty;
        public string CityId
        {
            get{return _cityId;}
            set{_cityId = value;}			
        }*/

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

        // This property used to decide whether to refrence BikeVersions table or not
        // If any of these criteria(FuelType, Transmission, Body Style) is selected by the user refrence to BikeVersions table is needed.
        // and it should present in FROM clause.
        bool _IsVersionTableRefrenceNeeded = false;
        bool IsVersionTableRefrenceNeeded
        {
            get { return _IsVersionTableRefrenceNeeded; }
            set { _IsVersionTableRefrenceNeeded = value; }
        }

        // Select parameters of sql query used to fatch user results 
        public string GetSelectClause()
        {
            return " ProfileId, LL.VersionId AS BikeVersionId, BM.MaskingName AS MakeMaskingName,BMO.MaskingName AS ModelMaskingName, LC.MaskingName AS CityMaskingName,"
                + " LL.Seller, Ll.SellerType,LL.EntryDate, "
                + " LL.MakeName, LL.ModelName, LL.VersionName, LL.LastUpdated, LL.AreaName, "
                + " LL.Price, LL.Kilometers, LL.MakeYear BikeYear, LL.Color, LL.CityName City, LL.CityId, ISNULL(LL.PhotoCount, 0) AS PhotoCount , LL.FrontImagePath, "
                + " CertificationId, Vs.BikeFuelType, Vs.BikeTransmission, LL.AdditionalFuel, LL.HostUrl ";
        }


        // FROM clause of sql query used to fatch user results
        public string GetFromClause()
        {
            string from_clause = " LiveListings AS LL WITH(NOLOCK), BikeMakes BM  WITH (NOLOCK), BikeModels BMO WITH (NOLOCK), BikeVersions Vs WITH(NOLOCK)  ";

            // LL_Cities table required only if used searcing with perticular city
            // if user is searcing within entire state then refrence to this table is not required.
            // Commented By : Ashish G. Kamble on 6 Dec 2013
            //if (CityDistance != 1)
            //{
            //    from_clause += " ,BWCities AS LC WITH(NOLOCK) ";
            //}
            from_clause += " ,BWCities AS LC WITH(NOLOCK) ";

            return from_clause;
        }

        /// <summary>
        ///      Where clause of sql query used to fatch user results 
        /// </summary>
        /// <returns></returns>
        public string GetWhereClause()
        {
            string where_clause = string.Empty;

            // user is searching within entire state
            // no lattitude and longitude required in this case.
            if (CityDistance == 1)
            {
                where_clause = " LL.StateId IN ( Select StateId FROM BWCities With(NoLock) Where Id = @CityId ) AND LC.ID = LL.CityId " + GetSearchCriteria();
            }
            else if (CityId != "")
            {
                where_clause = " LC.Id = @CityId AND "
                              + " LL.Lattitude BETWEEN LC.Lattitude - @Lattitude AND LC.Lattitude + @Lattitude AND "
                              + " LL.Longitude BETWEEN LC.Longitude - @Longitude AND LC.Longitude + @Longitude " + GetSearchCriteria();

                //where_clause = " LC.Id = @CityId " + GetSearchCriteria();
            }
            else
            {
                where_clause += "  LL.CityId = LC.ID " + GetSearchCriteria(); 
            }


            where_clause += " AND BM.ID = LL.MakeId AND BMO.ID = LL.ModelId AND LL.VersionId = Vs.Id ";

            return where_clause;
        }

        public string GetOrderByClause()
        {
            return SortCriteria;
        }


        // Sql Query to show number of records matched users criteria
        public string GetRecordCountQry()
        {
            return " Select Count(ProfileId) From " + GetFromClause() + " Where " + GetWhereClause();
        }


        //if query string contains city then apply search criteria for a particular city 
        void Init_CommandParameters()
        {
            try
            {
                string _city = qsColl.Get("city");
                if (_city != "0")
                {
                    string _dist = qsColl.Get("dist") != null ? qsColl.Get("dist") : "50";

                    if (_city != null && CommonOpn.IsNumeric(_city) && _dist != null && CommonOpn.IsNumeric(_dist))
                    {
                        CityId = _city;
                        CityDistance = Convert.ToInt32(_dist);
                    }
                    else
                    {
                        IsCriteriasParsed = false;
                    }

                    sqlCmdParams.Parameters.Add("@Lattitude", SqlDbType.Decimal).Value = Lattitude;
                    sqlCmdParams.Parameters.Add("@Longitude", SqlDbType.Decimal).Value = Longitude;
                    sqlCmdParams.Parameters.Add("@CityId", SqlDbType.BigInt).Value = CityId;

                    HttpContext.Current.Trace.Warn("Lattitude : " + Lattitude + ",Longitude : " + Longitude + ",CityId : " + CityId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        //Add all search criteria to sql query
        void Init_SearchCriterias()
        {
            // All search criterias will be added to this StringBuilder in the form of sql query
            sbClause = new StringBuilder();

            // object of 'Database' class. nedeed to querysreing IN clause value 
            Database db = new Database();

            // Get method returns null in the following cases: 1) if the specified key is not found; and 2) 
            // if the specified key is found and its associated value is null. This method does not distinguish between the two cases.						
            string make_str = string.Empty;
            string model_str = string.Empty;
            bool isValidModelStr = false;
            bool isValidMakeStr = false;

            // if make make selected by the user
            if (qsColl.Get("make") != null)
            {
                // Get all the make ids in form of comma seperated string from NameValueCollection
                // I will pass this string to the IN clause value of make id
                make_str = qsColl.Get("make");

                // If models are selected, remove respective make ids from 'make_str'
                // because while model is selcted we need to show only those perticular models rather then all model of that brand				
                if (qsColl.Get("model") != null)
                {
                    make_str = "," + make_str + ",";

                    // Get all the model ids in form of comma seperated string from NameValueCollection
                    ///model_str = qsColl.Get("model");

                    // Get all the models and assign to an array
                    // These models are in a form like  '10.36'. Where part before dot(.) is a make id and other part is modelId 
                    // We need to extract these make id(s) from model array and need to replace from 'make_str'
                    string[] modelArray = qsColl.GetValues("model");

                    for (int i = 0; i < modelArray.Length; i++)
                    {
                        // extract make id from seected model query string
                        string makeId = modelArray[i].Split('.')[0];

                        // remove this make id from model because its no more needed
                        ///model_str = model_str.Replace(makeId + ".", "");
                        model_str += modelArray[i].Split('.')[1] + ",";

                        // if extracted make id exists in 'make_str', remove it, because user opted for particular model 
                        // make id no more needed
                        Regex reg = new Regex("(," + makeId + ",)");
                        make_str = reg.Replace(make_str, ",");
                    }// for

                    if (model_str.Length > 0)
                        model_str = model_str.Substring(0, model_str.Length - 1);

                    //remove the leading and the ending ','	
                    if (make_str.Length > 1)
                        make_str = make_str.Substring(1, make_str.Length - 2);
                    else
                        make_str = "";
                }// if


                isValidModelStr = ValidateInClause(model_str);
                isValidMakeStr = ValidateInClause(make_str);

                // Make is optec by the user
                if (make_str != "" && isValidMakeStr)
                {
                    // left bracket(only needed while model is selected)
                    string model_bracket = model_str != "" && isValidModelStr ? "(" : "";

                    // Append sql query to the StringBuilder object for selected makes. Also make the IN clause value parameterized to avoid Sql Injection
                    sbClause.Append(" AND " + model_bracket + " MakeId IN (" + db.GetInClauseValue(make_str, "MakeId", sqlCmdParams) + ")");
                }             
            }

            // get the selected models
            if (model_str != "" && isValidModelStr)
            {
                string applied_cond = " AND ";

                // If nither make_str nor model_str is blank that means we have to apply OR condition between them.
                // for ex. qry1 + "AND ( MakeId IN(make_str) OR ModelId IN(model_str) )" + qry2
                if (make_str != "" && isValidMakeStr)
                    applied_cond = " OR ";

                // Append sql query to the StringBuilder object for selected models. Also make the IN clause value parameterized to avoid Sql Injection
                sbClause.Append(applied_cond + "ModelId IN (" + db.GetInClauseValue(model_str, "ModelId", sqlCmdParams) + ") ");

                // right bracket(only needed while model is selected)
                if (make_str != "" && isValidMakeStr)
                    sbClause.Append(")");
            }

            // get the selected models
            if (qsColl.Get("seller") != null)
            {
                string seller_type = qsColl.Get("seller");

                if (seller_type.IndexOf(',') < 0)
                {
                    sqlCmdParams.Parameters.Add("@SellerType", SqlDbType.SmallInt).Value = seller_type;
                    sbClause.Append(" AND SellerType = @SellerType");
                }
            }

            // Append 'Body Style' to the sql query
            string body_style = qsColl.Get("bs");
            if (body_style != null && ValidateInClause(body_style))
            {
                // If any of these criteria(FuelType, Transmission, Body Style) is selected by the user refrence to BikeVersions table is needed.		
                IsVersionTableRefrenceNeeded = true;
                sbClause.Append(" AND Vs.BodyStyleId IN(" + db.GetInClauseValue(body_style, "BodyStyleId", sqlCmdParams) + ")");
            }

            // Append 'Fuel Type' to the sql query
            string fuel = qsColl.Get("fuel");
            if (fuel != null && ValidateInClause(fuel))
            {
                // If any of these criteria(FuelType, Transmission, Body Style) is selected by the user refrence to BikeVersions table is needed.		
                IsVersionTableRefrenceNeeded = true;
                sbClause.Append(" AND Vs.BikeFuelType IN(" + db.GetInClauseValue(fuel, "FuelType", sqlCmdParams) + ")");
            }


            // Append transmission to the sql query
            string trans = qsColl.Get("tm");
            if (trans != null)
            {
                if (trans.IndexOf(",") < 0 && CommonOpn.IsNumeric(trans))
                {
                    // If any of these criteria(FuelType, Transmission, Body Style) is selected by the user refrence to BikeVersions table is needed.		
                    IsVersionTableRefrenceNeeded = true;

                    sbClause.Append(" AND Vs.BikeTransmission = @Transmission");
                    sqlCmdParams.Parameters.Add("@Transmission", SqlDbType.TinyInt).Value = trans;
                }
            }

            GetSearchClause("budget", ref sbClause);
            GetSearchClause("year", ref sbClause);
            GetSearchClause("kms", ref sbClause);
        }

        public string GetSearchCriteria()
        {           
            return sbClause.ToString();
        }

        public void GetSearchClause(string param, ref StringBuilder sbClause)
        {
            string _param = qsColl.Get(param);

            if (_param != null)
            {
                if (ValidateParamIndex(_param, "6"))
                {
                    string[] paramValArray = qsColl.GetValues(param);

                    switch (param)
                    {
                        case "budget":
                            sbClause.Append(GetBudgetClause(paramValArray));
                            break;

                        case "year":
                            sbClause.Append(GetYearClause(paramValArray));
                            break;

                        case "kms":
                            sbClause.Append(GetKmsClause(paramValArray));
                            break;

                        default:
                            break;
                    }
                }// if
            }
        }

        public string GetBudgetClause(string[] paramValArray)
        {
            string conditionClause = string.Empty;

            if (paramValArray.Length > 0)
                conditionClause = " AND ( ";

            for (int i = 0; i < paramValArray.Length; ++i)
            {
                switch (paramValArray[i])
                {
                    case "0":
                        conditionClause += " LL.Price Between 0 And 10000 ";
                        break;
                    case "1":
                        conditionClause += " LL.Price Between 10001 And 20000 ";
                        break;
                    case "2":
                        conditionClause += " LL.Price Between 20001 And 35000 ";
                        break;
                    case "3":
                        conditionClause += " LL.Price Between 35001 And 50000 ";
                        break;
                    case "4":
                        conditionClause += " LL.Price Between 50001 And 80000 ";
                        break;
                    case "5":
                        conditionClause += " LL.Price Between 80001 And 150000 ";
                        break;
                    case "6":
                        conditionClause += " LL.Price >= 150001 ";
                        break;
                    default:
                        conditionClause = string.Empty;
                        break;
                }
                if (paramValArray.Length > 1 && i != (paramValArray.Length - 1) && conditionClause != string.Empty)
                    conditionClause += " OR ";
            }

            if (conditionClause != "")
                conditionClause += ")";

            return conditionClause;
        }

        public string GetYearClause(string[] paramValArray)
        {
            string conditionClause = string.Empty;

            if (paramValArray.Length > 0)
                conditionClause = " AND ( ";

            for (int i = 0; i < paramValArray.Length; ++i)
            {
                switch (paramValArray[i])
                {
                    case "0":
                        conditionClause += " Year(MakeYear) Between " + (DateTime.Today.Year - 1) + " And " + DateTime.Today.Year ;
                        break;
                    case "1":
                        conditionClause += " Year(MakeYear) Between " + (DateTime.Today.Year - 3) + " And " + (DateTime.Today.Year - 1);
                        break;
                    case "2":
                        conditionClause += " Year(MakeYear) Between " + (DateTime.Today.Year - 5) + " And " + (DateTime.Today.Year - 3);
                        break;
                    case "3":
                        conditionClause += " Year(MakeYear) Between " + (DateTime.Today.Year - 8) + " And " + (DateTime.Today.Year - 5);
                        break;
                    case "4":
                        conditionClause += " Year(MakeYear) <= " + (DateTime.Today.Year - 8);
                        break;
                    default:
                        conditionClause = string.Empty;
                        break;
                }
                if (paramValArray.Length > 1 && i != (paramValArray.Length - 1) && conditionClause != string.Empty)
                    conditionClause += " OR ";
            }

            if (conditionClause != "")
                conditionClause += ")";

            return conditionClause;
        }

        public string GetKmsClause(string[] paramValArray)
        {
            string conditionClause = string.Empty;

            if (paramValArray.Length > 0)
                conditionClause = " AND ( ";

            for (int i = 0; i < paramValArray.Length; ++i)
            {
                switch (paramValArray[i])
                {
                    case "0":
                        conditionClause += " Kilometers Between 0 And 5000 ";
                        break;
                    case "1":
                        conditionClause += " Kilometers Between 5000 And 15000 ";
                        break;
                    case "2":
                        conditionClause += " Kilometers Between 15000 And 30000 ";
                        break;
                    case "3":
                        conditionClause += " Kilometers Between 30000 And 50000 ";
                        break;
                    case "4":
                        conditionClause += " Kilometers Between 50000 And 80000 ";
                        break;
                    case "5":
                        conditionClause += " Kilometers >= 80000 ";
                        break;
                    default:
                        conditionClause = string.Empty;
                        break;
                }
                if (paramValArray.Length > 1 && i != (paramValArray.Length - 1) && conditionClause != string.Empty)
                    conditionClause += " OR ";
            }

            if (conditionClause != "")
                conditionClause += ")";

            return conditionClause;
        }

        private string SortCriteria
        {
            get
            {
                string retVal = "";

                string sortCriteria = string.Empty;
                string sortOrder = string.Empty;

                sortCriteria = qsColl.Get("sc");
                sortOrder = qsColl.Get("so");

                switch (sortCriteria)
                {
                    case "0":
                        retVal = "MakeYear " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    case "1":
                        retVal = "MakeName " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    case "2":
                        retVal = "LL.Price " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    case "3":
                        retVal = "Kilometers " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    case "4":
                        retVal = "SellerType " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    case "5":
                        retVal = "LL.CityName " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    case "6":
                        retVal = "LastUpdated " + (sortOrder == "1" ? "DESC" : "ASC");
                        break;

                    default:
                        retVal = "LastUpdated DESC";//Priority, SellerType DESC, LastUpdated DESC
                        break;
                }

                return retVal;
            }
        }

        bool ValidateInClause(string match_str)
        {
            Regex reg = new Regex(@"^([0-9]{1,3},?)+$");          
            return reg.IsMatch(match_str);
        }

        bool ValidateParamIndex(string match_str, string index_boundry)
        {           
            Regex reg = new Regex(@"^([0-" + index_boundry + "],?)+$");         
            return reg.IsMatch(match_str);
        }
    }
}