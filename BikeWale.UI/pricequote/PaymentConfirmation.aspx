<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.PaymentConfirmation" Trace="false" %>
<!Doctype html>
<html>
<head>
    <% 
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PriceQuote_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
<style>
    .map-box {width:290px;height:90px;}
</style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="bg-white header-fixed-inner">
            <div class="container">
                <div class="grid-12">
                    <div class="padding-bottom15 text-center">
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
   	
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>New Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="booking-success-heading font30 text-black margin-top10 margin-bottom10">Congratulations on your booking!</h1> 
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    
	    <section class="container">
        <div class="grid-12">
            <div class="content-box-shadow">
                <div class="booking-success-alert-container">
                    <div class="booking-success-alert">
                        <p class="text-bold">
                            <span class="inline-block booking-sprite booking-success-icon margin-right10"></span>
                            <span class="inline-block font18">We have received your payment of
                                <span class="fa fa-rupee margin-left5"></span>
                                <span class="font20"><%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(_objPQ.objBookingAmt.Amount)) %></span>
                            </span>
                        </p>
                    </div>
                </div>
                                                                                                                                                                               
            	<div class="grid-4 content-inner-block-10 inline-block margin-top10 margin-bottom10">
                	<div class="imageWrapper margin-top10">
                    	<img src="<%= Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath,_objPQ.objQuotation.HostUrl,Bikewale.Utility.ImageSize._310x174) %>"  alt="<%= bikeName %>" title="<%= bikeName %>">
                    </div>
                </div>
                <div class="grid-4 content-inner-block-10 inline-block margin-top10 margin-bottom10">
                	<h3 class="margin-bottom15"><%= bikeName%></h3>
                	<div class="font14">
                    	<table>
                        	<tbody>
                            	<tr>
                                	<td width="200" class="padding-bottom10">On road price:</td>
                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span> <%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(totalPrice)) %></td>
                                </tr>
                                <tr>
                                	<td>Advance booking:</td>
                                    <td align="right" class="text-bold"><span class="fa fa-rupee margin-right5"></span> <%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(_objPQ.objBookingAmt.Amount)) %></td>
                                </tr>
                                 <% if (IsInsuranceFree)
                                           {%>
                                <tr>
                                    <td>Free Insurance Amount:</td>
                                    <td align="right" class="text-bold"><span class="fa fa-rupee margin-right5"></span> <%=Bikewale.Common.CommonOpn.FormatPrice(insuranceAmount.ToString()) %></td>
                                </tr>
                                        <%}%>
                                <tr>
                                	<td colspan="2" class="padding-bottom10"><a id="cancellation-box" href="#">Hassle free cancellation policy</a></td>
                                </tr>
                                <tr>
                                	<td colspan="2"><div class="border-solid-top padding-bottom10"></div></td>
                                </tr>
                                <tr>
                                	<td>Balance amount:</td>
                                    <td align="right" class="font18 text-bold"><span class="fa fa-rupee margin-right5"></span> <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(totalPrice - _objPQ.objBookingAmt.Amount - insuranceAmount)) %></td>
                                </tr>
                                <tr>
                                	<td class="font12" colspan="2">*Balance amount payable at the dealership</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="grid-4 content-inner-block-10 inline-block border-solid-left margin-top10 margin-bottom10">
                    <!--  Dealer details starts here -->
                	<div class="booking-dealer-details ">
                        <h3 class="font18 margin-bottom15"><%= organization %></h3>
                        <p class="font14 text-light-grey margin-bottom10"><%= address %></p>
                        <p class="font14 margin-bottom10"><span class="fa fa-phone margin-right5"></span><%=contactNo %></p>
                        <div id="divMap" class="hide margin-top10 border-solid"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
   		</div>
   	</section>
  

    <section>
    	<div class="container">
        	<div class="grid-12 booking-details-section">
            	<h2 class="text-bold text-center margin-top50 margin-bottom20">Booking details</h2>
                <div class="content-box-shadow content-inner-block-20">
                	<div class="grid-6 booking-info-container border-solid-right">
                    	<div class="booked-bike-info border-solid-bottom padding-bottom10 padding-top10">
                            <p class="font14 margin-bottom10">Booking ID:<span class="font18 margin-left5 text-bold"><%= bookingRefNum %></span></p>
                            <p class="font14 margin-bottom5">Assigned dealership:<span class="margin-left5 text-bold"><%= organization %></span></p>
                            <p class="font14 margin-bottom5">Selected bike:<span class="margin-left5 text-bold"><%= bikeName %></span></p>
                            <p class="font14 margin-bottom5">Selected colour:<span class="margin-left5 text-bold"><%= objCustomer.objColor.ColorName %></span></p>
                        </div>
                        <div class="booked-used-info padding-top10 padding-bottom10">
                        	<p class="font16 margin-bottom10 text-bold">Personal information</p>
                            <p class="font14 margin-bottom5">Name:<span class="margin-left5 text-bold"><%= objCustomer.objCustomerBase.CustomerName %></span></p>
                            <p class="font14 margin-bottom5">Email ID:<span class="margin-left5 text-bold"><%= objCustomer.objCustomerBase.CustomerEmail %></span></p>
                            <p class="font14 margin-bottom5">Mobile no:<span class="margin-left5 text-bold"><%= objCustomer.objCustomerBase.CustomerMobile %></span></p>
                        </div>
                    </div>
                    <div class="grid-6 availed-offers-container">
                        <!-- offers container -->
                    	<div class="availed-offers-info margin-left10 padding-top10 padding-bottom10">
                        	<p class="font16 margin-bottom10 text-bold">
                                Availed exclusive Bikewale offers  
                        	</p>
                            <ul>
                                <asp:Repeater ID="rptOffers" runat="server">
                                    <ItemTemplate>
                                        <% if (IsInsuranceFree)
                                           {%>
                                        <li>Free Insurance for 1 year worth Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(insuranceAmount.ToString()) %>  at the dealership</li>
                                        <%
                                           }
                                           else
                                           {%>
                                        <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                        <% 
                                                    }
                                        %>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
     
    <section>
    	<div class="container margin-bottom20">
        	<div class="grid-12 alternative-section">
            	<h2 class="text-bold text-center margin-top50 margin-bottom20">What next!</h2>
        		<div class="content-box-shadow content-inner-block-20">
                	<div class="next-step-box">
                    	<img src="/images/next-steps-thumb.jpg" usemap="#nextSteps">
                        <map name="nextSteps">
                          <area shape="rect" id="required-document" coords="424,23,587,72" href="#" >
                        </map>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
		</div>
    </section>
    <div class="clear"></div>

        <!--cancellation popup starts here-->
        <div class="bw-popup bw-popup-lg hide cancellation-popup">
            <div class="popup-inner-container">

                <div class="cancel-policy-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                <h2>Cancellation & Refund Policy</h2>
                <div class="popup-inner-content cancellation-list margin-top10">
                    <ul>
                        <li><strong>a.</strong> Cancellation must be requested <strong>within 15 calendar days of pre-booking the vehicle.</strong> </li>
                        <li><strong>b.</strong> Please email your <strong>Pre-Booking Cancellation Request'</strong> to <a class="blue" href="mailto:contact@bikewale.com">contact@bikewale.com</a> with a valid reason for cancellation, clearly stating <strong>the booking reference number, your mobile number and email address (that you used while pre-booking).</strong></li>

                        <li><strong>c.</strong> <strong>Cancellation will not be possible if you and dealership have proceeded further with purchase 
                                of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, 
                                submitting any documents, procurement of the vehicle by the dealership etc.
                        </li>
                        <li><strong>d.</strong> If the dealer has initiated the procurement of the bike upon customer’s pre-booking, cancellation will not be possible.</li>

                        <li><strong>e.</strong> For all valid requests, we will process the refund of full pre-booking amount to customer's account within 7 working days.</li>
                    </ul>
                </div>
            </div>
        </div>
        <!--cancellation popup ends here-->
        
        <!--required documents popup ends here-->
        <div class="bw-popup bw-popup-lg hide required-doc">
            <div class="popup-inner-container">

                <div class="req-document-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                <h2>Required Documents for Registration / Loan</h2>
                <div class="popup-inner-content margin-top10">

                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                            <tr>
                                <td valign="top" width="310">
                                    <div class="grey-bullets">
                                        <p><strong>Mandatory Documents:</strong></p>
                                        <ul>
                                            <li>Two Color Photographs.</li>
                                            <li>PAN Card.</li>
                                        </ul>
                                        <p class="margin-top20"><strong>Identity Proof:</strong></p>
                                        <ul>
                                            <li>Passport / Voter ID / Driving License.</li>
                                        </ul>
                                        <p class="margin-top20"><strong>Additional Documents for Loan:</strong></p>
                                        <ul>
                                            <li>Last 6 Months Bank Statement.</li>
                                            <li>Salary Slip / Latest I.T. Return</li>
                                        </ul>
                                    </div>
                                </td>
                                <td>
                                    <div class="grey-bullets res-proof">
                                        <p><strong>Residential Address Proof:</strong></p>
                                        <p class="margin-top10">(Self-Owned House)</p>
                                        <ul>
                                            <li>Light Bil / Passport.</li>
                                            <li>Ration Card (Relation Proof).</li>
                                        </ul>
                                        <p class="margin-top10">(Rented House)</p>
                                        <ul>
                                            <li>Registered Rent Agreement + Police N.O.C.</li>
                                            <li>Rent Home Electricity Bill.</li>
                                            <li>Permanent Address Proof.</li>
                                            <li>Ration Card (Relation Proof).</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <!--required documents popup ends here-->
        

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
        <script type="text/javascript">
            $(document).ready(function () {

                var cityArea = "<%= objCustomer.objCustomerBase.cityDetails.CityName +'_'+ objCustomer.objCustomerBase.AreaDetails.AreaName %>  ";
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

                //$('#blackOut-window, .rsa-popup,.bw-popup').hide();

                //$('#btnPrintReceipt').click(function () {
                //    window.print();
                //    //printWindow = window.open("", "mywindow", "location=0,status=0,scrollbars=1");
                //    //printWindow.document.write("<div style='width:100%;text-align:left;'>");
                //    //printWindow.document.write("<input type='button' id='btnPrint' value='Print' style='width:100px' onclick='window.print()' />");
                //    //printWindow.document.write("<input type='button' id='btnCancel' value='Cancel' style='width:100px' onclick='window.close()' />");
                //    //printWindow.document.write(document.getElementById('news').innerHTML);
                //    //printWindow.document.write("</div>");
                //    //printWindow.document.close();
                //    //printWindow.focus();
                //});

                //$("#rsa").click(function () {
                //    $('#blackOut-window').show();
                //    $('.rsa-popup').show();
                //});

                //$('#RequiredDoc').click(function () {
                //    $("#blackOut-window").show();
                //    $('.bw-popup-lg').show();
                //});

                //$('.white-close-btn').click(function () {
                //    $("#blackOut-window").hide();
                //    $('.rsa-popup').hide();
                //});

                //$('.close-btn').click(function () {
                //    $("#blackOut-window").hide();
                //    $('.bw-popup').hide();
                //});

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

