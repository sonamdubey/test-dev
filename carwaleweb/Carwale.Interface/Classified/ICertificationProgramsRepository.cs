using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified
{
    public interface ICertificationProgramsRepository
    {
        int CreateProgram(CertificationProgramsDetails certificationProgramsDetails);
        void UpdateProgram(int certificationId, CertificationProgramsDetails certificationProgramsDetails);
    }
}
