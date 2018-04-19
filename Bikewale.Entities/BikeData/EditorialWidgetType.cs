using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    public enum EditorialWidgetType
    {
        Popular,
        Upcoming,
        OtherBrands
    }

    public enum EditorialPageWidgetPosition
    {
        First,
        Second
    }

    public enum EditorialWidgetColumnPosition
    {
        Left,
        Right
    }

    public enum EditorialWidgetCategory
    {
        Popular_All,
        Popular_Make,
        Popular_Make_Scooters,
        Popular_BodyStyle,
        Popular_Cruisers,
        Popular_Sports,
        Popular_Scooters,
        Popular_Series,

        Upcoming_All,
        Upcoming_Make,
        Upcoming_Scooters,

        OtherBrands_All,

        Series_Scooters
    }

    /// <summary>
    /// Enum denoting the Type of the Page for which Editorial which data has to be returned. (e.g. `Listing`/`Detail` etc.)
    /// </summary>
    public enum EnumEditorialPageType
    {
        Listing,
        Detail,
        MakeListing,
    }
    
    public static class WidgetUtilities
        {

            public static readonly IDictionary<EditorialWidgetCategory, string> EditorialViewAllTitles = new Dictionary<EditorialWidgetCategory, string>()
            {
                { EditorialWidgetCategory.Popular_Make, "{0} Bikes"},
                { EditorialWidgetCategory.Popular_All, "Best Bikes in India"},
                { EditorialWidgetCategory.Popular_Cruisers, "Best Cruisers in India"},
                { EditorialWidgetCategory.Popular_Scooters, "Best Scooters in India"},
                { EditorialWidgetCategory.Popular_Sports, "Best Sports Bikes in India"},
                { EditorialWidgetCategory.Upcoming_All, "Upcoming Bikes in India"},
                { EditorialWidgetCategory.Upcoming_Make, "Upcoming {0} Bikes in India"},
                { EditorialWidgetCategory.Upcoming_Scooters, "Upcoming Scooters in India"},
                { EditorialWidgetCategory.OtherBrands_All, "Scooters in India"},
                { EditorialWidgetCategory.Popular_Make_Scooters, "{0} Scooters"},
                { EditorialWidgetCategory.Popular_Series, "{0} Bikes in India"},
                { EditorialWidgetCategory.Series_Scooters, "{0} Scooters in India"}
            };
        }
}
