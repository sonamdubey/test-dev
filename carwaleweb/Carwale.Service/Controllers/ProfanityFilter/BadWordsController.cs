using Carwale.Entity.ProfanityFilter;
using Carwale.Interfaces.ProfanityFilter;
using Carwale.Service.Filters;
using System.Linq;
using System.Web.Http;

namespace Carwale.Service.Controllers.ProfanityFilter
{
    public class BadWordsController : ApiController
    {
        private readonly IBadWordsRepository _badWordsRepo;

        public BadWordsController(IBadWordsRepository badWordsRepo)
        {
            _badWordsRepo = badWordsRepo;
        }

        [ApiAuthorization, HandleException, ValidateModel("badWords")]
        public IHttpActionResult Post([FromBody]BadWords badWords)
        {
            var words = badWords.Words.Select(w => w.Trim());
            _badWordsRepo.InsertBadWords(words);
            return Ok();
        }

        [ApiAuthorization, HandleException, ValidateModel("badWords")]
        public IHttpActionResult Delete([FromBody]BadWords badWords)
        {
            var words = badWords.Words.Select(w => w.Trim());
            _badWordsRepo.DeleteBadWords(words);
            return Ok();
        }
    }
}
