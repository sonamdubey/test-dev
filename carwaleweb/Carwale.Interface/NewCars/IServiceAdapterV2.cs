
namespace Carwale.Interfaces.NewCars
{
    public interface IServiceAdapterV2
    {        
        T Get<T, U>(U input);
    }  
}
