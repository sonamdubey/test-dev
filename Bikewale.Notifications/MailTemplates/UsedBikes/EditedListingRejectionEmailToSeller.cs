﻿using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Oct 2016
    /// Description :  Used Bike Approved Listing Individual Seller Email Template
    /// </summary>
    public class EditedListingRejectionEmailToSeller : ComposeEmailBase
    {
        private string sellerName,
            profileNo,
            bikeName,
            bikePrice;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        /// <param name="sellerEmail"></param>
        /// <param name="sellerName"></param>
        /// <param name="buyerName"></param>
        /// <param name="buyerEmail"></param>
        /// <param name="buyerContactNo"></param>
        /// <param name="profileNo"></param>
        /// <param name="bikeName"></param>
        /// <param name="bikePrice"></param>
        public EditedListingRejectionEmailToSeller(string sellerName, string profileNo, string bikeName)
        {
            this.sellerName = sellerName;
            this.profileNo = profileNo.ToUpper();
            this.bikeName = bikeName;
        }
        /// <summary>s
        /// Created by  :   Aditi Srivastava on 14 Nov 2016
        /// Description :   Prepares the Email Body when changes in listing are rejected
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},</p>", sellerName);
            sb.AppendFormat("<p>The changes to your {0} bike listing (profile id: {1}) have not been approved on BikeWale.</p>", bikeName, profileNo);
            sb.AppendFormat("<p>Following could be the reasons why the listings get rejected on BikeWale:");
            sb.AppendFormat("<ol><li> Inaccurate price</li><li> Inappropriate photos</li><li> Invalid manufacturing year</li></ol></p>");
            sb.AppendFormat("<p>We would request you to reupload the listing with correct details.</p>");
            sb.AppendFormat("<p>In case of any queries, please feel free to reach to <a href='mailto:contact@bikewale.com'>contact@bikewale.com</a></p>");
            sb.AppendFormat("<p>Thanks<br>");
            sb.AppendFormat("Team BikeWale<br>");
            sb.AppendFormat("<a href='https://www.bikewale.com/'>www.bikewale.com</a></p>");
            return sb.ToString();
        }
    }
}