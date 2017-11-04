using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Text;
using Bikewale.Common;

/// <summary>
/// This Class contains methods to fetch the model photos from all the content
/// </summary>
namespace Bikewale.New
{
    public class ModelPhotoGallery
    {
        #region Properties
        public string FromClause
        {
            get { return "Con_EditCms_Basic CB Inner Join Con_EditCms_Images CEI On CEI.BasicId = CB.Id And CEI.IsActive = 1 Inner Join BikeModels CMo On CMo.ID = CEI.ModelId Inner Join BikeMakes CMa On CMa.ID = CEI.MakeId Inner Join Con_PhotoCategory CP On CP.Id = CEI.ImageCategoryId "; }
        }

        public string WhereClause
        {
            get { return "CEI.ModelId = @ModelId And CB.CategoryId = 8 And CB.IsPublished = 1 ";}
        }
        #endregion

        #region FetchModelImageDetails
        /// <summary>
        /// Get the list of images and the corresponding images based on Bike model and image category
        /// </summary>
        /// <param name="modelId">Bike Model Id</param>
        /// <param name="startIndex">The first row in the list of images </param>
        /// <param name="makeName">Bike Make Name</param>
        /// <param name="modelName">Bike Model Name</param>
        /// <param name="mainCategory">Main Image Category</param>
        /// <returns>Html string of image list</returns>
        public string FetchModelImageDetails(string modelId, int startIndex, string makeName, string modelName, int mainCategory)
        {
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //StringBuilder sbImageDetails = new StringBuilder();
            //SqlCommand cmd = new SqlCommand();
            //DataSet ds = new DataSet();            

            //string sql = string.Empty, selectClause = string.Empty, fromClause = string.Empty, whereClause = string.Empty, imageCount = string.Empty;

            //selectClause = "CEI.Id, CEI.BasicId, ImageCategoryId, CP.Name As CategoryName, CEI.Caption, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge, CEI.OriginalImagePath,Case CP.MainCategory When 1 Then 'Interior' When 2 Then 'Exterior' End As MainCategory, "
            //    + " Case CB.CategoryId When 8 Then ('Road Test: ' + CMa.Name + ' ' + CMo.Name) When 1 Then CB.Title When 3 Then CB.Title Else CB.Title End As ArticleTitle, CB.Description, "
            //    + " Case CB.CategoryId When 1 Then ('/news/' + Convert(VarChar,CB.Id) + '-' +CB.Url + '.html') When 2 Then ('/research/comparos/' + CB.Url + '-' + Convert(VarChar,CB.Id) + '/') When 3 Then ('/research/' + [dbo].[ParseURL](CMa.Name) + '-bikes/' + [dbo].[ParseURL](CMo.Name) +'/buying-used-' + Convert(VarChar,CB.Id) + '/') When 5 Then ('/research/tipsadvice/' + CB.Url + '-' + Convert(VarChar,CB.Id) + '/') When 6 Then ('/research/features/' + CB.Url + '-' + Convert(VarChar,CB.Id) + '/') When 8 Then ('/research/' + [dbo].[ParseURL](CMa.Name) + '-bikes/' + [dbo].[ParseURL](CMo.Name) +'/roadtest-' + Convert(VarChar,CB.Id) + '/' )  End As ArticleUrl  ";

            //fromClause = this.FromClause;

            //whereClause = this.WhereClause; // 2= Comparison Test, 3 = Buying Used : Temporarily excluded from Image Gallery : Vikas-05/06/2012

            //if (mainCategory != 0)
            //{
            //    whereClause += " And CP.MainCategory = @MainCategory ";
            //}

            //sql = " Select * From (Select DENSE_RANK() Over (Order By Id Desc) AS RowN, * FROM ( SELECT " + selectClause + " From " + fromClause + " Where " + whereClause + " )AS tbl ) AS TopRecords Where "
            //    + " RowN >= " + (startIndex + 1).ToString() + " AND RowN <= " + (startIndex + 5).ToString() + " ";


            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;
            //if (mainCategory != 0)
            //{
            //    cmd.Parameters.Add("@MainCategory", SqlDbType.TinyInt).Value = mainCategory;
            //}
            //imageCount = GetTotalImageCount(modelId, mainCategory);
            //try
            //{
            //    ds = db.SelectAdaptQry(cmd);

            //    string largeImage = string.Empty, thumbImage = string.Empty, imageTitle = string.Empty, imageAltText = string.Empty, articleId = string.Empty,
            //        articleTitle = string.Empty, articleUrl = string.Empty;

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {                        
            //            //largeImage = "http://" + row["HostURL"].ToString() + row["ImagePathLarge"].ToString();
            //            //thumbImage = "http://" + row["HostURL"].ToString() + row["ImagePathThumbnail"].ToString();
            //            largeImage = Bikewale.Utility.Image.GetPathToShowImages(row["OriginalImagePath"].ToString(), row["HostURL"].ToString(), Bikewale.Utility.ImageSize._210x118);
            //            thumbImage = Bikewale.Utility.Image.GetPathToShowImages(row["OriginalImagePath"].ToString(), row["HostURL"].ToString(), Bikewale.Utility.ImageSize._110x61);
            //            imageTitle = makeName + " " + modelName + (row["CategoryName"].ToString() != string.Empty ? (" - " + row["CategoryName"].ToString()) : "") + (row["MainCategory"].ToString() != string.Empty ? (" (" + row["MainCategory"].ToString() + ") ") : "");
            //            imageAltText = row["Caption"].ToString();
            //            articleId = row["BasicId"].ToString();
            //            articleTitle = HttpUtility.HtmlEncode(row["ArticleTitle"].ToString().Replace("'", "&rsquo;"));
            //            articleUrl = row["ArticleUrl"].ToString();


            //            sbImageDetails.Append("<li>");
            //            sbImageDetails.AppendFormat("<a href='{0}'>", largeImage);
            //            sbImageDetails.AppendFormat("<img src='{0}' border='0' style='height:70px;' title='{1}' alt='{1}' desc='{2}' artID='{3}' artTitle='{4}' artUrl='{5}' imgCnt='{6}'/>",
            //                thumbImage, imageTitle, imageAltText, articleId, articleTitle, articleUrl, imageCount);
            //            sbImageDetails.Append("</a>");
            //            sbImageDetails.Append("</li>");
            //        }
            //    }
            //    else
            //    {
            //        sbImageDetails.Append("<li>");
            //        sbImageDetails.AppendFormat("<a href='{0}'>", "http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/no-img-big.png");
            //        sbImageDetails.AppendFormat("<img src='{0}' border='0' style='height:70px;' title='{1}' alt='{1}' desc='{2}' artID='{3}' artTitle='{4}' artUrl='{5}' imgCnt='{6}'/>",
            //            "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/no-img-thumb.png", "No Images Available", "No Images Available", "0", "-", "-", "0");
            //        sbImageDetails.Append("</a>");
            //        sbImageDetails.Append("</li>");
            //    }
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception err)
            //{
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //    ds.Dispose();
            //    cmd.Dispose();
            //}
            //return sbImageDetails.ToString();
        }
        #endregion

