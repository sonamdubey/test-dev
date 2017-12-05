
namespace Bikewale.Models.BikeModels
{
    public class MultiTabsWidgetVM
    {
        public string TabHeading1 { get; set; }
        public string TabHeading2 { get; set; }
        public string ViewPath1 { get; set; }
        public string ViewPath2 { get; set; }
        public string TabId1 { get; set; }
        public string TabId2 { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public MostPopularBikeWidgetVM MostPopularScooters { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingScooters { get; set; }
        public string ViewAllHref1 { get; set; }
        public string ViewAllHref2 { get; set; }
        public string ViewAllTitle1 { get; set; }
        public string ViewAllTitle2 { get; set; }
        public string ViewAllText1 { get; set; }
        public string ViewAllText2 { get; set; }
        public bool ShowViewAllLink1 { get; set; }
        public bool ShowViewAllLink2 { get; set; }
        public MultiTabWidgetPagesEnum Pages { get; set; }
        public string PageName { get; set; }
    }

    public enum MultiTabWidgetPagesEnum
    {
        PopularBikesAndPopularScooters = 1,
        UpcomingBikesAndUpcomingScooters = 2
    }
}
