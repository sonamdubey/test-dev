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
    public class ConfigurePageMetasRepository : IConfigurePageMetasRepository
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("ConfigurePageMetasRepository.GetPagesList"));
            }
            return objPageList;
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
        public uint SavePageMetas(PageMetasEntity objMetas)
        {
            uint pageMetaid = 0;            
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_id", objMetas.PageMetaId);
                    param.Add("par_pageid", objMetas.PageId);
                    param.Add("par_makeid", objMetas.MakeId);
                    param.Add("par_modelid", objMetas.ModelId);
                    param.Add("par_title", objMetas.Title);
                    param.Add("par_description", objMetas.Description);
                    param.Add("par_keywords", objMetas.Keywords);
                    param.Add("par_heading", objMetas.Heading);
                    param.Add("par_summary", objMetas.Summary);
                    param.Add("par_enterdby", objMetas.EnteredBy);

                    pageMetaid = connection.Query<uint>("setpagemetas", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ConfigurePageMetasRepository.GetPagesList"));
            }
            return pageMetaid;
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("ConfigurePageMetasRepository.GetPageMetasById"));
            }
            return objPageMetas;
        }
    }
}
