<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DetailedDealerQuotation" Async="true" Trace="false"%>
<%@ Register TagPrefix="PQ" TagName="pdf" src="~/controls/PQPdfTemplate.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>

<%
    title = "Dealer Price Quote";
    keywords = "Dealer, Price, " + BikeName + ",authorised";
    description = BikeName + "authorised dealer price";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398766000399";
%>
<!-- #include file="/includes/headermobile_noad.aspx" -->
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?14sept2015" /> 
<link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
<script src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/jquery-ui-1.10.4.custom.min.js"></script>
<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>

<%--<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/BikeBooking/BikeBooking.js"></script>--%>
        <div data-role="popup"  class="ui-content new-line15"  id="mapDiv">
            <div  id="map" style="height:300px;width:300px;"></div>
            <a href="#" data-rel="back" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext" class="ui-btn-right" id="closeBox">Close</a>
       </div>
   	    <div class="padding5 margin-bottom20">
            <h1>Dealer Price Certificate</h1>
    	    <div class="box1 box-top new-line5 bot-red new-line10">
                <h2 class="margin-bottom10"><b><%=BikeName %></b></h2>
                <div class="full-border bike-img">
            	    <img alt="<%= BikeName%>  Photos" src="<% =ImgPath%>" title="<%=BikeName %> Photos">
                </div>
             <div class="<%= objColors.Count == 0 ?"hide":"" %>">
                <div class="full-border new-line10 selection-box"><b>Color Options: </b>
                    <table width="100%">
                        <tr style="margin-bottom:5px;">
                            <td class="break-line" colspan="2"></td>
                        </tr>
                    <asp:Repeater id="rptColors" runat="server">
                        <ItemTemplate>
                            <tr>
                            <td style="width:30px;"><div style="width:30px;height:20px;margin:0 10px 0 0;border: 1px solid #a6a9a7;padding-top:5px;background-color:#<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>"></div></td>
                            <td><div class="new-line"><%# DataBinder.Eval(Container.DataItem,"ColorName") %></div></td>
                                </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    </table>
                </div>
            </div>
                <!--Price Breakup starts here-->
                <div class="new-line10">
            	    <h2 class="f-bold">On Road Price Breakup</h2>
                      <asp:Repeater ID="rptQuote" runat="server">
                            <HeaderTemplate>
                                    <table  width="100%" border="0" cellspacing="0" cellpadding="0">                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%-- Start 102155010 --%>
                                    <%--<tr>
                                <td height="30" align="left">
                                    <%# DataBinder.Eval(Container.DataItem,"CategoryName") %>
                                </td>
                                <td height="30" align="right"><span class="WebRupee">Rs.</span><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>--%>
                                <tr>
                                <td height="30" align="left">
                                    <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                </td>
                                <td height="30" align="right"><span class="WebRupee">Rs.</span><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>
                                <%-- End 102155010 --%>
                            </ItemTemplate>
                            <FooterTemplate>
                                    <tr align="left">
                                        <td height="1" colspan="2" class="break-line"></td>
                                    </tr>
                                <%-- Start 102155010 --%>
                                    <%--<tr>
                                        <td height="30" align="left">Total On Road Price</td>
                                        <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice( TotalPrice.ToString()) %></td>
                                    </tr>--%>
                                <%
                       if (IsInsuranceFree)
                       {
                           %>
                                <tr>
                                        <td height="30" align="left">Total On Road Price</td>
                                        <td height="30" align="right"><span class="WebRupee">Rs.</span><span style="text-decoration: line-through"><%=CommonOpn.FormatPrice( TotalPrice.ToString()) %></span></td>
                                </tr>
                                <tr>
                                        <td height="30" align="left">Minus Insurance</td>
                                        <td height="30" align="right"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice(insuranceAmount.ToString()) %></td>
                                </tr>
                                <tr>
                                        <td height="30" align="left"><b>BikeWale On Road (after insurance offer)</b></td>
                                        <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice((TotalPrice - insuranceAmount).ToString()) %></td>
                                    </tr>
                                <%
                       }
                       else
                       {%>
                           <tr>
                                        <td height="30" align="left">Total On Road Price</td>
                                        <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice( TotalPrice.ToString()) %></td>
                                    </tr>
                       <%}
                                     %>                                
                                <%-- End 102155010 --%>

                                   <tr align="left">
                                        <td height="1" colspan="2" class="break-line"></td>
                                    </tr>
                                    <tr>
                                        <%if(noOfDays > 0){%>
                                            <td colspan="2" align="right" class="f-12"><i>Approximate vehicle waiting period:  <%=noOfDays %> days </i></td>
                                        <%}else{ %>
                                            <td colspan="2" align="right" class="f-12"><i>Availability: Vehicle in stock </i></td>
                                        <%} %>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    <ul class="grey-bullet">
                        <asp:Repeater id="rptDisclaimer" runat="server">
                            <ItemTemplate>
                                <li><i><%# Container.DataItem %></i></li>
                            </ItemTemplate>
                        </asp:Repeater>
                        </ul>
                </div>
                <!--Price Breakup ends here-->            
                <!--Exciting Offers section starts here-->           
                 
                 <% if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                   { %>
                <div class="bw-offer-box break-line new-line10" id="divOffers">
                    <h2 class="f-bold"><%= IsInsuranceFree ? "BikeWale Ganapati Offer" : "Exclusive Offers for BikeWale Customers"%></h2>                    
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
                                                        <img src="http://img.carwale.com/bikewaleimg/images/bikebooking/images/offer-list-pic1.jpg" class="margin-bottom10"/>
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
                                                        <img src="http://img.carwale.com/bikewaleimg/images/bikebooking/images/offer-list-pic2.jpg" class="margin-bottom10"/>
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
                                                        <img src="http://img.carwale.com/bikewaleimg/images/bikebooking/images/offer-list-pic3.jpg" class="margin-bottom10"/>
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
                                       <li>Get one year of <a target="_blank" href="/Absure Bike RSA.pdf"  style="text-decoration:underline">Bike Roadside Assistance</a>  absolutely FREE.</li>
                                    </ul>
                                    <p><a id="rsa" target="_blank" href="/m/pricequote/rsa.aspx" style="color: #0056cc !important;">How to avail above offers?</a></p>--%>
                                    <%-- End 102155010 --%>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                    </div>
                    <div class="new-line10" runat="server" id="divBookBike" visible="false">
                        <button data-role="none" id="btnBookBike" type="button" class="rounded-corner5 margin-bottom10" onClick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Prebook_to_avail_offer' });">Book to Avail Offer</button>                     
                    </div>
                    <div class="new-line10" runat="server" id="divBikeBooked" visible="false" style="text-align:center;">
                      You have already booked this bike.                
                    </div>
                </div>              
                <%} %>
                    
                <!--Exciting Offers section ends here-->
                 <!--Loan EMI section starts here-->
                <%if (objCEMI != null) {%>
                <div>
                    <div class="rounded-corner5 new-line10 selection-box">
            	        <div class="selected-input-text floatleft f-bold">Bank Loan Details <span class="f-12" style="font-weight:normal;">(Loan EMI starting from <span class="WebRupee">Rs.</span><b><%= CommonOpn.FormatPrice(Math.Ceiling(objCEMI.EMI).ToString()) %></b>)</span></div>
                
                        <div class="clear"></div>
                    </div>
                </div>
                <div  style="display: block;" class="rounded-corner5 selection-box">
                    <table  width="100%" border="0" cellspacing="0" cellpadding="0">                
                        <tr>
                            <td height="30" align="left"> Minimum Down Payment</td>
                            <td height="30" align="right"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice(objCEMI.DownPayment.ToString()) %></td>
                        </tr>
                        <tr>
                            <td height="30" align="left">Loan Amount (up to <%=objCEMI.objEMI.LoanToValue %>% of On-road Price)</td>
                            <td height="30" align="right"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice( objCEMI.LoanAmount.ToString()) %></td>
                        </tr>
                        <tr>
                            <td height="30" align="left">Maximum Tenure</td>
                            <td height="30" align="right"><%= objCEMI.objEMI.Tenure %> Months</td>
                        </tr>
                        <tr>
                            <td height="30" align="left">Interest Rate</td>
                            <td height="30" align="right"><%=objCEMI.objEMI.RateOfInterest %> %</td>
                        </tr>
                        <tr class="<%= !String.IsNullOrEmpty(objCEMI.objEMI.LoanProvider) ? "" : "hide" %>">
                            <td height="30" align="left">Loan Provider</td>
                            <td height="30" align="right"><%=objCEMI.objEMI.LoanProvider %></td>
                        </tr>
                        <tr align="left">
                            <td height="1" colspan="2" class="break-line"></td>
                        </tr>    
                        <tr>
                            <td height="30" align="left">EMI</td>
                            <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%= CommonOpn.FormatPrice(Math.Ceiling(objCEMI.EMI).ToString()) %></td>
                        </tr>  
                        <tr style="font-size:12px;">
                            <td colspan="2"><i>*This is an indicative EMI value. The final loan amount, interest rate and EMI depend on buyer's credit profile and can be decided by the financing bank only. Moreover, an additional loan processing fee may be charged by the bank.</i></td>
                        </tr>           
                </table>
                </div>
                <%} %>
                <!--Loan EMI section ends here-->

            
                <!--Bike Availability with Dealer section starts here-->
                <h2 class="f-bold new-line10">Assigned Dealer Details</h2>
                <%if (_objPQ.objDealer != null){ %>
                <div class="bike-dealer-data new-line5 bw-offer-box">
          
                    	    <div class="bw-sprite dealer-address floatleft"></div>
                            <div class="floatleft dealer-info">
                        	    <h3><b><%= _objPQ.objDealer.Organization %></b></h3>
                        	    <div class="margin-top5"><%= _objPQ.objDealer.Address %></div>
                                <div><%=address %>.</div>
                                <div class="margin-top5"><b>Working Hours : </b><%=   _objPQ.objDealer.WorkingTime   %></div>
                                <div class="blue-text margin-top5" id="divgMap"><a href="#mapDiv" data-rel="popup" data-position-to="window" style="text-decoration:underline">Locate on Map</a></div>
                            </div>
                            <div class="clear"></div>
                     
                            <div class="margin-top5"> 
                                <div class="bw-sprite mobile-icon floatleft new-line5"></div>
                                <div class="dealer-contact-info floatleft"><p><%= contactNo%></p></div>
                            </div>
                            <div class="clear"></div>
                </div>
                <%} %>
                <!--Bike Availability with Dealer section ends here-->
            
                <%  if (_objPQ.objFacilities != null && _objPQ.objFacilities.Count > 0){ %>
                <!--Facilities Offered by Dealership section starts here-->
                <div id="divFacility">
                    <div class="rounded-corner5 new-line10 selection-box">
            	        <div class="selected-input-text floatleft f-bold">Facilities Offered by Dealership</div>
                        <div class="clear"></div>
                    </div>

                    <div class="grey-bullet rounded-corner5 selection-box" style="display:block;">   
                        <asp:Repeater ID="rptFacility" runat="server">
                            <HeaderTemplate>
                                <ul>                                        
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li><%#DataBinder.Eval(Container.DataItem,"Facility") %></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <!--Facilities Offered by Dealership section ends here-->
                <%} %>
            
                <!--Documents for Registration & Loan section starts here-->
                <div class="rounded-corner5 new-line10 selection-box bw-accordion">
            	    <div class="selected-input-text floatleft f-bold">Required Documents for Registration & Loan</div>
                    <div class="bw-sprite collapse-icon floatright"></div>
                    <div class="clear"></div>
                </div>
                <div class="expanded-info-div  hide">
                    <ol>
                	    <li><b>Mandatory Documents:</b>
                    	    <ul class="light-grey-bullets">
                                <li>2 Color Photographs</li>
                                <li>PAN Card</li>
                            </ul>
                        </li>
           
                        <li><b>Identity Proof: </b>
                    	    <ul class="light-grey-bullets">
                        	     <li>Passport / Voter ID / Driving License</li>
                            </ul>
                        </li>
                        <li>
                            <b>Residential / Address Proof:</b>
                                    <div class="margin-top5">(Self-Owned House)</div>
                            	    <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                	    <li>Light Bill / Passport</li>
                                        <li>Ration Card (Relation Proof)</li>
                                    </ul>
                                    <div class="margin-top5">(Rented House)</div>
                                    <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                	    <li>Registered Rent Agreement + Police N.O.C.</li>
                                        <li>Rent Home Electricity Bill</li>
                                        <li>Permanent Address Proof</li>
                                        <li>Ration Card (Relation Proof)</li>
                                    </ul>
                        </li>
                        <li>
                            <b>Additional Documents for Loan:</b>
                            <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                <li>Last 6 Months Bank Statement</li>
                                <li>Salary Slip / Latest I.T. Return</li>
                            </ul>
                        </li>
                    </ol>
                </div>
                <!--Documents for Registration & Loan section ends here-->
                <div>
                    <div class="f-bold rounded-corner5 new-line10 selection-box">Next steps </div>
                    <div class="nex-steps-list rounded-corner5 selection-box">
                        <div class="grey-bullet">
                            <ul>
                                <li>
                                    <b>Get in touch with Dealership</b>
                                    <ul class="light-grey-bullets">
                                        <li><%= organization %> will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convenient time.</li>
                                    </ul>
                                </li>
                                <li>
                                    <b>Claim your Price</b>
                                    <ul class="light-grey-bullets">
                                        <li>Please present this price certificate to dealership to claim the price for your purchase.</li>
                                    </ul>
                                </li>
                                <li>
                                    <b>Be ready with Documentation</b>
                                    <ul class="light-grey-bullets">
                                        <li>Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</li>
                                    </ul>
                                </li>
                                <li>
                                    <b>Buy your Bike!</b>
                                    <ul class="light-grey-bullets">
                                        <li>Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased <%=BikeName %>.</li>
                                    </ul>
                                </li>
                            </ul>
                          <%--  <li> <%= _objPQ.objDealer.Organization %> will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convenient time.</li>
                            <li>Please present this price certificate to dealership to claim the price for your purchase.</li>
                            <li>Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</li>
                            <li>Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased <%=BikeName %>.</li>--%>
                        </div>
                    </div>
                    <div class="new-line10"><b>This price quote is Valid till: <%= CommonOpn.GetValidDate(7).ToString("dd MMM, yyyy") %>.</b></div>
                    <div class="blue-text font12"><i><a target="_blank" href="/termsconditions.aspx">Please read other Important Terms and Conditions</a></i></div>
            </div>

            </div>

        <div>
          <div class="new-line10 nex-steps-list">
                <h3><i>Disclaimer</i></h3>
                <ul class="light-grey-bullets margin-bottom5 margin-top5 new-line5">
                          <li><i>The bike prices mentioned here are indicative and are supplied by their authorised dealerships. BikeWale takes utmost care in gathering precise and accurate information, financial quotes and promotional offers. However, it may not reflect the final price you may pay.</i></li>
                                <li><i>The services offered by our associate dealerships or financial partners are independent of BikeWale. BikeWale does not take ownership or offer any guarantee for these services and shall not be held responsible or liable for them.</i></li>
                                <li><i>All promotional offers displayed here are provided by the respective dealers/manufacturers/financiers along with BikeWale.</i></li>
                                <li><i>BikeWale shall not be held liable for any consequences arising either directly or indirectly from your use of the information provided in this price quote.</i></li>  
                                <li><i>The prices mentioned here are with respect to cash/cheque payments. The dealership may charge an additional charge for credit / debit card payments.</i></li>
                                <li><i>The accessories shown with the bike may not be part of the standard product and may attract additional charges.</i></li>
                                <li><i>Vehicle availability (if mentioned here) is tentative and to be considered from the date of actual booking with the dealership, which usually involves a booking amount payment.</i></li>                        
                </ul>
            </div>
            <div class="bottom-btns">
                        <asp:Button type="submit" Text="Save this Price Certificate" id="btnSavePdf" runat="server" data-role="none"  visible="false" data-theme="b" data-mini="true" class="rounded-corner5"  />
                 <%--   <div style="display:none">
                        <PQ:pdf runat="server" ID="PQPdfTemplate"/>
                </div>--%>
            </div>
         </div>
        </div>
        <!--Book Appointment starts here-->
        <div class="bw-popup book-app-details hide">
    	    <div class="popup-inner-container">
                <div class="bwmsprite close-btn floatright"></div>
                <h1>Book an Appointment</h1>
                <div class="selection-box rounded-corner5 new-line10">
                    <input type="text" name="date" id="date" value="" data-role="none" />
                    <div class="bw-sprite datepicker-icon"></div>
                </div>
                <div class="new-line10">
            	    <ul>
                	    <li>
                            <div class="book-app-select">
                                <select name="hour" id="select-hour">
                                    <option value="0">Select Hour</option>
                                </select>
                            </div>
                        </li>
                        <li>
                            <div class="margin-left-10 book-app-select">
                                <select name="minute" id="select-min">
                                    <option value="0">Select Minute</option>
                                </select>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="clear"></div>
                <div class="new-line10">The dealer will get in touch with you to confirm the appointment</div>
                <button data-role="none" class="rounded-corner5" id="btnBookApp">Book an Appointment</button>
            </div>
        </div>
        <!--Book Appointment ends here-->

        <!--Color popup starts here-->
        <div class="bw-popup color-popup hide">
            <div class="popup-inner-container">
                <div class="bwmsprite close-btn floatright"></div>
                <h1>Select Bike Color</h1>
                <div class="color-palette margin-top20">
                    <div>
                        Please select your preferred color for this bike to be shared with the dealership. You can still change the color directly at the dealership:
                    </div>
                    <asp:Repeater id="rptPopupColors" runat="server">
                        <HeaderTemplate>
                            <ul class="margin-top20">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                            <div class="color-outer-div">
                                <div class="color-inner-div" style="background-color:#<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>"></div>
                                <div class="bw-sprite" color-id="<%#DataBinder.Eval(Container.DataItem,"ColorId") %>"></div>
                            </div>
                            <div class="margin-top-5"><%#DataBinder.Eval(Container.DataItem,"ColorName") %></div>
                        </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                            <div class="clear"></div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <button id="btnConfirmColor" data-role="none" class="rounded-corner5 margin-bottom10" onClick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Proceed _to_prebook' });">Proceed to Pre-Book</button>
                <div class="font12 lightgray"><i>*Please note that bike availability, pricing might vary for different color options. Dealership will provide the actual delivery date for your selected color.</i></div>
            </div>
        </div>
        <!--Color popup ends here-->
