using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Models;
using Dapper;
using System;
using System.Data;
using System.Linq;


namespace BikewaleOpr.DALs.Banner
{
    public class BannerRepository : IBannerRepository
    {
        public BannerVM GetBannerDetails(uint bannerId)
        {
            BannerVM objBannerVM = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_id", bannerId);
                    objBannerVM = new BannerVM();
                    var obj= connection.QueryMultiple("gethomepagebanner", param: param, commandType: CommandType.StoredProcedure);
                    objBannerVM.DesktopBannerDetails = obj.Read<BannerDetails>().FirstOrDefault();
                    objBannerVM.MobileBannerDetails=obj.Read<BannerDetails>().FirstOrDefault();

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BannerRepository.GetBannerDetails bannerId: {0}", bannerId));
            }

            return objBannerVM;

        }

    }
}
