using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using Carwale.Entity.CMS;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.Utility;
using Carwale.Notifications.Logs;
using Dapper;
using Carwale.Service;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.Articles;
using Carwale.BL.GrpcFiles;
using System.Linq;
using Grpc.CMS;
using Carwale.UI.PresentationLogic;
using Carwale.Entity.Geolocation;
using Carwale.BL.CMS;

namespace Carwale.UI.Editorial
{

    public class Sitemap : System.Web.UI.Page
    {
        private static readonly string mydomain = "https://www.carwale.com";
        private static readonly string _cdnHostUrl = System.Configuration.ConfigurationManager.AppSettings["CDNHostURL"];
        private static readonly int _marutiMakeId = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            XmlTextWriter writer = new XmlTextWriter(Server.MapPath("google-news-sitemap\\google-news-sitemap.xml"), Encoding.UTF8);
            try
            {
                List<ArticleSummary> articleDetails = new List<ArticleSummary>();
                ICMSContent cmsrepo = UnityBootstrapper.Resolve<ICMSContent>();
                articleDetails = cmsrepo.GoogleSiteMapDetails((int)CMSAppId.Carwale);
                // Creating the SiteMap XML using XMLTextWriter
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "news", null, "http://www.google.com/schemas/sitemap-news/0.9");
                writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                if (articleDetails.Count > 0)
                {
                    int i = 0;
                    while (i < articleDetails.Count)
                    {
                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", mydomain + articleDetails[i].ArticleUrl.ToString());
                        writer.WriteStartElement("news:news");
                        writer.WriteStartElement("news:publication");
                        writer.WriteElementString("news:name", "CarWale");
                        writer.WriteElementString("news:language", "en");
                        writer.WriteEndElement();
                        writer.WriteElementString("news:genres", "PressRelease, Blog");
                        writer.WriteElementString("news:geo_locations", "India");
                        writer.WriteElementString("news:publication_date", Convert.ToDateTime(articleDetails[i].DisplayDate).ToString("yyyy-MM-ddThh:mm:sszzz"));
                        writer.WriteElementString("news:keywords", articleDetails[i].Tags.ToString());
                        writer.WriteElementString("news:title", articleDetails[i].Title.ToString());
                        writer.WriteEndElement();
                        if (!String.IsNullOrEmpty(articleDetails[i].HostUrl.ToString()))
                        {
                            writer.WriteStartElement("image:image");
                            writer.WriteElementString("image:loc", articleDetails[i].HostUrl + ImageSizes._0X0 + (articleDetails[i].OriginalImgUrl ?? string.Empty).Split(new string[] { "&q", "?q" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault());
                            writer.WriteElementString("image:title", articleDetails[i].Title.ToString());
                            writer.WriteElementString("image:caption", articleDetails[i].Title.ToString());
                            writer.WriteElementString("image:geo_location", "India");
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        i++;
                    }
                }
                writer.WriteEndDocument();

                ICarModelCacheRepository modelCacheRepo = UnityBootstrapper.Resolve<ICarModelCacheRepository>();
                List<CarMakeModelEntityBase> modelsInfo = modelCacheRepo.GetAllModels("new");
                if ((Request.QueryString["image"] ?? string.Empty).Equals("true"))
                {
                    CreateImageSiteMaps(modelsInfo);
                }
                if ((Request.QueryString["comparecar"] ?? string.Empty).Equals("true"))
                {
                    var orderedList = modelsInfo.OrderByDescending(y => y.ModelId).ToList();
                    CreateCompareCarSiteMap(orderedList);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                writer.Close();

            }
        }
        private void CreateImageSiteMaps(List<CarMakeModelEntityBase> modelsInfo)
        {
            XmlTextWriter ImageWriter = new XmlTextWriter(Server.MapPath("image-sitemap\\image-sitemap.xml"), System.Text.Encoding.UTF8);
            InitSiteMap(ImageWriter);
            ImageWriter.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
            foreach (var model in modelsInfo)
            {
                var modelImages = GetModelImages(model.ModelId);
                foreach (var image in modelImages)
                {
                    CreateImageNode(ImageWriter, image);
                }
            }

            EndSiteMap(ImageWriter);
        }
        private void CreateCompareCarSiteMap(List<CarMakeModelEntityBase> modelList)
        {
            int count = modelList.Count;
            foreach (var basemodel in modelList)
            {
                count--;
                if (count >= 1)
                {
                    XmlTextWriter CompareCarWriter = new XmlTextWriter(Server.MapPath("image-sitemap\\comparecar-" + Format.FormatSpecial(basemodel.ModelName) + ".xml"), System.Text.Encoding.UTF8);
                    InitSiteMap(CompareCarWriter);
                    CreateCompareCarNode(CompareCarWriter, basemodel, modelList);
                    EndSiteMap(CompareCarWriter);
                }
            }
        }
        private void CreateCompareCarNode(XmlTextWriter writer, CarMakeModelEntityBase basemodel, List<CarMakeModelEntityBase> models)
        {
            try
            {
                foreach (var targetmodel in models)
                {
                    if (basemodel.ModelId > targetmodel.ModelId)
                    {
                        writer.WriteStartElement("url");
                        string CompareCarUrl = string.Format("{0}/comparecars/{1}-{2}-vs-{3}-{4}/", mydomain, Format.FormatSpecial(basemodel.MakeName), basemodel.MaskingName, Format.FormatSpecial(targetmodel.MakeName), targetmodel.MaskingName);
                        writer.WriteElementString("loc", CompareCarUrl);
                        writer.WriteEndElement();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void InitSiteMap(XmlTextWriter writer)
        {
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
        }

        private void EndSiteMap(XmlTextWriter writer)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        private void CreateImageNode(XmlTextWriter writer, ModelImage image)
        {
            try
            {
                var url = CMSCommon.GetImageUrl(image.MakeBase.MakeName, image.ModelBase.MaskingName, image.ImageName, image.ImageId);
                writer.WriteStartElement("url");
                string ImageLocation = string.Format("{0}{1}", mydomain, url);
                writer.WriteElementString("loc", ImageLocation);
                string imagePath = _cdnHostUrl + "600x337" + image.OriginalImgPath;

                string makeName = image.MakeBase.MakeId == _marutiMakeId ? string.Empty : string.Format("{0} ", image.MakeBase.MakeName);
                string caption = string.Format("{0}{1} {2}", makeName, image.ModelBase.ModelName, image.ImageCategory);
                WriteImageNode(writer, imagePath, caption);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            writer.WriteEndElement();
        }

        private void WriteImageNode(XmlTextWriter writer, string imagePath, string caption)
        {
            writer.WriteStartElement("image", "image", "http://www.google.com/schemas/sitemap-image/1.1");
            try
            {
                writer.WriteElementString("image", "loc", "http://www.google.com/schemas/sitemap-image/1.1", imagePath);
                writer.WriteElementString("image", "title", "http://www.google.com/schemas/sitemap-image/1.1", caption);
                writer.WriteElementString("image", "caption", "http://www.google.com/schemas/sitemap-image/1.1", caption);
                writer.WriteElementString("image", "geo_location", "http://www.google.com/schemas/sitemap-image/1.1", "India");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            writer.WriteEndElement();
        }

        private List<ModelImage> GetModelImages(int modelId)
        {
            try
            {
                IPhotos _photorepo = UnityBootstrapper.Resolve<IPhotos>();

                return _photorepo.GetModelPhotosByCount(new Entity.CMS.URIs.ModelPhotosBycountURI()
                {
                    ApplicationId = (ushort)Carwale.Entity.Enum.Application.CarWale,
                    CategoryIdList = string.Format("{0},{1}", (int)CMSContentType.RoadTest, (int)CMSContentType.Images),
                    ModelId = modelId
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

    }//class
}//namespace
