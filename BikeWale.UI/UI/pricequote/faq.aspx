<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Pricequote.faq" %>
<html>
<head>
<meta charset="utf-8">
<title>Bike Booking FAQ's</title>
<!-- #include file="/UI/includes/globalStaticFiles.aspx"-->
<script runat="server">	
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<link href="/UI/css/bw-pq.css?<%= staticFileVersion %>" rel="stylesheet" />
    <script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>

<link rel="stylesheet"  href="<%= staticUrl%>/UI/css/bw-pq-new.css?<%= staticFileVersion %>" />

</head>
<body class="header-fixed-inner">
<div class="main-container">
    <div class="header-fixed">
        <div class="left-float margin-left50">
        	<a href="/" class="bwsprite bw-logo"></a>
        </div>
    	<div class="clear"></div>
    </div>
    <div class="container_12">
    	<h1 class=" margin-top15 margin-bottom10">Frequently Asked Questions</h1>
            <div class="faq-box margin-bottom20">
            	<ul>
                	<li>
                    	<h3>1. What is pre-booking? Why should I pre-book?</h3>
                        <p>Pre-booking is the only way to immediately block the offers that come with a particular bike. Once you pre-book, you become entitled to avail any offers mentioned against the bike model, along with securing the mentioned price for the bike. Since most offers are limited in number and available for a limited period, you should use pre-booking option to secure them before they expire.</p>
                    </li>
                    <li>
                    	<h3>2. How can I pre-book a bike on BikeWale?</h3>
                        <p>To pre-book a bike on BikeWale, you have to pay a fixed pre-booking amount online mentioned against the vehicle of your interest. This amount will be adjusted in the total on-road price of the bike. Your payment gets transferred to BikeWale through a fully-secured payment gateway.</p>
                    </li>
                    <li>
                    	<h3>3. What happens when I pre-book a bike on BikeWale?</h3>
                        <p>On successful receipt of the pre-booking payment, we will immediately send you a Pre-Booking Confirmation receipt on your email and SMS, which contains the following information:</p>
                            <div class="faq-list">
                                <ul>
                                    <li>a. Details of your payment</li>
                                    <li>b. Details of your selected bike (model, version, color) and the vehicle pricing</li>
                                    <li>c. Contact details of assigned dealership who will be responsible to complete further formalities and deliver the vehicle</li>
                                    <li>d. List of offers that you just secured by pre-booking</li>
                                </ul>
                            </div>
                    </li>
                    <li>
                    	<h3>4. What happens to the money I pay while pre-booking?</h3>
                        <p>The money that you pay for pre-booking is further transferred to the assigned bike dealership from which you are supposed to purchase the vehicle. This money will be adjusted against the final on-road price of the vehicle, i.e. you have to pay only the remaining amount to the dealership.</p>
                    </li>
                    <li>
                    	<h3>5. How is the dealership assigned for purchase? Can I change the dealership that has been assigned?</h3>
                        <p>We assign a dealership for your purchase within network of our partner dealerships. Dealership’s name and contact details are mentioned while pre-booking, so you know which dealer you are pre-booking your bike with. You will not be ableto change the dealership that has been assigned to you.</p>
                    </li>
                    <li>
                    	<h3>6. Will the dealer call me when I pre-book?</h3>
                        <p>Yes, you will get a call from the assigned dealership after you have pre-booked a bike on BikeWale to schedule your visit to dealership. You will be required to go to the dealership to complete the remaining procedure and take delivery of the vehicle. Please carry the print out of Pre-Booking Confirmation, required documents for RTO / Bank Loan, remaining payment amount etc to the dealership to avoid multiple visits. You can yourself call the dealership on the mentioned contact numbers to fix-up a visit. We do send dealership contact details on your email and SMS, apart from showing them on website.</p>
                    </li>
                    <li>
                    	<h3>7. Can I pre-book the bike for others, for example my family member?</h3>
                        <p>Yes, you can pre-book vehicle for your near and dear ones. </p>
                    </li>
                    <li>
                    	<h3>8. I have pre-booked a model. Now I want to change it to another model?</h3>
                        <p>We recommend you to thoroughly make up your mind for the vehicle of your choice before pre-booking and should avoid changing the it later. However, if you make a mistake and want to change the bike model, you can email us the <strong>‘Pre-Booking Change Request’</strong> on <a class="blue" href="mailto:contact@bikewale.com">contact@bikewale.com</a> with a reason for the same, clearly stating the pre-booking reference number, your mobile number (that you used while pre-booking), along with the new model you now want to pre-book. We will reply within 2 working days to confirm the change of pre-booking to the new model.</p>
                    </li>
                    <li>
                    	<h3>9. Will I be charged any fee for pre-booking feature?</h3>
                        <p>No, BikeWale does not charge any fee for pre-booking. The full amount you pay is counted against the purchase price of the vehicle and gets adjusted in the final payment.</p>
                    </li>
                    <li class="faq-content content-box-shadow content-inner-block-20 margin-bottom20" id="faq10">
                	    <h3>10. I want to cancel the booking. How can I do that? What happens to the refund?</h3>
                        <p>BikeWale strongly recommends you to carefully select your bike before booking. However, you still have the option to cancel a booking in case you change your mind. To cancel the booking, you will have to reach out to the dealership and inform about the cancellation request mentioning booking reference number and your mobile number (that you used while booking). For all valid cancellation requests, full booking amount will be refunded back to you by the dealership. Should you have any concerns regarding cancelling your booking, please feel free to write to us at <a class="blue" href="mailto:contact@bikewale.com">contact@bikewale.com</a>.<br>
                        <strong>The important conditions for cancellation</strong> are following:</p>
                        <ol>
                    	    <li>a. Cancellation must be requested <strong>within 15 days</strong> of booking the vehicle.</li>
                            <li>b. <strong>Cancellation will not be possible if you and dealership have proceeded further with purchase of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, submitting any documents, procurement of vehicle by the dealership etc.</li>
                            <li>c. If the dealer has initiated the procurement of the bike based upon your booking, cancellation will not be possible.</li>
                        </ol>
                    </li>                                      
                    <li>
                    	<h3>11. What are the mandatory documents that I need to carry to the dealership?</h3>
                        <p>The documents include any Govt. Photo ID proof of the person on which the bike will be registered (PAN Card, Driving License etc.), Any Govt. Address Proof of the person on which the bike will be registered, Two photographs, Additional documents for Finance options (Last 6-months bank statement, last year’s Income Tax Return Copies etc)</p>
                        <p>The full list of documents is mentioned on our website in pre-booking process and also sent to your email address in the pre-booking confirmation email.</p>
                    </li>
                    <li>
                    	<h3>12. Where do I have to pay the balance amount? How much will it be?</h3>
                        <p>You will pay the balance amount directly to the assigned dealership during your visit to the showroom. The pre-booking amount will be adjusted in the final on-road price of the vehicle.</p>
                    </li>
                    <li>
                    	<h3>13. Can I change the color preference post visiting the dealership?</h3>
                        <p>Yes you can. However, please note that different color versions might have different pricing / availability / waiting periods and some color variants might even not be available from the manufacturers end. Dealership will provide the exact details on your visit to dealership.</p>
                    </li>
                    <li>
                    	<h3>14. How will I get the benefits of the offers?</h3>
                        <p>Depending upon the offer, you will get the benefit of some offers directly at the dealership, while taking
                           delivery. Such offers may include cash-back benefit, exchange bonus, free bike accessories, free bike insurance etc. 
                           Other offers will be delivered to you when you inform us about completion of your purchase by filling in a short form, which will be available on our website and shall be sent to your email as link. We will verify the purchase with dealership, and immediately ship the offer benefits to the provided address.</p>
                    </li>
                    <li>
                    	<h3>15. Can I pre-book more than one bikes on BikeWale?</h3>
                        <p>Yes. There is no limit to the number of pre-bookings allowed on BikeWale.</p>
                    </li>
                    <li class="faq-content content-box-shadow content-inner-block-20 margin-bottom20" id="faq8">
                	    <h3>16. I have booked a model. Now I want to change it to another model?</h3>
                        <p>We recommend you to thoroughly make up your mind for the vehicle you wish to purchase before you proceed for booking. You should avoid changing it later. However, if you still wish to change the model that you have booked on BikeWale, you can reach out to the dealership mentioning the BikeWale booking reference number, your mobile number (that you used while booking), along with the new model you now wish to book. If the vehicle is available with the dealership then he may allow you to change the model. However, it is at the dealer's discretion whether this change is permitted. Should you have any concerns regarding changing the bike model with the dealership, please feel free to write to us at <a class="blue" href="mailto:contact@bikewale.com">contact@bikewale.com</a>.</p>
                    </li>                    
                    <li>
                    	<h3>17. When will I get my money refunded, once I request cancellation?</h3>
                        <p>Upon requesting a cancellation with the dealership, the dealership will initiate the refund process. The booking amount will be refunded to you within 15 working days.</p>
                    </li>
                    <li>
                    	<h3>18. Will I be charged any fee for cancellation?</h3>
                        <p>No, there is no fee for cancellation. Full booking amount will be refunded to you within 15 working days.</p>
                    </li>
                </ul>
            </div>
    </div>
</div>
</body>
</html>
