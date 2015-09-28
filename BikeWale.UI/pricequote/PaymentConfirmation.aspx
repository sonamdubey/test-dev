<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.PaymentConfirmation" Trace="false" %>
<%
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
%>
<!-- #include file="/includes/pgheader.aspx" -->
<link href="/css/bw-pq.css?<%= staticFileVersion %>" rel="stylesheet" />
<link href="/css/bw-pq-new.css?<%= staticFileVersion %>" rel="stylesheet" />
<link href="/css/rsa.css" rel="stylesheet" />
<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
<style>
    .map-box {width:250px;height:140px;}
</style>
<div id="blackOut-window"></div>
<div class="rsa-popup bw-popup">
    <!--header starts here-->
    <div class="rsa-header">
        <div class="bw-sprite white-close-btn right-float margin-top5"></div>
        <div class="left-float margin-right10 header-seperator">
            <img class="margin-right10" src="http://img1.carwale.com/bikewaleimg/images/bikebooking/images/rsa-logo.png" border="0">
                        
        </div>
        <div class=" left-float margin-top5">
            <h1>FREE Bike Roadside Assistance Offer</h1>
        </div>
        <div class="clear"></div>
    </div>
    <!--header ends here-->
    <!--inner content starts here-->
    <div class="popup-inner-content">
        <h2>Three simple steps to avail the offer:</h2>
        <div class="steps">
            <h2>Step 1:</h2>
            <p>Purchase your bike and get it registered with RTO.</p>
        </div>
        <div class="seperator"></div>
            
        <div class="steps">
            <h2>Step 2:</h2>
            <p>Email the following details to <a class="blue" href="#">contact@bikewale.com</a> with subject as <span class="color-text">"Free RSA Offer"</span>:</p>
                
                <div class="rsa-details margin-left20 margin-top10">
                    <ol>
                        <li>Mobile number and Email address used to avail the Dealer Price Certificate</li>
                        <li>Vehicle Registration Number (e.g. MH 06 AT 8875)</li>
                        <li>Full Name as per Vehicle Registration</li>
                        <li>Complete Communication Address as per Vehicle Registration</li>
                        <li>Date of vehicle Delivery</li>
                        <li>Name and address of the dealership from where the bike was purchased  </li>
                    </ol>
                </div>
        </div>
        <div class="seperator"></div>
            
        <div class="steps">
            <h2>Step 3:</h2>
            <p>On receipt of above details we will verify your purchase from the dealership and dispatch your<strong> FREE Bike Roadside Assistance Certificate</strong> on your email address within 5 working days.
        </p>
        </div>
    </div>
    <!--header starts here-->
