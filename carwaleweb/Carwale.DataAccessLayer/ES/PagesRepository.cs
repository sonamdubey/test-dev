using Carwale.Entity.Common;
using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.ES
{
    public class PagesRepository : RepositoryBase, IPagesRepository
    {       
        public List<Pages> GetPagesAndProperties(int applicationId, int platformId)
        {
            List<Pages> pagesAndProperties = new List<Pages>();
            try
            {                
                var param = new DynamicParameters();                
                param.Add("v_ApplicationId", applicationId);
                param.Add("v_PlatformId", platformId);                

                using (var con = EsMySqlReadConnection)
                {
                    var response = con.QueryMultiple("cwmasterdb.GetPagesandProperties", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetPagesandProperties");

                    var pages = response.Read<IdName>().ToList();
                    var properties = response.Read<PropertiesEntity>().ToList();

                    foreach (var page in pages)
                    {
                        var pageProperties = new Pages();

                        pageProperties.Id = page.Id;
                        pageProperties.Name = page.Name;                      

                        if (properties != null && properties.Count > 0)
                            pageProperties.Properties = properties.Where(pId => pId.PageId == page.Id).ToList();

                        pagesAndProperties.Add(pageProperties);
                    }
                }                
            }            
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return pagesAndProperties;
        }
    }
}