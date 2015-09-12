<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.BookingSummary_New" %>

<!doctype html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->  
</head>
<body class="bg-light-grey">
    <form runat="server">
    <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <link href="/m/css/bwm-booking.css" rel="stylesheet" type="text/css" />   

        <section class="container bg-white box-shadow padding-bottom20 margin-bottom10 clearfix"><!--  Discover bikes section code starts here -->
        <div class="grid-12">
                <div class="imageWrapper margin-top10">
                    <img src="http://imgd8.aeplcdn.com/227x128//bikewaleimg/models/490b.jpg?20140909123254" alt="" title="">
                </div>
                <div class="margin-top10">
                	<h3 class="margin-bottom15">Pulsar 150 AS</h3>
                	<div class="font14">
                    	<table>
                        	<tbody>
                            	<tr>
                                	<td width="200" class="padding-bottom10">On road price:</td>
                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span>1,25,657</td>
                                </tr>
                                <tr>
                                	<td>Advance booking:</td>
                                    <td align="right" class="text-bold"><span class="fa fa-rupee margin-right5"></span>3,000</td>
                                </tr>
                                <tr>
                                	<td colspan="2" class="padding-bottom10"><a href="#">Hassle free cancellation policy</a></td>
                                </tr>
                                <tr>
                                	<td colspan="2"><div class="border-solid-top padding-bottom10"></div></td>
                                </tr>
                                <tr>
                                	<td>Balance amount:</td>
                                    <td align="right" class="font18 text-bold"><span class="fa fa-rupee margin-right5"></span>1,25,657</td>
                                </tr>
                                <tr>
                                	<td class="font12 text-medium-grey">*Balance amount payable at the dealership</td>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="clear"></div>
   		</div>
   	</section>
    
    <section id="offerSection" class="container bg-white box-shadow margin-bottom20 clearfix"><!--  Don't know which car to buy section code starts here -->
        <div class="grid-12">
        	<h2 class="margin-top30 margin-bottom20 text-center">Avail great offers in 3 simple steps</h2>
            <div class="booking-wrapper">
                
                <div class="bike-to-buy-tabs booking-tabs">
                    <ul class="margin-bottom20">
                        <li class="first">
                            <div id="personal-info-tab" class="bike-booking-part active-tab text-bold" data-tabs-buy="personalInfo">
                                <div class="bike-booking-image">
                                    <span class="booking-sprite buy-icon personalInfo-icon-selected">
                                    </span>
                                </div>
                            </div>
                        </li>
                        <li class="middle">
                            <div id="customize-tab" class="bike-booking-part disabled-tab" data-tabs-buy="customize">
                                <div class="bike-booking-image">
                                    <span class="booking-sprite buy-icon customize-icon-grey">
                                    </span>
                                </div>
                            </div>
                        </li>
                        <li class="last">
                            <div id="confirmation-tab" class="bike-booking-part disabled-tab" data-tabs-buy="confirmation">
                                <div class="bike-booking-image">
                                    <span class="booking-sprite buy-icon confirmation-icon-grey"></span>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
                
                <div id="personalInfo" class="padding-bottom20">
                    <p class="font18 text-center">
                        <span class="iconTtl text-bold">Personal Information</span>
                    </p>
                    <p class="font14 text-center margin-top20">Please provide us contact details for your booking</p>
                    <div class="personal-info-form-container margin-top10">
                    	<div class="form-control-box personal-info-list">
                            <input type="text" class="form-control get-first-name" placeholder="First name"
                            id="getFirstName">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                        </div>
                        <div class="form-control-box personal-info-list">
                            <input type="text" class="form-control get-last-name" placeholder="Last name"
                            id="getLastName">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your last name</div>
                        </div>
                        <div class="form-control-box personal-info-list">
                            <input type="text" class="form-control get-email-id" placeholder="Email address"
                            id="getEmailID">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                        </div>
                        <div class="form-control-box personal-info-list">
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no."
                            id="getMobile">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                        </div>
                        <div class="clear"></div>
                        <button class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn">Next</button>
                    </div>
                    <div class="mobile-verification-container margin-top20 hide">
                        <p class="font12 text-center margin-bottom10 padding-left15 padding-right15">Please confirm your contact details and enter the OTP for mobile verfication</p>
                        <div class="form-control-box  padding-left15 padding-right15">
                            <input type="text" class="form-control get-otp-code text-center" placeholder="Enter OTP" id="getOTP">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter a valid OTP</div>
                        </div>
                        <div class="clear"></div>
                        <button class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</button>
                    </div>
                </div>
                 
                <div id="customize" class="hide">
                    <p class="font18 text-center">
                        <span class="iconTtl text-bold">Customize</span>
                    </p>
                    <p class="font14 text-center margin-top20 varient-heading-text">Choose your variant</p>
                    <ul class="varientsList margin-top10">
                    	<li>
                        <div class="clear text-left">
                            <div class="varient-item border-solid content-inner-block-10 rounded-corner2">
                                <div class="grid-12 alpha margin-bottom10">
                                    <h3 class="font16">Pearl Sunbeam White-ABS</h3>
                                    <p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>
                                </div>
                                <div class="grid-12 alpha omega">
                                    <p class="font18"><span class="fa fa-rupee margin-right5"></span>1,67,673</p>
                                    <p class="font12 text-green">Now available</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        </li>
                        <li>
                        <div class="clear text-left">
                            <div class="varient-item border-solid content-inner-block-10 rounded-corner2">
                                <div class="grid-12 alpha margin-bottom10">
                                    <h3 class="font16">Pearl Sunbeam White-ABS</h3>
                                    <p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>
                                </div>
                                <div class="grid-12 alpha omega">
                                    <p class="font18"><span class="fa fa-rupee margin-right5"></span>1,67,673</p>
                                    <p class="font12 text-light-grey">Waiting of 60 days</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        </li>
                        <li>
                        <div class="clear text-left">
                            <div class="varient-item border-solid content-inner-block-10 rounded-corner2">
                                <div class="grid-12 alpha margin-bottom10">
                                    <h3 class="font16">Pearl Sunbeam White-ABS</h3>
                                    <p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>
                                </div>
                                <div class="grid-12 alpha omega">
                                    <p class="font18"><span class="fa fa-rupee margin-right5"></span>1,67,673</p>
                                    <p class="font12 text-light-grey">Ex-showroom, Delhi</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        </li>
                        <li>
                        <div class="clear text-left">
                            <div class="varient-item border-solid content-inner-block-10 rounded-corner2">
                                <div class="grid-12 alpha margin-bottom10">
                                    <h3 class="font16">Pearl Sunbeam White-ABS</h3>
                                    <p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>
                                </div>
                                <div class="grid-12 alpha omega">
                                    <p class="font18"><span class="fa fa-rupee margin-right5"></span>1,67,673</p>
                                    <p class="font12 text-green">Now available</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        </li>
                     </ul>
                    <div class="clear"></div>
                    <div class="border-solid-top margin-bottom20"></div>
                    
                    <!-- colours code starts here -->
                    <div class="colours-wrap margin-bottom20">
                        <h2 class="margin-top30 margin-bottom20 text-center">Colours</h2>
                        <div class="jcarousel-wrapper">
                            <div class="jcarousel">
                                <ul class="text-center">
                                    <li class="available-colors">
                                        <div class="color-box red"><span class="ticked hide"></span></div>
                                        <p class="font16 text-medium-grey">Dazzling Red</p>
                                    </li>
                                    <li class="available-colors">
                                        <div class="color-box blue"><span class="ticked hide"></span></div>
                                        <p class="font16 text-medium-grey">Dazzling Blue</p>
                                    </li>
                                    <li class="available-colors">
                                        <div class="color-box red"><span class="ticked hide"></span></div>
                                        <p class="font16 text-medium-grey">Dazzling Red</p>
                                    </li>
                                    <li class="available-colors">
                                        <div class="color-box blue"><span class="ticked hide"></span></div>
                                        <p class="font16 text-medium-grey">Californoia Blue</p>
                                    </li>
                                    <li class="available-colors">
                                        <div class="color-box red"><span class="ticked hide"></span></div>
                                        <p class="font16 text-medium-grey">Dazzling Red</p>
                                    </li>
                                    <li class="available-colors">
                                        <div class="color-box red"><span class="ticked hide"></span></div>
                                        <p class="font16 text-medium-grey">Dazzling Red</p>
                                    </li>
                                </ul>
                             </div>
                             <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination"></p>
    
                          </div>
                    </div>
                    
                    <p class="font12 margin-bottom20">* Colours are subject to availabilty, can be selected at the dealership</p>
                    <button class="customize-submit-btn btn btn-full-width btn-orange margin-bottom20">Next</button>
                    <div class="clear"></div>
                    <!--<div class="btn btn-link btn-full-width customizeBackBtn">Back</div>-->
                </div>
                 
                <div id="confirmation" class="hide">
                	<p class="font18 text-center">
                        <span class="iconTtl text-bold">Confirmation</span>
                    </p>
                    <!--<p class="font14 text-center margin-top20">Choose your variant</p>-->
                    <div class="feedback-container margin-top30">
                    	<p class="text-bold font16">Congratulations!</p>
                        <p class="margin-bottom20">Hi <span>Amit Choubey,</span></p>
                        <p class="margin-bottom15" style="line-height:22px;">you can now book your bike by just paying <span class="font22"><span class="fa fa-rupee margin-right5"></span><span class="text-bold font24">3000</span></span></p>
                        <p class="margin-bottom15">You can pay that booking amount using a Credit Card/Debit Card/Net Banking.</p>
                        <p> 
