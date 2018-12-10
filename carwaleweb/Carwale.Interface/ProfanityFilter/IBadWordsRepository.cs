using System.Collections.Generic;

namespace Carwale.Interfaces.ProfanityFilter
{
    public interface IBadWordsRepository
    {
        void InsertBadWords(IEnumerable<string> badWords);
        void DeleteBadWords(IEnumerable<string> badWords);
    }
}
