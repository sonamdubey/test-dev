using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
namespace BikewaleAutoSuggest
{
    public class GetBikeListDb
    {
        private static string _con = ConfigurationManager.AppSettings["connectionString"];
        /// <summary>
        /// Modified by : Rajan Chauhan on 10 Jan 2017
        /// Description : Added PhotosCount
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TempList> GetBikeList()
        {
            IList<TempList> objList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getautosuggestmakemodellist_12032018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<TempList>();

                            while (dr.Read())
                            {
                                objList.Add(new TempList()
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                    ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]),
                                    Make = string.Format("{0} Bikes", dr["Make"]),
                                    Model = Convert.ToString(dr["Model"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    New = Convert.ToBoolean(dr["New"]),
                                    Futuristic = Convert.ToBoolean(dr["Futuristic"]),
                                    PhotosCount = Convert.ToUInt32(dr["PhotosCount"])
                                });
                            }

                            dr.NextResult();

                            while (dr.Read())
                            {
                                objList.Add(new TempList()
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                    ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]),
                                    Make = Convert.ToString(dr["Make"]),
                                    Model = Convert.ToString(dr["Model"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    New = Convert.ToBoolean(dr["New"]),
                                    Futuristic = Convert.ToBoolean(dr["Futuristic"]),
                                    UserRatingsCount = SqlReaderConvertor.ToInt32(dr["RatingsCount"]),
                                    ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewsCount"]),
                                    PhotosCount = SqlReaderConvertor.ToUInt32(dr["PhotosCount"])
                                });
                            }
                            dr.Close();
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

        public static IEnumerable<BikeList> GetSuggestList(IEnumerable<TempList> objBikeList)
        {
            IList<BikeList> objSuggestList = null;
            int count = objBikeList.Count();
            try
            {
                objSuggestList = new List<BikeList>();
                string bikeName;
                foreach (TempList bikeItem in objBikeList)
                {
                    if (bikeItem.Model == "")
                        bikeName = bikeItem.Make;
                    else
                        bikeName = string.Format("{0} {1}", bikeItem.Make, bikeItem.Model);
                    BikeList ObjTemp = new BikeList();

                    ObjTemp.Id = string.Format("{0}_{1}", bikeItem.MakeId, bikeItem.ModelId);
                    ObjTemp.name = bikeName;

                    ObjTemp.mm_suggest = new BikeSuggestion();
                    ObjTemp.output = bikeName;

                    ObjTemp.payload = new PayLoad()
                    {
                        MakeId = Convert.ToString(bikeItem.MakeId),
                        ModelId = Convert.ToString(bikeItem.ModelId),
                        MakeMaskingName = Convert.ToString(bikeItem.MakeMaskingName),
                        ModelMaskingName = Convert.ToString(bikeItem.ModelMaskingName),
                        Futuristic = Convert.ToString(bikeItem.Futuristic),
                        IsNew = Convert.ToString(bikeItem.New),
                        UserRatingsCount = Convert.ToString(bikeItem.UserRatingsCount),
                        ExpertReviewsCount = Convert.ToString(bikeItem.ExpertReviewsCount),
                        PhotosCount = Convert.ToString(bikeItem.PhotosCount)
                    };

                    ObjTemp.mm_suggest.Weight = count;

                    ObjTemp.mm_suggest.input = new List<string>();
                    string[] tokens = bikeName.Split(' ');

                    int length = Math.Min(tokens.Length, 5);
                    // For creating input in mm_suggest
                    //example :- bajaj pulsar rs200 than combination will be bajaj,pulsar,rs200,bajaj pulsar and so on
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
                            ObjTemp.mm_suggest.input.Add(value.Trim());

                    }

                    //For Royal Enfield Bikes add Bullet in Suggestion
                    if (bikeName.Contains("Royal Enfield"))
                        ObjTemp.mm_suggest.input.Add("Bullet");

                    ObjTemp.mm_suggest.contexts = new Context();
                    ObjTemp.mm_suggest.contexts.types = new List<string>();

                    ObjTemp.mm_suggest.contexts.types.Add("AllMakeModel");

                    if (bikeItem.ModelId > 0 && bikeItem.New && !bikeItem.Futuristic)
                        ObjTemp.mm_suggest.contexts.types.Add("PriceQuoteMakeModel");

                    if (bikeItem.UserRatingsCount > 0)
                        ObjTemp.mm_suggest.contexts.types.Add("UserReviews");

                    if (!bikeItem.Futuristic && bikeItem.ModelId > 0)
                        ObjTemp.mm_suggest.contexts.types.Add("NonUpcomingBikes");
                    if (bikeItem.New && !bikeItem.Futuristic && bikeItem.PhotosCount > 0)
                        ObjTemp.mm_suggest.contexts.types.Add("BikeImages");

                    objSuggestList.Add(ObjTemp);
                    count--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Suggest List Exception  : " + ex.Message);
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name + " :Exception in Generating Suggestion List for make model :", ex);
            }
            return objSuggestList;
        }
    }   //class
}   //namespace