Book your bike online at BikeWale and make the balance payment at the dealership to avail great offers.</p>
<p>For further assistance call toll free on <span class="text-bold">1800 456 7890.</span></p>
                    </div>
                    <div class="btn btn-full-width btn-orange margin-top20 margin-bottom10">Pay Now</div>
                    <div class="clear"></div>
                    <!--<div class="btn btn-full-width btn-link confirmationBackBtn">Back</div>-->
                 </div>
                 
                 <button state="personalInfo" id="btnRecommend" class="hide margin-top30 budget-price-next-btn btn btn-full-width btn-orange text-uppercase margin-bottom30">next</button>
                 
            </div>
        </div>
        <div class="clear"></div>   
    </section>
    
    
    <section><!--  toll-free code starts here -->
        <div class="container">
        	<div class="grid-12">
            	<div class="content-box-shadow content-inner-block-15 margin-bottom15 text-medium-grey text-center">
                	<a href="tel:1800 457 9781" class="font20 text-grey call-text-green" style="text-decoration:none;"><span class="fa fa-phone text-green margin-right5"></span> 1800 457 9781</a>
                </div>
			</div>
       </div>
       <div class="clear"></div>   
    </section>
    
    <section><!--  What next code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-center margin-top30 margin-bottom20">What next!</h2>
            	<div class="what-next-box content-box-shadow content-inner-block-10 margin-bottom15 font16">
                	<ul>
                    	<li>
                        	<div class="inner-thumb table-cell">
                            	<div class="whatnext-sprite get-dealership"></div>
                            </div>
                            <p class="table-cell text-bold padding-left10">Get in touch with dealership</p>
                        </li>
                        <li>
                        	<div class="inner-thumb table-cell">
                            	<div class="whatnext-sprite show-id"></div>
                            </div>
                            <p class="table-cell text-bold padding-left10">Show them your Booking Id</p>
                        </li>
                        <li>
                        	<div class="inner-thumb table-cell">
                            	<div class="whatnext-sprite submit-list"></div>
                            </div>
                            <p class="table-cell text-bold padding-left10"><a href="#">Submit list of required documents</a></p>
                        </li>
                        <li>
                        	<div class="inner-thumb table-cell">
                            	<div class="whatnext-sprite pay-balance"></div>
                            </div>
                            <p class="table-cell text-bold padding-left10">Pay balance amount & get best deals</p>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>

    <!-- #include file="/includes/footerBW_Mobile.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script src="/m/src/bwm-booking.js" type="text/javascript"></script>
 </form>
</body>
</html>