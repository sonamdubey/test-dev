<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.BookingSummary" trace="false"%>

<%@ Import Namespace="Bikewale.Common" %>
<%
    title = BikeName + " Booking Summary";
    description = "Authorise dealer price details of a bike " + BikeName;
    keywords = BikeName + ", price, authorised, dealer,Booking ";
    //AdId = "1395986297721";
    //AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/pgheader.aspx" -->
<link href="/css/bw-pq.css?30july2015" rel="stylesheet" />
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq-new.css?23july2015" />
<link rel="stylesheet" type="text/css" href="/css/rsa.css?v=3.0"/>
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/jquery-ui-1.10.4.custom.min.css" />
<div class="main-container">
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
            <div class="bw-popup bw-popup-lg required-doc">
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
            <!--bw dpc popup starts here-->
            <div class="bw-popup bw-popup-md">
	            <div class="popup-inner-container">
    	             <div class="bw-sprite close-btn right-float"></div>
                    <h2>On-Road Price Breakup</h2>
        
                    <div class="popup-inner-content popup-price">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <asp:Repeater ID="rptQuote" runat="server">
                            <ItemTemplate>
                                <%-- Start 102155010 --%>
                                <%--<tr>
                                    <td width="240">
                                        <%# DataBinder.Eval(Container.DataItem,"CategoryName") %>
                                    </td>
                                    <td width="100" class="numeri-cell" align="right">
                                        <span id="Span1"><span class="WebRupee">Rs.</span><b>  <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></b></span>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td width="240">
                                        <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                    </td>
                                    <td width="100" class="numeri-cell" align="right">
                                        <span id="Span1"><span class="WebRupee">Rs.</span><b>  <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></b></span>
                                    </td>
                                </tr>
                                <%-- End 102155010 --%>
                            </ItemTemplate>     
                        </asp:Repeater> 
                        <tr><td colspan="2"><div class="dotted-hr"></div><td></tr>
                        <%-- Start 102155010 --%>
                        <%--<tr>
                            <td class="price2">Total On Road Price</td>
                            <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs</span> <%= Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></td>
                        </tr>                        --%>
                                <%
                       if (IsInsuranceFree)
                       {
                           %>
                        <tr>
                            <td class="">Total On Road Price</td>
                            <td width="100" class="numeri-cell" align="right"><span class="WebRupee">Rs</span><span style="text-decoration: line-through"><%= Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></span></td>
                        </tr>                        
                        <tr>
                            <td class="">Minus Insurance</td>
                            <td width="100" class="numeri-cell" align="right"><span class="WebRupee">Rs</span><%= Bikewale.Common.CommonOpn.FormatPrice(insuranceAmount.ToString()) %></td>
                        </tr>
                        <tr>
                            <td class="price2">BikeWale On Road (after insurance offer)</td>
                            <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs</span> <%= Bikewale.Common.CommonOpn.FormatPrice((TotalPrice - insuranceAmount).ToString()) %></td>
                        </tr>
                        <%
                       }
                       else
                       {
                           %>
                        <tr>
                            <td class="price2">Total On Road Price</td>
                            <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs</span> <%= Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></td>
                        </tr>                        
                        <%
                       }
                                     %>                                
                                <%-- End 102155010 --%>
                    </table>
                    <ul class="std-ul-list">
                        <asp:Repeater id="rptDisclaimer" runat="server">
                            <ItemTemplate>
                                <li><i><%# Container.DataItem %></i></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    </div>
                </div>
            </div>
            <!--bw dpc popup ends here-->
            <!--cancellation popup starts here-->
            <div class="bw-popup bw-popup-lg cancellation-popup">
    	        <div class="popup-inner-container">
                <div class=" bw-sprite close-btn right-float"></div>
        	        <h2>Cancellation & Refund Policy</h2>
                    <div class="popup-inner-content cancellation-list">
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
	<div class="container_12">
    	<div class="grid_8 margin-top10">
        	<h1 class="margin-bottom5">Pre-Book to Avail Offer</h1>
				<!--make payment div starts here-->
                <div class="inner-content">
                    <%-- Start 102155010 --%>
            		<%--<h2 class=" margin-bottom10 payment-pg-heading"><%=BikeName %> (Color: <%= color%>)</h2>--%>
                    <%-- End 102155010 --%>
                    <%if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0){ %>
                        <div class="bw-offer-box" id="divOffers">
                            <h2><%= IsInsuranceFree ? "BikeWale Ganapati Offer" : "Exclusive BikeWale Offers"%></h2>
                            <div class="margin-top5 margin-left5">
                                  <asp:Repeater ID="rptOffers" runat="server">
                                        <HeaderTemplate>
                                                <ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%-- Start 102155010 --%>                              
                                                    <%--<li style="<%# DataBinder.Eval(Container.DataItem,"OfferCategoryId").ToString() == "3" ? "display:none;" : ""%>"><%# DataBinder.Eval(Container.DataItem,"OfferText")%> </li>--%>
                                            <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                            <%-- End 102155010 --%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%-- Start 102155010 --%>
                                            <%--<li>
                                        	<p>Get one FREE Helmet worth Rs. 1500 from the following available options</p>
                                            <div class="bw-offer-item">
                                            	<ul>
                                                	<li>
                                                    	<div class="offer-pic"><img src="http://img.carwale.com/bikewaleimg/images/bikebooking/images/offer-list-pic1.jpg"></div>
                                                        <p class="margin-top10"><strong>Vega Cruiser Open Face Helmet (Size: M)</strong></p>
                                                        <div class="item-specification margin-top10">
                                                        	<ul>
                                                            	<li>- Scratch & Crack Resistant</li>
                                                                <li>- Texture finish</li>
                                                                <li>- UV protection visor</li>
                                                                <li>- Color: Red</li>
                                                            </ul>
                                                        </div>
                                                    </li>
                                                    <li>
                                                    	<div class="offer-pic"><img src="http://img.carwale.com/bikewaleimg/images/bikebooking/images/offer-list-pic2.jpg"></div>
                                                        <p class="margin-top10"><strong>Replay Dream Plain Flip-up Helmet (Size: M)</strong></p>
                                                        <div class="item-specification margin-top10">
                                                        	<ul>
                                                            	<li>- Dual Full-cum-open face</li>
                                                                <li>- Hard coated visor</li>
                                                                <li>- Superior paint finish</li>
                                                                <li>- Color: Matt Cherry Red</li>
                                                            </ul>
                                                        </div>
                                                    </li>
                                                    <li>
                                                    	<div class="offer-pic"><img src="http://img.carwale.com/bikewaleimg/images/bikebooking/images/offer-list-pic3.jpg"></div>
                                                        <p class="margin-top10"><strong>Vega Cliff Full Face Helmet (Size: M)</strong></p>
                                                        <div class="item-specification margin-top10">
                                                        	<ul>
                                                            	<li>- ABS shell</li>
                                                                <li>- Scratch resistant</li>
                                                                <li>- Lightweight & compact</li>
                                                                <li>- Color: Black</li>
                                                            </ul>
                                                        </div>
                                                    </li>
                                                    <div class="clear"></div>
                                                </ul>
                                            </div>
                                        </li>
                                                    <li>Get one year of <a target="_blank" href="/Absure Bike RSA.pdf">Bike Roadside Assistance</a>  absolutely FREE. <a id="rsa" class="blue">How to avail the offer?</a></li>--%>
                                            <%-- End 102155010 --%>
                                                </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                            </div>
                        </div>
                    <%} %>
                    	
                    <div class="bw-payment-box">
                        <ul>
                            <li>
                                <p class="font14">Total On-Road Price </p>
                                <%-- Start 102155010 --%>
                                <% if(insuranceAmount == 0){ %>
                                <p class="font14"><strong><span class="WebRupee">Rs</span><%=TotalPrice %></strong></p>
                                <%}
                                  else{ %>
                                <p class="font14"><strong><span class="WebRupee">Rs</span><%=TotalPrice - insuranceAmount %></strong></p>
                                <%} %>
                                <%-- End 102155010 --%>
                                <p><a  id="ViewBreakup" class="blue" href="#">View breakup</a></p>
                            </li>
                                                        <li>
                                <p class="font14">Booking Amount </p>
                                <p class="font14"><strong><span class="WebRupee">Rs</span><%=BooingAmt %></strong></p>
                                <p><a id="cancellation-box" class="blue" href="#">Hassle-free Cancellation</a></p>
                            </li>
                            <li>
                                <p class="font14">Remaining Amount </p>
                                <%-- Start 102155010 --%>
                                <% if(insuranceAmount == 0){ %>
                                <p class="font14"><strong><span class="WebRupee">Rs</span><script>document.write('<%=TotalPrice %>' - '<%=BooingAmt %>');</script></strong></p>
                                <%}
                                  else{ %>
                                <p class="font14"><strong><span class="WebRupee">Rs</span><script>document.write('<%=TotalPrice %>' - '<%=BooingAmt %>' - '<%=insuranceAmount%>');</script></strong></p>
                                <%} %>
                                <%-- End 102155010 --%>
                                <p>(To be paid at the dealership)</p>
                            </li>
                            <div class="clear"></div>
                        </ul>
                    </div>
                    <div class="mid-box center-align"><asp:Button id="btnMakePayment" class="action-btn" Text="Make Payment" runat="server"/></div>
                </div>
                <!--make payment div ends here-->
                <!--steps starts here-->
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
                                    <p class=" left-align">Visit dealership and pay the remaining amount. Be ready with all <a class="blue" id="RequiredDoc" href="#">required documents</a>.</p>
                                </li>
                                <li>
                                    <h2>Buy Your Bike</h2>
                                    <p><span class="bw-sprite buy-your-bike"></span></p>
                                    <p class=" left-align">Avail offer benefit and take delivery of your bike.</p>
                                </li>
                            </ul>
                        	<div class="clear"></div>
                		</div>
                <!--steps end here-->
        		</div>
                <div class="margin-bottom20">
                	<ul class="grey-bullets">
                    	<li>
                        	<span>In case of cancellation of booking, you can claim a refund from us.</span>
                        </li>
                        <li>
                        	<span>For more information, read <a class="blue" href="/pricequote/faq.aspx" target="_blank">FAQs</a>.</span>
                        </li>
                    </ul>
                </div>
        	</div>
    
            <div class="grid_4 right-grid">
                <!--Dealer detailes div stsrts here-->
                <div id="divControl">
                    <%if (objCustomer != null && objCustomer.objCustomerBase != null){ %>
                    <div class="inner-content">
                	    <h2 class="payment-pg-heading">Contact Details <!--<span class="edit-box edit-box-alignment">--<span class=" margin-right5 bw-sprite edit-pencil"></span>Edit</span>--></h2>
                        <div class="clear"></div>
                        <div class="edit-details margin-top10">
                    	    <p class="margin-bottom10">Name: <span><strong><%=objCustomer.objCustomerBase.CustomerName %></strong></span></p>
                            <p class="margin-bottom10">Mobile Number: <span><strong><%=objCustomer.objCustomerBase.CustomerMobile %></strong></span></p>
                            <p>Email ID: <span><strong><%=objCustomer.objCustomerBase.CustomerEmail %></strong></span></p>
                        </div>
                    </div>
                    <%} %>
                    <div class="inner-content">
                	    <h2 class="payment-pg-heading margin-bottom10">Authorised Dealer Details</h2>
                    	    <div>
                        	    <h3><%=organization %></h3>
                                <p><%=address %>.</p>
                            </div>
                    </div>
                </div>
                <!--Dealer detailes div ends here-->
            </div>
    </div>
</div>
</form>
<script type="text/ecmascript">
    $(document).ready(function () {

        //push tags
        $("#btnMakePayment").click(function () {
            dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","")%>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Make_Payment' });
        });

        $('.bw-popup, #blackOut-window,').hide();
        $('#ViewBreakup').click(function () {
            $("#blackOut-window").show();
            $('.bw-popup-md').show();
        });

        $('#RequiredDoc').click(function () {
            $("#blackOut-window").show();
            $('.bw-popup-lg.required-doc').show();
        });

        $("#rsa").click(function () {
            $('#blackOut-window').show();
            $('.rsa-popup').show();
        });

        $('#cancellation-box').click(function () {
            $("#blackOut-window").show();
            $('.cancellation-popup').show();
        });

        $('.close-btn').click(function () {
            $("#blackOut-window").hide();
            $('.cancellation-popup').hide();
        });
        $('.white-close-btn').click(function () {
            $("#blackOut-window").hide();
            $('.rsa-popup').hide();
        });

        $('.close-btn').click(function () {
            $("#blackOut-window").hide();
            $('.bw-popup').hide();
        });
    });

</script>
    </div>
</body>
</html>
