using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Carwale.UI.Common;
using Ajax;
using Carwale.UI.Used;
using Carwale.DAL.Classified.UsedCarPhotos;
using Carwale.DAL.Classified.SellCar;

namespace CarwaleAjax
{
    public class AjaxSellCar
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool RemoveCarPhotos(string inquiryId, string photoId)
        {
            bool isRemoved = false;

            if (CommonOpn.IsNumeric(inquiryId) && CommonOpn.IsNumeric(photoId))
            {
                CarPhotosRepository carPhotoRepo = new CarPhotosRepository();
                isRemoved = carPhotoRepo.RemoveCarPhoto(Convert.ToInt32(inquiryId), Convert.ToInt32(photoId));
            }

            return isRemoved;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool MakeMainImage(string inquiryId, string photoId)
        {
            bool isDone = false;


            if (CommonOpn.IsNumeric(inquiryId) && CommonOpn.IsNumeric(photoId))
            {
                bool _IsDealer = CurrentUser.Role == "DEALER" ? true : false;

                CarPhotosRepository carPhotoRepo = new CarPhotosRepository();
                isDone = carPhotoRepo.UpdateMainImage(Convert.ToInt32(inquiryId), Convert.ToInt32(photoId), _IsDealer);
            }

            return isDone;
        }
        
    }
}