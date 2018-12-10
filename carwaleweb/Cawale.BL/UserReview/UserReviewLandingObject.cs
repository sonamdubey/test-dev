using Carwale.BL.Experiments;
using Carwale.Entity.Enum;
using Carwale.Entity.UserReview;
using System.Collections.Generic;

namespace Carwale.BL.UserReview
{
    public static class UserReviewLandingObject
    {
        public readonly static List<FilterDetails> AllFilters = new List<FilterDetails>
        {
            new FilterDetails{ Id =  (int)ReviewFilter.MostRecent, Name = "Most Recent"},
            new FilterDetails{ Id =  (int)ReviewFilter.MostHelpFul, Name = "Most Helpful"},
            new FilterDetails{ Id =  (int)ReviewFilter.MostRead, Name = "Most Read"},
            new FilterDetails{ Id =  (int)ReviewFilter.MostReviewed, Name = "Most Reviewed"},
        };

        public static List<string> TermsAndConditions(int platformId)
        {
            return new List<string>
            {
             "This promotion is brought to you by Automotive Exchange Private Limited (hereafter referred to as 'CarWale'). Any participation in this regard is voluntary.",
            "The campaign starts on 10th September 2018 and a winner will be announced every week on CarWale. The winner will be notified via e-mail.",
            "This promotion is open only to bonafide citizens of India in their individual capacity who are above the age of 18 years as on 01 June 2018. Employees of CarWale (and any group or subsidiaries companies) and their immediate family members (spouses, children and parents) are not eligible to win any gifts under this promotion.",
            "In order to participate, a user needs to submit a complete user review of any car available in India on CarWale.",
            "Each user is allowed to submit as many entries as they like during the promotion period, however each user will be entitled to only one gift under the terms of this promotion, if selected, irrespective of the number of entries submitted.",
            "CarWale will, at its own sole discretion, select and announce one or more winners within five (5) business days of the last day of the promotion period.",
            string.Format("The winner(s) will be gifted with an Amazon voucher worth Rs. {0}.",ProductExperiments.VoucherPrice(platformId)),
            "CarWale reserves the right to independently verify the authenticity of the information entered by the user, especially with respect to the originality of the review.",
            "All gifts will be dispatched to the winner(s) within thirty (30) days of the end of the promotion period.",
            "Winner(s) will be contacted individually on the e-mail address they provide while submitting their user review. If a winner remains non-contactable for a period of seven (7) days, CarWale reserves the right to withdraw the gift and/or assign it to another user.",
            "This promotion cannot be combined with any other promotion, offer or discount from CarWale.",
            "All decisions with respect to this promotion, including number of gifts and type of gifts, will be at the sole discretion of CarWale and the same will be final and binding on a non-contestable basis.",
            "Gifts are non-transferable and cannot be exchanged for cash. No cash claim can be made in lieu of the gifts.",
            "CarWale will not be held responsible for the suitability or merchantability of any gift.",
            "CarWale shall not be liable for any direct or indirect loss or damage whatsoever that may be suffered, or for any personal injury that may be suffered, as a result of participating in this promotion.",
            "Any dispute arising out of or in connection with this promotion shall be subject to the exclusive jurisdiction of the courts in Mumbai only. The existence of a dispute, if any, shall not constitute a claim against CarWale.",
            "CarWale reserves the right to withdraw and / or alter any / all of the terms and conditions of this promotion at any time without prior notice.",
            "By participating in this promotion, the participants accept the terms and conditions as specified above."
            };
        }

        public readonly static List<string> howToWin = new List<string> 
        {
            "Be original. Our examiners get a stroke on seeing copied text. Be kind to them.",
            "Write no less than 150 words. We are sure you have more to tell.",
            "Spill all the beans. Write all the pros and cons. The more the details, the more your chances of winning.",
            "Keep calm and be good. The contest judges are incorruptible.",
            "Be patient. Winners will be announced every week. The winners will be notified via e-mail."
        };
    }
}
