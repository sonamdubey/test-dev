using System;
using System.Web;
using System.IO;
using Carwale.UI.Common;
using Carwale.Notifications;

namespace Uploadify
{
    public class Upload : IHttpHandler 
	{       
        public void ProcessRequest (HttpContext context) 
		{
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            System.Drawing.Image original_image = null;
           
            try
            {
                HttpPostedFile jpeg_image_upload = context.Request.Files["Filedata"];
				
                try
                {
                    // Retrieve the uploaded image
                    original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                }
                catch (Exception ex)
                {
					//original_image.Dispose();
					context.Response.Write("Error in uploading file");						
                	context.Response.StatusCode = 406;					
					ErrorClass objErr = new ErrorClass(ex, context.Request.ServerVariables["URL"]);
					objErr.SendMail();
					return;
                }
               
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
				
				
				// If Directory does not exists. Create it.
                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);
				
				// Get the name of orginal uploaded file
				string orginalFileName = jpeg_image_upload.FileName;
				
				// Get the index last index position of .(Dot)
				int lastDotIndex =  orginalFileName.LastIndexOf('.');
				
				// Extract file extension from orginal file
				string fileExtension = orginalFileName.Substring(lastDotIndex, orginalFileName.Length - lastDotIndex);
				
				// Generate file name using current datetime
                string fileName = DateTime.Now.ToString("yyyyMMddssfff");
				
				// Append the file extension to the generated file name.
				string savedLocation = savepath + @"\\" + fileName + fileExtension;
				
				CommonOpn op = new CommonOpn();
				//op.SendMail("satish@carwale.com", "content type", jpeg_image_upload.ContentType, true);
				
				// Save image at provided path
                jpeg_image_upload.SaveAs(savedLocation);
				original_image.Dispose();
				/***********************************************************************/
					// If user want to resize images of desired size.
					// Get all the sizes concanated with pipe(|) in scriptData
				/*************************************************************************/
				string fileSizes = context.Request["size"].ToString().ToLower();
                string inquiryId = context.Request["inquiryId"].ToString();

                if (fileSizes != "" && CommonOpn.IsNumeric(inquiryId))
				{
                    //Prepand inquiry id to the file name, Just to make it more distinguished
                    fileName = inquiryId + "_" + fileName;

                    //Each required file size is concanated with pipe.	
                    string[] arrFileSizes = null;

                    if (fileSizes.IndexOf('|') > 0)
                        arrFileSizes = fileSizes.Split('|');
                    else
                        arrFileSizes[0] = fileSizes;

                    string mailcont = arrFileSizes.Length.ToString();
                    op.SendMail("satish@carwale.com", "ScriptData", mailcont, true);

					for( int i = 0; i < arrFileSizes.Length; i++ )
					{
						string[] imgDimention = arrFileSizes[i].Split('x');
						
						// Desired file width
						int imgWidth = Convert.ToInt32(imgDimention[0]);
						
						// Desired file height
						int imgHeight = Convert.ToInt32(imgDimention[1]);
						
						//Final destination path. Where user want to save images
						string targetLocation = savepath + @"\\" + fileName + "_" + imgWidth + fileExtension;												
						
						// Save the image at desired location of desired size
						ImagingFunctions.ResizeImage(savedLocation, targetLocation, imgWidth, imgHeight);
					}

                    
				}
				
				// Delete original image as finish with the resizing
				File.Delete(savedLocation);
				
				// Return file name(might be required to map with database)
              	context.Response.Write(fileName);
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
            get{ return false;}			
        }
    }
}
