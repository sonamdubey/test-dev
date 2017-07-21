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

        public bool SaveBanner(BannerVM objbanner)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_mobilehtml", objbanner.MobileBannerDetails.HTML);
                    param.Add("par_desktophtml", objbanner.DesktopBannerDetails.HTML);
                    param.Add("par_mobilecss", objbanner.MobileBannerDetails.CSS);
                    param.Add("par_desktopcss", objbanner.DesktopBannerDetails.CSS);
                    param.Add("par_mobilejs", objbanner.MobileBannerDetails.JS);
                    param.Add("par_desktopjs", objbanner.DesktopBannerDetails.JS);
                    param.Add("par_horiizontalpositiondesktop", objbanner.DesktopBannerDetails.HorizontalPosition);
                    param.Add("par_verticalpositiondesktop", objbanner.DesktopBannerDetails.VerticalPosition);
                    param.Add("par_backgroundcolordesktop", objbanner.DesktopBannerDetails.BackgroundColor);
                    param.Add("par_bannerpositiondesktop", objbanner.DesktopBannerDetails.ButtonPosition);
                    param.Add("par_bannertitledesktop", objbanner.DesktopBannerDetails.BannerTitle);
                    param.Add("par_buttontextdesktop", objbanner.DesktopBannerDetails.ButtonText);
                    param.Add("par_hosturldektop", objbanner.DesktopBannerDetails.HostUrl);
                    param.Add("par_buttontypedesktop", objbanner.DesktopBannerDetails.ButtonType);
                    param.Add("par_targethrefdesktop", objbanner.DesktopBannerDetails.TargetHref);
                    param.Add("par_buttoncolordesktop", objbanner.DesktopBannerDetails.ButtonColor);
                    param.Add("par_jumbotrondepthdesktop", objbanner.DesktopBannerDetails.JumbotronDepth);
                    param.Add("par_horizontalpositionmobile", objbanner.MobileBannerDetails.HorizontalPosition);
                    param.Add("par_verticalpositionmobile", objbanner.MobileBannerDetails.VerticalPosition);
                    param.Add("par_backgroundcolormobile", objbanner.MobileBannerDetails.BackgroundColor);
                    param.Add("par_bannerpositionmobile", objbanner.MobileBannerDetails.ButtonPosition);
                    param.Add("par_bannertitlemobile", objbanner.MobileBannerDetails.BannerTitle);
                    param.Add("par_buttontextmobile", objbanner.MobileBannerDetails.ButtonText);
                    param.Add("par_hosturlmobile", objbanner.MobileBannerDetails.HostUrl);
                    param.Add("par_buttontypemobile", objbanner.MobileBannerDetails.ButtonType);
                    param.Add("par_targethrefmobile", objbanner.MobileBannerDetails.TargetHref);
                    param.Add("par_buttoncolormobile", objbanner.MobileBannerDetails.ButtonColor);
                    param.Add("par_jumbotrondepthmobile", objbanner.MobileBannerDetails.JumbotronDepth);
                    param.Add("par_originalimagepathdesktop", objbanner.DesktopBannerDetails.OriginalImagePath);
                    param.Add("par_originalimagepathmobile", objbanner.MobileBannerDetails.OriginalImagePath);
                    param.Add("par_targetdesktop", objbanner.DesktopBannerDetails.Target);
                    param.Add("par_targetmobile", objbanner.MobileBannerDetails.Target);
                }
            }
            catch (Exception ex)
            {
            }


            return true;
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
