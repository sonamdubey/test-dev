<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.BookingSummary" %>

<%@ Import Namespace="Bikewale.Common" %>
<%
    title = BikeName + " Booking Summary";
    description = "Authorise dealer price details of a bike " + BikeName;
    keywords = BikeName + ", price, authorised, dealer,Booking ";
    //AdId = "1395986297721";
    //AdPath = "/1017752/BikeWale_New_";

%>
<!-- #include file="/includes/PaymentHeaderMobile.aspx" -->
<link rel="stylesheet"  href="/m/css/bw-new-style.css?<%= staticFileVersion %>" /> 
<link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
<script src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/jquery-ui-1.10.4.custom.min.js"></script>

<div class="padding5">
    <h1>Pre-Book to Avail Offer</h1>
        <div class="box1 new-line10">
            <h2 class="margin-bottom10 f-bold <%=_objPQ.objOffers.Count>0?"":"hide" %>"><%= BikeName %></h2>
            <%-- Start 102155010 --%>
            <%--<%if(!String.IsNullOrEmpty(objCustomer.objColor.ColorName)) {%>            
            <h4 class="f-bold">Color : <%= objCustomer.objColor.ColorName %></h4>            
            <% } %>--%>
            <%-- End 102155010 --%>
                    <!--Exciting Offers section starts here-->
                    <% if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0){ %>
                     <div class="bw-offer-box break-line new-line10 <%=_objPQ.objOffers.Count>0?"":"hide" %>" id="divOffers">
                        <h2 class="f-bold"><%= IsInsuranceFree ? "BikeWale Offer" : "Exclusive Offers for BikeWale Customers"%></h2>
                        <div class="margin-top5 margin-left5 new-line10">
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
                                        Get one Helmet from following options absolutely FREE!
                                        <div class="m-carousel m-fluid m-carousel-photos">            
                                            <div class="m-carousel-inner" style="transform: translate(-1059px, 0px);">
                                                <div class="m-item">
                                                    <div class="mainH font14 text-center">
                            	                        <p class="margin-bottom10"><strong>Option 1: Vega Cruiser Helmet</strong></p>
                                                        <img src="http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/offer-list-pic1.jpg" class="margin-bottom10"/>
                                	                        <div class="centerContent margin-top-5">
                                    	                        <ul>
                                        	                        <li>Scratch & Crack Resistant</li>
                                                                    <li> Texture finish</li>
                                                                    <li>UV protection visor</li>
                                                                    <li>Color: Red, Size: M</li>
                                                                </ul>
                                                            </div>
                                                    </div>
                                                </div>
                                                <div class="m-item">
                                                    <div class="mainH font14 text-center">
                            	                        <p class="margin-bottom10"><strong>Option 2: Replay Flip-up Helmet</strong></p>
                                                        <img src="http://imgd2.aeplcdn.com/0x0/bw/static/design15/old-images/d/offer-list-pic2.jpg" class="margin-bottom10"/>
                                	                        <div class="centerContent margin-top-5">
                                    	                        <ul>
                                        	                        <li>Dual Full-cum-open face</li>
                                                                    <li>Hard coated visor</li>
                                                                    <li>Superior paint finish</li>
                                                                    <li>Color: Matt Cherry Red,Size: M</li>
                                                                </ul>
                                                            </div>
                                                    </div>
                                                </div>
                                                <div class="m-item">
                                                    <div class="mainH font14 text-center">
                            	                        <p class="margin-bottom10"><strong>Option 3: Vega Cliff Full Face Helmet</strong></p>
                                                        <img src="http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/offer-list-pic3.jpg" class="margin-bottom10"/>
                                	                        <div class="centerContent margin-top-5">
                                    	                        <ul>
                                        	                        <li>ABS shell</li>
                                                                    <li>Scratch resistant</li>
                                                                    <li>Lightweight & compact</li>
                                                                    <li>Color: Black, Size: M</li>
                                                                </ul>
                                                            </div>
                                                    </div>
                                                </div>
                                            </div>                    
                                            <div class="m-carousel-controls m-carousel-hud">
                                                <a data-slide="prev" href="#" class="m-carousel-prev ui-link">Previous</a>
                                                <a data-slide="next" href="#" class="m-carousel-next ui-link">Next</a>
                                            </div>
                    
                                        </div>
                                        </li>
                                           <li>Get one year of <a target="_blank" href="/Absure Bike RSA.pdf"  style="text-decoration:underline">Bike Roadside Assistance</a>  absolutely FREE. <a id="rsa" target="_blank" href="/m/pricequote/rsa.aspx" style="color: #0056cc !important;">How to avail the offer?</a></li>--%>
                                        <%-- End 102155010 --%>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                        </div>
                    </div>
                    <%} %>
              <!--Exciting Offers section ends here-->
            <div class="new-line10">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                    <td height="40" align="left">Total On-Road Price <br /><a id="viewBreakup" class="blue-text font12">View breakup</a></td>
                    <% if(insuranceAmount == 0){ %>
                    <td height="40" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=TotalPrice %></td>
                        <%}else{ %>
                    <td height="40" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=TotalPrice - insuranceAmount %></td>
                        <%} %>
                    </tr>
                    <tr>
                    <td height="40" align="left">Booking Amount <br /><a id="cancellationLink" class="blue-text font12">Hassle-free Cancellation</a></td>
                    <td height="40" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=BooingAmt %></td>
                    </tr>
                    <tr>
                    <td height="40" align="left">Remaining Amount <br /><span class="font12">(To be paid at the dealership)</span></td>
                    <%-- Start 102155010 --%>
                    <% if(insuranceAmount == 0){ %>
                    <td height="40" align="right" class="f-bold"><span class="WebRupee">Rs.</span><script>document.write('<%=TotalPrice %>' - '<%=BooingAmt %>');</script></td>
                        <%}
                                  else{ %>
                    <td height="40" align="right" class="f-bold"><span class="WebRupee">Rs.</span><script>document.write('<%=TotalPrice %>' - '<%=BooingAmt %>' - '<%=insuranceAmount %>');</script></td>
                        <%} %>
                                <%-- End 102155010 --%>
                    </tr>
                </table>
            </div>
            <asp:Button data-role="none" class="rounded-corner5 margin-top15" runat="server" id="btnMakePayment" Text="Make Payment"/>
        </div>
    <div class="box1 new-line10">
        <h2 class="margin-bottom10 f-bold floatleft">Contact Details</h2>
        <!--<div class="edit-btn">
            <span class="bw-sprite edit-icon"></span>
            <span>Edit</span>
        </div>-->
        <div class="clear"></div>
        <div class="break-line margin-bottom10"></div>
        <%if(objCustomer.objCustomerBase != null) {%>
        <div class="contact-details">
            <div class="done-data margin-bottom10">Name: <span class="f-bold" id="name"><%=objCustomer.objCustomerBase.CustomerName %></span></div>
         <!--   <div class="rounded-corner5 selection-box edit-data">
                <input type="text" placeholder="Name" data-role="none" id="editName" />
            </div>-->
            <div class="done-data margin-bottom10">Mobile Number: <span class="f-bold" id="mob"><%=objCustomer.objCustomerBase.CustomerMobile %></span></div>
        <!--    <div class="rounded-corner5 selection-box edit-data">
                <input type="tel" placeholder="Mobile Number" data-role="none" id="editMob" />
            </div>-->
            <div class="done-data">Email: <span class="f-bold" id="email"><%=objCustomer.objCustomerBase.CustomerEmail %></span></div>
          <!--  <div class="rounded-corner5 selection-box edit-data">
                <input type="email" placeholder="Email" data-role="none" id="editEmail" />
            </div>
            <button data-role="none" class="rounded-corner5">Done</button>-->
        </div>
        <%} %>
    </div>
    <div class="box1 new-line10">
        <h2 class="margin-bottom10 f-bold">Authorised Dealer Details</h2>
        <div class="break-line margin-bottom10"></div>
        <h2 class="f-bold"><%=organization %></h2>
        <div><%=address %>.</div>
    </div>
    <div class="box1 new-line10">
        <h2 class="margin-bottom10 f-bold">Steps to Avail your Offer</h2>
        <div class="break-line margin-bottom10"></div>
        <div class="steps-offer">
            <ul>
                <li>
                    <span class="bw-sprite offer-reserved light-opacity floatleft"></span>
                    <span class="width75 floatleft"><strong>Reserve Offer</strong><br />Make payment to reserve your offer and pre-book your bike.</span>
                </li>
                <li>
                    <span class="bw-sprite booking-amount floatleft"></span>
                    <span class="width75 floatleft"><strong>Visit Dealership</strong><br />Visit dealership and pay the remaining amount. Be ready with all <a href="#" id="reqDoc" class="blue-text">required documents</a>.</span>
                </li>
                <li>
                    <span class="bw-sprite bike-delivery floatleft"></span>
                    <span class="width75 floatleft"><strong>Buy Your Bike</strong><br />Avail offer benefit and take delivery of your bike.</span>
                </li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
    <div class="new-line10 bottom-info-list font12">
        <ul>
            <li>In case of cancellation of booking, you can claim a refund from us.</li>
            <li>For more information, read <a href="#" id="faqLink" class="blue-text">FAQs</a>.</li>
        </ul>
    </div>
