
namespace AppNotification.Interfaces
{
    public interface IAPIService<T, TResponse>
    {
        TResponse Request(T t);
        void UpdateResponse(T t, TResponse t2);
    }
}
