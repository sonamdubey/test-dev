using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using Carwale.DAL.Classified.SellCar;
using Carwale.UI.Common;

namespace CarwaleAjax
{
    /// <summary>
    /// Created By : Ashish G. kamble on 20 Nov 2013
    /// Summary : Class have ajax methods for mycarwale section.
    /// </summary>
    public class AjaxMyCarwale
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string UpdateCarIsArchived(int inquiryId)
        {
            SellCarRepository sellCarRepo = new SellCarRepository();
            if(sellCarRepo.IsCustomerAuthorizedToManageCar(Convert.ToInt32(CurrentUser.Id), inquiryId))
                sellCarRepo.UpdateCarIsArchived(inquiryId);
            return "0";
        }
    }
}