</div>

            <!--required documents popup ends here-->
            <div class="bw-popup bw-popup-lg">
	            <div class="popup-inner-container">
    	            <div class="bw-sprite close-btn right-float"></div>
                    <h2>Required Documents for Registration / Loan</h2>
                        <div class="popup-inner-content">
            
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
<div class="main-container">
	<div class="container_12">
    	<div class="grid_8 margin-top10">
        	<h1 class="margin-bottom5">Congratulations! Your payment has been received.</h1>
            <div class="inner-content">
                <div>
                    <h3 class="margin-top5 <%= !String.IsNullOrEmpty(bookingRefNum) ? "" : "hide"  %>">Booking reference number: <%= bookingRefNum %></h3>
                	<p class="margin-top5"> Please see below your booking details. These details have been sent on your email and mobile.</p>
                </div>
            </div>
            
            <!--Get pq code starts here-->
                <div id="get-pq-new" class="inner-content">
                    <div id="div_ShowPQ">
                    	<h2 class="payment-pg-heading">Booked Bike</h2>
                        	<p class="margin-top10"><strong><%= bikeName%></strong></p>
                        	<%--<p><strong>Color: <%= objCustomer.objColor.ColorName %></strong></p>--%>
                                
                                <div class="bw-offer-box margin-top10">
                                    <h2><%= IsInsuranceFree ? "BikeWale Ganapati Offer" : "Exclusive Offers for BikeWale Customers"%></h2>
                                    <div class="margin-top5">
                                        <asp:Repeater ID="rptOffers" runat="server">
                                            <HeaderTemplate>
                                                    <ul>                                        
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                    <%--
                                                        <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%>   <%# DataBinder.Eval(Container.DataItem,"OfferCategoryId").ToString() == "3" ? "<a id=\"rsa\" class=\"blue\">How to avail the offer?</a>" : ""  %></li>--%>
                                                <%-- Start 102155010 --%>
                                                <%--<li><%# DataBinder.Eval(Container.DataItem,"OfferCategoryId").ToString() != "3" ? DataBinder.Eval(Container.DataItem,"OfferText") : ""%> </li>--%>
                                                <%
                                                    if (IsInsuranceFree)
                                                    {
                                                        %>
                                                <li>Free Insurance for 1 year worth Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(insuranceAmount.ToString()) %>  at the dealership</li>
                                                <%
                                                    }
                                                    else
                                                    {%>
                                                <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                                       <% 
                                                    }
                                                     %>                                                
                                                <%-- End 102155010 --%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%-- Start 102155010 --%>
                                                        <%--<li>Get one year of <a target="_blank" href="/Absure Bike RSA.pdf">Bike Roadside Assistance</a>  absolutely FREE. <a id="rsa" class="blue">How to avail the offer?</a></li>--%>
                                                   <%-- <li><a href="/Absure Bike RSA.pdf" target="_blank">Get RSA Details</a></li>--%>                                                
                                                <%-- End 102155010 --%>
                                                    </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                    </div>
                	<p class="margin-top10">Balance Amount Payable at Dealership: <span class="WebRupee">Rs.</span> <%=totalPrice - Convert.ToUInt32(Bikewale.Common.CommonOpn.FormatPrice(_objPQ.objBookingAmt.Amount.ToString())) - insuranceAmount %></p>
                </div>
            <!--Get pq code ends here-->
            <!--next steps starts here-->
            	<div class="inner-content">
                	<h2 class="payment-pg-heading">Next Steps</h2>
                    	<div class="margin-top10 bw-box">
                        	<ul>
                                <li>
                                    <h2>Reserve Offer</h2>
                                    <p class="margin-bottom5"><span class="bw-sprite reserve-offer"></span></p>
                                    <p class=" left-align">Make payment to reserve your offer and pre-book your bike.</p>
                                </li>
                                <li>
                                    <h2>Visit Dealership</h2>
                                    <p class="margin-bottom10"><span class="bw-sprite visit-dealership"></span></p>
                                    <p class=" left-align">Visit dealership and pay the remaining amount. Be ready with all <a id="RequiredDoc" class="blue" href="#">required documents</a>.</p>
                                </li>
                                <li>
                                    <h2>Buy Your Bike!</h2>
                                    <p><span class="bw-sprite buy-your-bike"></span></p>
                                    <p class=" left-align">Avail offer benefit and take delivery of your bike.</p>
                                </li>
                            </ul>
                        	<div class="clear"></div>
                		</div>
                <!--steps end here-->
        		</div>
            <!--next steps ends here-->
            <div class="mid-box margin-top15 center-align margin-bottom20"><input type="submit" class="action-btn text_white" id="btnPrintReceipt" value="Print Receipt" name="btnPrintReceipt" onClick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","")%>        ', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Print_Receipt' });"></div>
        </div>
        
        <div class="grid_4 right-grid">
            <div id="divControl">
                <div class="inner-content">
                	<h2 class="payment-pg-heading margin-bottom10">Authorised Dealer Details</h2>
                        <div>
                            <h3 class="margin-bottom5"><span class="bw-sprite dealer-search margin-right10"></span><%= organization %></h3>
                                <div class="margin-left20">
                                    <p><%= address %></p>
                                                        
                                    <%if (!String.IsNullOrEmpty(_objPQ.objDealer.WorkingTime)) {%>
                                    <div class="margin-top10"><span><b>Working Hours: </b></span><%=   _objPQ.objDealer.WorkingTime   %></div>
                                    <%} %>
                                    <%--<div class="margin-top10"><iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3770.9863217196075!2d72.99639100000005!3d19.064338999999993!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3be7c136b2c080cb%3A0x225353b221740ef0!2sCarWale!5e0!3m2!1sen!2sin!4v1418196092146" width="250" height="140" frameborder="0" style="border:0"></iframe></div>--%>
                                    <div class="margin-top10 margin-right20 map-content hide" id="divMapWindow">
                                          <div id="divMap"></div>
                                    </div>

                                    <!--<p class=" margin-top5 margin-bottom5"><a href="#" class="blue">Get Directions</a></p>-->
                                </div>
                            <div class="padding-left10 margin-top10">
                                <p class="margin-top5"><span class="bw-sprite call padding-left10 left-float padding-right10"></span><%=contactNo %></p>
                                <div class="clear"></div>
                            </div>
                        </div>
                </div>
                <div class="inner-content hide">
                	<h2 class="payment-pg-heading">Steps to Avail your Offer</h2>
                    	<div class="avail-offers-box margin-top10">
                        	<ul>
                            	<li>
                                	<div class="list-thumbnail">
                                    	<span class="bw-sprite reserve-offer"></span>	
                                    </div>
                                    <p>Offer reserved for you and bike has been pre-booked</p>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                	<div class="list-thumbnail">
                                    	<span class="bw-sprite buy-your-bike"></span>
                                    </div>
                                    <p>Visit dealership and pay the remaining booking amount</p>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                	<div class="list-thumbnail center-align">
                                		<span class="bw-sprite avail-offer"></span>
                                	</div>
                                	<p>Get offer on bike delivery</p>
                                	<div class="clear"></div>
                                </li>
                            </ul>
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {

        var cityArea = GetGlobalCityArea();
        if(cityArea != undefined){
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Payment_Confirmation', 'lab': cityArea });
        }

        $('#blackOut-window, .rsa-popup,.bw-popup').hide();

        $('#btnPrintReceipt').click(function () {
            window.print();
            //printWindow = window.open("", "mywindow", "location=0,status=0,scrollbars=1");
            //printWindow.document.write("<div style='width:100%;text-align:left;'>");
            //printWindow.document.write("<input type='button' id='btnPrint' value='Print' style='width:100px' onclick='window.print()' />");
            //printWindow.document.write("<input type='button' id='btnCancel' value='Cancel' style='width:100px' onclick='window.close()' />");
            //printWindow.document.write(document.getElementById('news').innerHTML);
            //printWindow.document.write("</div>");
            //printWindow.document.close();
            //printWindow.focus();
        });

        $("#rsa").click(function () {
            $('#blackOut-window').show();
            $('.rsa-popup').show();
        });

        $('#RequiredDoc').click(function () {
            $("#blackOut-window").show();
            $('.bw-popup-lg').show();
        });

        $('.white-close-btn').click(function () {
            $("#blackOut-window").hide();
            $('.rsa-popup').hide();
        });

        $('.close-btn').click(function () {
            $("#blackOut-window").hide();
            $('.bw-popup').hide();
        });

        var latitude = '<%=lattitude %>';
        var longitude = '<%= longitude%>';

        if (latitude > 0 && longitude > 0) {
            $("#divMapWindow").removeClass("hide");
            $("#divMap").addClass("map-box");
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
    });
</script>
    </div>
</form>
</body>
</html>