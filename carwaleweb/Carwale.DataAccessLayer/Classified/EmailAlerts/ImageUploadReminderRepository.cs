using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Carwale.Entity.Classified.EmailAlerts;

namespace Carwale.DAL.Classified.EmailAlerts
{
    public class ImageUploadReminderRepository : RepositoryBase
    {
        public List<ImageUploadReminderData> GetEmailData()
        {
            List<ImageUploadReminderData> dataList = null;
            using (var con = ClassifiedMySqlReadConnection)
            {
                dataList = con.Query<ImageUploadReminderData>("UsedCarPhotoUploadReminderData_v2", commandType: CommandType.StoredProcedure).ToList();
            }
            return dataList;
        }
    }
}
