using Bikewale.Entities.Authors;

namespace Bikewale.Interfaces.Authors
{
    public interface IAuthorsCacheRepository
    {
        AuthorsMaskingReponse GetAuthorsMaskingResponse(string maskingName);
    }
}