        #region GetTotalImageCount
        /// <summary>
        /// PopulateWhere to get the count of all images for a model based on conditions.
        /// </summary>       
        /// <param name="modelId">Bike Model Id</param>
        /// <param name="mainCategory">Main Image Cateogry</param>
        /// <returns>Image Count</returns>
        public string GetTotalImageCount(string modelId, int mainCategory)
        {
            throw new Exception("Method not used/commented");

            //string sql = string.Empty, imageCount = string.Empty;
            //SqlDataReader dr = null;
            //Database db = new Database();
            //SqlCommand cmd = new SqlCommand();

            //sql = " Select Count(*) As TotalImageCount From " + this.FromClause + " Where " + this.WhereClause;

            //if (mainCategory != 0)
            //{
            //    sql += " And CP.MainCategory = @MainCategory ";
            //}

            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;
            //if (mainCategory != 0)
            //{
            //    cmd.Parameters.Add("@MainCategory", SqlDbType.TinyInt).Value = mainCategory;
            //}

            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    while (dr.Read())
            //    {
            //        imageCount = dr["TotalImageCount"].ToString();
            //    }
            //}
            //catch (SqlException ex)
            //{

            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"].ToString());
            //    
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"].ToString());
            //    
            //}
            //finally
            //{
            //    //Dispose of all the objects and Close connection objects.
            //    if (dr != null)
            //    {
            //        dr.Close();
            //    }
            //    cmd.Dispose();
            //    db.CloseConnection();
            //}
            //return imageCount;
        }
        #endregion
    }
}