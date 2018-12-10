update PQ_SponsoredDealeAd_Templates set Template = '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head>
 <meta name="viewport" content="width=device-width" />
 <meta https-equiv="Content-Type" content="text/html; charset=UTF-8" />
 <title>Book Notification For Customer</title>
</head>
<body>
 <div style="max-width:680px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; background:#eeeeee;padding:10px 10px 20px 10px; word-wrap:break-word">
        
        <div style="background:#fff; margin:5px 0px 0px 0px">
            <div style="padding:10px; border-top:solid 7px #0e3a51;">
                <div style="float:left; max-width:130px;">
                  <a target="_blank" style="text-decoration:none; outline:none;" href="https://www.carwale.com/"><img border="0" style="width:100%;" title="CarWale" alt="CarWale" src="https://img.carwale.com/Mailer/PQimages/CW-offer-logo.jpg"></a>
                </div>
                
                <div style="float:right; max-width:128px;margin-top:4px;">
                <a href="https://www.carwale.com/" target="_blank" style="text-decoration:none; outline:none;color:#333;">www.carwale.com</a> 
                </div>
                <div style="clear:both"></div>
            </div>          
            <div style="background:url(images/offer-shadow.png) repeat-x 0px 0px #fff; padding:0px 0px 0 10px; height:6px;"></div>
            <!-- Body content part code starts here -->
            <div style="padding:10px; line-height:1.6em">
             <p style="font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;">Dear @Model.MailerName,</p>                
                <p style="font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;">
                Thank you for your request to book <strong>@(Model.MakeName+" "+Model.ModelName+" "+Model.VersionName)</strong> through CarWale. You will be notified once the dealer confirms this booking. In case the dealer is not in a position to fulfill your booking request, we would initiate a full refund of your booking amount within 3 working days. 
                </p>
                
                <p style="font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;">Below are the details of your transaction:</p>                
                           
                <div style="margin-bottom:15px;">
                 <table cellpadding="5" cellspacing="0" border="1" width="100%" style="border-collapse:collapse">
                        <tr><td width = "30%"><strong>Car</strong></td><td><strong>@(Model.MakeName+" "+Model.ModelName+" "+Model.VersionName)</strong></td></tr>
                        <tr><td><strong>Color</strong></td><td><strong>@Model.ColourName</strong></td></tr>
                        <tr><td>Year</td><td><strong>@Model.MakeYear</strong></td></tr>
      <tr><td>City</td><td><strong>@Model.CityName</strong></td></tr>
                        <tr><td>On Road Price</td><td><strong>Rs.@Model.OnRoadPrice/-</strong></td></tr>
                        <tr><td>Discount</td><td><strong>Rs.@Model.SavingPrice/-</strong></td></tr>
                        <tr><td>Offer Price</td><td><strong>Rs.@Model.OfferPrice/-</strong></td></tr>
                        <tr><td>Booking Amount Paid To CarWale</td><td><strong>Rs.@Model.Amount/-</strong></td></tr>
      <tr><td>Transaction Id</td><td><strong>@Model.TransactionId</strong></td></tr>
                        <tr style="display:@(string.IsNullOrEmpty(Model.OfferExistance) ? "none" : "");"><td>Applicable Offers / promotion on this purchase</td><td><strong>@Model.OfferExistance</strong></td></tr>
                    </table>
                </div>
 <p style="font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;"> We have notified the dealer about your booking request.<br>  Below are the details of the dealer who will service your request:</p>  
<div style="margin-bottom:15px;">
                 <table cellpadding="5" cellspacing="0" border="1" width="100%" style="border-collapse:collapse"> 
  <tr style="display:@(string.IsNullOrEmpty(Model.DealerName) ? "none" : "");"><td width = "30%"><strong>Dealer Name</strong></td><td><strong>@Model.DealerName</strong></td></tr>
                         <tr style="display:@(string.IsNullOrEmpty(Model.DealerAddress) ? "none" : "");"><td><strong>Dealer Address</strong></td><td><strong>@Model.DealerAddress</strong></td></tr>
 <tr style="display:@(string.IsNullOrEmpty(Model.ContactPerson) ? "none" : "");"><td><strong>Contact Person at Dealership</strong></td><td><strong>@Model.ContactPerson</strong></td></tr>
 <tr style="display:@(string.IsNullOrEmpty(Model.DealerMobile) ? "none" : "");"><td><strong>Contact number</strong></td><td><strong>@Model.DealerMobile</strong></td></tr>

</table>
</div>  

                <p style="font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;">Next Steps:<br>
We will call you to confirm the booking and connect you with the dealer. In case the dealer is not in a position to fulfill your booking request, you will get a full refund of your reservation amount paid to CarWale.<br> Please feel free to call us at  <strong>@Model.TollFreeNumber</strong> for any queries.</p>                    
                <p style="font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:20px 0 15px 0;">
                 Best Regards,<br />
                    <strong>Team CarWale</strong>
                </p>
            </div>
            <!-- Body content part code ends here -->
        </div>
        <div style="background:url(images/offer-bottom-shadow.jpg) center center no-repeat #eeeeee; height:9px; width:100%"></div>
        <div style="padding:0px 0px 5px 0px;width:100%">      
            <div style="width:100px; margin:0 auto">
    <div style=" float:left;padding-right:5px;"><a href="httpss://twitter.com/Carwale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan" target="_blank"><img src="https://img.carwale.com/Mailer/PQimages/offer-t-icon.jpg" alt="Twitter" title="Twitter" border="0"/></a></div>
                <div style="float:left; padding-right:5px;"><a href="httpss://www.facebook.com/CarWale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan" target="_blank"><img src="https://img.carwale.com/Mailer/PQimages/offer-fb-icon.jpg" alt="Facebook" title="Facebook" border="0" /></a></div>
                <div style="float:left; "><a href="httpss://plus.google.com/+CarWale/posts?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan" target="_blank"><img src="https://img.carwale.com/Mailer/PQimages/offer-g-icon.jpg" alt="Google+" title="Google+" border="0" /></a></div>
                <div style="clear:both;"></div>
            </div>
            <div style="clear:both;"></div>
        </div>
 </div>
</body>
</html>' where TemplateId  = 360
