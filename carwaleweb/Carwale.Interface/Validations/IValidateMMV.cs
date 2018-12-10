namespace Carwale.Interfaces.Validations
{
    public interface IValidateMmv
    {
        bool IsModelVersionValid(int versionId);
        bool IsModelValid(int modelId);
    }
}
