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
}
