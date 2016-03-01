<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.PaymentConfirmation" Trace="false" %>
<!doctype html>
<html>
<head>
    <%
        title = bikeName + " Booking Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
     <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

<style>
    .map-box {width:290px;height:90px;}
</style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->               
    <section class="container bg-white box-shadow padding-bottom20 margin-bottom10 clearfix"><!--  Discover bikes section code starts here -->
        <div class="grid-12">
                <div class="imageWrapper margin-top10">
                   <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath,_objPQ.objQuotation.HostUrl,Bikewale.Utility.ImageSize._310x174) %>"  alt="<%= bikeName %>" title="<%= bikeName %>">
                </div>
                <div class="margin-top10">
                	<h3 class="margin-bottom15"><%= bikeName%></h3>
                	<div class="font14">
                    	<table>
                        	<tbody>
                            	<tr>
                                	<td width="200" class="padding-bottom10">On road price:</td>
                                    <td align="right" class="padding-bottom10 text-bold"><span class="bwmsprite inr-xsm-icon margin-right5"></span> <%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(totalPrice)) %></td>
                                </tr>
                                <tr>
                                	<td>Advance booking:</td>
                                    <td align="right" class="text-bold"><span class="bwmsprite inr-xsm-icon margin-right5"></span> <%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(_objPQ.objBookingAmt.Amount)) %></td>
                                </tr>
                                 <% if (_objPQ.objQuotation.discountedPriceList != null && _objPQ.objQuotation.discountedPriceList.Count > 0)
                                           {%>
                                <asp:Repeater ID="rptDiscount" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName")%></td>
                                            <td align="right" class="text-bold"><span class="bwmsprite inr-xsm-icon margin-right5"></span> <%# Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))%> </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                        <%}%>
                                <tr>
                                	<td colspan="2" class="padding-bottom10"><a id="cancellation-box" href="#">Hassle free cancellation policy</a></td>
                                </tr>
                                <tr>
                                	<td colspan="2"><div class="border-solid-top padding-bottom10"></div></td>
                                </tr>
                                <tr>
                                	<td>Balance amount:</td>
                                    <td align="right" class="font18 text-bold"><span class="bwmsprite inr-xsm-icon margin-right5"></span> <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(totalPrice - _objPQ.objBookingAmt.Amount - totalDiscount)) %></td>
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
    
        <div class="container bg-white clearfix padding-top10 margin-top20 box-shadow">
            <div class="grid-12">
                <div class="confirmation-info text-center">
                    
                    <h1 class="text-black"><span class="inline-block booking-sprite booking-success-icon margin-right10"></span>Congratulations on your booking!</h1>
                    <p class="font16 padding-top25 padding-bottom10 text-bold">We have received your payment of </p>
                    <p class="font30 text-bold border-solid-bottom padding-bottom10"><span class="bwmsprite inr-xsm-icon margin-right5"></span><%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(_objPQ.objBookingAmt.Amount)) %></p>
                </div>
                <h2 class="padding-top15 padding-bottom20 text-center">Booking details</h2>
                <p class="font14 ">Booking ID: <span class="font18 text-bold"><%= bookingRefNum %></span></p>

                <div class="font14 padding-top20">
                    <p>Assigned dealership: </p>
                    <p class="text-bold"><%= organization %></p>
                </div>
                <div class="font14 padding-top20 padding-bottom20">
                    <p>Selected bike:</p>
                    <p class="text-bold"><%= bikeName %></p>
                </div>
                <% if (objCustomer.objColor != null && !String.IsNullOrEmpty(objCustomer.objColor.ColorName))
                   { %>
                <div class="font14 padding-bottom20">
                    <p>Selected colour: </p>
                    <p class="text-bold"><%= objCustomer.objColor.ColorName %></p>
                </div>
                <%} %>

                <p class="font16 text-bold padding-top20 border-solid-top">Personal information</p>
                <div class="font14 padding-top20">
                    <p>Name: </p>
                    <p class="text-bold"><%= objCustomer.objCustomerBase.CustomerName %></p>
                </div>
                <div class="font14 padding-top20">
                    <p>Email ID: </p>
                    <p class="text-bold"><%= objCustomer.objCustomerBase.CustomerEmail %></p>
                </div>
                <div class="font14 padding-top20 border-solid-bottom padding-bottom20">
                    <p>Mobile no: </p>
                    <p class="text-bold"><%= objCustomer.objCustomerBase.CustomerMobile %></p>
                </div>

                <%if(_objPQ!=null && _objPQ.objOffers!=null && _objPQ.objOffers.Count > 0) {%>
                    <p class="font16 text-bold padding-top20">Availed exclusive Bikewale offers </p>
                    <ul class="confirmation-offers">
                        <asp:Repeater ID="rptOffers" runat="server">
                            <ItemTemplate>
                                <% if (IsInsuranceFree)
                                   {%>
                                <li class="font14 padding-bottom10">Free Insurance for 1 year worth Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(insuranceAmount.ToString()) %>  at the dealership</li>
                                <%
                               }
                                   else
                                   {%>
                                <li class="font14 padding-bottom10"><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                <% 
                               }
                                %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                <%} %>

                
            </div>
        </div>
    
    
    	<div class="grid-12">
            <div class="content-box-shadow content-inner-block-10 bg-white clearfix margin-top20">
            <div class="font18 text-bold text-black padding-bottom10"><%= organization %></div>
            <div class="font14"><%= address %></div>
            <div class="font14 padding-top10"><span class="bwmsprite tel-grey-icon"></span> <%=contactNo %></div>
            <div id="divMap" class="hide border-solid margin-top10"></div>            
            </div>
        </div>
       
   
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
                            <p class="table-cell text-bold padding-left10"><a href="#" id="required-document">Submit list of required documents</a></p>
                        </li>
                        <li>
                        	<div class="inner-thumb table-cell">
                            	<div class="whatnext-sprite pay-balance"></div>
                            </div>
                            <p class="table-cell text-bold padding-left10">Pay balance amount &amp; get best deals</p>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
           <!--Cancellation & refund policy popup starts here-->
        <div class="bwm-popup cancellation-popup hide">
            <div class="popup-inner-container">
                <div class="cancel-policy-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                <h3>Cancellation & Refund Policy</h3>
                <div class="lower-alpha-list cancellation-list content-inner-block-10">
                    <ol>
                        <li>Cancellation must be requested <strong>within 15 calendar days of pre-booking the vehicle.</strong></li>
                        <li>Please email your <strong>‘Pre-Booking Cancellation Request’</strong> to <a href="mailto:contact@bikewale.com" class="blue-text">contact@bikewale.com</a> with a valid reason for cancellation, clearly stating the <strong>pre-booking reference number, your mobile number and email address (that you used while pre-booking).</strong></li>
                        <li><strong>Cancellation will not be possible if you and dealership have proceeded further with purchase of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, submitting any documents, procurement of vehicle by the dealership etc.</li>
                        <li>If the dealer has initiated the procurement of the bike upon customer’s pre-booking, cancellation will not be possible.</li>

                        <li>For all valid requests, we will process the refund of full pre-booking amount to customer’s account within 7 working days.</li>
                    </ol>
                </div>
            </div>
        </div>
        <!--Cancellation & refund policy popup ends here-->

        <!--Documents popup starts here-->
        <div class="bwm-popup required-doc hide">
            <div class="popup-inner-container">
                <div class="req-document-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                <h3 class="margin-bottom20">List of Required Documents</h3>
                <div class="f-bold margin-top-10 text-bold">Mandatory Documents:</div>
                <div class="doc-list">
                    <ul>
                        <li>Two Color Photographs.</li>
                        <li>PAN Card.</li>
                    </ul>
                </div>
                <div class="f-bold margin-top10 text-bold">Identity Proof:</div>
                <div class="doc-list">
                    <ul>
                        <li>Passport / Voter ID / Driving License.</li>
                    </ul>
                </div>
                <div class="f-bold margin-top10 text-bold">Additional Documents for Loan:</div>
                <div class="doc-list">
                    <ul>
                        <li>Last 6 Months Bank Statement.</li>
                        <li>Salary Slip / Latest I.T. Return.</li>
                    </ul>
                </div>
                <div class="f-bold margin-top20 margin-bottom10  text-bold">Residential Address Proof:</div>
                <div class="lightgray">(Self-Owned House)</div>
                <div class="doc-list">
                    <ul>
                        <li>Light Bil / Passport.</li>
                        <li>Ration Card (Relation Proof).</li>
                    </ul>
                </div>
                <div class="lightgray margin-top15">(Rented House)</div>
                <div class="doc-list">
                    <ul>
                        <li>Registered Rent Agreement + Police N.O.C..</li>
                        <li>Rent Home Electricity Bill.</li>
                        <li>Permanent Address Proof.</li>
                        <li>Ration Card (Relation Proof).</li>
                    </ul>
                </div>
            </div>
        </div>
        <!--Documents popup ends here-->
   <!-- #include file="/includes/footerBW_Mobile.aspx" -->
   <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
        <script>
            $(function () {

                var cityArea = "<%= objCustomer.objCustomerBase.cityDetails.CityName +'_'+ objCustomer.objCustomerBase.AreaDetails.AreaName %>";
                if (cityArea != undefined) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Payment_Confirmation', 'lab': cityArea });
                }

                var latitude = '<%=lattitude %>';
                var longitude = '<%= longitude%>';

                if (latitude > 0 && longitude > 0) {
                    $("#divMap").removeClass("hide").addClass("map-box");
                    var myCenter = new google.maps.LatLng(latitude, longitude);

                    function initialize() {
                        var mapProp = {
                            center: myCenter,
                            zoom: 16,
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        };

                        var map = new google.maps.Map(document.getElementById("divMap"), mapProp);

                        var marker = new google.maps.Marker({
                            position: myCenter,
                        });

                        marker.setMap(map);
                    }

                    google.maps.event.addDomListener(window, 'load', initialize);
                }

                $(document).delegate("#cancellation-box", "click", function (e) {
                    e.preventDefault();
                    $(".blackOut-window").show();
                    $('.cancellation-popup').show();
                });

                $(document).delegate(".cancel-policy-close-btn", "click", function () {
                    $(".cancellation-popup").hide();
                    unlockPopup();
                });

                $(document).delegate("#required-document", "click", function (e) {
                    e.preventDefault();
                    $(".blackOut-window").show();
                    $('.required-doc').show();
                });

                $(document).delegate(".req-document-close-btn", "click", function () {
                    $(".required-doc").hide();
                    unlockPopup();
                });


                function unlockPopup() {
                    $('body').removeClass('lock-browser-scroll');
                    $(".blackOut-window").hide();
                }

                $(document).delegate(".blackOut-window", "click", function () {
                    $(".cancellation-popup").hide();
                    $('.required-doc').hide();
                });
            });            
        </script>
  </form>
</body>
</html>
    
   


