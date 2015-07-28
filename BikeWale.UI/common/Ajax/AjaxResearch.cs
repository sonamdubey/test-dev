using System;
using System.Web;
using AjaxPro;
using Bikewale.New;

namespace Bikewale.Ajax
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 30/8/2012
    /// Class contains ajax functions related to research section
    /// </summary>
    public class AjaxResearch
    {
        #region FetchModelImageDetails
        /// <summary>
        /// Get the list of images and the corresponding images based on car model and image category
        /// </summary>
        /// <param name="modelId">Car Model Id</param>
        /// <param name="startIndex">The first row in the list of images </param>
        /// <param name="makeName">Car Make Name</param>
        /// <param name="modelName">Car Model Name</param>
        /// <param name="mainCategory">Main Image Category</param>
        /// <returns>Html string of image list</returns>
        [AjaxPro.AjaxMethod()]
        public string FetchModelImageDetails(string modelId, int startIndex, string makeName, string modelName, int mainCategory)
        {
            ModelPhotoGallery objMPG = new ModelPhotoGallery();
            return objMPG.FetchModelImageDetails(modelId, startIndex, makeName, modelName, mainCategory);
        }
        #endregion


    }   // End of class
}   // End of namespace