using Bikewale.ElasticSearch.Entities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Consumer;


namespace BikeIndex
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: DAL Layer
    /// </summary>
    public class BikeModelRepository
    {
        
        private string[] _pricestypes = { "RTO", "Insurance", "Exshowroom" };

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 20th Feb 2018
        /// Description: Assigns status according to the new and futuristic flags associated with it
        /// Modified by: Dhruv Joshi
        /// Dated: 21st Feb 2018
        /// Description: Storing data in the specs individually instead of an entity
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="isFuturistic"></param>
        /// <returns></returns>
        private BikeStatus GetStatus(bool isNew, bool isFuturistic)
        {
            if(!isNew)
            {
                if(!isFuturistic)
                {
                    return BikeStatus.Discontinued;
                }
                else 
                {
                    return BikeStatus.Upcoming;
                }
            }
            else
            {
                if(!isFuturistic)
                {
                    return BikeStatus.New;
                }
                else
                {
                    return BikeStatus.Invalid;
                }
            }
            
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 20th Feb 2018
        /// Description: Fetching data for bikeindex from db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeModelDocument> GetBikeModelList()
        {
        
            IList<BikeModelDocument> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdataforbikeindex"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<BikeModelDocument>();
                            uint weight_count = 1;
                            while (dr.Read())
                            {
                                IList<PriceEntity> objPrices = new List<PriceEntity>();

                                //price components
                                for (int i = 0; i < _pricestypes.Length; i++)
                                {
                                    objPrices.Add(new PriceEntity()
                                    {
                                        PriceType = _pricestypes[i],
                                        PriceValue = Convert.ToUInt32(dr[_pricestypes[i]])
                                    });
                                }

                                objList.Add(new BikeModelDocument()
                                    {
                                        //doc id and Weight
                                        Id = Convert.ToString(dr["ModelId"]),
                                        Weight = weight_count++,

                                        //make details
                                        BikeMake = new MakeEntity()
                                        {
                                            MakeId = Convert.ToUInt32(dr["MakeId"]),
                                            MakeName = Convert.ToString(dr["MakeName"]),
                                            MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                            MakeStatus = GetStatus(Convert.ToBoolean(dr["IsNewMake"]), Convert.ToBoolean(dr["IsFuturisticMake"]))
                                        },

                                        //model details
                                        BikeModel = new ModelEntity()
                                        {
                                            ModelId = Convert.ToUInt32(dr["ModelId"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                            ModelStatus = GetStatus(Convert.ToBoolean(dr["IsNewModel"]), Convert.ToBoolean(dr["IsFuturisticModel"]))
                                        },

                                        //top version
                                        TopVersion = new VersionEntity()
                                        {
                                            VersionId = Convert.ToUInt32(dr["TopVersionId"]),
                                            VersionName = Convert.ToString(dr["VersionName"]),
                                            Mileage = Convert.ToUInt32(dr["Mileage"]),
                                            KerbWeight = Convert.ToUInt32(dr["KerbWeight"]),
                                            Displacement = Convert.ToDouble(dr["Displacement"]),
                                            Power = Convert.ToDouble(dr["Power"]),
                                            PriceList = objPrices,
                                            Exshowroom = Convert.ToUInt32(dr["Exshowroom"]),
                                            Onroad = Convert.ToUInt32(dr["RTO"]) + Convert.ToUInt32(dr["Insurance"]) + Convert.ToUInt32(dr["Exshowroom"]),
                                            VersionStatus = GetStatus(Convert.ToBoolean(dr["IsNewVersion"]), Convert.ToBoolean(dr["IsFuturisticVersion"]))
                                        },

                                        BikeName = Convert.ToString(dr["MakeName"]) + " " + Convert.ToString(dr["ModelName"]),
                                        BodyStyleId = Convert.ToUInt32(dr["BodyStyleId"]),

                                        //bike media/reviews
                                        BikeImage = new ImageEntity()
                                        {
                                            ImageURL = Convert.ToString(dr["ImageURL"]),
                                            HostURL = Convert.ToString(dr["HostURL"])
                                        },
                                        ImageCount = Convert.ToUInt32(dr["ImageCount"]),
                                        VideosCount = Convert.ToUInt32(dr["VideosCount"]),
                                        ExpertReviewsCount = Convert.ToUInt32(dr["ExpertReviewsCount"]),
                                        UserReviewsCount = Convert.ToUInt32(dr["UserReviewsCount"]),
                                        ReviewRatings = Convert.ToDouble(dr["ReviewRatings"]),
                                        RatingsCount = Convert.ToUInt32(dr["RatingsCount"]),
                                       
                                    });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception at Bikewale.ElasticSearch.BikeIndex.BikeModelRepository ->GetBikeList() ", ex);
                Console.WriteLine("Exception Message  : " + ex.Message);
            }
            return objList;
        }

    }
}
