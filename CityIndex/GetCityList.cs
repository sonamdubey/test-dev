using Consumer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using Bikewale.CoreDAL;

using System.Text.RegularExpressions;
using MySql.CoreDAL;
namespace CityAutoSuggest
{
    public class GetCityList
    {
        public static List<CityTempList> CityList()
        {
            List<CityTempList> objCity = null;
            Regex r = new Regex(@"\[([A-z0-9\s\S]+)?(\-)?([A-z0-9\s\S]+)?\]");
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcities"))
                {
                    cmd.CommandText = "GetCitiesCS";                                          //----New SP-----
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, 7));
                       // Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd,ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objCity = new List<CityTempList>();
                            while (dr.Read())
                                objCity.Add(new CityTempList()
                                {
                                    CityId = Convert.ToInt32(dr["Value"]),                          //  Add CityId into Payload
                                    CityName = dr["Text"].ToString(),                               //  Add CityName into Payload
                                    MaskingName = dr["MaskingName"].ToString(),                     //  Add Masking Name into Payload
                                    Wt = Convert.ToInt32(dr["Count"]),                          //Add PQ for that city
                                    StateName = dr["StateName"].ToString()                      //Add StateName
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
        public static List<CityList> GetSuggestList(List<CityTempList> objCityList)
        {
            List<CityList> objSuggestList = null;
            int count = objCityList.Count;
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
                htd.Add("Una (Gujarat)", "Una"); htd.Add("Una (HP)", "Una"); htd.Add("Gurgaon", "Gurugram");
                htd.Add("Bangalore", "Bengaluru");             
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
                    ObjTemp.mm_suggest.output = cityItem.CityName + ", " + cityItem.StateName;  //Display
                    ObjTemp.mm_suggest.weight = cityItem.Wt;                                    //Weight corresponding to PQ

                    ObjTemp.mm_suggest.payload = new Payload()                                  //Payload
                    {
                        CityId = cityItem.CityId,                                               //Add CityId in Payload
                        CityMaskingName = cityItem.MaskingName.Trim(),                          //Add masking name in payload
                    };

                    if (htf.ContainsKey(cityItem.CityName))                                     //Set flag true for duplicate city
                        ObjTemp.mm_suggest.payload.IsDuplicate = true;

                    //ObjTemp.mm_suggest.Weight = count;                                        //This is Basically For Order By Text

                    ObjTemp.mm_suggest.input = new List<string>();
                    cityName = cityName.Replace('-', ' ');                                      //Remove - From Name
                    string[] combinations = cityName.Split(' ');                                //Break City in Diff Token

                    ObjTemp.mm_suggest.input.Add(ObjTemp.mm_suggest.output);                    //Add output as input
                    //Generate all combination of a string
                    int l = combinations.Length;
                    for (int p = 1; p <= l; p++)
                    {
                        printSeq(l, p, combinations, ObjTemp);                                  //Add All Tokens
                    }

                    if (ht.ContainsKey(cityName))
                    {
                        string newcity = string.Empty;                                          //Generate all combinations for new string
                        newcity = ht[cityName].ToString();                                      //Take Old City
                        string[] newcombinations = newcity.Split(' ');                          //Break City in Diff Token
                        int l_new = newcombinations.Length;
                        for (int p = 1; p <= l_new; p++)
                        {
                            printSeq(l_new, p, newcombinations, ObjTemp);                       //Add All Tokens
                        }
                    }

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

        public static void printSeqUtil(int n, int k, ref int len, int[] arr, string[] combination, CityList obj)
        {
            if (len == k)                                                                       //If length of current increasing sequence becomes k, print it
            {
                if (k == 1)
                    obj.mm_suggest.input.Add(String.Format("{0}", combination[arr[0] - 1].Trim()));
                else if (k == 2)
                    obj.mm_suggest.input.Add(String.Format("{0} {1}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim()));
                else if (k == 3)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim()));
                else if (k == 4)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim()));
                return;
            }

            int i = (len == 0) ? 1 : arr[len - 1] + 1;
            len++;	// Increase length
            // Put all numbers (which are greater than the previous element) at new position.
            while (i <= n)
            {
                arr[len - 1] = i;
                printSeqUtil(n, k, ref len, arr, combination, obj);
                i++;
            }
            // This is important. The variable 'len' is shared among all function calls in recursion tree. Its value must be brought back before next iteration of while loop
            len--;
        }

        public static void printSeq(int l, int p, string[] combination, CityList obj)
        {
            int[] arr = new int[p];
            int len = 0;
            printSeqUtil(l, p, ref len, arr, combination, obj);
        }
    }
}
