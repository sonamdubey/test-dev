using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using AppWebApi.Common;
using Carwale.Entity.Enum;
using Carwale.Utility;
using Carwale.Service;
using Carwale.Cache.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.BL.CMS.Photos;
using Carwale.Entity.CMS.Photos;

namespace AppWebApi.Models
{
    public class NewCarPhotos
    {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string categoryId = "-1";
        private string CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        private string ModelId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        //added by supriya on 11/6/2014
        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        public List<Photo> photos = new List<Photo>();
        public List<Item> criterias = new List<Item>();


        public NewCarPhotos(string modelId, string categoryId)
        {

            if (modelId != null && modelId != "")
                ModelId = modelId;

            if (categoryId != null && categoryId != "")
                CategoryId = categoryId;

            GetPhotosCriteria();
            GetMakeAndModelName();
            PopulateNewCarPhotos();
        }

        private void PopulateNewCarPhotos()
        {
            //string sql = " SELECT "
            //           + " ROW_NUMBER() OVER (ORDER BY DisplayDate Desc) AS Row,"
            //           + " CMa.Name AS MakeName,CMo.Name AS ModelName,CMO.MaskingName, "
            //           + " CEI.Id, CEI.BasicId, ImageCategoryId, CP.Name As CategoryName, CEI.Caption, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge, CEI.OriginalImgPath, "
            //           + "	Case CP.MainCategory"
            //           + "	When 1 Then 'Interior'"
            //           + " When 2 Then 'Exterior'"
            //           + "	End As MainCategory,"
            //           + " Case CB.CategoryId"
            //           + " When 8 Then ('Road Test: ' + CMa.Name + ' ' + CMo.Name)"
            //           + " When 1 Then CB.Title"
            //           + " When 3 Then CB.Title"
            //           + " Else CB.Title"
            //           + " End As ArticleTitle,"
            //           + " CB.Description,"
            //           + "	Case CB.CategoryId"
            //           + "	When 1 Then ('/news/' + Convert(VarChar,CB.Id) + '-' +CB.Url + '.html')"
            //           + " When 2 Then ('/research/comparos/' + CB.Url + '-' + Convert(VarChar,CB.Id) + '/')"
            //           + "	When 3 Then ('/research/' + [dbo].[ParseURL](CMa.Name) + '-cars/' + [dbo].[ParseURL](CMo.Name) +'/buying-used-' + Convert(VarChar,CB.Id) + '/')"
            //           + " When 5 Then ('/research/tipsadvice/' + CB.Url + '-' + Convert(VarChar,CB.Id) + '/')"
            //           + " When 6 Then ('/research/features/' + CB.Url + '-' + Convert(VarChar,CB.Id) + '/')"
            //           + "	When 8 Then ('/research/' + [dbo].[ParseURL](CMa.Name) + '-cars/' + [dbo].[ParseURL](CMo.Name) +'/roadtest-' + Convert(VarChar,CB.Id) + '/' )"
            //           + "	End As ArticleUrl"
            //           + "	From Con_EditCms_Basic CB WITH (NOLOCK) "
            //           + " Inner Join Con_EditCms_Images CEI WITH (NOLOCK) "
            //           + " On CEI.BasicId = CB.Id And CEI.IsActive = 1"
            //           + " Inner Join CarModels CMo WITH (NOLOCK) "
            //           + " On CMo.ID = CEI.ModelId"
            //           + " Inner Join CarMakes CMa WITH (NOLOCK) "
            //           + " On CMa.ID = CEI.MakeId"
            //           + " Inner Join Con_PhotoCategory CP WITH (NOLOCK) "
            //           + " On CP.Id = CEI.ImageCategoryId"
            //           + " Where CEI.ModelId = @ModelId And CB.CategoryId IN(8,10) And CB.IsPublished = 1 AND CB.ApplicationID=1";

            //if (CategoryId != "-1")
            //{
            //    sql += " AND CP.MainCategory=@CategoryId";
            //}

            ////SqlCommand cmd = new SqlCommand(sql);
            ////cmd.Parameters.Add("@ModelId", SqlDbType.VarChar).Value = ModelId;
            ////cmd.Parameters.Add("@CategoryId", SqlDbType.VarChar).Value = CategoryId;
            //SqlParameter[] param = { new SqlParameter("@ModelId", ModelId), new SqlParameter("@CategoryId", CategoryId) };
            //Database db = new Database();
            //SqlDataReader dr = null;

            try
            {
                IPhotos _carPhotosBL = UnityBootstrapper.Resolve<CMSPhotosBL>();
                int modelId,categoryId;
                int.TryParse(ModelId, out modelId);
                int.TryParse(CategoryId, out categoryId);
                var photosURI = new ModelPhotoURI()
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    CategoryIdList = ((ushort)CMSContentType.Images).ToString()+","+((ushort)CMSContentType.RoadTest).ToString(),
                    ModelId = modelId,
                };
                List<ModelImage> images = new List<ModelImage>();
                images= _carPhotosBL.GetNewCarPhotos(photosURI);
                foreach(var image in images)
                {
                    if (categoryId == -1 || image.MainImgCategoryId == categoryId)
                    {
                        Photo photo = new Photo();
                        photo.SmallPicUrl = ImageSizes.CreateImageUrl(image.HostUrl, ImageSizes._160X89, image.OriginalImgPath);
                        photo.LargePicUrl = ImageSizes.CreateImageUrl(image.HostUrl, ImageSizes._600X337, image.OriginalImgPath);
                        string MainCategory = image.MainImgCategoryId == 1 ? ImageTypes.Interior.ToString() : ImageTypes.Exterior.ToString();
                        photo.Category = MainCategory;
                        string captionName = image.MakeBase.MakeName + " " + image.ModelBase.ModelName;
                        if (!string.IsNullOrEmpty(image.ImageCategory))
                            captionName = captionName + " - " + image.ImageCategory;
                        if (!string.IsNullOrEmpty(MainCategory))
                            captionName = captionName + " (" + MainCategory + ")";
                        if (CategoryId != "-1")
                            ShareUrl = "https://www.carwale.com/" + CommonOpn.FormatSpecial(image.MakeBase.MakeName) + "-cars/" + image.ModelBase.MaskingName + "/" + MainCategory + "-photos/";
                        else
                            ShareUrl = "https://www.carwale.com/" + CommonOpn.FormatSpecial(image.MakeBase.MakeName) + "-cars/" + image.ModelBase.MaskingName + "/photos/";

                        photo.CaptionName = captionName;
                        photo.HostUrl = image.HostUrl;
                        photo.OriginalImgPath = image.OriginalImgPath;
                        photos.Add(photo);
                    }
                    }

                //dr = db.SelectQry(sql, param);

                //Photo photo;
                //while (dr.Read())
                //{
                //    photo = new Photo();
                //    photo.SmallPicUrl = ImageSizes.CreateImageUrl(dr["HostURL"].ToString(), ImageSizes._160X89, dr["OriginalImgPath"].ToString());
                //    photo.LargePicUrl = ImageSizes.CreateImageUrl(dr["HostURL"].ToString(), ImageSizes._600X337, dr["OriginalImgPath"].ToString());
                //    photo.Category = dr["MainCategory"].ToString(); //assign based on MainCategoryId
                //    //MakeName + ModelName + (DataBinder.Eval(Container.DataItem, "CategoryName").ToString() != string.Empty ? (" - " + DataBinder.Eval(Container.DataItem, "CategoryName").ToString()) : "") + (DataBinder.Eval(Container.DataItem, "MainCategory").ToString() != string.Empty ? (" (" + DataBinder.Eval(Container.DataItem, "MainCategory").ToString() + ") ") : "")  %>'
                //    string captionName = dr["MakeName"].ToString() + " " + dr["ModelName"].ToString();
                //    if (dr["CategoryName"].ToString() != null && dr["CategoryName"].ToString() != "") //ImageCategory
                //        captionName = captionName + " - " + dr["CategoryName"].ToString();

                //    if (dr["MainCategory"].ToString() != null && dr["MainCategory"].ToString() != "")
                //        captionName = captionName + " (" + dr["MainCategory"].ToString() + ")";

                //    if (CategoryId != "-1")
                //        ShareUrl = "https://www.carwale.com/" + CommonOpn.FormatSpecial(dr["MakeName"].ToString()) + "-cars/" + dr["MaskingName"].ToString() + "/" + dr["MainCategory"].ToString() + "-photos/";
                //    else
                //        ShareUrl = "https://www.carwale.com/" + CommonOpn.FormatSpecial(dr["MakeName"].ToString()) + "-cars/" + dr["MaskingName"].ToString() + "/photos/";

                //    photo.CaptionName = captionName;
                //    photo.HostUrl = dr["HostURL"].ToString();
                //    photo.OriginalImgPath = dr["OriginalImgPath"].ToString();
                //    photos.Add(photo);
                //}
            }
            catch (Exception err)
            {
                ServerErrorOccured = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void GetMakeAndModelName()
        {
            CarModelsCacheRepository modelCacheRepo = UnityBootstrapper.Resolve<CarModelsCacheRepository>();
            CarModelDetails modelInfo = new CarModelDetails();
            modelInfo = modelCacheRepo.GetModelDetailsById(Convert.ToInt32(ModelId));
            if(modelInfo != null)
            {
                MakeName = modelInfo.MakeName;
                ModelName = modelInfo.ModelName;
            }

            //string sql = "SELECT Mk.ID AS MakeId, Mk.Name AS MakeName, Mo.ID AS ModelId, Mo.Name AS ModelName, Mo.LargePic, Mo.SmallPic, Mo.HostURL,"
            //           + " Looks, Performance, Comfort, ValueForMoney, FuelEconomy, ReviewRate, ReviewCount, Mo.Futuristic,Mo.MinPrice "
            //           + " FROM CarModels Mo WITH(NOLOCK), CarMakes Mk  WITH(NOLOCK)"
            //           + " WHERE Mo.CarMakeId = Mk.Id AND Mo.ID =@ModelId";

            ////SqlCommand cmd = new SqlCommand(sql);
            ////cmd.Parameters.Add("@ModelId", SqlDbType.VarChar).Value = ModelId;
            //SqlParameter[] param = { new SqlParameter("@ModelId", ModelId) };
            //Database db = new Database();
            //SqlDataReader dr = null;

            //try
            //{
            //    //dr = db.SelectQry(sql, param);

            //    //if (dr.Read())
            //    //{
            //    //    MakeName = dr["MakeName"].ToString();
            //    //    ModelName = dr["ModelName"].ToString();
            //    //}
            //}
            //catch (Exception err)
            //{
            //    ServerErrorOccured = true;
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
        }

        private void GetPhotosCriteria()
        {
            Item item;

            item = new Item();
            item.Id = "-1";
            item.Name = "All Pictures";
            item.Url = CommonOpn.ApiHostUrl + "NewCarPhotos/?modelId=" + ModelId + "&categoryId=-1";
            criterias.Add(item);

            item = new Item();
            item.Id = "1";
            item.Name = "Interior";
            item.Url = CommonOpn.ApiHostUrl + "NewCarPhotos/?modelId=" + ModelId + "&categoryId=1";
            criterias.Add(item);

            item = new Item();
            item.Id = "2";
            item.Name = "Exterior";
            item.Url = CommonOpn.ApiHostUrl + "NewCarPhotos/?modelId=" + ModelId + "&categoryId=2";
            criterias.Add(item);
        }
    }
}