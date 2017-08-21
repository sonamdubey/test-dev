using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity.ConfigurePageMetas;
using BikewaleOpr.Interface.ConfigurePageMetas;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DALs.ConfigurePageMetas
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 17-Aug-2017
    /// Summary: DAL for Page meta repository
    /// 
    /// </summary>
    /// <seealso cref="BikewaleOpr.Interface.ConfigurePageMetas.IPageMetasRepository" />
    public class PageMetasRepository : IPageMetasRepository
    {
        public IEnumerable<PageEntity> GetPagesList()
        {
            IEnumerable<PageEntity> objPageList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    objPageList = connection.Query<PageEntity>("getbwpagemaster", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasRepository.GetPagesList"));
            }
            return objPageList;
        }                 
        

        public bool SavePageMetas(PageMetasEntity objMetas)
        {
            bool isSuccess = false;            
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_id", objMetas.PageMetaId);
                    param.Add("par_pageid", objMetas.PageId);
                    param.Add("par_makeid", objMetas.MakeId);
                    param.Add("par_modelid", objMetas.ModelId == 0 ? null: objMetas.ModelId);
                    param.Add("par_title", objMetas.Title);
                    param.Add("par_description", objMetas.Description);
                    param.Add("par_keywords", objMetas.Keywords);
                    param.Add("par_heading", objMetas.Heading);
                    param.Add("par_summary", objMetas.Summary);
                    param.Add("par_enterdby", objMetas.EnteredBy);
                    param.Add("par_bothPlatform", objMetas.BothPlatform);
                   
                    connection.Execute("setpagemetas", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasRepository.GetPagesList"));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Method to get active or inactive or both page metas list.
        /// </summary>
        /// <param name="pageMetaStatus">0 for inactive, 1 for active and 2 for both.</param>
        /// <returns></returns>
        public IEnumerable<PageMetaEntity> GetPageMetas(uint pageMetaStatus)
        {
            IEnumerable<PageMetaEntity> objPageMetasList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_status", pageMetaStatus);
                    objPageMetasList = connection.Query<PageMetaEntity>("getpagemetas", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ConfigurePageMetasRepository.GetPageMetas_pageMetaStatus : {0}", pageMetaStatus));
            }
            return objPageMetasList;
        }

        
        public PageMetasEntity GetPageMetasById (uint pageMetaId)
        {
            PageMetasEntity objPageMetas = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_id", pageMetaId);

                    objPageMetas = connection.Query<PageMetasEntity>("getpagemetasbyid", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasRepository.GetPageMetasById"));
            }
            return objPageMetas;
        }

        public bool UpdatePageMetaStatus(uint id, ushort status)
        {
            bool result = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_id", id);
                    param.Add("par_status", status);
                    
                    connection.Execute("setpagemetastatus", param: param, commandType: CommandType.StoredProcedure);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasRepository.UpdatePageMetaStatus: id:{0}", id));
            }
            return result;
        }
    }
}
