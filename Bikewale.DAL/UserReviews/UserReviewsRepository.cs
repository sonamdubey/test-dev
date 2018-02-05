using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class have functions to interact with database to get the user reviews data.
    /// </summary>
    public class UserReviewsRepository : IUserReviewsRepository
    {

        /// <summary>
        /// Writteb By : Ashwini Todkar 
        /// Summary    : method to get list of top most reviewed bikes.
        /// </summary>
        /// <param name="totalRecords">count of top records </param>
        /// <returns></returns>
        public List<ReviewTaggedBikeEntity> GetMostReviewedBikesList(ushort totalRecords)
        {
            List<ReviewTaggedBikeEntity> objBikeList = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getmostreviewedbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getmostreviewedbikes";
                    //cmd.Parameters.Add("@topcount", SqlDbType.Int).Value = totalRecords;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, totalRecords));

                    BikeMakeEntityBase objMakeBase = null;
                    BikeModelEntityBase objModelBase = null;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objBikeList = new List<ReviewTaggedBikeEntity>();
                            while (dr.Read())
                            {
                                objMakeBase = new BikeMakeEntityBase();
                                objModelBase = new BikeModelEntityBase();

                                objMakeBase.MakeName = Convert.ToString(dr["BikeMake"]);
                                objMakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                                objModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                objBikeList.Add(new ReviewTaggedBikeEntity()
                                {
                                    ReviewsCount = Convert.ToUInt32(dr["TotalReviews"]),
                                    MakeEntity = objMakeBase,
                                    ModelEntity = objModelBase

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return objBikeList;
        }

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary    : Method to get list of all reviewed bikes.
        /// </summary>
        /// <returns></returns>
        public List<ReviewTaggedBikeEntity> GetReviewedBikesList()
        {
            List<ReviewTaggedBikeEntity> objBikeList = null;

            try
            {


                using (DbCommand cmd = DbFactory.GetDBCommand("getreviewedbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    BikeMakeEntityBase objMakeBase = null;
                    BikeModelEntityBase objModelBase = null;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objBikeList = new List<ReviewTaggedBikeEntity>();
                        objMakeBase = new BikeMakeEntityBase();
                        objModelBase = new BikeModelEntityBase();

                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objMakeBase.MakeName = dr["BikeMake"].ToString();
                                objMakeBase.MaskingName = dr["MakeMaskingName"].ToString();
                                objModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModelBase.ModelName = dr["ModelName"].ToString();
                                objModelBase.MaskingName = dr["ModelMaskingName"].ToString();

                                objBikeList.Add(new ReviewTaggedBikeEntity()
                                {
                                    MakeEntity = objMakeBase,
                                    ModelEntity = objModelBase,
                                    ReviewsCount = Convert.ToUInt32(dr["TotalReviews"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return objBikeList;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// Summary    : Method to get list of top most red bikes reviews
        /// </summary>
        /// <param name="totalRecords">top record count</param>
        /// <returns></returns>
        public List<ReviewsListEntity> GetMostReadReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviewList = null;


            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getmostreadreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, totalRecords));

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objReviewList = new List<ReviewsListEntity>();
                            while (dr.Read())
                            {
                                objReviewEntity = new ReviewEntityBase();
                                objReviewRating = new ReviewRatingEntityBase();
                                objTaggedBike = new ReviewTaggedBikeEntity();

                                objReviewEntity.ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]);
                                objReviewEntity.ReviewTitle = dr["Title"].ToString();
                                objReviewEntity.WrittenBy = dr["CustomerName"].ToString();
                                objReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);

                                objReviewRating.OverAllRating = float.Parse(dr["OverallR"].ToString());

                                objTaggedBike.MakeEntity.MakeName = dr["MakeName"].ToString();
                                objTaggedBike.MakeEntity.MaskingName = dr["MakeMaskingName"].ToString();

                                objTaggedBike.ModelEntity.ModelName = dr["ModelName"].ToString();
                                objTaggedBike.ModelEntity.MaskingName = dr["ModelMaskingName"].ToString();

                                objReviewList.Add(new ReviewsListEntity()
                                {
                                    ReviewEntity = objReviewEntity,
                                    ReviewRating = objReviewRating,
                                    TaggedBike = objTaggedBike
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objReviewList;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// Summary    : Method to get list of top helpfule bike reviews.
        /// </summary>
        /// <param name="totalRecords">top record count</param>
        /// <returns></returns>
        public List<ReviewsListEntity> GetMostHelpfulReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviewList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmosthelpfulreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getmosthelpfulreviews";
                    //cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, totalRecords));

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objReviewList = new List<ReviewsListEntity>();
                            while (dr.Read())
                            {
                                objReviewEntity = new ReviewEntityBase();
                                objReviewRating = new ReviewRatingEntityBase();
                                objTaggedBike = new ReviewTaggedBikeEntity();

                                objReviewEntity.ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]);
                                objReviewEntity.ReviewTitle = dr["Title"].ToString();
                                objReviewEntity.WrittenBy = dr["CustomerName"].ToString();
                                objReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);

                                objReviewRating.OverAllRating = float.Parse(dr["OverallR"].ToString());

                                objTaggedBike.MakeEntity.MakeName = dr["MakeName"].ToString();
                                objTaggedBike.MakeEntity.MaskingName = dr["MakeMaskingName"].ToString();

                                objTaggedBike.ModelEntity.ModelName = dr["ModelName"].ToString();
                                objTaggedBike.ModelEntity.MaskingName = dr["ModelMaskingName"].ToString();

                                objReviewList.Add(new ReviewsListEntity()
                                {
                                    ReviewEntity = objReviewEntity,
                                    ReviewRating = objReviewRating,
                                    TaggedBike = objTaggedBike
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objReviewList;
        }

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary    : Method to get list of top recent bikes reviews.
        /// </summary>
        /// <param name="totalRecords">top record count</param>
        /// <returns></returns>
        public List<ReviewsListEntity> GetMostRecentReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviewList = null;


            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getmostrecentreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getmostrecentreviews";
                    //cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, totalRecords));

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objReviewList = new List<ReviewsListEntity>();
                            while (dr.Read())
                            {
                                objReviewEntity = new ReviewEntityBase();
                                objReviewRating = new ReviewRatingEntityBase();
                                objTaggedBike = new ReviewTaggedBikeEntity();

                                objReviewEntity.ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]);
                                objReviewEntity.ReviewTitle = dr["Title"].ToString();
                                objReviewEntity.WrittenBy = dr["CustomerName"].ToString();
                                objReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);

                                objReviewRating.OverAllRating = float.Parse(dr["OverallR"].ToString());

                                objTaggedBike.MakeEntity.MakeName = dr["MakeName"].ToString();
                                objTaggedBike.MakeEntity.MaskingName = dr["MakeMaskingName"].ToString();

                                objTaggedBike.ModelEntity.ModelName = dr["ModelName"].ToString();
                                objTaggedBike.ModelEntity.MaskingName = dr["ModelMaskingName"].ToString();

                                objReviewList.Add(new ReviewsListEntity()
                                {
                                    ReviewEntity = objReviewEntity,
                                    ReviewRating = objReviewRating,
                                    TaggedBike = objTaggedBike
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return objReviewList;
        }

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary    : Method to get list of top most rated bikes reviews.
        /// </summary>
        /// <param name="totalRecords">top record count</param>
        /// <returns></returns>
        public List<ReviewsListEntity> GetMostRatedReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviewList = null;


            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getmostratedreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getmostratedreviews";
                    //cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, totalRecords));

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objReviewList = new List<ReviewsListEntity>();

                            while (dr.Read())
                            {
                                objReviewEntity = new ReviewEntityBase();
                                objReviewRating = new ReviewRatingEntityBase();
                                objTaggedBike = new ReviewTaggedBikeEntity();

                                objReviewEntity.ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]);
                                objReviewEntity.ReviewTitle = dr["Title"].ToString();
                                objReviewEntity.WrittenBy = dr["CustomerName"].ToString();
                                objReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);

                                objReviewRating.OverAllRating = float.Parse(dr["OverallR"].ToString());

                                objTaggedBike.MakeEntity.MakeName = dr["MakeName"].ToString();
                                objTaggedBike.MakeEntity.MaskingName = dr["MakeMaskingName"].ToString();

                                objTaggedBike.ModelEntity.ModelName = dr["ModelName"].ToString();
                                objTaggedBike.ModelEntity.MaskingName = dr["ModelMaskingName"].ToString();

                                objReviewList.Add(new ReviewsListEntity()
                                {
                                    ReviewEntity = objReviewEntity,
                                    ReviewRating = objReviewRating,
                                    TaggedBike = objTaggedBike
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return objReviewList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 11 June 2014
        /// Summary : To get User Rating by ModelId
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ReviewRatingEntity GetBikeRatings(uint modelId)
        {
            ReviewRatingEntity objRate = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelrating_24042017"))
                {
                    //cmd.CommandText = "getmodelrating";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@modelid", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objRate = new ReviewRatingEntity();

                            objRate.ComfortRating = Convert.ToSingle(dr["Comfort"]);
                            objRate.FuelEconomyRating = Convert.ToSingle(dr["FuelEconomy"]);
                            objRate.PerformanceRating = Convert.ToSingle(dr["Performance"]);
                            objRate.StyleRating = Convert.ToSingle(dr["Looks"]);
                            objRate.ValueRating = Convert.ToSingle(dr["ValueForMoney"]);
                            objRate.OverAllRating = Convert.ToSingle(dr["ReviewRate"]);
                            objRate.IsReviewAvailable = Convert.ToBoolean(dr["isReviewAvailable"]);

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return objRate;
        }

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary    : Method to get  total reviews  and list of reviews of a model.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="filter">MostRescent,MostRated,MostRead,MostHelpful</param>
        /// <param name="totalReviews">total reviews available for a model</param>
        /// Modified By:Rakesh Yadav On to fetch MakeMaskingName and ModelMaskingName on 08 Sep 2015
        /// <returns></returns>
        public ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter)
        {
            ReviewListBase reviews = null;
            List<ReviewEntity> objRatingList = null;

            uint totalReviews = 0;

            try
            {
                reviews = new ReviewListBase();
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikereviewslist_12052017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, endIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, (versionId > 0) ? versionId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_filter", DbType.Int32, filter));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objRatingList = new List<ReviewEntity>();
                            while (dr.Read())
                            {
                                ReviewEntity objRating = new ReviewEntity();

                                objRating.NewReviewId = SqlReaderConvertor.ToUInt32(dr["NewReviewId"]);
                                objRating.ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]);
                                objRating.WrittenBy = dr["CustomerName"].ToString();
                                objRating.OverAllRating.OverAllRating = Convert.ToSingle(dr["OverallR"]);
                                objRating.Pros = dr["Pros"].ToString();
                                objRating.Cons = dr["Cons"].ToString();
                                objRating.Comments = dr["SubComments"].ToString();
                                objRating.ReviewTitle = dr["Title"].ToString();
                                objRating.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);
                                objRating.Liked = Convert.ToUInt16(dr["Liked"]);
                                objRating.Disliked = Convert.ToUInt16(dr["Disliked"]);
                                objRating.MakeMaskingName = dr["MakeMaskingName"].ToString();
                                objRating.ModelMaskingName = dr["ModelMaskingName"].ToString();
                                objRatingList.Add(objRating);

                                totalReviews = Convert.ToUInt32(dr["RecordCount"]);
                            }

                            dr.Close();
                            reviews.ReviewList = objRatingList;
                            reviews.TotalReviews = totalReviews;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return reviews;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// Summary    : Method to get review details like Rating,Aouther,Written Date also related next and prev review id
        /// Modified By : Suresh Prajapati on 20 Aug 2014
        /// Summary : To retrieve new and used flag for bike model
        /// Modified By:- Subodh Jain 19 Jan 2017
        /// Summary :- modified Sp for specs
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetReviewDetails(uint reviewId)
        {
            ReviewDetailsEntity objReview = null;


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcustomerreviewinfo_16012017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int32, reviewId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objReview = new ReviewDetailsEntity();

                            objReview.BikeEntity.VersionEntity.VersionName = Convert.ToString(dr["Version"]);
                            objReview.BikeEntity.VersionEntity.VersionId = SqlReaderConvertor.ToInt32(dr["versionid"]);
                            objReview.ReviewEntity.Comments = Convert.ToString(dr["Comments"]);
                            objReview.ReviewEntity.Cons = Convert.ToString(dr["Cons"]);
                            objReview.ReviewEntity.Disliked = SqlReaderConvertor.ToUInt16(dr["Disliked"]);
                            objReview.ReviewEntity.Liked = SqlReaderConvertor.ToUInt16(dr["Liked"]);
                            objReview.ReviewEntity.Pros = Convert.ToString(dr["Pros"]);
                            objReview.ReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);
                            objReview.ReviewEntity.ReviewTitle = Convert.ToString(dr["Title"]);
                            objReview.ReviewEntity.WrittenBy = Convert.ToString(dr["CustomerName"]);
                            objReview.ReviewEntity.Viewed = Convert.ToUInt32(dr["viewed"]);
                            objReview.ModelSpecs = new MinSpecsEntity();
                            objReview.BikeEntity.MakeEntity.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                            objReview.BikeEntity.MakeEntity.MakeName = Convert.ToString(dr["Make"]);
                            objReview.BikeEntity.ModelEntity.ModelName = Convert.ToString(dr["Model"]);
                            objReview.BikeEntity.ModelEntity.ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]);
                            objReview.BikeEntity.ModelEntity.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                            objReview.BikeEntity.MakeEntity.MaskingName = Convert.ToString(dr["makemaskingname"]);
                            objReview.ReviewRatingEntity.ModelRatingLooks = SqlReaderConvertor.ToFloat(dr["Looks"]);
                            objReview.ReviewRatingEntity.PerformanceRating = SqlReaderConvertor.ToFloat(dr["Performance"]);
                            objReview.ReviewRatingEntity.ComfortRating = SqlReaderConvertor.ToFloat(dr["Comfort"]);
                            objReview.ReviewRatingEntity.ValueRating = SqlReaderConvertor.ToFloat(dr["ValueForMoney"]);
                            objReview.ReviewRatingEntity.FuelEconomyRating = SqlReaderConvertor.ToFloat(dr["FuelEconomy"]);
                            objReview.ReviewRatingEntity.OverAllRating = SqlReaderConvertor.ToFloat(dr["ReviewRate"]);
                            objReview.ModelBasePrice = Convert.ToString(dr["MinPrice"]);
                            objReview.ModelHighendPrice = Convert.ToString(dr["MaxPrice"]);
                            objReview.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                            objReview.IsFuturistic = Convert.ToBoolean(dr["Futuristic"]);
                            objReview.New = Convert.ToBoolean(dr["new"]);
                            objReview.Used = Convert.ToBoolean(dr["used"]);
                            objReview.HostUrl = Convert.ToString(dr["HostURL"]);
                            objReview.ModelSpecs.FuelEfficiencyOverall = SqlReaderConvertor.ToUInt16(dr["fuelefficiencyoverall"]);
                            objReview.ModelSpecs.KerbWeight = SqlReaderConvertor.ToUInt16(dr["kerbweight"]);
                            objReview.ModelSpecs.MaxPower = SqlReaderConvertor.ToFloat(dr["maxpower"]);
                            objReview.ModelSpecs.Displacement = SqlReaderConvertor.ToFloat(dr["displacement"]);

                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format(" UserReviewsRepository.GetReviewDetails() --> ReviewId: {0}", reviewId));

            }

            return objReview;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// Summary    : Method to report review abuse
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="comment"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AbuseReview(uint reviewId, string comment, string userId)
        {

            bool success = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomerreviewsabuse"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int32, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reportedby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 500, comment));


                    success = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return success;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// Summary    : Method to update viewed count of a user review page
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public bool UpdateViews(uint reviewId)
        {
            bool success = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatereviewviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int32, reviewId));

                    success = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return success;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// function updates the liked and the disliked field of the customer reviews table
        /// according to the review id passed and the helpful value which is either true or false
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isHelpful"></param>
        /// <returns></returns>
        public bool UpdateReviewUseful(uint reviewId, bool isHelpful)
        {
            bool success = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomerreviewshelpful"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int32, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_helpful", DbType.Boolean, isHelpful));

                    success = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return success;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : get all user reiews data for user reviews section
        /// Modified by Sajal Gupta on 3-11-20017
        /// Desc :  Added subquestionid in question options
        /// </summary>
        /// <returns></returns>
        public UserReviewsData GetUserReviewsData()
        {
            UserReviewsData objData = null;
            IList<UserReviewOverallRating> overallRating = null;
            IList<UserReviewQuestion> questions = null;
            IList<UserReviewRating> ratings = null;
            IList<UserReviewPriceRange> priceRange = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getuserreviewsstaticdata"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objData = new UserReviewsData();
                            overallRating = new List<UserReviewOverallRating>();
                            while (dr.Read())
                            {
                                overallRating.Add(
                                    new UserReviewOverallRating()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["id"]),
                                        Value = SqlReaderConvertor.ToUInt16(dr["rating"]),
                                        Heading = Convert.ToString(dr["heading"]),
                                        Description = Convert.ToString(dr["Description"]),
                                        ResponseHeading = Convert.ToString(dr["ResponseHeading"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalIImagePath"]),
                                        HostUrl = Convert.ToString(dr["hosturl"])
                                    });
                            }
                            objData.OverallRating = overallRating;

                            if (dr.NextResult())
                            {
                                questions = new List<UserReviewQuestion>(); ;
                                while (dr.Read())
                                {
                                    questions.Add(
                                    new UserReviewQuestion()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                        Heading = Convert.ToString(dr["Heading"]),
                                        Description = Convert.ToString(dr["Description"]),
                                        DisplayType = (UserReviewQuestionDisplayType)Convert.ToInt32(dr["DisplayType"]),
                                        Type = (UserReviewQuestionType)Convert.ToInt32(dr["QuestionType"]),
                                        Order = SqlReaderConvertor.ToUInt16(dr["DisplayOrder"])
                                    });
                                }
                            }
                            objData.Questions = questions;

                            if (dr.NextResult())
                            {
                                ratings = new List<UserReviewRating>(); ;
                                while (dr.Read())
                                {
                                    ratings.Add(
                                    new UserReviewRating()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["RatingId"]),
                                        QuestionId = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                        Text = Convert.ToString(dr["RatingText"]),
                                        Value = Convert.ToString(dr["RatingValue"]),
                                        SubQuestionId = SqlReaderConvertor.ToUInt32(dr["SubQuestionId"])
                                    });
                                }
                            }
                            objData.Ratings = ratings;


                            if (dr.NextResult())
                            {
                                priceRange = new List<UserReviewPriceRange>(); ;
                                while (dr.Read())
                                {
                                    priceRange.Add(
                                    new UserReviewPriceRange()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                        RangeId = SqlReaderConvertor.ToUInt32(dr["PricerangeId"]),
                                        QuestionId = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                        MinPrice = SqlReaderConvertor.ToUInt32(dr["MinPrice"]),
                                        MaxPrice = SqlReaderConvertor.ToUInt32(dr["MaxPrice"]),
                                    });
                                }
                            }
                            objData.PriceRange = priceRange;

                            dr.Close();
                        }
                    }
                }

                if (objData != null && objData.Ratings != null && objData.Questions != null)
                {
                    //set ratings for question
                    var objQuestionratings = objData.Ratings.GroupBy(x => x.QuestionId);

                    //set pricerangeIds for question
                    var priceRangeIds = objData.PriceRange.GroupBy(x => x.QuestionId);

                    foreach (var question in objData.Questions)
                    {
                        foreach (var rating in objQuestionratings)
                        {
                            if (rating.Key == question.Id)
                            {
                                question.Rating = rating.ToList();
                                break;
                            }
                        }

                        foreach (var priceId in priceRangeIds)
                        {
                            if (priceId.Key == question.Id)
                            {
                                question.PriceRangeIds = priceId.Select(x => x.RangeId).ToList();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objData;

        }


        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Save user review ratings
        /// Modified by : Aditi Srivastava on 29 May 2017
        /// Summary     : Added sourceId parameter
        /// Modified by : Sajal Gupta on 05-07-2017
        /// Summary     : Changed SP
        /// Modified by : Sajal Gupta on 17-07-2017
        /// Summary     : Changed SP
        /// </summary>
        /// <param name="overAllrating"></param>
        /// <param name="ratingQuestionAns"></param>
        /// <param name="userName"></param>
        /// <param name="emailId"></param>
        /// <param name="customerId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public uint SaveUserReviewRatings(InputRatingSaveEntity inputSaveEntity, uint customerId, uint reviewId)
        {
            uint reviewIdNew = 0;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("saveuserratings_04082017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, inputSaveEntity.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, inputSaveEntity.MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallrating", DbType.String, inputSaveEntity.OverAllrating));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionrating", DbType.String, inputSaveEntity.RatingQuestionAns));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_username", DbType.String, inputSaveEntity.UserName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, inputSaveEntity.EmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_returnurl", DbType.String, inputSaveEntity.ReturnUrl));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientIP", DbType.String, CurrentUser.GetClientIP()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_platformid", DbType.Int16, inputSaveEntity.PlatformId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewId", DbType.Int16, reviewId > 0 ? reviewId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Int16, (inputSaveEntity.SourceId.HasValue && inputSaveEntity.SourceId.Value > 0) ? inputSaveEntity.SourceId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, inputSaveEntity.UtmzCookieValue));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            reviewIdNew = SqlReaderConvertor.ToUInt32(dr["reviewId"]);
                        }
                        dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {


                ErrorClass.LogError(ex, string.Format("UserReviewsRepository.SaveUserReviewRatings() reviewId-{0} makeId-{1} modelId-{2}", reviewId, inputSaveEntity.MakeId, inputSaveEntity.ModelId));
            }

            return reviewIdNew;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : Save user reviews by user with comments and title
        /// Modified by : Sajal Gupta on 05-07-2017
        /// Summary     : Changed SP
        /// Modified by : Sajal Gupta on 17-07-2017
        /// Summary     : Changed SP
        ///  Modified by : Sajal Gupta on 31-08-2017
        /// Summary     : Changed SP
        /// Modified by : Sajal Gupta on 06-09-2017
        /// Summary     : Changed SP to add milaege
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="tipsnAdvices"></param>
        /// <param name="comment"></param>
        /// <param name="commentTitle"></param>
        /// <param name="reviewsQuestionAns"></param>
        /// <returns></returns>
        public bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns, uint mileage)
        {
            bool IsSaved = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("saveuserreviews_06092017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.UInt32, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewTips", DbType.String, tipsnAdvices));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewDescription", DbType.String, comment));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewTitle", DbType.String, commentTitle));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionrating", DbType.String, reviewsQuestionAns));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mileage", DbType.UInt32, mileage > 0 ? mileage : Convert.DBNull));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    IsSaved = true;
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return IsSaved;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get user reviews summary for all pages
        /// Modified by : Aditi Srivastava on 8 May 2017
        /// Summary    : Get return url from database
        /// Modified by : Ashutosh Sharma on 24-Aug-2017
        /// Description :  Changed SP from 'getUserReviewSummary_12072017' to 'getUserReviewSummary_24082017'
        ///             to get SelectedRatingText and MinHeading
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public UserReviewSummary GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objUserReviewSummary = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getUserReviewSummary_06092017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewId", DbType.UInt32, reviewId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objUserReviewSummary = new UserReviewSummary()
                            {
                                CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                CustomerName = Convert.ToString(dr["CustomerName"]),
                                Description = Convert.ToString(dr["Comments"]),
                                Title = Convert.ToString(dr["ReviewTitle"]),
                                Tips = Convert.ToString(dr["ReviewTips"]),
                                OverallRatingId = SqlReaderConvertor.ToUInt16(dr["overallratingId"]),
                                ReturnUrl = Convert.ToString(dr["ReturnUrl"]),
                                PlatformId = SqlReaderConvertor.ToUInt16(dr["PlatformId"]),
                                Make = new BikeMakeEntityBase()
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                    MaskingName = Convert.ToString(dr["makemasking"]),
                                    MakeName = Convert.ToString(dr["makeName"])
                                },
                                Model = new BikeModelEntityBase()
                                {
                                    ModelId = SqlReaderConvertor.ToInt32(dr["modelId"]),
                                    MaskingName = Convert.ToString(dr["modelmasking"]),
                                    ModelName = Convert.ToString(dr["modelName"])
                                },
                                OriginalImagePath = Convert.ToString(dr["OriginalImgPath"]),
                                HostUrl = Convert.ToString(dr["hostUrl"]),
                                Mileage = Convert.ToString(dr["mileage"]),
                                ReviewAge = FormatDate.GetTimeSpan(SqlReaderConvertor.ToDateTime(dr["EntryDate"])),
                                ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]),
                                CustomerId = SqlReaderConvertor.ToUInt64(dr["CustomerId"])
                            };
                        }

                        if (objUserReviewSummary != null && dr.NextResult())
                        {
                            var objQuestions = new List<UserReviewQuestion>();
                            while (dr.Read())
                            {
                                objQuestions.Add(new UserReviewQuestion()
                                {
                                    SelectedRatingId = SqlReaderConvertor.ToUInt32(dr["answerValue"]),
                                    Id = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                    SelectedRatingText = Convert.ToString(dr["answerText"]),
                                    MinHeading = Convert.ToString(dr["minHeading"])
                                });
                            }
                            objUserReviewSummary.Questions = objQuestions;
                        }

                        if (dr != null)
                            dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objUserReviewSummary;
        }


        public bool IsUserVerified(uint reviewId, ulong customerId)
        {

            bool isVerified = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("checkcustomerreview"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.UInt32, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            isVerified = SqlReaderConvertor.ToBoolean(dr["status"]);
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return isVerified;


        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Apr 2017
        /// Description :   Returns user reviews from old table
        /// </summary>
        /// <returns></returns>
        public ReviewListBase GetUserReviews()
        {
            ReviewListBase reviews = null;
            ICollection<ReviewEntity> objRatingList = null;
            try
            {
                reviews = new ReviewListBase();
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikereviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objRatingList = new List<ReviewEntity>();
                            while (dr.Read())
                            {
                                ReviewEntity objRating = new ReviewEntity();

                                objRating.ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]);
                                objRating.WrittenBy = Convert.ToString(dr["CustomerName"]);
                                objRating.OverAllRating.OverAllRating = SqlReaderConvertor.ToFloat(dr["OverallR"]);
                                objRating.Pros = Convert.ToString(dr["Pros"]);
                                objRating.Cons = Convert.ToString(dr["Cons"]);
                                objRating.Comments = Convert.ToString(dr["SubComments"]);
                                objRating.ReviewTitle = Convert.ToString(dr["Title"]);
                                objRating.ReviewDate = SqlReaderConvertor.ToDateTime(dr["EntryDateTime"]);
                                objRating.Liked = SqlReaderConvertor.ToUInt16(dr["Liked"]);
                                objRating.Disliked = SqlReaderConvertor.ToUInt16(dr["Disliked"]);
                                objRating.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objRating.ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                objRating.TaggedBike = new ReviewTaggedBikeEntity();
                                objRating.TaggedBike.MakeEntity = new BikeMakeEntityBase();

                                objRating.TaggedBike.MakeEntity.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                objRating.TaggedBike.MakeEntity.MakeName = Convert.ToString(dr["makename"]);
                                objRating.TaggedBike.MakeEntity.MaskingName = objRating.MakeMaskingName;

                                objRating.TaggedBike.ModelEntity = new BikeModelEntityBase();

                                objRating.TaggedBike.ModelEntity.ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]);
                                objRating.TaggedBike.ModelEntity.ModelName = Convert.ToString(dr["modelname"]);
                                objRating.TaggedBike.ModelEntity.MaskingName = objRating.ModelMaskingName;

                                objRating.TaggedBike.VersionEntity = new BikeVersionEntityBase();

                                objRating.TaggedBike.VersionEntity.VersionId = SqlReaderConvertor.ToInt32(dr["versionid"]);
                                objRatingList.Add(objRating);
                            }

                            dr.Close();
                            reviews.ReviewList = objRatingList;
                            reviews.TotalReviews = (uint)objRatingList.Count;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetUserReviews");

            }

            return reviews;
        }

        public BikeReviewsInfo GetBikeReviewsInfo(uint modelId, uint? skipReviewId)
        {
            BikeReviewsInfo objBikeReviewInfo = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikereviewsinfo_10102017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.UInt32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_skipreviewid", DbType.UInt32, (skipReviewId.HasValue ? skipReviewId.Value : 0)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objBikeReviewInfo = new BikeReviewsInfo()
                            {
                                Make = new BikeMakeEntityBase()
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                    MaskingName = Convert.ToString(dr["makemasking"]),
                                    MakeName = Convert.ToString(dr["makeName"])
                                },
                                Model = new BikeModelEntityBase()
                                {
                                    ModelId = SqlReaderConvertor.ToInt32(dr["modelId"]),
                                    MaskingName = Convert.ToString(dr["modelmasking"]),
                                    ModelName = Convert.ToString(dr["modelName"])
                                },
                                OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                HostUrl = Convert.ToString(dr["hostUrl"]),
                                IsDiscontinued = SqlReaderConvertor.ToBoolean(dr["IsDiscontinued"]),
                                TotalReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                MostHelpfulReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                MostRecentReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                PostiveReviews = SqlReaderConvertor.ToUInt32(dr["postivereviews"]),
                                NegativeReviews = SqlReaderConvertor.ToUInt32(dr["negativereviews"]),
                                NeutralReviews = SqlReaderConvertor.ToUInt32(dr["neutralreviews"])
                            };
                        }
                        if (dr != null)
                            dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objBikeReviewInfo;
        }

        /// <summary>
        /// Created by Sajal Gupta on 14-07-2017
        /// Description : Dal Function to fetch review questions aggrgate value by modelid
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public QuestionsRatingValueByModel GetReviewQuestionValuesByModel(uint modelId)
        {
            QuestionsRatingValueByModel objQuestionsList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getreviewquestionvaluebymodel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.UInt32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objQuestionsList = new QuestionsRatingValueByModel();
                            objQuestionsList.ModelId = modelId;
                            IList<QuestionRatingsValueEntity> objList = new List<QuestionRatingsValueEntity>();

                            while (dr.Read())
                            {
                                QuestionRatingsValueEntity objQuestion = new QuestionRatingsValueEntity();

                                objQuestion.ModelId = SqlReaderConvertor.ToUInt32(dr["modelId"]);
                                objQuestion.QuestionId = SqlReaderConvertor.ToUInt16(dr["questionId"]);
                                objQuestion.AverageRatingValue = SqlReaderConvertor.ToFloat(dr["aggregateValue"]);
                                objQuestion.QuestionHeading = Convert.ToString(dr["heading"]);
                                objQuestion.QuestionDescription = Convert.ToString(dr["description"]);

                                objList.Add(objQuestion);
                            }

                            dr.Close();

                            objQuestionsList.QuestionsList = objList;
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("Bikewale.DAL.Used.Search.GetReviewQuestionValuesByModel({0})", modelId));

            }

            return objQuestionsList;
        }

        /// <summary>
        /// Modified By :Snehal Dange on 12 Oct 2017
        /// Description : Changed Sp to getbikeratingsandreviewsinfo_12102017 .Added isScooterOnly parameter
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId)
        {
            BikeRatingsReviewsInfo objBikeRatingReviewInfo = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeratingsandreviewsinfo_12102017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.UInt32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objBikeRatingReviewInfo = new BikeRatingsReviewsInfo()
                            {
                                RatingDetails = new BikeRatingsInfo()
                                {
                                    Make = new BikeMakeEntityBase()
                                    {
                                        MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                        MaskingName = Convert.ToString(dr["makemasking"]),
                                        MakeName = Convert.ToString(dr["makeName"]),
                                        IsScooterOnly = SqlReaderConvertor.ToBoolean(dr["IsScooterOnly"])
                                    },
                                    Model = new BikeModelEntityBase()
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(dr["modelId"]),
                                        MaskingName = Convert.ToString(dr["modelmasking"]),
                                        ModelName = Convert.ToString(dr["modelName"])
                                    },
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["hostUrl"]),
                                    OverallRating = SqlReaderConvertor.ToFloat(dr["overallrating"]),
                                    IsDiscontinued = SqlReaderConvertor.ToBoolean(dr["IsDiscontinued"]),
                                    TotalReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                    OneStarRatings = SqlReaderConvertor.ToUInt32(dr["onestars"]),
                                    TwoStarRatings = SqlReaderConvertor.ToUInt32(dr["twostars"]),
                                    ThreeStarRatings = SqlReaderConvertor.ToUInt32(dr["threestars"]),
                                    FourStarRatings = SqlReaderConvertor.ToUInt32(dr["fourstars"]),
                                    FiveStarRatings = SqlReaderConvertor.ToUInt32(dr["fivestars"]),
                                    TotalRatings = SqlReaderConvertor.ToUInt32(dr["totalratings"]),
                                    BodyStyle = (EnumBikeBodyStyles)Convert.ToUInt16(dr["BodyStyleId"]),
                                    ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewsCount"])
                                },
                                ReviewDetails = new BikeReviewsInfo()
                                {
                                    TotalReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                    MostHelpfulReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                    MostRecentReviews = SqlReaderConvertor.ToUInt32(dr["totalreviews"]),
                                    PostiveReviews = SqlReaderConvertor.ToUInt32(dr["postivereviews"]),
                                    NegativeReviews = SqlReaderConvertor.ToUInt32(dr["negativereviews"]),
                                    NeutralReviews = SqlReaderConvertor.ToUInt32(dr["neutralreviews"])
                                },
                                Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                            };
                        }

                        if (dr != null && dr.NextResult() && objBikeRatingReviewInfo != null)
                        {
                            objBikeRatingReviewInfo.ObjQuestionValue = new QuestionsRatingValueByModel();
                            objBikeRatingReviewInfo.ObjQuestionValue.ModelId = modelId;
                            IList<QuestionRatingsValueEntity> objList = new List<QuestionRatingsValueEntity>();

                            while (dr.Read())
                            {
                                QuestionRatingsValueEntity objQuestion = new QuestionRatingsValueEntity();

                                objQuestion.ModelId = SqlReaderConvertor.ToUInt32(dr["modelId"]);
                                objQuestion.QuestionId = SqlReaderConvertor.ToUInt16(dr["questionId"]);
                                objQuestion.AverageRatingValue = SqlReaderConvertor.ToFloat(dr["aggregateValue"]);
                                objQuestion.QuestionHeading = Convert.ToString(dr["heading"]);
                                objQuestion.QuestionDescription = Convert.ToString(dr["description"]);

                                objList.Add(objQuestion);
                            }
                            objBikeRatingReviewInfo.ObjQuestionValue.QuestionsList = objList;
                        }
                        if (dr != null)
                            dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objBikeRatingReviewInfo;
        }


        /// <summary>
        /// Created By : Sajal Gupta on 05-05-2017
        /// Description : Get user reviews summary for all pages
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId)
        {
            UserReviewSummary objUserReviewSummary = null;

            try
            {
                var reviews = GetUserReviewSummaryList(reviewId.ToString());

                if (reviews != null)
                    objUserReviewSummary = reviews.FirstOrDefault();
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Used.Search.GetUserReviewSummaryWithRating {0}", reviewId));
            }

            return objUserReviewSummary;
        }

        /// <summary>
        /// Created by Sajal gupta on 11-05-2017
        /// Descriptio : Creates hash table for reviews id mapping
        /// </summary>
        /// <returns></returns>
        public Hashtable GetUserReviewsIdMapping()
        {
            Hashtable htResult = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchuserreviewidmapping"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        htResult = new Hashtable();
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                if (!htResult.ContainsKey(dr["oldreviewid"]))
                                    htResult.Add(dr["oldreviewid"], dr["newReviewId"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.Used.Search.GetUserReviewSummaryWithRating");
            }
            return htResult;
        }



        /// <summary>
        /// created by sajal  gupta on 06-06-2017
        /// Description : get review id list of models.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeReviewIdListByCategory GetReviewsIdListByModel(uint modelId)
        {
            BikeReviewIdListByCategory objIdList = null;
            try
            {
                objIdList = new BikeReviewIdListByCategory();

                using (DbCommand cmd = DbFactory.GetDBCommand("getreviewidlist_10102017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.UInt32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {

                        if (dr != null)
                        {
                            objIdList.RecentReviews = new List<uint>();
                            while (dr.Read())
                            {
                                objIdList.RecentReviews.Add(SqlReaderConvertor.ToUInt32(dr["reviewid"]));
                            }
                        }

                        if (dr.NextResult())
                        {
                            objIdList.HelpfulReviews = new List<uint>();
                            while (dr.Read())
                            {
                                objIdList.HelpfulReviews.Add(SqlReaderConvertor.ToUInt32(dr["reviewid"]));
                            }
                        }

                        if (dr.NextResult())
                        {
                            objIdList.PositiveReviews = new List<uint>();
                            while (dr.Read())
                            {
                                objIdList.PositiveReviews.Add(SqlReaderConvertor.ToUInt32(dr["reviewid"]));
                            }
                        }

                        if (dr.NextResult())
                        {
                            objIdList.NegativeReviews = new List<uint>();
                            while (dr.Read())
                            {
                                objIdList.NegativeReviews.Add(SqlReaderConvertor.ToUInt32(dr["reviewid"]));
                            }
                        }

                        if (dr.NextResult())
                        {
                            objIdList.NeutralReviews = new List<uint>();
                            while (dr.Read())
                            {
                                objIdList.NeutralReviews.Add(SqlReaderConvertor.ToUInt32(dr["reviewid"]));
                            }
                        }

                        dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewsRepository.GetReviewsIdListByModel  modelId {0}", modelId));
            }

            return objIdList;
        }

        /// <summary>
        /// Created by Sajal Gupta on 8-06-2017
        /// Description : thios gets review summary list from db
        /// </summary>
        /// <param name="reviewIdList"></param>
        /// <returns></returns>
        public IEnumerable<UserReviewSummary> GetUserReviewSummaryList(string reviewIdList)
        {
            ICollection<UserReviewSummary> objSummaryList = null;
            UserReviewSummary objUserReviewSummary = null;
            ICollection<UserReviewQuestion> objQuestionList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getUserReviewSummaryList_14062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewIdList", DbType.String, reviewIdList));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objSummaryList = new List<UserReviewSummary>();
                            while (dr.Read())
                            {
                                objUserReviewSummary = new UserReviewSummary()
                                {
                                    ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]),
                                    OldReviewId = SqlReaderConvertor.ToUInt32(dr["OldReviewId"]),
                                    CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                    CustomerName = Convert.ToString(dr["CustomerName"]),
                                    Description = Convert.ToString(dr["Comments"]),
                                    SanitizedDescription = Convert.ToString(dr["santizedreview"]),
                                    Title = Convert.ToString(dr["ReviewTitle"]),
                                    Tips = Convert.ToString(dr["ReviewTips"]),
                                    UpVotes = SqlReaderConvertor.ToUInt32(dr["UpVotes"]),
                                    DownVotes = SqlReaderConvertor.ToUInt32(dr["DownVotes"]),
                                    Views = SqlReaderConvertor.ToUInt32(dr["Views"]),
                                    EntryDate = SqlReaderConvertor.ToDateTime(dr["EntryDate"]),
                                    ReviewAge = FormatDate.GetTimeSpan(SqlReaderConvertor.ToDateTime(dr["EntryDate"])),
                                    OverallRatingId = SqlReaderConvertor.ToUInt16(dr["overallratingId"]),
                                    OverallRating = new UserReviewOverallRating()
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["overallratingId"]),
                                        Value = SqlReaderConvertor.ToUInt16(dr["Rating"]),
                                        Heading = Convert.ToString(dr["Heading"]),
                                        Description = Convert.ToString(dr["Description"]),
                                    },
                                    Make = new BikeMakeEntityBase()
                                    {
                                        MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                        MaskingName = Convert.ToString(dr["makemasking"]),
                                        MakeName = Convert.ToString(dr["makeName"]),
                                        IsScooterOnly = Convert.ToBoolean(dr["isScooterOnly"])
                                    },
                                    Model = new BikeModelEntityBase()
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(dr["modelId"]),
                                        MaskingName = Convert.ToString(dr["modelmasking"]),
                                        ModelName = Convert.ToString(dr["modelName"])
                                    },
                                    OriginalImagePath = Convert.ToString(dr["OriginalImgPath"]),
                                    HostUrl = Convert.ToString(dr["hostUrl"]),
                                    TotalReviews = SqlReaderConvertor.ToUInt32(dr["TotalReviews"]),
                                    TotalRatings = SqlReaderConvertor.ToUInt32(dr["TotalRatings"]),
                                    OverAllModelRating = SqlReaderConvertor.ToFloat(dr["OverallModelRating"])
                                };
                                objSummaryList.Add(objUserReviewSummary);
                            }
                        }

                        if (dr.NextResult())
                        {
                            objQuestionList = new List<UserReviewQuestion>();
                            while (dr.Read())
                            {
                                objQuestionList.Add(new UserReviewQuestion()
                                {
                                    ReviewId = SqlReaderConvertor.ToUInt32(dr["reviewId"]),
                                    SelectedRatingId = SqlReaderConvertor.ToUInt32(dr["answerValue"]),
                                    Id = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                    Heading = Convert.ToString(dr["Heading"]),
                                    Description = Convert.ToString(dr["Description"]),
                                    DisplayType = (UserReviewQuestionDisplayType)Convert.ToInt32(dr["DisplayType"]),
                                    Type = (UserReviewQuestionType)Convert.ToInt32(dr["QuestionType"]),
                                    Order = SqlReaderConvertor.ToUInt16(dr["DisplayOrder"]),
                                    MinHeading = Convert.ToString(dr["minHeading"]),
                                    SelectedRatingText = Convert.ToString(dr["ratingtext"])
                                });
                            }
                        }

                        dr.Close();
                    }
                }

                if (objQuestionList != null)
                {
                    var groups = objQuestionList.GroupBy(x => x.ReviewId);

                    foreach (var group in groups)
                    {
                        objSummaryList.FirstOrDefault(s => s.ReviewId == group.Key).Questions = group.ToList();

                        foreach (var ele in group)
                        {
                            if (ele.Type == UserReviewQuestionType.Rating)
                            {
                                objSummaryList.FirstOrDefault(s => s.ReviewId == group.Key).IsRatingQuestion = true;
                                break;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewsRepository.GetUserReviewSummaryList");
            }

            return objSummaryList;
        }

        public IEnumerable<RecentReviewsWidget> GetRecentReviews()
        {
            IEnumerable<RecentReviewsWidget> objReviewsList = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();

                    objReviewsList = connection.Query<RecentReviewsWidget>("getrecentuserreview", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetRatingsList");
            }

            return objReviewsList;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar On 11th Aug 2017
        /// Summary: Get list of winner of user reviews contest (Top 4)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RecentReviewsWidget> GetUserReviewsWinners()
        {
            IEnumerable<RecentReviewsWidget> objReviewsWinnersList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    objReviewsWinnersList = connection.Query<RecentReviewsWidget>("getuserreviewswinners", commandType: CommandType.StoredProcedure);
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.UserReviews.UserReviewsRepository.GetUserReviewsWinners");
            }
            return objReviewsWinnersList;
        }


        /// <summary>
        /// Created by Sajal Gupta on 10-10-2017
        /// Description : Dal layer function to get data of top rated bikes        
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<TopRatedBikes> GetTopRatedBikes(uint topCount)
        {
            IEnumerable<TopRatedBikes> objTopRatedBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("gettopratedbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topCount", DbType.Int32, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objTopRatedBikes = PopulateTopRatedBikesWidget(dr);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format(" ModelVersionDescription.GetTopRatedBikes_topcount_{0}", topCount));
            }
            return objTopRatedBikes;
        }

        /// <summary>
        /// Created by Sajal Gupta on 10-10-2017
        /// Description : Dal layer function to get data of top rated bikes        
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<TopRatedBikes> GetTopRatedBikes(uint topCount, uint cityId)
        {
            IEnumerable<TopRatedBikes> objTopRatedBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("gettopratedbikesbycity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topCount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objTopRatedBikes = PopulateTopRatedBikesWidget(dr);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format(" ModelVersionDescription.GetTopRatedBikes_topcount_{0}_cityid_{1}", topCount, cityId));
            }
            return objTopRatedBikes;
        }

        private IEnumerable<TopRatedBikes> PopulateTopRatedBikesWidget(IDataReader dr)
        {
            IList<TopRatedBikes> objTopRatedBikes = new List<TopRatedBikes>();

            while (dr.Read())
            {
                objTopRatedBikes.Add(new TopRatedBikes()
                {
                    Make = new BikeMakeEntityBase
                    {
                        MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                        MakeName = Convert.ToString(dr["MakeName"]),
                        MaskingName = Convert.ToString(dr["MakeMaskingName"]),
                    },
                    Model = new BikeModelEntityBase
                    {
                        ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                        ModelName = Convert.ToString(dr["modelname"]),
                        MaskingName = Convert.ToString(dr["modelmaskingname"]),
                    },
                    City = new CityEntityBase
                    {
                        CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]),
                        CityName = Convert.ToString(dr["cityname"]),
                        CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                    },
                    ReviewRate = SqlReaderConvertor.ToFloat(dr["ReviewRate"]),
                    RatingsCount = SqlReaderConvertor.ToUInt32(dr["ratingscount"]),
                    ReviewCount = SqlReaderConvertor.ToUInt32(dr["reviewcount"]),
                    ExShowroomPrice = SqlReaderConvertor.ToUInt32(dr["price"]),
                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                    HostUrl = Convert.ToString(dr["hosturl"])
                });
            }
            return objTopRatedBikes;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To get popular bikes with expert reviews count
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<PopularBikesWithExpertReviews> GetPopularBikesWithExpertReviews(ushort topCount)
        {
            return FetchPopularBikesWithExpertReviews("getpopularbikeswithexpertreviews", topCount, 0);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To get popular bikes with expert reviews count by city
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularBikesWithExpertReviews> GetPopularBikesWithExpertReviewsByCity(ushort topCount, uint cityId)
        {
            return FetchPopularBikesWithExpertReviews("getpopularbikeswithexpertreviewsbycity", topCount, cityId);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To fetch popular bikes with expert reviews count
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private IEnumerable<PopularBikesWithExpertReviews> FetchPopularBikesWithExpertReviews(string spName, ushort topCount, uint cityId)
        {
            IList<PopularBikesWithExpertReviews> objBikesWithExpertReviews = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    if (cityId > 0)
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    }

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objBikesWithExpertReviews = new List<PopularBikesWithExpertReviews>();

                            while (dr.Read())
                            {
                                objBikesWithExpertReviews.Add(new PopularBikesWithExpertReviews()
                                {
                                    Make = new BikeMakeEntityBase
                                    {
                                        MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                        MakeName = Convert.ToString(dr["make"]),
                                        MaskingName = Convert.ToString(dr["makemaskingname"]),
                                    },
                                    Model = new BikeModelEntityBase
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                        ModelName = Convert.ToString(dr["model"]),
                                        MaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    },
                                    City = new CityEntityBase
                                    {
                                        CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]),
                                        CityName = Convert.ToString(dr["cityname"]),
                                        CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                    },
                                    ExpertReviewCount = SqlReaderConvertor.ToUInt32(dr["expertreviewscount"]),
                                    Price = SqlReaderConvertor.ToUInt32(dr["price"]),
                                    OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                    HostUrl = Convert.ToString(dr["hosturl"]),
                                    IsOnRoadPrice = SqlReaderConvertor.ToBoolean(dr["isonroadprice"]),
                                });

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.DAL.UserReviews.UserReviewsRepository.FetchPopularBikesWithExpertReviews_topcount_{0}_cityid_{1}_spName_{2}", topCount, cityId, spName));
            }
            return objBikesWithExpertReviews;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To fetch popular bikes with expert reviews count
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularBikesWithUserReviews> GetPopularBikesWithUserReviewsByMake(uint makeId)
        {
            IList<PopularBikesWithUserReviews> objBikesWithUserReviews = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getpopularbikeswithuserreviewsbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objBikesWithUserReviews = new List<PopularBikesWithUserReviews>();

                            while (dr.Read())
                            {
                                objBikesWithUserReviews.Add(new PopularBikesWithUserReviews()
                                {
                                    Make = new BikeMakeEntityBase
                                    {
                                        MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                        MakeName = Convert.ToString(dr["makename"]),
                                        MaskingName = Convert.ToString(dr["makemaskingname"]),
                                    },
                                    Model = new BikeModelEntityBase
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                        ModelName = Convert.ToString(dr["modelname"]),
                                        MaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    },
                                    ReviewCount = SqlReaderConvertor.ToUInt32(dr["reviewcount"]),
                                    RatingsCount = SqlReaderConvertor.ToUInt32(dr["ratingcount"]),
                                    OverallRating = SqlReaderConvertor.ToFloat(dr["reviewrate"]),
                                    OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                    HostUrl = Convert.ToString(dr["hosturl"])
                                });

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.DAL.UserReviews.UserReviewsRepository.GetPopularBikesWithUserReviewsBy_Make_{0}", makeId));
            }
            return objBikesWithUserReviews;
        }


        /// <summary>
        /// Created By:Snehal Dange on 17th Nov 2017
        /// Description: Get most helpful and recent reviews of popular models by make
        /// Modified by : Snehal Dange on 5th Feb 2017
        /// Description : Modified sp from 'getreviewsofpopularbikesbymake' to 'getreviewsofpopularbikesbymake_05022018'. Added modelcout with reviews and total make reviews.
        /// </summary>
        /// <param name="makeId"></param>
        public IEnumerable<BikesWithReviewByMake> GetBikesWithReviewsByMake(uint makeId)
        {
            IList<BikesWithReviewByMake> objBikesWithUserReviews = null;
            try
            {
                if (makeId > 0)
                {
                    objBikesWithUserReviews = new List<BikesWithReviewByMake>();
                    IList<PopularBikesWithUserReviews> objPopularModels = null;

                    using (DbCommand cmd = DbFactory.GetDBCommand("getreviewsofpopularbikesbymake_05022018"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (objBikesWithUserReviews != null)
                            {
                                if (dr != null)
                                {
                                    objPopularModels = new List<PopularBikesWithUserReviews>();
                                    while (dr.Read())
                                    {
                                        objPopularModels.Add(new PopularBikesWithUserReviews()
                                        {
                                            Make = new BikeMakeEntityBase
                                            {
                                                MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                                MakeName = Convert.ToString(dr["make"]),
                                                MaskingName = Convert.ToString(dr["makemaskingname"]),
                                            },
                                            Model = new BikeModelEntityBase
                                            {
                                                ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                                ModelName = Convert.ToString(dr["model"]),
                                                MaskingName = Convert.ToString(dr["modelmaskingname"]),
                                            },
                                            ReviewCount = SqlReaderConvertor.ToUInt32(dr["reviewcount"]),
                                            RatingsCount = SqlReaderConvertor.ToUInt32(dr["ratingscount"]),
                                            OverallRating = SqlReaderConvertor.ToFloat(dr["reviewrate"]),
                                            OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                            HostUrl = Convert.ToString(dr["hosturl"])
                                        });
                                    }

                                    BikesWithReviewByMake objModelReview = null;
                                    if (objPopularModels.Any())
                                    {
                                        foreach (var obj in objPopularModels)
                                        {
                                            objModelReview = new BikesWithReviewByMake();
                                            if (objModelReview != null)
                                            {
                                                objModelReview.BikeModel = obj;
                                                objBikesWithUserReviews.Add(objModelReview);
                                            }

                                        }
                                    }
                                    uint modelid = 0;
                                    Entities.UserReviews.V2.UserReviewSummary objRecentReviews = null;
                                    if (dr.NextResult())
                                    {
                                        while (dr.Read())
                                        {
                                            modelid = SqlReaderConvertor.ToUInt32(dr["ModelId"]);
                                            objRecentReviews = new Entities.UserReviews.V2.UserReviewSummary()
                                            {
                                                ReviewId = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                                OverallRatingId = SqlReaderConvertor.ToUInt16(dr["OverallRatingId"]),
                                                Description = Convert.ToString(dr["StrippedReview"]),
                                                Title = Convert.ToString(dr["Title"])
                                            };

                                            objBikesWithUserReviews.Where(m => m.BikeModel.Model.ModelId == modelid).FirstOrDefault().MostRecent = objRecentReviews;


                                        }
                                    }

                                    Entities.UserReviews.V2.UserReviewSummary objHelpfulReviews = null;
                                    if (dr.NextResult())
                                    {
                                        while (dr.Read())
                                        {
                                            modelid = SqlReaderConvertor.ToUInt32(dr["ModelId"]);
                                            objHelpfulReviews = new Entities.UserReviews.V2.UserReviewSummary()
                                            {
                                                ReviewId = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                                OverallRatingId = SqlReaderConvertor.ToUInt16(dr["OverallRatingId"]),
                                                Description = Convert.ToString(dr["StrippedReview"]),
                                                Title = Convert.ToString(dr["Title"])
                                            };

                                            objBikesWithUserReviews.Where(m => m.BikeModel.Model.ModelId == modelid).FirstOrDefault().MostHelpful = objHelpfulReviews;

                                        }
                                    }
                                    if (dr.NextResult())
                                    {
                                        while (dr.Read())
                                        {
                                            if (objBikesWithUserReviews.Any())
                                            {
                                                objBikesWithUserReviews.FirstOrDefault().MakeReviewCount = SqlReaderConvertor.ToUInt32(dr["makereviewcount"]);
                                                objBikesWithUserReviews.FirstOrDefault().ModelCountWithUserReviews = SqlReaderConvertor.ToUInt32(dr["modelcountwithreviews"]);
                                            }

                                        }
                                    }
                                    dr.Close();
                                }
                            }


                        }
                    }
                }
            }
            catch (Exception err)
            {
                Bikewale.Notifications.ErrorClass.LogError(err, string.Format("Bikewale.DAL.UserReviews.UserReviewsRepository.GetBikesWithReviewsByMake_Make_{0}", makeId));
            }
            return objBikesWithUserReviews;
        }

    }// class end
}
