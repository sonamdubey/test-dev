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
                    var obj = connection.QueryMultiple("gethomepagebanner_20072017", param: param, commandType: CommandType.StoredProcedure);
                    objBannerVM.DesktopBannerDetails = obj.Read<BannerDetails>().FirstOrDefault();
                    objBannerVM.MobileBannerDetails = obj.Read<BannerDetails>().FirstOrDefault();

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



        public uint SaveBannerBasicDetails(DateTime startDate, DateTime endDate, string bannerDescription, uint id)
        {
            uint campaignid = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_id", id);
                    param.Add("par_startdate", startDate);
                    param.Add("par_enddate", endDate);
                    param.Add("par_bannerdescription", bannerDescription);
                    campaignid = connection.Query<uint>("savebannerbasicdetails", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BannerRepository.GetBannerDetails bannerId: {0}"));
            }
            return campaignid;

        }

        public bool SaveBannerProperties(BannerDetails objBanner, uint platformId,uint campaignId)
        {
            int success = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_horizontalposition", objBanner.HorizontalPosition);
                    param.Add("par_verticalposition", objBanner.VerticalPosition);
                    param.Add("par_backgroundcolor", objBanner.BackgroundColor);
                    param.Add("par_bannerposition", objBanner.ButtonPosition);
                    param.Add("par_bannertitle", objBanner.BannerTitle);
                    param.Add("par_buttontext", objBanner.ButtonText);
                    param.Add("par_buttontype", objBanner.ButtonType);
                    param.Add("par_buttonhref", objBanner.TargetHref);
                    param.Add("par_buttoncolor", objBanner.ButtonColor);
                    param.Add("par_target", objBanner.Target);
                    param.Add("par_jumbotrondepth", objBanner.JumbotronDepth);
                    param.Add("par_html", objBanner.HTML);
                    param.Add("par_css", objBanner.CSS);
                    param.Add("par_js", objBanner.JS);
                    param.Add("par_id", campaignId);
                    param.Add("par_platformid", platformId);
                  success= connection.Execute("savebannerdetails", param: param, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BannerRepository.GetBannerDetails bannerId: {0}"));
            }
            return success > 0;
        }
    }
}
