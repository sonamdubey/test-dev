using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
    public class CertificationProgramsController : ApiController
    {
        private readonly ICertificationProgramsRepository _certificationProgramsRepository;
        private readonly IStockBL _stockBL;
        public CertificationProgramsController(ICertificationProgramsRepository certificationProgramsRepository, IStockBL stockBL)
        {
            _certificationProgramsRepository = certificationProgramsRepository;
            _stockBL = stockBL;
        }

        [ApiAuthorization, HandleException, LogApi, ValidateModel("certificationProgramsDetails")]
        public IHttpActionResult Post(CertificationProgramsDetails certificationProgramsDetails)
        {
            int certificationId = _certificationProgramsRepository.CreateProgram(certificationProgramsDetails);
            return Ok(certificationId);
        }

        [ApiAuthorization, HandleException, LogApi, ValidateModel("certificationProgramsDetails"), Route("api/certificationprograms/{certificationId}/")]
        public IHttpActionResult Put(int certificationId, CertificationProgramsDetails certificationProgramsDetails)
        {
            _certificationProgramsRepository.UpdateProgram(certificationId, certificationProgramsDetails);
            _stockBL.RefreshESStocksOfCertProg(certificationId);
            return Ok();
        }
    }
}
