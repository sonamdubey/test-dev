using Carwale.Entity.CarData;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface ICarMakes
    {
        List<CarMakeEntityBase> GetCarMakesByType(string type);
        CarMakeDescription GetCarMakeDescription(int makeId);
        string Title(string title, string makeName);
        string Description(string description, string makeName);
        string Keywords(string keywords, string makeName);
        string Heading(string heading, string makeName);
        string Summary(string summary, string makeName, int makeId);
        List<CarModelSummary> GetNewCarModelsWithDetails(int cityId, int makeId, bool orp = false);
        List<ModelSummary> GetActiveModelsWithDetails(int cityId, int makeId, bool orp = false);
    }
}
