using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Carwale.Entity;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Geolocation
{
    public class GeoCities : RepositoryBase,IRepository<Cities>
    {
        public IEnumerable<Cities> GetAll()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<Cities>("cwmasterdb.GetAllCitiesInfo_v16_11_7", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public IEnumerable<Cities> GetAllById(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<Cities> Find(SearchQuery<Cities> query, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Cities GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Create(Cities entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Cities entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }


        public bool Delete(Cities entity)
        {
            throw new NotImplementedException();
        }
    }
}
