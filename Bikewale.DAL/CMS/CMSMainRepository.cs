using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class have default implementation for the CMS contents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class CMSMainRepository<T, V> : ICMSContentRepository<T, V> where T : CMSContentListEntity, new()
                                                                where V : CMSPageDetailsEntity, new()
    {
        public virtual IList<T> GetContentList(int startIndex, int endIndex, out int recordCount, ContentFilter filters)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Summary : Method to get the cms page details. This method can be overriden to have customize implementation.
        /// </summary>
        /// <param name="contentId">Content id.</param>
        /// <param name="pageId">Page id for which content are required.</param>
        /// <returns></returns>
        public virtual V GetContentDetails(int contentId, int pageId)
        {
            V v = default(V);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcmspagedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_basicid", DbType.Int64, contentId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_priority", DbType.Byte, pageId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            v = new V();

                            List<CMSPageEntity> objPageList = new List<CMSPageEntity>();

                            while (dr.Read())
                            {
                                objPageList.Add(new CMSPageEntity()
                                {
                                    pageId = Convert.ToInt32(dr["PageId"]),
                                    PageName = Convert.ToString(dr["PageName"]),
                                    Priority = Convert.ToUInt16(dr["Priority"])
                                });
                            }

                            objPageList.Add(new CMSPageEntity()
                            {
                                pageId = 0,
                                PageName = "Photos",
                                Priority = Convert.ToUInt16(objPageList.Count + 1)
                            });

                            v.PageList = objPageList;

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    v.AuthorName = Convert.ToString(dr["AuthorName"]);
                                    v.Category = Convert.ToString(dr["Category"]);
                                    v.DisplayDate = Convert.ToString(dr["DisplayDate"]);
                                    v.FieldName = Convert.ToString(dr["FieldName"]);
                                    v.OtherInfoValue = Convert.ToString(dr["OtherInfoValue"]);
                                    v.SubCategory = Convert.ToString(dr["SubCategory"]);
                                    v.Title = Convert.ToString(dr["Title"]);
                                    v.Url = Convert.ToString(dr["Url"]);
                                    v.ValueType = Convert.ToString(dr["ValueType"]);
                                }
                            }

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    v.Data = Convert.ToString(dr["Data"]);
                                }
                            }

                            List<CMSBikeTagsEntity> objTags = new List<CMSBikeTagsEntity>();

                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    CMSBikeTagsEntity bikeTags = new CMSBikeTagsEntity();

                                    bikeTags.MakeBase.MakeId = Convert.ToInt32(dr["MakeId"]);
                                    bikeTags.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                    bikeTags.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                                    bikeTags.ModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                                    bikeTags.ModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                                    bikeTags.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                    bikeTags.VersionBase.VersionId = Convert.ToInt32(dr["VersionId"]);
                                    bikeTags.VersionBase.VersionName = Convert.ToString(dr["VersionName"]);

                                    objTags.Add(bikeTags);
                                }
                            }

                            v.TagsList = objTags;

                            List<CMSImageEntity> imageList = new List<CMSImageEntity>();

                            if (pageId == (objPageList.Count))
                            {
                                if (dr.NextResult())
                                {
                                    while (dr.Read())
                                    {
                                        imageList.Add(new CMSImageEntity()
                                        {
                                            Caption = Convert.ToString(dr["Caption"]),
                                            HostUrl = Convert.ToString(dr["HostUrl"]),
                                            ImageName = Convert.ToString(dr["ImageName"]),
                                            ImagePathCustom = Convert.ToString(dr["ImagePathCustom"]),
                                            ImagePathCustom140 = Convert.ToString(dr["ImagePathCustom140"]),
                                            ImagePathCustom200 = Convert.ToString(dr["ImagePathCustom200"]),
                                            ImagePathCustom88 = Convert.ToString(dr["ImagePathCustom88"]),
                                            ImagePathLarge = Convert.ToString(dr["ImagePathLarge"]),
                                            ImagePathOriginal = Convert.ToString(dr["ImagePathOriginal"]),
                                            ImagePathThumbnail = Convert.ToString(dr["ImagePathThumbnail"]),
                                            IsMainImage = Convert.ToBoolean(dr["IsMainImage"]),
                                            MakeId = Convert.ToInt32(dr["MakeId"]),
                                            ModelId = Convert.ToInt32(dr["ModelId"]),
                                            Sequence = Convert.ToInt16(dr["Sequence"])
                                        });
                                    }

                                    v.ImageList = imageList;
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return v;
        }   // End of GetContentDetails

        /// <summary>
        /// Function to update the views of the content
        /// </summary>
        /// <param name="contentId"></param>
        public void UpdateViews(int contentId)
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatecmsviews";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_basicid", DbType.Int64, contentId));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

        }   // End of UpdateViews


        public List<CMSFeaturedArticlesEntity> GetFeaturedArticles(List<EnumCMSContentType> contentTypes, ushort totalRecords)
        {
            List<CMSFeaturedArticlesEntity> featuredList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getfeaturedarticleslist";                    

                    string contentTypeList = string.Empty;

                    contentTypes.ForEach(s => contentTypeList += (int)s + ",");

                    if (contentTypeList.EndsWith(","))
                    {
                        contentTypeList = contentTypeList.Substring(0, contentTypeList.Length - 1);
                    }

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalRecords));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categorylist", DbType.String, 20, contentTypeList));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            featuredList = new List<CMSFeaturedArticlesEntity>();

                            while (dr.Read())
                            {
                                featuredList.Add(new CMSFeaturedArticlesEntity()
                                {
                                    ContentId = Convert.ToInt32(dr["BasicId"]),
                                    CategoryId = Convert.ToUInt16(dr["CategoryId"]),
                                    ContentUrl = Convert.ToString(dr["Url"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    LargePicUrl = Convert.ToString(dr["ImagePathLarge"]),
                                    SmallPicUrl = Convert.ToString(dr["ImagePathThumbnail"]),
                                    Title = Convert.ToString(dr["Title"])
                                });
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException exSql)
            {
                HttpContext.Current.Trace.Warn(exSql.Message);
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return featuredList;
        }



        public List<CMSFeaturedArticlesEntity> GetMostRecentArticles(List<EnumCMSContentType> contentTypes, ushort totalRecords)
        {
            List<CMSFeaturedArticlesEntity> featuredList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmostrecentarticleslist";

                    string contentTypeList = string.Empty;

                    contentTypes.ForEach(s => contentTypeList += (int)s + ",");

                    if (contentTypeList.EndsWith(","))
                    {
                        contentTypeList = contentTypeList.Substring(0, contentTypeList.Length - 1);
                    }

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalRecords));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categorylist", DbType.String, 20, contentTypeList));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            featuredList = new List<CMSFeaturedArticlesEntity>();

                            while (dr.Read())
                            {
                                featuredList.Add(new CMSFeaturedArticlesEntity()
                                {
                                    ContentId = Convert.ToInt32(dr["BasicId"]),
                                    CategoryId = Convert.ToUInt16(dr["CategoryId"]),
                                    ContentUrl = Convert.ToString(dr["Url"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    LargePicUrl = Convert.ToString(dr["ImagePathLarge"]),
                                    SmallPicUrl = Convert.ToString(dr["ImagePathThumbnail"]),
                                    Title = Convert.ToString(dr["Title"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException exSql)
            {
                HttpContext.Current.Trace.Warn(exSql.Message);
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return featuredList;
        }
    }   // class
}   // namespace
