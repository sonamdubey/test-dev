using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.PhotoGallery;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.CMS;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Data;
using Bikewale.Notifications;
using System.Web;

namespace Bikewale.DAL.PhotoGallery
{
    public class ModelPhotosRespository<T,U> : IModelPhotos<T,U> where T : ModelPhotoEntity, new()
    {

       
        /// <summary>
        /// Created By : Sadhana Upadhyay on 27 June 2014
        /// Summary : To get image list For given modelid and categoryid
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public List<ModelPhotoEntity> GetModelPhotosList(U modelId, List<EnumCMSContentType> categoryIdList)
        { 
            List<ModelPhotoEntity> objPhotosList = null;
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetModelPhotoList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    string categoryList = string.Empty;

                    categoryIdList.ForEach(s => categoryList += (int)s + ",");

                    if (categoryList.EndsWith(","))
                    {
                        categoryList = categoryList.Substring(0, categoryList.Length - 1);
                    }

                    cmd.Parameters.Add("@CategoryId", SqlDbType.VarChar, 25).Value = categoryList;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objPhotosList = new List<ModelPhotoEntity>();

                            while (dr.Read())
                            {
                                ModelPhotoEntity objPhotos = new ModelPhotoEntity();

                                objPhotos.ImageId = Convert.ToUInt32(dr["PhotoId"]);
                                objPhotos.HostUrl = Convert.ToString(dr["HostUrl"]);
                                objPhotos.ImagePathThumbnail = Convert.ToString(dr["ImagePathThumbnail"]);
                                objPhotos.ImagePathLarge = Convert.ToString(dr["ImagePathLarge"]);
                                objPhotos.ImageCategory = Convert.ToString(dr["PhotoCategory"]);
                                objPhotos.BasicId = Convert.ToInt32(dr["BasicId"]);
                                objPhotos.Caption = Convert.ToString(dr["Caption"]);
                                objPhotos.ArticleTitle = Convert.ToString(dr["ArticleTitle"]);
                                objPhotos.ArticleUrl = Convert.ToString(dr["ArticleUrl"]);
                                objPhotos.MakeBase.MakeId = Convert.ToInt32(dr["MakeId"].ToString());
                                objPhotos.MakeBase.MakeName = Convert.ToString(dr["Make"]);
                                objPhotos.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objPhotos.ModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objPhotos.ModelBase.ModelName = Convert.ToString(dr["Model"]);
                                objPhotos.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                objPhotosList.Add(objPhotos);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelImageList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelImageList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objPhotosList;
        }   //End of GetModelImageList

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 July 2014
        /// Summary : to get other model photos list by category id list and modelId
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="categoryIdList"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<ModelPhotoEntity> GetOtherModelPhotosList(int startIndex, int endIndex, int makeId, int modelId, List<EnumCMSContentType> categoryIdList, out int recordCount)
        {
            List<ModelPhotoEntity> objPhotosList = null;
            Database db = null;

            recordCount = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetOtherModelGalleryList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StartIndex", SqlDbType.Int).Value = startIndex;
                    cmd.Parameters.Add("@EndIndex", SqlDbType.Int).Value = endIndex;
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    string categoryId = string.Empty;

                    categoryIdList.ForEach(s => categoryId += (int)s + ",");

                    if (categoryId.EndsWith(","))
                    {
                        categoryId = categoryId.Substring(0, categoryId.Length - 1);
                    }

                    cmd.Parameters.Add("@CategoryId", SqlDbType.VarChar, 25).Value = categoryId;
                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objPhotosList = new List<ModelPhotoEntity>();

                            while (dr.Read())
                            {
                                recordCount = Convert.ToInt32(dr["RecordCount"]);
                            }

                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    ModelPhotoEntity objPhotos = new ModelPhotoEntity();

                                    objPhotos.HostUrl = Convert.ToString(dr["HostURL"]);
                                    objPhotos.ImagePathLarge = Convert.ToString(dr["LargePicPath"]);
                                    objPhotos.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                    objPhotos.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                    objPhotos.ModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                                    objPhotos.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                    objPhotosList.Add(objPhotos);
                                } 
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherModelPhotosList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherModelPhotosList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objPhotosList;
        }   //End of GetOtherModelPhotosList
    }   //End of class
}   //Emd of namespace
