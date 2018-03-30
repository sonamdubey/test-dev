using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace BikewaleOpr.DALs.Banner
{
    /// <summary>
    /// Created By :- Subodh Jain on 24 july 2017
    /// Summary :- Banner Repository
    /// </summary>
    public class BannerRepository : IBannerRepository
    {
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Get Banner Details
        /// </summary>
        public BannerVM GetBannerDetails(uint bannerId)
        {
            BannerVM objBannerVM = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_id", bannerId);

                    var obj = connection.QueryMultiple("gethomepagebanner_20072017", param: param, commandType: CommandType.StoredProcedure);
                    if (obj != null)
                    {
                        objBannerVM = new BannerVM();
                        objBannerVM.DesktopBannerDetails = obj.Read<BannerDetails>().FirstOrDefault();
                        objBannerVM.MobileBannerDetails = obj.Read<BannerDetails>().FirstOrDefault();
                        var objvm = obj.Read<dynamic>().FirstOrDefault();
                        if (objvm != null)
                        {
                            objBannerVM.StartDate = objvm.StartDate;
                            objBannerVM.EndDate = objvm.EndDate;
                            objBannerVM.BannerDescription = objvm.BannerDescription;
                        }


                        if (bannerId > 0)
                            objBannerVM.CampaignId = bannerId;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BannerRepository.GetBannerDetails bannerId:{0}", bannerId));
            }

            return objBannerVM;

        }

        /// <summary>
        /// created by Sajal gupta on 25-07-2017
        /// Desc : Function to get banners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BannerProperty> GetBanners(uint bannerStatus)
        {
            IEnumerable<BannerProperty> objBannerList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    
                    var param = new DynamicParameters();
                    param.Add("par_status", bannerStatus);
                    objBannerList = connection.Query<BannerProperty>("getHomePageBanners", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BannerRepository.GetBanners"));
            }
            return objBannerList;
        }

        public bool ChangeBannerStatus(uint bannerId, UInt16 bannerStatus)
        {
            bool status = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_reviewId", bannerId);
                    param.Add("par_status", bannerStatus);
                    connection.Execute("changebannerstatus", param: param, commandType: CommandType.StoredProcedure);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BannerRepository.StopBanner"));
            }
            return status;
        }

        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Save Descri,start date and end date of banner
        /// </summary>

        public uint SaveBannerBasicDetails(BannerVM objBanner)
        {
            uint campaignid = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {                    
                    var param = new DynamicParameters();
                    param.Add("par_id", objBanner.CampaignId);
                    param.Add("par_startdate", objBanner.StartDate);
                    param.Add("par_enddate", objBanner.EndDate);
                    param.Add("par_bannerdescription", objBanner.BannerDescription);
                    param.Add("par_userid", objBanner.UserId);
                    campaignid = connection.Query<uint>("savebannerbasicdetails", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BannerRepository.GetBannerDetails campaignid: {0}", campaignid));
            }
            return campaignid;

        }
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- =Save all the properties of banner desktop and mobile
        /// </summary>
        public bool SaveBannerProperties(BannerDetails objBanner, uint platformId, uint campaignId)
        {
            int success = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
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
                    param.Add("par_unmodifiedhtml", objBanner.UnModifiedHtml);
                    param.Add("par_unmodifiedcss", objBanner.UnModifiedCSS);
                    param.Add("par_js", objBanner.JS);
                    param.Add("par_id", campaignId);
                    param.Add("par_platformid", platformId);
                    param.Add("par_category", objBanner.Category);
                    param.Add("par_action", objBanner.Action);
                    param.Add("par_label", objBanner.Label);
                    success = connection.Execute("savebannerdetails", param: param, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BannerRepository.GetBannerDetails campaignId: {0}", campaignId));
            }
            return success > 0;
        }
    }
}
