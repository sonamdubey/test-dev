<%@ Page Language="C#"%>
<!doctype html>
<html>
<head>
    <%
        title = "FAQs - BikeWale";
        keywords = "";
        description = "";

        AdPath = "";
        AdId = "";    
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
    .faq-top-banner { background: #57605e url(http://img.aeplcdn.com/bikewaleimg/images/insurance-banner.jpg) no-repeat center center; background-size: cover; height: 130px; }
    .faq-content h3 { margin-bottom:15px; padding:5px 10px 15px; border-bottom:1px solid #ccc; }
    .faq-content p, .faq-content ol { font-size:14px; line-height:1.5; }
    .faq-content p { padding:0 10px 5px; }
    .faq-content ol { margin-left: 30px; list-style-type:lower-alpha; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container">
                <div class="faq-top-banner">
                  <!-- Top banner code starts here -->
			        <h1 class="text-uppercase text-white text-center padding-top30 font25">FAQ<span class="font16">s</span></h1>
                    <p class=" font16 text-white text-center">What can we help you with?</p>
                </div>
                <!-- Top banner code ends here --> 
            </div>
        </section>

        <section>
	        <div class="container margin-top10">
    	        <div class="grid-12">
        	        <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Why should I book my bike at BikeWale?</h3>
                        <p>Booking is the only way to immediately block the offers that come with a particular bike on BikeWale. Once you book your bike at BikeWale, you become entitled to avail any offers mentioned against the bike model, along with securing the mentioned price for the bike. Since most offers are limited in number and available for a limited period, you should use booking option to secure them before they expire.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>How can I book a bike on BikeWale?</h3>
                        <p>To book a bike, you have to pay a fixed booking amount online mentioned against the vehicle of your interest. This amount will be adjusted in the total on-road price of the bike. Your payment gets transferred to BikeWale through a fully-secured payment gateway.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>What happens when I book a bike on BikeWale?</h3>
                        <p>On successful receipt of the booking payment, we will immediately send you a Booking Confirmation receipt on your email and SMS, which contains the following information:</p>
                        <ol>
                            <li>Details of your payment</li>
                            <li>Details of your selected bike (model, version) and the vehicle pricing</li>
                            <li>Contact details of assigned dealership who will be responsible to complete further formalities and deliver the vehicle</li>
                            <li>List of offers that you just secured by booking</li>
                        </ol>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>What happens to the money I pay while booking?</h3>
                        <p>The money that you pay for booking is further transferred to the assigned bike dealership from which you are supposed to purchase the vehicle. This money will be adjusted against the final on-road price of the vehicle, i.e. you have to pay only the remaining amount to the dealership.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>How is the dealership assigned for purchase? Can I change the dealership that has been assigned?</h3>
                        <p>We assign a dealership for your purchase within network of our partner dealerships. Dealership’s name and contact details are mentioned while booking, so you know which dealer you are booking your bike with. You will not be able to change the dealership that has been assigned to you.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Will the dealer call me when I book?</h3>
                        <p>Yes, you will get a call from the assigned dealership after you have booked a bike on BikeWale to schedule your visit to dealership. You will be required to go to the dealership to complete the remaining procedure and take delivery of the vehicle. Please carry the print out of Booking Confirmation, required documents for RTO / Bank Loan, remaining payment amount etc to the dealership to avoid multiple visits. You can yourself call the dealership on the mentioned contact numbers to fix-up a visit. We do send dealership contact details on your email and SMS, apart from showing them on website.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Can I book the bike for others, for example my family member?</h3>
                        <p>Yes, you can book vehicle for your near and dear ones.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>I have booked a model. Now I want to change it to another model?</h3>
                        <p>We recommend you to thoroughly make up your mind for the vehicle of your choice before booking and should avoid changing it later. However, if you make a mistake and want to change the bike model, you can email us on <a href="mailto:contact@bikewale.com">contact@bikewale.com</a> with the subject as <strong>‘Booking Change Request’</strong> stating the reason for the same, clearly stating the booking reference number, your mobile number (that you used while booking), along with the new model you now want to book. We will reply within 2 working days to confirm the change of booking to the new bike model.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Will I be charged any fee for online booking?</h3>
                        <p>No, BikeWale does not charge any fee for booking. The full amount you pay is accounted for against the purchase price of the vehicle and gets adjusted in the final payment.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>I want to cancel the booking. How can I do that? What happens to the refund?</h3>
                        <p>BikeWale strongly recommends you to carefully select your bike before booking. However, you still have the option to cancel a booking in case you change your mind. For all valid cancellation requests, full booking amount will be refunded back to you. If you choose to cancel a booking, you can email us the <strong>‘Booking Cancellation Request’</strong> on <a href="mailto:contact@bikewale.com">contact@bikewale.com</a> with a valid reason for cancellation, clearly stating the booking reference number, your mobile number, email address (that you used while booking). Please note the following conditions to request cancellation and avail refund.<br><br>
                        <strong>The important conditions for cancellation</strong> are following:</p>
                        <ol>
                            <li>Cancellation must be requested within <strong>7 calendar days of booking the vehicle.</strong></li>
                            <li><strong>Cancellation will not be possible if you and dealership have proceeded further with purchase of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, submitting any documents, procurement of vehicle by the dealership etc.</li>
                            <li>If the dealer has initiated the procurement of the bike upon your booking, cancellation will not be possible.</li>
                        </ol>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>When will I get my money refunded, once I request cancellation?</h3>
                        <p>Upon requesting a cancellation, our team will verify with the dealership. Post successful verification, we will confirm your cancellation request and initiate the refund. The booking amount should be credited back to your account within 7 working days.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Will I be charged any fee for cancellation?</h3>
                        <p>No, there is no fee for cancellation. Full booking amount will be refunded to your account within 7 working days.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>What are the mandatory documents that I need to carry to the dealership?</h3>
                        <p>The documents include any Govt. Photo ID proof of the person on which the bike will be registered (PAN Card, Driving License etc.), Any Govt. Address Proof of the person on which the bike will be registered, Two photographs, Additional documents for Finance options (Last 6-months bank statement, last year’s Income Tax Return Copies etc)<br><br>
                        The full list of documents is mentioned on our website in booking process and also sent to your email address in the booking confirmation email.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Where do I have to pay the balance amount? How much will it be?</h3>
                        <p>You will pay the balance amount directly to the assigned dealership during your visit to the showroom. The booking amount will be adjusted in the final on-road price of the vehicle.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>Can I change the color preference post visiting the dealership?</h3>
                        <p>Yes you can. However, please note that different color versions might have different pricing / availability / waiting periods and some color variants might even not be available from the manufacturers end. Dealership will provide the exact details on your visit to dealership.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom10">
                        <h3>How will I get the benefits of the offers?</h3>
                        <p>Depending upon the offer, you will get the benefit of some offers directly at the dealership, while taking delivery. Such offers may include cash-back benefit, exchange bonus, free bike accessories, free bike insurance etc. Other offers will be delivered to you when you inform us about completion of your purchase by filling in a short form, which will be available on our website and shall be sent to your email as link. We will verify the purchase with dealership, and immediately ship the offer benefits to the provided address.</p>
                    </div>
            
                    <div class="faq-content content-box-shadow content-inner-block-10 margin-bottom20">
                        <h3>Can I book more than one bikes on BikeWale?</h3>
                        <p>Yes. There is no limit to the number of bookings allowed on BikeWale.</p>
                    </div>
            
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    </form>
</body>
</html>
