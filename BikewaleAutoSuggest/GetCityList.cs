using Consumer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleAutoSuggest
{
    public class GetCityList
    {
        private static string _con = ConfigurationManager.AppSettings["connectionString"];

        public static List<CityTempList> CityList()
        {
            List<CityTempList> objCity = null;
            try
            {
                using(SqlConnection con=new SqlConnection(_con))
                {
                    using(SqlCommand cmd=new SqlCommand())
                    {
                        cmd.CommandText = "GetCities";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = 7;

                        con.Open();

                        using(SqlDataReader dr=cmd.ExecuteReader())
                        {
                            if(dr!=null)
                            {
                                objCity = new List<CityTempList>();
                                while (dr.Read())
                                    objCity.Add(new CityTempList()
                                    {
                                        CityId = Convert.ToInt32(dr["Value"]),
                                        CityName = dr["Text"].ToString(),
                                        MaskingName = dr["MaskingName"].ToString()
                                    });
                            }
                        }
                        Logs.WriteInfoLog("City List fetched successfully from database");
                        Logs.WriteInfoLog("City List Count : " + objCity.Count);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in CityList : " + ex.Message);
                Logs.WriteErrorLog("Error in fetching CityList from Database : " + ex.Message);
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
                    string[] tokens = cityName.Split(' ');

                    string newcity = string.Empty;
                    if (ht.ContainsKey(cityName))
                    {
                        newcity = ht[cityName].ToString();
                        string[] newtokens = newcity.Split(' ');
                        for (int index = 0; index < newtokens.Length; index++)
                        {
                            if (!String.IsNullOrEmpty(newtokens[index].Trim()))
                                ObjTemp.mm_suggest.input.Add(newtokens[index].Trim());
                        }

                        for (int index = 0; index < newtokens.Length; index++)
                        {
                            for (int jindex = index + 1; jindex < newtokens.Length; jindex++)
                            {
                                ObjTemp.mm_suggest.input.Add(newtokens[index].Trim() + " " + newtokens[jindex].Trim());
                            }
                        }

                        for (int index = 0; index < newtokens.Length - 1; index++)
                        {
                            for (int jindex = index + 2; jindex < newtokens.Length; jindex++)
                            {
                                ObjTemp.mm_suggest.input.Add(newtokens[index].Trim() + " " + newtokens[index + 1].Trim() + " " + newtokens[jindex].Trim());
                            }
                        } 
                    }

                    for (int index = 0; index < tokens.Length; index++)
                    {
                        if (!String.IsNullOrEmpty(tokens[index].Trim()))
                            ObjTemp.mm_suggest.input.Add(tokens[index].Trim());
                    }

                    for (int index = 0; index < tokens.Length; index++)
                    {
                        for (int jindex = index + 1; jindex < tokens.Length; jindex++)
                        {
                            ObjTemp.mm_suggest.input.Add(tokens[index].Trim() + " " + tokens[jindex].Trim());
                        }
                    }

                    for (int index = 0; index < tokens.Length - 1; index++)
                    {
                        for (int jindex = index + 2; jindex < tokens.Length; jindex++)
                        {
                            ObjTemp.mm_suggest.input.Add(tokens[index].Trim() + " " + tokens[index + 1].Trim() + " " + tokens[jindex].Trim());
                        }
                    }

                    objSuggestList.Add(ObjTemp);
                    count--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Suggest List Exception  : " + ex.Message);
                Logs.WriteErrorLog("Error In creating City autosuggest list : " + ex.Message);
            }
            return objSuggestList;
        }
    }
}
