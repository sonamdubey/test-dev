using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 April 2018
    /// Description : File holding all the Helpers required to Render Editorial Widgets
    /// </summary>


    //Class to hold helpers related to `View all` button links.

    public static class EditorialWidgetHelper
    {

        public static readonly IDictionary<EditorialWidgetCategory, string> EditorialViewAllTitles = new Dictionary<EditorialWidgetCategory, string>
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

        public static readonly IDictionary<EditorialWidgetCategory, string> EditorialTabHeading =
            new Dictionary<EditorialWidgetCategory, string>
            {
                { EditorialWidgetCategory.Popular_Make, "{0} Bikes"},
                { EditorialWidgetCategory.Popular_All, "Popular Bikes"},
                { EditorialWidgetCategory.Popular_Cruisers, "Cruisers"},
                { EditorialWidgetCategory.Popular_Sports, "Sports Bikes"},
                { EditorialWidgetCategory.Popular_Scooters, "Popular Scooters"},
                { EditorialWidgetCategory.Upcoming_All, "Upcoming Bikes"},
                { EditorialWidgetCategory.Upcoming_Make, "Upcoming {0} Bikes"},
                { EditorialWidgetCategory.Upcoming_Scooters, "Upcoming Scooters"},
                { EditorialWidgetCategory.OtherBrands_All, "Other Brands"},
                { EditorialWidgetCategory.Popular_Make_Scooters, "{0} Scooters"},
                { EditorialWidgetCategory.Popular_Series, "{0} Bikes"},
                { EditorialWidgetCategory.Series_Scooters, "{0} Scooters"}
        };

        public static readonly IDictionary<EditorialWidgetCategory, string> EditorialTabHeading_Mobile =
            new Dictionary<EditorialWidgetCategory, string>
            {
                { EditorialWidgetCategory.Popular_Make, "Popular {0} Bikes"},
                { EditorialWidgetCategory.Popular_All, "Popular Bikes"},
                { EditorialWidgetCategory.Popular_Cruisers, "Popular Cruiser Bikes"},
                { EditorialWidgetCategory.Popular_Sports, "Popular Sports Bikes"},
                { EditorialWidgetCategory.Popular_Scooters, "Popular Scooters"},
                { EditorialWidgetCategory.Upcoming_All, "Upcoming Bikes"},
                { EditorialWidgetCategory.Upcoming_Make, "Upcoming {0} Bikes"},
                { EditorialWidgetCategory.Upcoming_Scooters, "Upcoming Scooters"},
                { EditorialWidgetCategory.OtherBrands_All, "Popular Scooter Brands"},
                { EditorialWidgetCategory.Popular_Make_Scooters, "Popular {0} Scooters"},
                { EditorialWidgetCategory.Popular_Series, "Popular {0} Bikes"},
                { EditorialWidgetCategory.Series_Scooters, "Popular {0} Scooters"}
            };
         }
    }
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
        MakeListing
    }
    
