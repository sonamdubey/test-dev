using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified
{
    public class CertificationProgramsRepository : RepositoryBase, ICertificationProgramsRepository
    {
        public int CreateProgram(CertificationProgramsDetails certificationProgramsDetails)
        {
            string query = "insert into certificationprograms values (null, @name, @logoUrl, null); select last_insert_id();";
            int certificationId;
            var param = new DynamicParameters();
            param.Add("name", certificationProgramsDetails.Name);
            param.Add("logoUrl", certificationProgramsDetails.LogoUrl);
            using (var con = ClassifiedMySqlMasterConnection)
            {
                certificationId = con.Query<int>(query, param, commandType: CommandType.Text).FirstOrDefault();
            }
            return certificationId;
        }
        
        public void UpdateProgram(int certificationId, CertificationProgramsDetails certificationProgramsDetails)
        {
            string query = "update certificationprograms set name = @name, logourl = @logoUrl where id = @id";
            var param = new DynamicParameters();
            param.Add("name", certificationProgramsDetails.Name);
            param.Add("logoUrl", certificationProgramsDetails.LogoUrl);
            param.Add("id", certificationId);
            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Query(query, param, commandType: CommandType.Text);
            }
        }
    }
}
