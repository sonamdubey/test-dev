using System;
using System.Web;
using System.Configuration;
using System.IO;
using Carwale.UI.Common;
using System.Collections.Specialized;
using RabbitMqPublishing;
using Carwale.Notifications;
using Carwale.DAL.Classified.SellCar;
using Carwale.Entity.Classified;
using Carwale.DAL.Classified.UsedCarPhotos;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.BL.Classified.UsedSellCar;
using Carwale.Entity.Enum;
using Carwale.Service;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.Classified.UsedCarPhotos;

namespace Carwale.UI.Used.SellCar
{
    public class UploadHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            System.Drawing.Image original_image = null;

            ICarPhotosRepository carPhotoRepo = UnityBootstrapper.Resolve<ICarPhotosRepository>();
            CustomerSellInquiryVehicleData vehicleData = new CustomerSellInquiryVehicleData();
            ISellCarRepository sellCarRepo = UnityBootstrapper.Resolve<ISellCarRepository>();
            ISellCarBL sellCarBL = UnityBootstrapper.Resolve<ISellCarBL>();

            try
            {
                HttpPostedFile jpeg_image_upload = context.Request.Files["Filedata"];

                // Check if we can make a image object from InputStream. if not its not a image file. Hence exception will occure.
                // if exception occures, stop any forther operations as its sure that uploaded file is not image file.
                try
                {
                    // Retrieve the uploaded image
                    original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                }
                catch (Exception ex)
                {
                    original_image.Dispose();
                    context.Response.Write("Error in uploading file");
                    context.Response.StatusCode = 406;
                    ErrorClass objErr = new ErrorClass(ex, context.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                    return;
                }

                // dispose the image if no excpetion comes
                original_image.Dispose();

                string savepath = "";
                string urlpath = "~/UploadedFiles/";

                //CommonOpn op = new CommonOpn();
                //op.SendMail("satish@carwale.com", "ScriptData", .ToString(), true);

                if (context.Request["folder"] != null)
                {
                    // Path where user want to save images
                    string tempPath = context.Server.UrlDecode(context.Request["folder"]);

                    urlpath = tempPath;

                    if (!tempPath.StartsWith("~"))
                        tempPath = "~" + tempPath;

                    if (!tempPath.EndsWith("/"))
                        tempPath += "/";

                    savepath = context.Server.MapPath(tempPath);
                }
                else
                    savepath = context.Server.MapPath(urlpath);

                string fileSizes = context.Request["size"].ToString().ToLower();
                string inquiryId = context.Request["inquiryId"].ToString();
                bool isDealer = context.Request["isDealer"].ToString() == "0" ? false : true;
                string profileId = (isDealer == true ? "D" : "S") + inquiryId;

                // convert to small case
                savepath = savepath.ToLower();

                string commonPath = CommonOpn.ImagePath;
                string realAbsolutePath = ConfigurationManager.AppSettings["CarwaleImgAbsolutePath"].ToString();
                HttpContext.Current.Trace.Warn("commonpath " + commonPath);
                string FolderPath = realAbsolutePath + @"\cw\ucp\" + profileId + @"\";

                HttpContext.Current.Trace.Warn("savePath : " + commonPath);
                HttpContext.Current.Trace.Warn("realAbsolutePath : " + realAbsolutePath);
                HttpContext.Current.Trace.Warn("folerPath : " + FolderPath);

                // If Directory does not exists. Create it.
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                // Get the name of orginal uploaded file
                string orginalFileName = jpeg_image_upload.FileName;

                // Get the index last index position of .(Dot)
                int lastDotIndex = orginalFileName.LastIndexOf('.');

                // Extract file extension from orginal file
                string fileExtension = orginalFileName.Substring(lastDotIndex, orginalFileName.Length - lastDotIndex);

                // Generate file name using current datetime
                // string fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                if (fileSizes != "" && CommonOpn.IsNumeric(inquiryId))
                {
                    
                    vehicleData = sellCarRepo.GetCustomerSellInquiryVehicleDetails(Convert.ToInt32(inquiryId));
                    string fileName = vehicleData.MakeYear.ToString("yyyy") + "-" + UrlRewrite.FormatSpecial(vehicleData.MakeName) + "-" + UrlRewrite.FormatSpecial(vehicleData.ModelName) + "-" + UrlRewrite.FormatSpecial(vehicleData.VersionName);

                    // Commented by Vikas. Filename changed to [makeyear-carname-photoId-size.extn] for seo purpose.
                    //string fileName = sellObjInq.MakeYear + "-" + UrlRewrite.FormatSpecial(sellObjInq.CarMake) + "-" + UrlRewrite.FormatSpecial(sellObjInq.CarModel) + "-" + UrlRewrite.FormatSpecial(sellObjInq.CarVersion);

                    //***********************************************************************
                    //* If user want to resize images of desired size.
                    //* Get all the sizes concanated with pipe(|) in scriptData
                    //*************************************************************************			
                    int photoId = -1;
                    string imageLarge = "", imageMedium = "", imageThumb = "";
                    //Each required file size is concanated with pipe.	
                    string[] arrFileSizes = null;

                    if (fileSizes.IndexOf('|') > 0)
                        arrFileSizes = fileSizes.Split('|');
                    else
                        arrFileSizes[0] = fileSizes;

                    imageLarge = /*fileName + */"-" + arrFileSizes[0] + fileExtension;
                    imageMedium = /*fileName + */"-" + arrFileSizes[1] + fileExtension;
                    imageThumb = /*fileName + */"-" + arrFileSizes[2] + fileExtension;
                    // Save data to table CarPhotos
                    string imgDesc = "";
                    bool isMain = false;
                    photoId = carPhotoRepo.SaveCarPhotos(new CarPhoto()
                    {
                        InquiryId = Convert.ToInt32(inquiryId),
                        ImageUrlFull = imageLarge,
                        ImageUrlThumb = imageMedium,
                        ImageUrlThumbSmall = imageThumb,
                        Description = imgDesc,
                        IsDealer = isDealer,
                        IsMain = isMain,
                        HostUrl = ConfigurationManager.AppSettings["CDNHostURL"].ToString(),
                        IsReplicated = 0,
                        FileName = fileName
                    });

                    // Append the file extension to the generated file name.
                    string savedLocation = FolderPath + @"\\" + fileName + "-" + photoId + fileExtension;


                    // Save image at provided path
                    jpeg_image_upload.SaveAs(savedLocation);


                    string dirPath = @"/cw/ucp/" + profileId + @"/";

                    //image queue in RabbitMq
                    if (ConfigurationManager.AppSettings["RabbitMQImage"].ToLower() == "true")
                    {
                        string hostUrl = "http://"+Utility.Network.GetMachineIP()+":"+System.Configuration.ConfigurationManager.AppSettings["localimgsiteport"];
                        string fullFileName = fileName + "-" + photoId.ToString() + fileExtension;
                        string imageUrl = String.Format("{0}{1}{2}", hostUrl, dirPath, fullFileName);
                        //string imageUrl = CommonRQ.UploadImageToCommonDatabase(photoId.ToString(), fileName + "-" + photoId.ToString() + fileExtension, ImageCategories.USEDSELLCARS, dirPath);
                        HttpContext.Current.Trace.Warn("imageurl " + imageUrl);
                        RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                        NameValueCollection nvc = new NameValueCollection();
                        //add items to nvc
                        //photoid ,originalimageurl,imagetargetpath
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.ID).ToLower(), photoId.ToString());
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "UsedSellCars");
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), Convert.ToString(1));
                        nvc.Add(CommonRQ.GetDescription(ImageKeys.ASPECTRATIO).ToLower(), "1.777");
                        //rabbitmqPublish.PublishToQueue("Image", nvc);
                        rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["IPCQueueNameMysql"].ToString(), nvc);
                    }
                    else
                    {
                        for (int i = 0; i < arrFileSizes.Length; i++)
                        {
                            string[] imgDimention = arrFileSizes[i].Split('x');

                            // Desired file width
                            int imgWidth = Convert.ToInt32(imgDimention[0]);

                            // Desired file height
                            int imgHeight = Convert.ToInt32(imgDimention[1]);

                            //Final destination path. Where user want to save images
                            string targetLocation = FolderPath + @"\\" + fileName + "-" + photoId + "-" + arrFileSizes[i] + fileExtension;

                            // Save the image at desired location of desired size
                            ImagingFunctions.ResizeImage(savedLocation, targetLocation, imgWidth, imgHeight);
                            File.Delete(savedLocation);
                            // Delete original image as finish with the resizing
                        }
                    }

                }

                bool updateForcefully = false;

                sellCarBL.UpdateSellCarCurrentStep(Convert.ToInt32(inquiryId), (int)Carwale.Entity.Classified.SellCarUsed.SellCarSteps.CarCondition, updateForcefully);

                // Return file name(might be required to map with database)
                context.Response.Write(savepath);
                context.Response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, context.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