</div>
<!--Price quote popup starts here-->
    <div class="bw-popup price-quote-popup hide">
    	<div class="popup-inner-container">
            <div class="bwmsprite close-btn floatright"></div>
            <h1>On Road Price Quote</h1>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="margin-top-10">
                <asp:Repeater id="rptPrice" runat="server">
                    <ItemTemplate>
                        <%-- Start 102155010 --%>
                        <%--<tr>
                            <td height="30" align="left"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %></td>
                            <td height="30" align="right"><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                        </tr>--%>
                        <tr>
                            <td height="30" align="left"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                            <td height="30" align="right"><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                        </tr>
                        <%-- End 102155010 --%>
                    </ItemTemplate>
                </asp:Repeater>
                <tr align="left">
                    <td height="1" colspan="2" class="break-line"></td>
                </tr>
                <%-- Start 102155010 --%>
                <%--<tr>
                    <td height="30" align="left">Total On Road Price</td>
                    <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%= TotalPrice %></td>
                </tr>--%>                
                                <%
                       if (IsInsuranceFree)
                       {
                           %>
                <tr>
                    <td height="30" align="left">Total On Road Price</td>
                    <td height="30" align="right"><span class="WebRupee">Rs.</span><span style="text-decoration: line-through"><%= TotalPrice %></span></td>
                </tr>
                <tr>
                    <td height="30" align="left">Minus Insurance</td>
                    <td height="30" align="right"><span class="WebRupee">Rs.</span><%= insuranceAmount %></td>
                </tr>
                <tr>
                    <td height="30" align="left">BikeWale On Road (after insurance offer)</td>
                    <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%= TotalPrice - insuranceAmount %></td>
                </tr>
                
                        <%
                       }
                       else
                       {
                           %>
                <tr>
                    <td height="30" align="left">Total On Road Price</td>
                    <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%= TotalPrice %></td>
                </tr>
                <%
                       }
                                     %>                                
                                <%-- End 102155010 --%>
            </table>
            <ul class="grey-bullet">
                <asp:Repeater id="rptDisclaimer" runat="server">
                    <ItemTemplate>
                        <li><i><%# Container.DataItem %></i></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
    <!--Price quote popup ends here-->

    <!--Documents popup starts here-->
    <div class="bw-popup documents-popup hide">
    	<div class="popup-inner-container">
            <div class="bwmsprite close-btn floatright"></div>
            <h1>List of Required Documents</h1>
            <div class="f-bold margin-top-10 margin-bottom10">Mandatory Documents:</div>
            <div class="doc-list">
                <ul>
                    <li>Two Color Photographs.</li>
                    <li>PAN Card.</li>
                </ul>
            </div>
            <div class="f-bold margin-top20 margin-bottom10">Identity Proof:</div>
            <div class="doc-list">
                <ul>
                    <li>Passport / Voter ID / Driving License.</li>
                </ul>
            </div>
            <div class="f-bold margin-top20 margin-bottom10">Additional Documents for Loan:</div>
            <div class="doc-list">
                <ul>
                    <li>Last 6 Months Bank Statement.</li>
                    <li>Salary Slip / Latest I.T. Return.</li>
                </ul>
            </div>
            <div class="f-bold margin-top20">Residential Address Proof:</div>
            <div class="lightgray margin-bottom10 margin-top-5">(Self-Owned House)</div>
            <div class="doc-list">
                <ul>
                    <li>Light Bil / Passport.</li>
                    <li>Ration Card (Relation Proof).</li>
                </ul>
            </div>
            <div class="lightgray margin-bottom10">(Rented House)</div>
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

