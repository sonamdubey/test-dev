using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
namespace CityAutoSuggest
{
    public class GetCityList
    {
        public static IEnumerable<CityTempList> CityList()
        {
            IList<CityTempList> objCity = null;
            Regex r = new Regex(@"\[([A-z0-9\s\S]+)?(\-)?([A-z0-9\s\S]+)?\]");
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcitiespq"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objCity = new List<CityTempList>();
                            while (dr.Read())
                                objCity.Add(new CityTempList()
                                {
                                    CityId = Convert.ToInt32(dr["id"]),                          //  Add CityId into Payload
                                    CityName = Convert.ToString(dr["name"]),                               //  Add CityName into Payload
                                    MaskingName = Convert.ToString(dr["citymaskingname"]),                     //  Add Masking Name into Payload
                                    Wt = Convert.ToInt32(dr["cnt"]),                          //Add PQ for that city
                                    StateName = Convert.ToString(dr["StateName"])                      //Add StateName
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CityList : " + ex.Message);
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name + " :Error in fetching CityList from Database: ", ex);
            }
            return objCity;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Corrected the new combination string
        /// </summary>
        /// <param name="objCityList"></param>
        /// <returns></returns>
        public static IEnumerable<CityList> GetSuggestList(IEnumerable<CityTempList> objCityList)
        {
            IList<CityList> objSuggestList = null;
            int count = objCityList.Count();
            try
            {
                Hashtable ht = new Hashtable();                                                 //Add old name corresponding to new city
                ht.Add("Vijaywada", "Bezawada"); ht.Add("Guwahati", "Gauhati"); ht.Add("Vadodara", "Baroda");
                ht.Add("Valsad", "Bulsar"); ht.Add("Shimla", "Simla"); ht.Add("Panjim", "Panaji");
                ht.Add("Bangalore", "Bengaluru"); ht.Add("Mysore", "Mysuru"); ht.Add("Mangalore", "Mangaluru");
                ht.Add("Belgaum", "Belagavi"); ht.Add("Hospet", "Hosapete"); ht.Add("Chikmagalur", "Chikkamagaluru");
                ht.Add("Thiruvananthapuram", "Trivandrum"); ht.Add("Kochi", "Cochin"); ht.Add("Kozhikode", "Calicut");
                ht.Add("Mumbai", "Bombay"); ht.Add("Pondicherry", "Puducherry"); ht.Add("Jalandhar", "Jullunder");
                ht.Add("Ropar", "Rupnagar"); ht.Add("Chennai", "Madras"); ht.Add("Kolkata", "Calcutta");
                ht.Add("Varanasi", "Banaras"); ht.Add("Bijapur", "Vijayapura"); ht.Add("Gurugram", "Gurgaon");
                ht.Add("Pune", "Poona"); ht.Add("Navi Mumbai", "New Bombay"); ht.Add("Nuh", "Mewat");
                ht.Add("Bengaluru", "Bangalore");


                Hashtable htd = new Hashtable();                                                //For Removing Text After Bracket
                htd.Add("Aurangabad (Bihar)", "Aurangabad"); htd.Add("Dindori - MH", "Dindori"); htd.Add("Mewat", "Nuh");
                htd.Add("Una (Gujarat)", "Una"); htd.Add("Una (HP)", "Una");
                Hashtable htf = new Hashtable();                                                //HashTable for Duplicate       

                Dictionary<string, decimal> City_Count = new Dictionary<string, decimal>();     //Create Dictionary
                foreach (CityTempList cityItem in objCityList)
                {
                    if (htd.ContainsKey(cityItem.CityName))                                     //For Duplicate Cities
                        cityItem.CityName = htd[cityItem.CityName].ToString();

                    if (!City_Count.ContainsKey(cityItem.CityName))                             //If Not Present then Add Cityname and 1
                        City_Count.Add(cityItem.CityName, 1);
                    else
                    {
                        City_Count.Remove(cityItem.CityName);                                   //If present then remove
                        htf.Add(cityItem.CityName, "P");                                        //If duplicate then Add present
                    }
                }
                City_Count.Clear();                                                             //Delete Dictionary
                objSuggestList = new List<CityList>();

                foreach (CityTempList cityItem in objCityList)
                {
                    CityList ObjTemp = new CityList();

                    ObjTemp.Id = cityItem.CityId.ToString();                                    //Add Document Id
                    ObjTemp.name = cityItem.CityName.Trim();                                    //Add Document Name
                    ObjTemp.mm_suggest = new CitySuggestion();                                  //Create Suggestion

                    if (htd.ContainsKey(cityItem.CityName))                                     //Handle 4 Cities
                        cityItem.CityName = htd[cityItem.CityName].ToString();

                    string cityName = cityItem.CityName.Trim();
                    ObjTemp.output = cityItem.CityName + ", " + cityItem.StateName;  //Display
                    ObjTemp.mm_suggest.weight = cityItem.Wt;                                    //Weight corresponding to PQ

                    ObjTemp.payload = new Payload()                                  //Payload
                    {
                        CityId = cityItem.CityId,
                        CityMaskingName = cityItem.MaskingName.Trim(),                          //Add masking name in payload
                    };

                    if (htf.ContainsKey(cityItem.CityName))                                     //Set flag true for duplicate city
                        ObjTemp.payload.IsDuplicate = true;

                    //ObjTemp.mm_suggest.Weight = count;                                        //This is Basically For Order By Text

                    ObjTemp.mm_suggest.input = new List<string>();
                    cityName = cityName.Replace('-', ' ');                                      //Remove - From Name
                    string[] tokens = cityName.Split(' ');                                //Break City in Diff Token

                    ObjTemp.mm_suggest.input.Add(ObjTemp.output);                    //Add output as input
                   
                    int length = Math.Min(tokens.Length,8);


                    ObjTemp.mm_suggest.input = new List<string>();

                    // For creating input in mm_suggest
                    for (int index = 1; index < 1 << length; index++)
                    {
                        int temp_value = index, jindex = 0;
                        string value = string.Empty;
                        while (temp_value > 0)
                        {
                            if ((temp_value & 1) > 0)
                                value = string.Format("{0} {1}", value, tokens[jindex]);
                            temp_value >>= 1;
                            jindex++;
                        }
                        if (!string.IsNullOrEmpty(value))
                            ObjTemp.mm_suggest.input.Add(value);

                    }
                    if (ht.ContainsKey(cityName))
                    {
                        string newcity = string.Empty;                                          //Generate all combinations for new string
                        newcity = ht[cityName].ToString();                                      //Take Old City
                        string[] newcombinations = newcity.Split(' ');                          //Break City in Diff Token
                        int l_new = newcombinations.Length;
                        for (int index = 1; index < 1 << l_new; index++)
                        {
                            int temp_value = index, jindex = 0;
                            string value = string.Empty;
                            while (temp_value > 0)
                            {
                                if ((temp_value & 1) > 0)
                                    value = string.Format("{0} {1}", value, newcombinations[jindex]);
                                temp_value >>= 1;
                                jindex++;
                            }
                            if (!string.IsNullOrEmpty(value))
                                ObjTemp.mm_suggest.input.Add(value);

                        }

                    }

                    ObjTemp.mm_suggest.contexts = new Context();
                    ObjTemp.mm_suggest.contexts.types = new List<string>();
                    ObjTemp.mm_suggest.contexts.types.Add("AllCity");
                    objSuggestList.Add(ObjTemp);                                                //Add document in list
                    count--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Suggest List Exception  : " + ex.Message);
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name + " :Error In creating City autosuggest list: ", ex);
            }
            return objSuggestList;
        }
    }
}
