using Carwale.DAL.CoreDAL;
using Carwale.Entity.Template;
using Carwale.Interfaces.Template;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.DAL.Template
{
    /// <summary>
    /// Author : sachin bharti on 23/11/15
    /// </summary>
    public class TemplatesRepository : ITemplatesRepository
    {
        /// <summary>
        /// Author : sachin bharti on 23/11/15
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<Templates> GetAll(short platformId, short typeId)
        {
            var dealerTemplates = new List<Templates>();

            try {

                using (DbCommand cmd = DbFactory.GetDBCommand("CW_GetDealerPQAdTemplates"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int16, platformId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_TypeId", DbType.Int16, typeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            dealerTemplates.Add(
                                new Templates() {
 
                                    UniqueName = dr["TemplateName"].ToString(), 
                                    Html = dr["Template"].ToString(), 
                                    TemplateType = Convert.ToInt32(dr["TemplateType"]) 
                                });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return dealerTemplates;
        }

        /// <summary>
        /// Author : sachin bharti on 23/11/15
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Templates GetById(int templateId)
        {
            var template = new Templates();

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetTemplateById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_TemplateId", DbType.Int16 , templateId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.NewCarMySqlReadConnection))
                    {
                        if(dr.Read())
                        {
                            template.UniqueName = dr["TemplateName"].ToString();
                            template.Html = dr["Template"].ToString();
                            template.TemplateType = Convert.ToInt32(dr["TemplateType"]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return null;
            }
            return template;
        }
    }
}
