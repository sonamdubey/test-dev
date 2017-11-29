namespace Bikewale.Models.PopUp
{
    public class BikeCityPopup
    {
        public string ApiUrl { get; set; }
        public string PopupShowButtonMessage { get; set; }
        public string PopupSubHeading { get; set; }
        public string FetchDataPopupMessage { get; set; }
        public string RedirectUrl { get; set; }

        public sbyte IsCityWrapperPresent { get; set; }

    }
}
