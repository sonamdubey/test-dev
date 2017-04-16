using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
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

                ErrorClass objErr = new ErrorClass(ex, string.Format(" UserReviewsRepository.GetReviewDetails() --> ReviewId: {0}", reviewId));

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
                                        Description = Convert.ToString(dr["Description"])
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
                                        Value = Convert.ToString(dr["RatingValue"])
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
                                question.Rating = rating;
                                break;
                            }
                        }

                        foreach (var priceId in priceRangeIds)
                        {
                            if (priceId.Key == question.Id)
                            {
                                question.PriceRangeIds = priceId.Select(x => x.RangeId);
                                break;
                            }
                        }
                    }



                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return objData;

        }


        public uint SaveUserReviewRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint customerId, uint makeId, uint modelId)
        {
            uint reviewId = 0;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("saveuserratings"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallrating", DbType.String, overAllrating));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ratingQuestionAns", DbType.String, ratingQuestionAns));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userName", DbType.String, userName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailId", DbType.String, emailId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            reviewId = SqlReaderConvertor.ToUInt32(dr["reviewId"]);
                        }
                        dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return reviewId;
        }

        public bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns)
        {
            bool IsSaved = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("saveuserreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.UInt32, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tipsnadvices", DbType.String, tipsnAdvices));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comment", DbType.String, comment));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_commenttitle", DbType.String, commentTitle));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ratingquestionans", DbType.String, reviewsQuestionAns));

                    IsSaved = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }

            catch (Exception ex)
            {

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return IsSaved;
        }



        public UserReviewSummary GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objUserReviewSummary = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getUserReviewSummary"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewId", DbType.UInt32, reviewId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objUserReviewSummary = new UserReviewSummary()
                            {
                                ModelId = SqlReaderConvertor.ToUInt32(dr["modelId"]),
                                CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                CustomerName = Convert.ToString(dr["CustomerName"]),
                                Description = Convert.ToString(dr["Comments"]),
                                Tips = Convert.ToString(dr["ReviewTitle"]),
                                OverallRatingId = SqlReaderConvertor.ToUInt16(dr["overallratingId"])
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
                                    Id = SqlReaderConvertor.ToUInt32(dr["QuestionId"])
                                });
                            }
                        }

                        dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objUserReviewSummary;
        }
    }
}
