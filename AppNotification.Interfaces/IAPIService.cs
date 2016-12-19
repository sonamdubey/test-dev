
using System.Collections.Generic;
namespace AppNotification.Interfaces
{
    public interface IAPIService<TResponse>
    {
        TResponse Request(List<string> regKeyList);
        //void UpdateResponse(List<string> regKeyList, TResponse t2);
    }
}