<script>
    $(document).ready(function (e) {

        $("#btnSavePdf").click(function () {
            dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel%>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Download_PDF' });
        });

        var date = null;

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
        $("#bookApp").click(function () {
            $(".book-app-details").show();
            $("html, body").animate({ scrollTop: $(".book-app-details").offset().top }, 0);
        });
        $(".book-app-details").find(".close-btn").click(function () {
            $(".book-app-details").hide();
        });

        var latitude = '<%=lattitude %>';
        var longitude = '<%=longitude%>';

        if (latitude > 0 && longitude > 0) {
            var myCenter = new google.maps.LatLng(latitude, longitude);

            function initialize() {
                var mapProp = {
                    center: myCenter,
                    zoom: 16,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                var map = new google.maps.Map(document.getElementById("map"), mapProp);

                var marker = new google.maps.Marker({
                    position: myCenter,
                });

                marker.setMap(map);
            }

            google.maps.event.addDomListener(window, 'load', initialize);
        }
        else {
            $("#divgMap").addClass("hide");
        }

        $("#closeBox").click(function () {
            document.body.style.overflow = "visible";

        });

        $("#btnBookApp").click(function () {

            if (date == null) {
                alert("Please Select date.");
                return false;
            }
            else {

                date.setHours($("select#select-hour option").filter(":selected").val());
                date.setMinutes($("select#select-min option").filter(":selected").val());

                var pqid = '<%= pqId%>';

                BookAnAppointment(pqid, date);
            }
        });

        function BookAnAppointment(pqid, date) {

            var appDate = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes() + ":0";   //"2009/02/26 18:37:58";

            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                data: '{"pqId":"' + pqid + '","date":"' + appDate + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateAppointmentDate"); },
                success: function (response) {
                    if (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');

                        if (resObj) {
                            alert("Your Appointment has been sheduled successfully.");
                            $(".book-app-details").hide();
                        }
                        else {
                            alert("Appointment not sheduled ,Please try again. ");
                        }
                    }
                }
            });
        }

        $("#date").each(function () {
            $(this).datepicker({
                minDate: 0,
                altField: "#" + $(this).attr("id"),
                showOtherMonths: true,
                dateFormat: "dd/mm/yy",
                timeFormat: "HH:mm",
                changeMonth: true, changeYear: true,
                onSelect: function (dateText, inst) {
                    date = $(this).datepicker('getDate'),
                    day = date.getDate(),
                    month = date.getMonth() + 1,
                    year = date.getFullYear();
                }
            });
        });

        for (i = 1 ; i <= 60; i++) {

            if (i <= 24) {
                $('#select-hour').append($('<option>', {
                    value: i,
                    text: i
                }));
            }

            $('#select-min').append($('<option>', {
                value: i,
                text: i
            }));
        }

        // for color pop-up

        //$("#btnBookBike").click(function () {
        //    location.href = "/m/pricequote/BookingSummary.aspx";
        //});
        <%-- Start 102155010 --%>
        //$("#btnBookBike").click(function () {
        //    $(".color-popup").show();
        //});
        $(".color-popup").find(".close-btn").click(function () {
            $(".color-popup").hide();
        });
        $(".color-palette li").click(function () {
            $(".color-palette li").find('.bw-sprite').removeClass("color-tick");
            $(this).find('.bw-sprite').addClass("color-tick");
        });

        <%--$("#btnConfirmColor").click(function () {
            var colorId = $(".color-palette li").find('.color-tick').attr('color-id');
            var pqId = '<%= pqId%>';

            if (colorId > 0)
            {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                    data: '{"pqId":"' + pqId + '","colorId":"' + colorId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdatePQBikeColor"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        if (resObj == false) {
                            alert("please Select color to proceed.");
                        }
                        else {
                            location.href = "/m/pricequote/BookingSummary.aspx";
                        }
                    }
                });
            }
            else
            {
                alert("Please select color to proceed.");
            }
        });--%>
        $("#btnBookBike").click(function () {
            location.href = "/m/pricequote/BookingSummary.aspx";
        });
        <%-- End 102155010 --%>
    });

</script>
<!-- #include file="/includes/footermobile_noad.aspx" -->
<!--<div class="bottom-info"></div>-->