<!--Cancellation & refund policy popup starts here-->
    <div class="bw-popup cancellation-popup hide">
    	<div class="popup-inner-container">
            <div class="bwmsprite close-btn floatright"></div>
            <h1>Cancellation & Refund Policy</h1>
            <div class="lower-alpha-list">
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

    <!--Faqs popup starts here-->
    <div class="bw-popup faq-popup hide">
    	<div class="popup-inner-container">
            <div class="bwmsprite close-btn floatright"></div>
            <h1>Frequently Asked Questions</h1>
            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">1. What is pre-booking?<br /> Why should I pre-book?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Pre-booking is the only way to immediately block the offers that come with a particular bike. Once you pre-book, you immediately become entitled to avail all offers mentioned against the model. Since most offers are limited in number and available for a limited period, you should use the pre-booking option to secure them.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">2. How can I pre-book a bike on BikeWale?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">To pre-book a bike on BikeWale, you have to pay online a fixed pre-booking amount mentioned against the vehicle of your interest. Your payment gets transferred to us through a fully-secured payment gateway and entitles you to avail any offers on your selected bike.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">3. What happens when I pre-book a bike on BikeWale?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">
                <div>On successful receipt of the pre-booking payment, we send you a receipt on your email, which contains the following information:</div>
                <div class="lower-alpha-list">
                    <ol>
                        <li>Details of your payment.</li>
                        <li>Details of your selected bike (model, variant, color) and the vehicle pricing.</li>
                        <li>Dealership contact details who will be responsible to complete further formalities and deliver the vehicle.</li>
                        <li>List of offers that you just secured by pre-booking and procedure to avail the same.</li>
                    </ol>
                </div>
            </div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">4. What happens to the money I pay while pre-booking?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">The money that you pay for pre-booking is further transferred to the dealership from which you are supposed to purchase the vehicle. This money will be adjusted against the final price of the vehicle, i.e. you have to pay only the remaining amount to the dealership.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">5. How is the dealership assigned for purchase? Can I change the dealership that has been assigned?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Dealership’s name and contact details are mentioned while pre-booking, so you already know which dealer you are pre-booking your bike. The customer cannot change the dealership that has been assigned.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">6. Will the dealer call me when I pre-book?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Yes, you will get a call from the assigned dealership after you have pre-booked a bike on BikeWale toschedule your visit to dealership. You will be required to go to the dealership to complete the formalities and take delivery of the vehicle. Alternatively, you can yourself call the dealership on the mentioned contact numbers. We do send dealership contact details on your email and SMS, apart from showing them on website.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">7. Can I pre-book the bike for others, for example my family member?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Yes, you can pre-book any vehicle for your near and dear ones.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">8. I have pre-booked a model. Now I want to change it to another model?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">We recommend you to thoroughly make up your mind for the vehicle of your choice before pre-booking and should avoid changing the model later. However, if you make a mistake and want to change the model, you can email us on <a href="mailto:contact@bikewale.com" class="blue-text">contact@bikewale.com</a> with a valid reason for the same, stating the booking reference number, your mobile number (that you used while pre-booking), along with the new model you now want to pre-book. We will reply within 2 working days to confirm the pre-booking of the new model.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">9. Will I be charged any fee for pre-booking feature?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">No, BikeWale does not charge any fee for pre-booking. The full amount you pay is counted against thepurchase price of the vehicle and gets adjusted in the final payment.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">10. I want to cancel the pre-booking. How can I do that? What happens to the refund?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">
                <div>BikeWale strongly recommends you to carefully select your bike before pre-booking. However, you cancancel a pre-booking within 3 calendar days of pre-booking, If you ever choose to cancel a pre-booking, you can email us on <a href="mailto:contact@bikewale.com" class="blue-text">contact@bikewale.com</a> with a valid reason for cancellation, stating the booking reference number, your mobile number (that you used while pre-booking). We will reply within 2 working days to confirm the cancellation.</div>
                <div class="margin-top-10 f-bold">The important conditions for cancellation are following:</div>
                <div class="lower-alpha-list">
                    <ol>
                        <li>Cancellation should be requested within 3 calendar days of pre-booking only</li>
                        <li>Cancellation will not be possible if customer and dealership have proceeded further with purchase of the vehicle. These conditions include giving depositing any money with the dealership, submitting the documents etc.</li>
                        <li>If the dealer has initiated the procurement of the bike upon customer’s pre-booking, cancellation will not be possible.</li>
                    </ol>
                </div>
            </div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">11. When I cancel, when will get my money refunded?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Upon requesting a cancellation, our team will verify with the dealer. Post successful verification, we will confirm your cancellation request and initiate the refund. The pre-booking amount should be credited back to your account within 7 working days.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">12. Will I be charged any fee for pre-booking feature?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">A nominal cancellation processing fee of <span class="WebRupee">Rs.</span>100 will be charged. After deducting <span class="WebRupee">Rs.</span>100, the entire pre-booking amount will be refunded to customer’s account within 7 working days.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">13. What are the mandatory documents that I need to carry to the dealership?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">The documents include any Govt. Photo ID proof of the person on which the bike will be registered (PAN Card, Driving License etc.), Any Govt. Address Proof of the person on which the bike will be registered, Two photographs, Additional documents for Finance options (Last 6-months bank statement, ITR Copies etc)<br /><br />The full list of documents is mentioned on our website while booking and also sent to your email address in the pre-booking confirmation email.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">14. Where do I have to pay the balance amount? How much will it be?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">You will pay the balance amount on the allocated dealership. The booking amount will be adjusted in the final on-road price of the vehicle.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">15. Can I change the color preference post visiting the dealership?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Yes you can. However, please note that different colors might have different wating periods and some colors might not be available from the manufacturers end. Dealer will provide the exact waiting period / delivery time on your visit to dealership.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">16. How will I get the benefits of the offers?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Depending upon the offer, you will get the benefit of some offers directly at the dealership, while takingdelivery. Such offers may include cash-back, free accessories, free insurance etc.<br /><br /> Other offers will be delivered to you when you tell us about completed delivery of your vehicle by filling in a short form. We will verify the purchase with dealership, and immediately ship the offer benefits to provided address.</div>

            <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	<div class="selected-input-text floatleft f-bold">17. Can I pre-book more than one bikes on BikeWale?</div>
                <div class="bw-sprite collapse-icon floatright"></div>
                <div class="clear"></div>
            </div>
            <div class="expanded-info-div hide">Yes. There is no limit to the number of pre-bookings allowed on BikeWale.</div>
        </div>
    </div>
    <!--Faqs popup ends here-->
