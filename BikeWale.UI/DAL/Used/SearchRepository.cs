using Bikewale.Entities.Used;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 11 sept 2016
    /// Summary : Class have functions related to used bikes search
    /// </summary>
    public class SearchRepository : ISearchRepository
    {
        /// <summary>
        /// Function to get the used bikes search results from the database
        /// Modified by :   Sumit Kate on 25 Oct 2016
        /// Description :   read LastUpdated from data reader
        /// Modified by :   Sangram Nandkhile on 09 Nov 2016
        /// Description :   Added lower() for profile id
        /// </summary>
        /// <param name="searchQuery">Pass acutal sql query which needs to be executed in db</param>
        /// <returns></returns>
        public SearchResult GetUsedBikesList(string searchQuery)
        {
            SearchResult objResult = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(searchQuery))
                {
                    cmd.CommandType = CommandType.Text;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objResult = new SearchResult();

                            IList<UsedBikeBase> objBikesList = new List<UsedBikeBase>();

                            // populate bikes listings into the entity
                            while (dr.Read())
                            {
                                UsedBikeBase objBike = new UsedBikeBase();

                                objBike.InquiryId = Convert.ToUInt32(dr["inquiryid"]);
                                objBike.ProfileId = Convert.ToString(dr["profileid"]).ToLower();
                                objBike.AskingPrice = Convert.ToUInt32(dr["price"]);
                                objBike.KmsDriven = Convert.ToUInt32(dr["kilometers"]);
                                objBike.ModelMonth = Convert.ToString(dr["bikemonth"]);
                                objBike.ModelYear = Convert.ToString(dr["bikeyear"]);
                                objBike.NoOfOwners = Convert.ToUInt16(dr["owner"]);
                                objBike.SellerType = Convert.ToUInt16(dr["sellertype"]);
                                objBike.TotalPhotos = Convert.ToUInt16(dr["photocount"]);

                                objBike.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                                objBike.CityName = Convert.ToString(dr["city"]);

                                objBike.MakeMaskingName = Convert.ToString(dr["makemaskingname"]);
                                objBike.MakeName = Convert.ToString(dr["makename"]);
                                objBike.ModelMaskingName = Convert.ToString(dr["modelmaskingname"]);
                                objBike.ModelName = Convert.ToString(dr["modelname"]);
                                objBike.VersionName = Convert.ToString(dr["versionname"]);

                                objBike.Photo = new BikePhoto();
                                objBike.Photo.HostUrl = Convert.ToString(dr["hosturl"]);
                                objBike.Photo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                objBike.LastUpdated = Utility.SqlReaderConvertor.ToDateTime(dr["lastupdated"]);
                                objBikesList.Add(objBike);
                            }

                            // Add fetched listings to the result set
                            objResult.Result = objBikesList;

                            // process the next result for the total number of listings
                            dr.NextResult();

                            if (dr.Read())
                            {
                                objResult.TotalCount = Convert.ToInt32(dr["RecordCount"]);
                            }

                            if (dr != null)
                                dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.DAL.Used.Search.GetUsedBikesList({0})", searchQuery));
                
            }


            return objResult;
        }
    }
}
