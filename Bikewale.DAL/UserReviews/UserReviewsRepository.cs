using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;

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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetMostReviewedBikes";
                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;

                    BikeMakeEntityBase objMakeBase = null;
                    BikeModelEntityBase objModelBase = null;

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetMostReviewedBikesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMostReviewedBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetReviewedBikes";
                  
                    BikeMakeEntityBase objMakeBase = null;
                    BikeModelEntityBase objModelBase = null;

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetReviewedBikesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetReviewedBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetMostReadReviews";
                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;

                    ReviewEntityBase  objReviewEntity =null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetMostReadReviews sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMostReadReviews ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetMostHelpfulReviews";
                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetMostHelpfulReviews sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMostHelpfulReviews ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetMostRecentReviews";
                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetMostRecentReviews sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMostRecentReviews ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetMostRatedReviews";
                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = totalRecords;

                    ReviewEntityBase objReviewEntity = null;
                    ReviewRatingEntityBase objReviewRating = null;
                    ReviewTaggedBikeEntity objTaggedBike = null;

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetMostRatedReviews sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMostRatedReviews ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetModelRating";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            objRate = new ReviewRatingEntity();

                            objRate.ComfortRating =Convert.ToSingle(dr["Comfort"]);
                            objRate.FuelEconomyRating = Convert.ToSingle(dr["FuelEconomy"]);
                            objRate.PerformanceRating = Convert.ToSingle(dr["Performance"]);
                            objRate.StyleRating = Convert.ToSingle(dr["Looks"]);
                            objRate.ValueRating = Convert.ToSingle(dr["ValueForMoney"]);
                            objRate.OverAllRating =Convert.ToSingle(dr["ReviewRate"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeRatings sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeRatings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
        /// <returns></returns>
        public List<ReviewEntity> GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, out uint totalReviews)
        {
            List<ReviewEntity> objRatingList = null;
            Database db = null;
            totalReviews = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetBikeReviewsList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StartIndex", SqlDbType.Int).Value = startIndex;
                    cmd.Parameters.Add("@EndIndex", SqlDbType.Int).Value = endIndex;
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    if (versionId > 0)
                        cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;

                    cmd.Parameters.Add("@Filter", SqlDbType.Int).Value = filter;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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

                                objRatingList.Add(objRating);

                                totalReviews = Convert.ToUInt32(dr["RecordCount"]);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetNewLaunchedBikesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetNewLaunchedBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objRatingList;
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
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerReviewDetails_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ReviewId", SqlDbType.Int).Value = reviewId;

                    db = new Database();
                
                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetReviewDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetReviewDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            bool success = false;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("UpdateCustomerReviewsAbuse"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReviewId", SqlDbType.Int).Value = reviewId;
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 500).Value = comment;
                    cmd.Parameters.Add("@ReportedBy", SqlDbType.Int).Value = userId;

                    success = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("SQL Exception in AbuseReview () : " + sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("AbuseReview Exception : " + ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            bool success = false;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("UpdateReviewViews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReviewId", SqlDbType.Int).Value = reviewId;

                    success = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("UpdateReviewViews SQL Exception : " + sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateReviewViews Exception : " + ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            finally
            {
                db.CloseConnection();
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

            Database db = null;
            bool success = false;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("UpdateCustomerReviewsHelpful"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReviewId", SqlDbType.Int).Value = reviewId;
                    cmd.Parameters.Add("@Helpful", SqlDbType.Bit).Value = isHelpful;

                    success = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("UpdateReviewUseful SQL Exception : " + sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateReviewUseful Exception : " + ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return success;
        }
    }
}
