using Bikewale.Entities.AppDeepLinking;

namespace Bikewale.Interfaces.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// </summary>
    public interface IDeepLinking
    {
      DeepLinkingEntity GetParameters(string url);
    }
}
