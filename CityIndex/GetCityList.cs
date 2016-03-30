using Consumer;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace CityAutoSuggest
{
    public class GetCityList
    {
        private static string _con = ConfigurationManager.AppSettings["connectionString"];
        protected static readonly ILog log = LogManager.GetLogger(typeof(GetCityList));
        public static List<CityTempList> CityList()
        {
            List<CityTempList> objCity = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_con))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetCities";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = 7;

                        con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                objCity = new List<CityTempList>();
                                while (dr.Read())
                                    objCity.Add(new CityTempList()
                                    {
                                        CityId = Convert.ToInt32(dr["Value"]),                          //  Add CityId into Payload
                                        CityName = dr["Text"].ToString(),                               //  Add CityName into Payload
                                        MaskingName = dr["MaskingName"].ToString()                      //  Add Masking Name into Payload
                                    });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CityList : " + ex.Message);
                log.Error(MethodBase.GetCurrentMethod().Name + " :Error in fetching CityList from Database: ", ex);
            }
            return objCity;
        }

        public static List<CityList> GetSuggestList(List<CityTempList> objCityList)
        {
            List<CityList> objSuggestList = null;
            int count = objCityList.Count;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("Vijaywada", "Bezawada");           //Vijaywada
                ht.Add("Guwahati", "Gauhati");              //Guwahati
                ht.Add("Vadodara", "Baroda");               //Vadodara
                ht.Add("Valsad", "Bulsar");                 //Valsad
                ht.Add("Shimla", "Simla");                  //Shimla
                ht.Add("Panjim", "Panaji");                 //Panjim
                ht.Add("Bangalore", "Bengaluru");           //Bangalore 
                ht.Add("Mysore", "Mysuru");                 //Mysore  
                ht.Add("Mangalore", "Mangaluru");           //Mangalore 
                ht.Add("Belgaum", "Belagavi");              //Belgaum
                ht.Add("Hospet", "Hosapete");               //Hospet
                ht.Add("Chikmagalur", "Chikkamagaluru");    //Chikmagalur
                ht.Add("Thiruvananthapuram", "Trivandrum"); //Thiruvananthapuram
                ht.Add("Kochi", "Cochin");                  //Kochi
                ht.Add("Kozhikode", "Calicut");             //Kozhikode
                ht.Add("Mumbai", "Bombay");                 //Mumbai
                ht.Add("Pondicherry", "Puducherry");        //Pondicherry
                ht.Add("Jalandhar", "Jullunder");           //Jalandhar
                ht.Add("Ropar", "Rupnagar");                //Ropar
                ht.Add("Chennai", "Madras");                //Chennai
                ht.Add("Kolkata", "Calcutta");              //Kolkata
                ht.Add("Varanasi", "Banaras");              //Varanasi
                ht.Add("Bijapur", "Vijayapura");            //Vijayapura
                ht.Add("Pune", "Poona");                     //Pune
                ht.Add("Navi Mumbai", "New Bombay");         //Navi Mumbai
                objSuggestList = new List<CityList>();

                foreach (CityTempList cityItem in objCityList)
                {
                    string cityName = cityItem.CityName.Trim();
                    CityList ObjTemp = new CityList();

                    ObjTemp.Id = cityItem.CityId.ToString();
                    ObjTemp.name = cityItem.CityName.Trim();

                    ObjTemp.mm_suggest = new CitySuggestion();
                    ObjTemp.mm_suggest.output = cityItem.CityName;

                    ObjTemp.mm_suggest.payload = new Payload()
                    {
                        CityId = cityItem.CityId,
                        CityMaskingName = cityItem.MaskingName.Trim()
                    };

                    ObjTemp.mm_suggest.Weight = count;

                    ObjTemp.mm_suggest.input = new List<string>();
                    cityName = cityName.Replace('-', ' ');
                    string[] combinations = cityName.Split(' ');

                    //Generate all combination of a string
                    int l = combinations.Length;
                    for (int p = 1; p <= l; p++)
                    {
                        printSeq(l, p, combinations, ObjTemp);
                    }

                    //Generate all combinations for new string
                    string newcity = string.Empty;
                    if (ht.ContainsKey(cityName))
                    {
                        newcity = ht[cityName].ToString();
                        string[] newcombinations = newcity.Split(' ');
                        int l_new = combinations.Length;
                        for (int p = 1; p <= l_new; p++)
                        {
                            printSeq(l_new, p, combinations, ObjTemp);
                        }
                    }

                    objSuggestList.Add(ObjTemp);
                    count--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Suggest List Exception  : " + ex.Message);
                log.Error(MethodBase.GetCurrentMethod().Name + " :Error In creating City autosuggest list: ", ex);
            }
            return objSuggestList;
        }

        public static void printSeqUtil(int n, int k, ref int len, int[] arr, string[] combination, CityList obj)
        {
            // If length of current increasing sequence becomes k, print it
            if (len == k)
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
