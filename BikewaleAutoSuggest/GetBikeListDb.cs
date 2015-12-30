using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleAutoSuggest
{
    public class GetBikeListDb
    {
        private static string _con = ConfigurationManager.AppSettings["connectionString"];

        public static List<TempList> GetBikeList()
        {
            List<TempList> objList = null;

            try
            {
                using(SqlConnection conn=new SqlConnection(_con))
                {
                    using(SqlCommand cmd=new SqlCommand())
                    {
                        cmd.CommandText = "GetAutoSuggestMakeModelList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        conn.Open();

                        SqlDataReader dr = cmd.ExecuteReader();

                        if(dr!=null)
                        {
                            objList = new List<TempList>();

                            Logs.WriteInfoLog("Make Model Result Fetched From db");
                            while(dr.Read())
                            {
                                objList.Add(new TempList()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),
                                    ModelId = Convert.ToInt32(dr["ModelId"]),
                                    Make = dr["Make"].ToString() + " Bikes",
                                    Model = dr["Model"].ToString(),
                                    MakeMaskingName = dr["MakeMaskingName"].ToString(),
                                    ModelMaskingName = dr["ModelMaskingName"].ToString(),
                                    New = Convert.ToBoolean(dr["New"]),
                                    Futuristic = Convert.ToBoolean(dr["Futuristic"])
                                });
                            }

                            dr.NextResult();

                            while (dr.Read())
                            {
                                objList.Add(new TempList()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),
                                    ModelId = Convert.ToInt32(dr["ModelId"]),
                                    Make = dr["Make"].ToString(),
                                    Model = dr["Model"].ToString(),
                                    MakeMaskingName = dr["MakeMaskingName"].ToString(),
                                    ModelMaskingName = dr["ModelMaskingName"].ToString(),
                                    New = Convert.ToBoolean(dr["New"]),
                                    Futuristic = Convert.ToBoolean(dr["Futuristic"])
                                });
                            }
                        }
                        Logs.WriteInfoLog("Result added to list successfully.");
                        Logs.WriteInfoLog("List Length : " + objList.Count);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception Message  : " + ex.Message);
                Logs.WriteErrorLog("Exception in GetBikeList : " + ex.Message);
            }
            return objList;
        }

        public static List<BikeList> GetSuggestList(List<TempList> objBikeList)
        {
            List<BikeList> objSuggestList = null;
            int count = objBikeList.Count;
            try
            {
                objSuggestList = new List<BikeList>();
                foreach(TempList bikeItem in objBikeList)
                {
                    string bikeName = (bikeItem.Make + " " + bikeItem.Model).Trim();
                    BikeList ObjTemp = new BikeList();

                    ObjTemp.Id = bikeItem.MakeId.ToString() + "_" + bikeItem.ModelId.ToString();
                    ObjTemp.name = bikeItem.Make + " " + bikeItem.Model;

                    ObjTemp.mm_suggest = new BikeSuggestion();
                    ObjTemp.mm_suggest.output = bikeItem.Make + " " + bikeItem.Model;

                    ObjTemp.mm_suggest.payload = new PayLoad()
                    {
                        MakeId = bikeItem.MakeId.ToString(),
                        ModelId = bikeItem.ModelId.ToString(),
                        MakeMaskingName = bikeItem.MakeMaskingName,
                        ModelMaskingName = bikeItem.ModelMaskingName,
                        Futuristic=bikeItem.Futuristic.ToString()
                    };

                    ObjTemp.mm_suggest.Weight = count;

                    ObjTemp.mm_suggest.input = new List<string>();
                    bikeName = bikeName.Replace('-', ' ').Replace("'","");
                    string[] tokens = bikeName.Split(' ');

                    if (tokens.Length == 1)    //If Display Name has length=1
                    {
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim());
                    }
                    else if (tokens.Length == 2)    //If Display Name has length=2
                    {
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim());
						
                    }
                    else if (tokens.Length == 3)    //If Display Name has length=3
                    {
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim());

                        ////For Royal Enfield add Bullet in Suggestion
                        //if(tokens[0].Equals("Royal",StringComparison.CurrentCultureIgnoreCase) && tokens[1].Equals("Enfield",StringComparison.CurrentCultureIgnoreCase))
                        //    ObjTemp.mm_suggest.input.Add("Bullet");


                        //For Royal Enfield Bikes add Bullet in Suggestion
                        if (bikeName.Contains("Royal Enfield"))
                        {
                            ObjTemp.mm_suggest.input.Add("Bullet");
                            ObjTemp.mm_suggest.input.Add("Bullet Bikes");
                        }
                    }
                    else if (tokens.Length == 4)    //If Display Name has length=4
                    {
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[3].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim() + " " + tokens[3].Trim());


                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim());

                        ////For Royal Enfield add Bullet in Suggestion
                        //if(tokens[0].Equals("Royal",StringComparison.CurrentCultureIgnoreCase) && tokens[1].Equals("Enfield",StringComparison.CurrentCultureIgnoreCase))
                        //    ObjTemp.mm_suggest.input.Add("Bullet");

                        //For Royal Enfield Bikes add Bullet in Suggestion
                        if (bikeName.Contains("Royal Enfield"))
                            ObjTemp.mm_suggest.input.Add("Bullet");

                    }
                    else if (tokens.Length == 5)    //If Display Name has length=5
                    {
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[4].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[3].Trim() + " " + tokens[4].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[2].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());
                        ObjTemp.mm_suggest.input.Add(tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());

                        ObjTemp.mm_suggest.input.Add(tokens[0].Trim() + " " + tokens[1].Trim() + " " + tokens[2].Trim() + " " + tokens[3].Trim() + " " + tokens[4].Trim());

                        ////For Royal Enfield add Bullet in Suggestion
                        //if(tokens[0].Equals("Royal",StringComparison.CurrentCultureIgnoreCase) && tokens[1].Equals("Enfield",StringComparison.CurrentCultureIgnoreCase))
                        //ObjTemp.mm_suggest.input.Add("Bullet");

                        //For Royal Enfield Bikes add Bullet in Suggestion
                        if (bikeName.Contains("Royal Enfield"))
                            ObjTemp.mm_suggest.input.Add("Bullet");
                    }
                    else   //If display name has length > 5
                    {
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

                        ////For Royal Enfield add Bullet in Suggestion
                        //if (tokens[0].Equals("Royal", StringComparison.CurrentCultureIgnoreCase) && tokens[1].Equals("Enfield", StringComparison.CurrentCultureIgnoreCase))
                        //    ObjTemp.mm_suggest.input.Add("Bullet");

                        //For Royal Enfield Bikes add Bullet in Suggestion
                        if (bikeName.Contains("Royal Enfield"))
                            ObjTemp.mm_suggest.input.Add("Bullet");
                    }
                                //for (int index = 0; index < tokens.Length; index++)
                                //{
                                //    if (!String.IsNullOrEmpty(tokens[index].Trim()))
                                //        ObjTemp.mm_suggest.input.Add(tokens[index].Trim());
                                //}

                                //for (int index = 0; index < tokens.Length; index++)
                                //{
                                //    for (int jindex = index + 1; jindex < tokens.Length; jindex++)
                                //    {
                                //        ObjTemp.mm_suggest.input.Add(tokens[index].Trim() + " " + tokens[jindex].Trim());
                                //    }
                                //}

                                //for (int index = 0; index < tokens.Length - 1; index++)
                                //{
                                //    for (int jindex = index + 2; jindex < tokens.Length; jindex++)
                                //    {
                                //        ObjTemp.mm_suggest.input.Add(tokens[index].Trim() + " " + tokens[index + 1].Trim() + " " + tokens[jindex].Trim());
                                //    }
                                //}
                                //ObjTemp.mm_suggest.input.Add(bikeName);

                    objSuggestList.Add(ObjTemp);
                    count--;
                }
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Get Suggest List Exception  : " + ex.Message);
                Logs.WriteErrorLog("Exception in Generating Suggestion List for make model : " + ex.Message);
            }
            return objSuggestList;
        }
    }   //class
}   //namespace
