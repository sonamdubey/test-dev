
namespace Carwale.Entity.Enum
{
    public enum ModelMenuEnum
    {
        Overview = 1,
        PriceInCity = 2,
        VersionPage = 3,
        MileagePage = 4
    }

    /// Written by Meet Shah on 9 Nov, 2017.
    /// <summary>
    /// This extension method is used to output page names 
    /// for ModelMenuEnum.
    /// </summary>
    public static class ModelMenuEnumExtensions
    {
        public static string ToPageName(this ModelMenuEnum type)
        {
            switch (type)
            {
                case ModelMenuEnum.Overview:
                    return "ModelPage";
                case ModelMenuEnum.PriceInCity:
                    return "CityPage";
                case ModelMenuEnum.VersionPage:
                    return "VersionPage";
                case ModelMenuEnum.MileagePage:
                    return "MileagePage";
                default:
                    return string.Empty;
            }
        }
    }
}
