using Carwale.Entity.Classified.SellCarUsed;
using System;
using System.Web.UI;

namespace Carwale.UI.Common
{
    /// <summary>
    /// Created By : Ashish G. kamble on 26 Nov 2013
    /// Summary : Class to show inquiry message in my carwale
    /// </summary>
    public class InquiryStatus : Page
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string ButtonText { get; set; }
        public string ButtonCommandName { get; set; }
        public bool ShowButton { get; set; }

        public InquiryStatus()
        {

        }

        public InquiryStatus(string llInquiryId, bool isListingCompleted, string currentStep, string paymentMode, int freeListingCnt, int paidListingCnt, string expDate, bool isPremium)
        {
            if (String.IsNullOrEmpty(llInquiryId))
            {
                // Inquiry is not live

                if (isListingCompleted == true && Convert.ToInt32(currentStep) == ((int)SellCarSteps.Confirmation))
                {
                    // Car is removed from listing 

                    DateTime carExpDate = Convert.ToDateTime(expDate);
                    TimeSpan noOfDaysDiff = carExpDate.Date - DateTime.Now.Date;

                    if (noOfDaysDiff.Days < 0)
                    {
                        // Listing is expired
                        Message = "Your ad has expired and is no longer displayed to used car buyers.";
                        Status = "Your ad is not live";
                        ShowButton = false;
                    }
                    else
                    {
                        // Listing is removed by customer

                        ShowButton = false;
                        Message = "You chose to stop showing your ad to buyers on CarWale. If you would like to start showing your ad again, please contact us at contact@carwale.com.";
                        Status = "Your ad is not live";
                    }
                }
                else if (isListingCompleted == false && Convert.ToInt32(currentStep) == ((int)SellCarSteps.Confirmation))
                {
                    //cheque payment and free listing count > 0
                    ShowButton = false;
                    Message = "Your ad is not live on Carwale.";
                    Status = "Your ad is not live";
                }
                else if (isListingCompleted == false && Convert.ToInt32(currentStep) <= ((int)SellCarSteps.Confirmation))
                {
                    // listing is not completed

                    ShowButton = true;
                    ButtonCommandName = "CompleteYourListing";
                    ButtonText = "Complete Your Listing";

                    Message = "Your ad is not yet visible to potential buyers on CarWale. To start getting inquiries for your car:";
                    Status = "Your ad is not live";
                }
            }
            else
            {
                // Inquiry is live

                // Free listing payment mode 2
                if (paymentMode == "2")
                {
                    ShowButton = false;
                    Message = "Buyers can see your ad as part of our free listings for 90 days.";
                    Status = "Your ad is live";
                }
                else
                {
                    if (isPremium)
                    {
                        // Ad is live as premium ad
                        ShowButton = false;
                        Status = "Your ad is live";
                    }
                    else
                    {
                        // Ad is not live as premium ad.
                        ShowButton = false;
                        Message = "Your ad is now displayed on CarWale as a free listing as the premium package expired.";
                        Status = "Your ad is live";
                    }
                }
            }
        }   // InquiryStatus constructor
    } // class
} // namespace