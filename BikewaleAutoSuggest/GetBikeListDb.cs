using Bikewale.CoreDAL;
using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

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
                using (DbCommand cmd = DbFactory.GetDBCommand("getautosuggestmakemodellist"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            objList = new List<TempList>();

                            while (dr.Read())
                            {
                                objList.Add(new TempList()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),                                     //  Add MakeId
                                    ModelId = Convert.ToInt32(dr["ModelId"]),                                   //  Add ModelId
                                    Make = dr["Make"].ToString() + " Bikes",                                    //  Add MakeName
                                    Model = dr["Model"].ToString(),                                             //  Add ModelName
                                    MakeMaskingName = dr["MakeMaskingName"].ToString(),                         //  Add MakeMaskingName
                                    ModelMaskingName = dr["ModelMaskingName"].ToString(),                       //  Add ModelMaskingName
                                    New = Convert.ToBoolean(dr["New"]),                                         //  Add New Flag
                                    Futuristic = Convert.ToBoolean(dr["Futuristic"])                            //  Add Futuristic
                                });
                            }

                            dr.NextResult();

                            while (dr.Read())
                            {
                                objList.Add(new TempList()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),                                     //  Add MakeId
                                    ModelId = Convert.ToInt32(dr["ModelId"]),                                   //  Add ModelId
                                    Make = dr["Make"].ToString(),                                               //  Add MakeName
                                    Model = dr["Model"].ToString(),                                             //  Add ModelName
                                    MakeMaskingName = dr["MakeMaskingName"].ToString(),                         //  Add MakeMaskingName
                                    ModelMaskingName = dr["ModelMaskingName"].ToString(),                       //  Add ModelMaskingName
                                    New = Convert.ToBoolean(dr["New"]),                                         //  Add New Flag
                                    Futuristic = Convert.ToBoolean(dr["Futuristic"])                            //  Add Futuristic
                                });
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine("Exception Message  : " + ex.Message);
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

                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[0].Trim(), tokens[1].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[0].Trim(), tokens[2].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[0].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[0].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[1].Trim(), tokens[2].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[1].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[1].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[2].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[2].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1}", tokens[3].Trim(), tokens[4].Trim()));

                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[0].Trim(), tokens[1].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[0].Trim(), tokens[1].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[0].Trim(), tokens[2].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[0].Trim(), tokens[2].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[0].Trim(), tokens[3].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[1].Trim(), tokens[2].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[1].Trim(), tokens[2].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[1].Trim(), tokens[3].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2}", tokens[2].Trim(), tokens[3].Trim(), tokens[4].Trim()));

                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2} {3}", tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim(), tokens[3].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2} {3}", tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2} {3}", tokens[0].Trim(), tokens[1].Trim(), tokens[3].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2} {3}", tokens[0].Trim(), tokens[2].Trim(), tokens[3].Trim(), tokens[4].Trim()));
                        ObjTemp.mm_suggest.input.Add(string.Format("{0} {1} {2} {3}", tokens[1].Trim(), tokens[2].Trim(), tokens[3].Trim(), tokens[4].Trim()));

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
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name + " :Exception in Generating Suggestion List for make model :", ex);
            }
            return objSuggestList;
        }
    }   //class
}   //namespace