<script>
    $(document).ready(function () {

        //push tags
        $("#btnMakePayment").click(function () {
            dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking -<%= MakeModel.Replace("'","")%>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Make_Payment' });
        });

        //For Price Quote popup
        $("#viewBreakup").click(function () {
            $(".price-quote-popup").show();
        });
        $(".price-quote-popup").find(".close-btn").click(function () {
            $(".price-quote-popup").hide();
        });
        //For Documents popup
        $("#reqDoc").click(function () {
            $(".documents-popup").show();
        });
        $(".documents-popup").find(".close-btn").click(function () {
            $(".documents-popup").hide();
        });

        //For Cancellation popup
        $("#cancellationLink").click(function () {
            $(".cancellation-popup").show();
        });
        $(".cancellation-popup").find(".close-btn").click(function () {
            $(".cancellation-popup").hide();
        });

        $(".bw-accordion").click(function () {
            var obj = $(this).next();
            $(".expanded-info-div").slideUp();
            $(".bw-sprite").removeClass('expand-icon');

            if (obj.is(":hidden")) {
                obj.slideDown();
                $(this).find(".bw-sprite").addClass('expand-icon');
            } else {
                obj.slideUp();
                $(this).find(".bw-sprite").removeClass('expand-icon');
            }
        });

        //For Faqs popup
        $("#faqLink").click(function () {
            $(".faq-popup").show();
        });
        $(".faq-popup").find(".close-btn").click(function () {
            $(".faq-popup").hide();
        });

        //    $(".edit-data").hide();
        //    $(".contact-details").find("button").hide();
        //    $(".edit-btn").click(function () {
        //        $(this).hide();
        //        $(".contact-details").find("button").show();
        //        $(".edit-data").show();
        //        $(".done-data").hide();
        //        $("#editName").val($("#name").text());
        //        $("#editMob").val($("#mob").text());
        //        $("#editEmail").val($("#email").text());
        //    });
        //    $(".contact-details button").click(function () {
        //        $(this).hide();
        //        $(".edit-btn").show();
        //        $(".edit-data").hide();
        //        $(".done-data").show();
        //        $("#name").text($("#editName").val());
        //        $("#mob").text($("#editMob").val());
        //        $("#email").text($("#editEmail").val());
        //    });
    });
</script>
</div>
            <!-- inner-section code ends here-->
        </div>
    </div> 
    <div id="divForPopup" style="display:none;"></div>
</form>
</body>
</html>
