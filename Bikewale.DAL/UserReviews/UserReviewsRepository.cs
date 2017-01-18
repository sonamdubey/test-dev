﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace Bikewale.DAL.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class have functions to interact with database to get the user reviews data.
    /// </summary>
    public class UserReviewsRepository : IUserReviews
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
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

                                objReviewEntity.ReviewId = Convert.ToInt32(dr["ReviewId"]);
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

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
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

                                objReviewEntity.ReviewId = Convert.ToInt32(dr["ReviewId"]);
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

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                                objReviewEntity.ReviewId = Convert.ToInt32(dr["ReviewId"]);
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

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                                objReviewEntity.ReviewId = Convert.ToInt32(dr["ReviewId"]);
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

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelrating"))
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

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikereviewslist"))
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

                                objRating.ReviewId = Convert.ToInt32(dr["ReviewId"]);
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return reviews;
        }

        /// <summary>
        /// Written By : Ashwini Todkar 
        /// Summary    : Method to get review details like Rating,Aouther,Written Date also related next and prev review id
        /// Modified By : Suresh Prajapati on 20 Aug 2014
        /// Summary : To retrieve new and used flag for bike model
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetReviewDetails(uint reviewId)
        {
            ReviewDetailsEntity objRating = null; //


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcustomerreviewdetails_new"))
                {
                    //cmd.CommandText = "getcustomerreviewdetails_new";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@ReviewId", SqlDbType.Int).Value = reviewId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int32, reviewId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objRating = new ReviewDetailsEntity();

                            objRating.BikeEntity.MakeEntity.MakeName = dr["Make"].ToString();
                            objRating.BikeEntity.MakeEntity.MaskingName = dr["MakemaskingName"].ToString();
                            objRating.BikeEntity.ModelEntity.ModelName = dr["Model"].ToString();
                            objRating.BikeEntity.ModelEntity.MaskingName = dr["ModelMaskingName"].ToString();
                            objRating.BikeEntity.ModelEntity.ModelId = Convert.ToInt32(dr["ModelId"]);
                            objRating.BikeEntity.VersionEntity.VersionId = Convert.ToInt32(dr["VersionId"]);
                            objRating.HostUrl = dr["HostURL"].ToString();
                            objRating.LargePicUrl = dr["LargePic"].ToString();
                            objRating.New = Convert.ToBoolean(dr["New"]);
                            objRating.Used = Convert.ToBoolean(dr["Used"]);
                            objRating.BikeEntity.Price = Convert.ToUInt32(dr["MinPrice"]);
                            objRating.ReviewEntity.ReviewId = Convert.ToInt32(dr["ReviewId"]);
                            objRating.ReviewEntity.ReviewTitle = dr["Title"].ToString();
                            objRating.ReviewRatingEntity.StyleRating = Convert.ToSingle(dr["StyleR"]);
                            objRating.ReviewRatingEntity.ComfortRating = Convert.ToSingle(dr["ComfortR"]);
                            objRating.ReviewRatingEntity.PerformanceRating = Convert.ToSingle(dr["PerformanceR"]);
                            objRating.ReviewRatingEntity.FuelEconomyRating = Convert.ToSingle(dr["FuelEconomyR"]);
                            objRating.ReviewRatingEntity.ValueRating = Convert.ToSingle(dr["ValueR"]);
                            objRating.ReviewRatingEntity.OverAllRating = Convert.ToSingle(dr["OverallR"]);
                            objRating.ReviewEntity.Pros = dr["Pros"].ToString();
                            objRating.ReviewEntity.Cons = dr["Cons"].ToString();
                            objRating.ReviewEntity.Comments = dr["Comments"].ToString();
                            objRating.ReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);
                            objRating.ReviewEntity.Liked = Convert.ToUInt16(dr["Liked"]);
                            objRating.ReviewEntity.Disliked = Convert.ToUInt16(dr["Disliked"]);
                            objRating.ReviewEntity.Viewed = Convert.ToUInt16(dr["Viewed"]);
                            objRating.ReviewEntity.WrittenBy = dr["CustomerName"].ToString();
                            objRating.OriginalImagePath = dr["OriginalImagePath"].ToString();
                            //Get Previous review page details                           
                            if (dr.NextResult())
                            {
                                if (dr != null && dr.Read())
                                {
                                    objRating.PrevReviewId = Convert.ToUInt32(dr["PrevReviewId"]);
                                    // objRating.PrevReviewTitle = dr["PrevReviewTitle"].ToString();
                                }
                            }

                            //Get next review page details
                            if (dr.NextResult())
                            {
                                if (dr != null && dr.Read())
                                {
                                    objRating.NextReviewId = Convert.ToUInt32(dr["NextReviewId"]);
                                    //objRating.NextReviewTitle = dr["NextReviewTitle"].ToString();
                                }
                            }

                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objRating;
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 500, userId));


                    success = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }

            catch (Exception ex)
            {

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    //cmd.Parameters.Add("@ReviewId", SqlDbType.Int).Value = reviewId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int32, reviewId));

                    success = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }

            catch (Exception ex)
            {

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return success;
        }
    }
}
