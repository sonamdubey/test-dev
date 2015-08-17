using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.CoreDAL;
using Bikewale.Notifications;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish Kamble on 24 Apr 2014
    /// Summary : class have functions related to the bike series.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeSeriesRepository<T,U> : IBikeSeries<T,U> where T : BikeSeriesEntity, new()
    {
        /// <summary>
        /// Summary : Function to get all bike models for the given series id.
        /// </summary>
        /// <param name="seriesId">series id should be positive number only.</param>
        /// <returns>Returns list containing the bikemodelentity.</returns>
        public List<BikeModelEntity> GetModelsList(U seriesId)
        {
            List<BikeModelEntity> objList = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetBikeModelsBySeries";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {                        
                        objList = new List<BikeModelEntity>();

                        while (dr.Read())
                        {
                            BikeModelEntity objModel = new BikeModelEntity();

                            objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                            objModel.ModelName = Convert.ToString(dr["ModelName"]);
                            objModel.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                            objModel.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                            objModel.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                            objModel.ReviewCount = Convert.ToInt32(dr["ReviewCount"]);
                            objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                            objModel.ModelSeries.MaskingName = Convert.ToString(dr["SeriesMaskingName"]);
                            objModel.ModelSeries.SeriesName = Convert.ToString(dr["SeriesName"]);
                            objModel.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                            objModel.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            objModel.HostUrl = Convert.ToString(dr["HostURL"]);
                            objModel.SmallPicUrl = Convert.ToString(dr["SmallPic"]);
                            objModel.LargePicUrl = Convert.ToString(dr["LargePic"]);
                            objModel.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);

                            objList.Add(objModel);
                        }                        
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objList;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            T t = default(T);
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetSeriesDetails";

                    cmd.Parameters.Add("@BikeSeriesId", SqlDbType.Int).Value = id;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            t = new T();

                            if (dr.Read())
                            {
                                t.SeriesId = (int)Convert.ChangeType(id, typeof(int));
                                t.SeriesName = Convert.ToString(dr["NAME"]);
                                t.MaskingName = Convert.ToString(dr["SeriesMaskingName"]);
                                t.ModelCount = Convert.ToInt32(dr["ModelCount"]);
                                t.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                t.MakeBase.MakeId = Convert.ToInt32(dr["MakeId"]);
                                t.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetById sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetById ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return t;
        }


        public BikeDescriptionEntity GetSeriesDescription(U seriesId)
        {
            throw new NotImplementedException();
        }


        public List<BikeModelEntityBase> GetModelsListBySeriesId(U seriesId)
        {
            Database db = null;
            List<BikeModelEntityBase> objModels = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetBikeModelBySeriesId";

                    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objModels = new List<BikeModelEntityBase>();

                            while (dr.Read())
                            {
                                objModels.Add(new BikeModelEntityBase()
                                {
                                    ModelId = Convert.ToInt32(dr["ModelId"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    MaskingName = Convert.ToString(dr["ModelMaskingName"])
                                });    
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelsListBySeriesId sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelsListBySeriesId ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objModels;
        }   // end of GetModelsListBySeriesId

    }   // class
}   // namespace
