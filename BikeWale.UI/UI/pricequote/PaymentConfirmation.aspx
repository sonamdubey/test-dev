<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.PaymentConfirmation" Trace="false" %>

<!Doctype html>
<html>
<head>
    <% 
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PQ_";
        isAd300x250BTFShown = false;
        isAd300x250Shown = false;
    %>
    <!-- #include file="/UI/includes/headscript.aspx" -->
    <link href="<%= staticUrl  %>/UI/css/booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <style>
        .map-box {width: 290px;height: 90px;}
        .inr-sm { width:8px; height:12px; background-position:-110px -468px; }
        .phone-grey-icon { width:11px; height:15px; background-position:-53px -444px; position:relative; top:0; }
    </style>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->

        <section class="bg-white">
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
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>New Bikes</li>
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
                                <span class="bwsprite inr-lg"></span>&nbsp;
                                    <span class="font20"><%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(_objPQ.objBookingAmt.Amount)) %></span>
                                </span>
                            </p>
                        </div>
                    </div>

                    <div class="grid-4 content-inner-block-10 inline-block margin-top10 margin-bottom10">
                        <div class="imageWrapper margin-top10">
                            <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath,_objPQ.objQuotation.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bikeName %>" title="<%= bikeName %>">
                        </div>
                    </div>
                    <div class="grid-4 content-inner-block-10 inline-block margin-top10 margin-bottom10">
                        <h3 class="margin-bottom15"><%= bikeName%></h3>
                        <div class="font14">
                            <table>
                                <tbody>
                                    <tr>
                                        <td width="200" class="padding-bottom10">On road price:</td>
                                        <td align="right" class="padding-bottom10 text-bold"><span class="bwsprite inr-sm margin-right5"></span><%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(totalPrice)) %></td>
                                    </tr>
                                    <tr>
                                        <td>Advance booking:</td>
                                        <td align="right" class="text-bold"><span class="bwsprite inr-sm margin-right5"></span><%=Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(_objPQ.objBookingAmt.Amount)) %></td>
                                    </tr>
                                    <% if (_objPQ.objQuotation.discountedPriceList != null && _objPQ.objQuotation.discountedPriceList.Count > 0)
                                           {%>
                                        <asp:Repeater ID="rptDiscount" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName")%></td>
                                                    <td align="right" class="text-bold"><span class="bwsprite inr-sm margin-right5"></span> <%# Bikewale.Common.CommonOpn.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))%> </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <%}%>
                                    <tr>
                                        <td colspan="2" class="padding-bottom10"><a id="cancellation-box" href="#">Hassle free cancellation policy</a></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="border-solid-top padding-bottom10"></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Balance amount:</td>
                                        <td align="right" class="font18 text-bold"><span class="bwsprite inr-lg margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(totalPrice - _objPQ.objBookingAmt.Amount - totalDiscount)) %></td>
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
                            <p class="font14 margin-bottom10"><span class="bwsprite phone-grey-icon margin-right5"></span><%=contactNo %></p>
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
                        <div class="grid-6 booking-info-container <%=(_objPQ != null && _objPQ.objOffers != null && _objPQ.objOffers.Count > 0) ? "border-solid-right" : "" %>">
                            <div class="booked-bike-info border-solid-bottom padding-bottom10 padding-top10">
                                <p class="font14 margin-bottom10">Booking ID:<span class="font18 margin-left5 text-bold"><%= bookingRefNum %></span></p>
                                <p class="font14 margin-bottom5">Assigned dealership:<span class="margin-left5 text-bold"><%= organization %></span></p>  
                                <p class="font14 margin-bottom5">Selected bike:<span class="margin-left5 text-bold"><%= bikeName %></span></p>
                                <% if (objCustomer.objColor != null && !String.IsNullOrEmpty(objCustomer.objColor.ColorName))
                                   { %>
                                <p class="font14 margin-bottom5">Selected colour:<span class="margin-left5 text-bold"><%= objCustomer.objColor.ColorName %></span></p>
                                <%} %>
                            </div>
                            <div class="booked-used-info padding-top10 padding-bottom10">
                                <p class="font16 margin-bottom10 text-bold">Personal information</p>
                                <p class="font14 margin-bottom5">Name:<span class="margin-left5 text-bold"><%= objCustomer.objCustomerBase.CustomerName %></span></p>
                                <p class="font14 margin-bottom5">Email ID:<span class="margin-left5 text-bold"><%= objCustomer.objCustomerBase.CustomerEmail %></span></p>
                                <p class="font14 margin-bottom5">Mobile no:<span class="margin-left5 text-bold"><%= objCustomer.objCustomerBase.CustomerMobile %></span></p>
                            </div>
                        </div>
                        <%if (_objPQ != null && _objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                          { %>
                            <div class="grid-6 availed-offers-container">
                                <!-- offers container -->
                                <div class="availed-offers-info margin-left10 padding-top10 padding-bottom10">
                                    <p class="font16 margin-bottom10 text-bold offertxt">
                                        Availed exclusive Bikewale offers
                                    </p>
                                    <% if(IsInsuranceFree) { %>
                                    <p>Free Insurance for 1 year worth Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(insuranceAmount.ToString()) %>  at the dealership</p>
                                    <% } else { %>
                                    <ul>
                                        <asp:Repeater ID="rptOffers" runat="server">
                                            <ItemTemplate>
                                               <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                                    <span class="tnc font9 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>">View terms</span>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <% } %>
                                </div>
                            </div>
                        <%} %>
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
                            <img src="/UI/image/next-steps-thumb.jpg" usemap="#nextSteps">
                            <map name="nextSteps">
                                <area shape="rect" id="required-document" coords="424,23,587,72" href="#">
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
                            <li><strong>a.</strong> Cancellation must be requested <strong>within 15 days of booking the vehicle.</strong></li>
                            <li><strong>b.</strong> To cancel the booking, you will have to reach out to the dealership and inform about the cancellation request mentioning booking reference number and your mobile number (that you used while booking).</li>
                            <li><strong>c.</strong> <strong>Cancellation will not be possible if you and dealership have proceeded further with purchase 
                                    of the vehicle.</strong> These conditions include payment of additional amount directly to the dealership, 
                                    submitting any documents, procurement of the vehicle by the dealership etc.
                            </li>
                            <li><strong>d.</strong> If the dealer has initiated the procurement of the bike upon customer’s booking, cancellation will not be possible.</li>

                            <li><strong>e.</strong> For all valid cancellation requests, full booking amount will be refunded back to you by the dealership within 15 working days.</li>
                            <li><strong>f.</strong> Should you have any concerns regarding cancelling your booking, please feel free to write to us at <a href="mailto:contact@bikewale.com">contact@bikewale.com</a>.</li>
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

        <!-- Terms and condition Popup start -->
            <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                <div class="fixed-close-btn-wrapper">
                    <div class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
                <h3>Terms and conditions</h3>
                <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                    <img class="lazy" data-original="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  src="" />
                </div>
                <div id="terms" class="breakup-text-container padding-bottom10 font14">
                </div>
                <div id='orig-terms' class='hide'>
                </div>
            </div>
            <!-- Terms and condition Popup Ends -->

        <!-- #include file="/UI/includes/footerBW.aspx" -->                                    
        <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&sensor=false"></script>
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

            $('.tnc').on('click', function (e) {
                LoadTerms($(this).attr("id"));
            });

            $('.termsPopUpCloseBtn ').on("click", function () {
                $('#termsPopUpContainer').hide();
                $(".blackOut-window").hide();
            });

            function LoadTerms(offerId) {
                $("div#termsPopUpContainer").show();
                $(".blackOut-window").show();
                if (offerId != 0 && offerId != null) {
                    $(".termsPopUpContainer").css('height', '150')
                    $('#termspinner').show();
                    $('#terms').empty();
                    $.ajax({
                        type: "GET",
                        url: "/api/Terms/?offerId=" + offerId,
                        dataType: 'json',
                        success: function (response) {
                            $('#termspinner').hide();
                            if (response != null)
                                $('#terms').html(response);
                        },
                        error: function (request, status, error) {
                            $("div#termsPopUpContainer").hide();
                            $(".blackOut-window").hide();
                        }
                    });
                } else {
                    $("#terms").load("/UI/statichtml/tnc.html");
                }

                $(".termsPopUpContainer").css('height', '500');
            }
            $('.blackOut-window').on("click", function () {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            });
        </script>
    </form>

</body>
</html>

