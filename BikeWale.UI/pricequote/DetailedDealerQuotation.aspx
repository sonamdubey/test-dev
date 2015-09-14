<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DetailedDealerQuotation" Trace="false" %>

<%@ Register TagPrefix="SB" TagName="SimilarBike" Src="~/controls/SimilarBikes.ascx" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>

<%@ Import Namespace="Bikewale.Common" %>
<%
    title = BikeName + " Dealer Price Quote";
    description = "Authorise dealer price details of a bike " + BikeName;
    keywords = BikeName + ", price, authorised, dealer ";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
    //canonical = "http://www.bikewale.com/pricequote/";
    //alternate = "http://www.bikewale.com/m/pricequote/";
%>
<!-- #include file="/includes/headNew.aspx" -->

<PW:PopupWidget runat="server" ID="PopupWidget" />

<%--<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq.css" />--%>
<link href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq.css?14sept2015" rel="stylesheet" />
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq-new.css?14sept2015" />
<link rel="stylesheet" type="text/css" href="/css/rsa.css?v=3.0" />
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/jquery-ui-1.10.4.custom.min.css" />
<script type="text/javascript" src="http://st.aeplcdn.com/bikewale/src/common/bt.js?v1.1"></script>
<script type="text/javascript" src="../src/jquery-ui-1.10.4.custom.min.js"></script>
<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
<script type="text/javascript" src="/src/common/jquery.colorbox-min.js?v=1.0"></script>
<style>
    .colours {
        list-style: outside none none;
        display: inline;
    }

        .colours li {
            float: left;
        }

    .map-box {
        width: 276px;
        height: 170px;
    }
    #get-pq-new h2 { border-bottom:2px solid #c62000; padding-bottom:10px; }
</style>

<!--bw color popup ends here-->
<div class="main-container">
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a class="blue" href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/new/">New</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/pricequote/">On-Road Price Quote</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Dealer Detailed Price Quote</strong></li>
            </ul>
            <div class="clear"></div>
        </div>
        <div class="grid_8 margin-top10">
            <h1 class="margin-bottom5">Detailed Dealer Price Quote</h1>
            <!--Get pq code starts here-->
            <div id="get-pq-new" class="inner-content">
                <h2 class=""><%=BikeName %></h2>
                <div id="div_ShowPQ">
                    <%--<h2><%=BikeName %></h2>--%>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" class="tbl-default margin-top10">
                        <tr>
                            <td style="width: 100px; vertical-align: central; border-right: 1px solid #E5E5E5; padding: 5px;">
                                <div class="show-pq-pic">
                                    <img alt="<%= BikeName%>  Photos" src="<% =ImgPath%>" title="<%=BikeName %> Photos">
                                </div>
                                <div class="margin-top5 dotted-hr <%= objColors.Count == 0 ? "hide" : "" %>" style="padding-right: 10px;"></div>
                                <div class="<%= objColors.Count == 0 ? "hide" : "" %>" style="float: left; margin-right: 3px; padding-top: 3px;">Color: </div>
                                <div style="overflow: hidden;">
                                    <ul class="colours <%= objColors.Count == 0 ? "hide" : "" %>">
                                        <asp:repeater id="rptColors" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div title="<%#DataBinder.Eval(Container.DataItem,"ColorName") %>" style="background-color:#<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>;height:15px;width:15px;margin:5px;border:1px solid #a6a9a7;"></div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:repeater>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                            </td>

                            <td valign="top" style="padding-left: 20px;">

                                <asp:repeater id="rptQuote" runat="server">
                                        <HeaderTemplate>
                                               <table>
                                                    <%--<h2><%= BikeName %></h2>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%-- Start 102155010 --%>
                                             <%--<tr>
                                            <td width="370">
                                               <%# DataBinder.Eval(Container.DataItem,"CategoryName") %>
                                            </td>
                                            <td width="100" class="numeri-cell" align="right">
                                                <span id="Span1"><span class="WebRupee">Rs.</span><b>  <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></b></span>
                                            </td>
                                        </tr>--%>
                                            <tr>
                                            <td width="370">
                                               <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                            </td>
                                            <td width="100" class="numeri-cell" align="right">
                                                <span id="Span1"><span class="WebRupee">Rs.</span><b>  <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></b></span>
                                            </td>
                                        </tr>
                                            <%-- End 102155010 --%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                <tr><td colspan="2"><div class="dotted-hr"></div><td></tr>
                                            <%-- Start 102155010 --%>
                                                <%--<tr>
                                                    <td class="price2">Total On Road Price</td>
                                                    <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs.</span><%=Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></td>
                                                </tr>--%>                                            
                                <%
                                    if (insuranceAmount > 0)
                                    {
                           %>
                                            <tr>
                                                <td class="">Total On Road Price</td>
                                                <td width="100" class="numeri-cell" align="right"><span class="WebRupee">Rs.</span><span style="text-decoration: line-through"><%=Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></span></td>
                                            </tr>
                                            <tr>
                                                 <td class="">Minus Insurance</td>
                                                 <td width="100" class="numeri-cell" align="right"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice(insuranceAmount.ToString()) %></td>
                                            </tr>
                                            <tr>
                                                 <td class="price2"><b>BikeWale On Road (after insurance offer)</b></td>
                                                 <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice((TotalPrice - insuranceAmount).ToString()) %></td>
                                            </tr>
                                            <%
                       }
                       else
                       {
                           %>
                                            <tr>
                                                <td class="price2">Total On Road Price</td>
                                                <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs.</span><%=Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></td>
                                            </tr>
                                            <%
                       }
                                     %>                                
                                <%-- End 102155010 --%>
                                                <tr><td colspan="2"><div class="dotted-hr"></div><td></tr>
                                               <tr>
                                                   <%if (numOfDays > 0)
                                                     {%>
                                                     <td colspan="2" align="right"><i>Approximate vehicle waiting period:  <%=numOfDays %> days </i></td>
                                                   <%}
                                                     else
                                                     { %>
                                                     <td colspan="2" align="right"><i>Availability: Vehicle in stock </i></td>
                                                   <%} %>
                                                </tr>
                                        </FooterTemplate>
                                    </asp:repeater>
                                <tr>
                                    <td colspan="3">
                                        <ul class="std-ul-list">
                                            <asp:repeater id="rptDisclaimer" runat="server">
                                                    <ItemTemplate>
                                                        <li><i><%# Container.DataItem %></i></li>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                        </ul>
                                    </td>
                                </tr>
                    </table>
                    </td>
                        </tr>
                    </table>        
                </div>
            </div>
            <!--Get pq code ends here-->

            <!--Exciting offers div starts here-->
            <%if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
              { %>
            <div class="bw-offer-box" id="divOffers">
                <h2><%= IsInsuranceFree ? "BikeWale Ganapati Offer" : "Exclusive Offers for BikeWale Customers"%></h2>
                <%
                  if (!IsInsuranceFree)
                  { 
                %>
                <a id="rsa" class="blue">How to avail the offer?</a>
                <%
                      }
                %>
                <div class="margin-top5 margin-left5">
                    <asp:repeater id="rptOffers" runat="server">
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
                                        <li>Get one year of <a target="_blank" href="/Absure Bike RSA.pdf">Bike Roadside Assistance</a>  absolutely FREE.</li>--%>
                                    <%-- End 102155010 --%>
                                        </ul>
                                </FooterTemplate>
                            </asp:repeater>
                </div>

                <div class="mid-box margin-top15 center-align" runat="server" id="divBookBike" visible="false">
                    <input type="button" class="action-btn text_white" id="btnBookBike" value="Book to Avail Offer" name="btnBookBike" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","")%>        ', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Prebook_to_avail_offer' });"></div>
                <div class="mid-box margin-bottom10 center-align margin-top10" runat="server" id="divBikeBooked" visible="false"><b>You have already booked this bike.</b></div>
            </div>
            <%} %>
            <!--Exciting offers div ends here-->

            <!--bw accordion code starts here-->
            <%if (objCEMI != null)
              {%>
            <div class="margin-bottom20">
                <div class=" drop-down-title  border-radius5" style="cursor: text;">
                    <div class="right-float margin-top5"></div>
                    <h2>Bank Loan Details <span style="font-size: 14px; font-weight: normal;">(Loan EMI starting from <span class="WebRupee">Rs.</span><b><%=Bikewale.Common.CommonOpn.FormatPrice( Math.Ceiling(objCEMI.EMI).ToString()) %></b>)</span> </h2>
                </div>
                <div class="inner-content" style="display: block !important;">
                    <table class="tbl-default" cellpadding="0" cellspacing="0" width="100%" border="0">
                        <tr>
                            <td>Minimum Down Payment</td>
                            <td align="right"><span class="WebRupee">Rs.</span><b><%=Bikewale.Common.CommonOpn.FormatPrice((objCEMI.DownPayment.ToString())) %></b></td>
                        </tr>
                        <tr>
                            <td>Loan Amount (up to <%=objCEMI.objEMI.LoanToValue %>% of On-road Price) </td>
                            <td align="right"><span class="WebRupee">Rs.</span><b><%= Bikewale.Common.CommonOpn.FormatPrice((objCEMI.LoanAmount.ToString())) %></b></td>
                        </tr>
                        <tr>
                            <td>Maximum Tenure</td>
                            <td align="right"><b><%= objCEMI.objEMI.Tenure %> Months </b></td>
                        </tr>
                        <tr>
                            <td>Interest Rate</td>
                            <td align="right"><b><%=objCEMI.objEMI.RateOfInterest %> % </b></td>
                        </tr>
                        <tr class="<%= !String.IsNullOrEmpty(objCEMI.objEMI.LoanProvider) ? "" : "hide" %>">
                            <td>Loan Provider</td>
                            <td align="right"><b><%= objCEMI.objEMI.LoanProvider %></b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="dotted-hr"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="price2">EMI<br />
                                <td class="price2" align="right"><span class="WebRupee">Rs.</span><%= Bikewale.Common.CommonOpn.FormatPrice(Math.Ceiling(objCEMI.EMI).ToString()) %></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="font-size: 11px !important">
                                <i>*This is an indicative EMI value. The final loan amount, interest rate and EMI depend on buyer's credit profile and can be decided by the financing bank only. Moreover, an additional loan processing fee may be charged by the bank.</i>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <%} %>

            <!--bw accordion code ends here-->

            <!--bw accordion code starts here-->
            <div class="margin-bottom20">
                <div class=" drop-down-title bw-accordion border-radius5">
                    <div class="bw-sprite collapse-icon right-float margin-top5"></div>
                    <h2>Required Documents For RTO & Loan</h2>
                </div>
                <div class="inner-content expanded-info-div border-radius5">
                    <div class="grid_8 alpha omega">
                        <div class="grid_4 alpha omega">

                            <ul>
                                <li>
                                    <h3>Mandatory Documents:</h3>
                                    <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                        <li>2 Color Photographs</li>
                                        <li>PAN Card</li>
                                    </ul>
                                </li>
                                <li>
                                    <h3>Identity Proof:</h3>
                                    <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                        <li>Passport / Voter ID / Driving License</li>
                                    </ul>
                                </li>
                                <li>
                                    <h3>Additional Documents for Loan:</h3>
                                    <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                        <li>Last 6 Months Bank Statement</li>
                                        <li>Salary Slip / Latest I.T. Return</li>
                                    </ul>
                                </li>
                            </ul>
                        </div>

                        <div class="grid_4 alpha omega cust-facilities" style="padding-left: 10px;">
                            <h3>Residential / Address Proof:</h3>
                            <div class="margin-top5">(Self-Owned House)</div>
                            <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                <li>Light Bill / Passport</li>
                                <li>Ration Card (Relation Proof)</li>
                            </ul>
                            <div>(Rented House)</div>
                            <ul class="light-grey-bullets margin-bottom5 margin-top5">
                                <li>Registered Rent Agreement + Police N.O.C.</li>
                                <li>Rent Home Electricity Bill</li>
                                <li>Permanent Address Proof</li>
                                <li>Ration Card (Relation Proof)</li>
                            </ul>

                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <!--bw accordion code starts here-->
            <div class="inner-content">
                <h2 class=" payment-pg-heading margin-bottom20">Next Steps</h2>
                <div class="bw-box bw-box-small">
                    <ul>
                        <li>
                            <h2>Get in touch with Dealership</h2>
                            <p class="margin-bottom10"><span class="bw-sprite dealer-confirm"></span></p>
                            <p class=" left-align"><%=organization %> will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convinient time.</p>
                        </li>
                        <li>
                            <h2>Claim Your Price</h2>
                            <p class="margin-bottom10"><span class="bw-sprite claim-price"></span></p>
                            <p class=" left-align">Please present this certificate to dealership to claim the price for your purchase.</p>
                        </li>
                        <li>
                            <h2>Be ready with the Documentation</h2>
                            <p class="margin-bottom10"><span class="bw-sprite documents"></span></p>
                            <p class=" left-align">Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</p>
                        </li>
                        <li>
                            <h2>Buy Your Bike</h2>
                            <p class="margin-bottom10"><span class="bw-sprite buy-your-bike"></span></p>
                            <p class=" left-align"><%=organization %> Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased <%=BikeName %></p>
                        </li>
                        <div class="clear"></div>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="margin-top20">
                <p><b>This price quote is valid only till <%= CommonOpn.GetValidDate(7).ToString("dd MMM, yyyy") %>.</b><br />
                </p>
                <p><i>Please read other <a target="_blank" href="/termsconditions.aspx" class="blue">Important Terms & Conditions</a></i></p>
            </div>

                <div class="mid-box margin-top15 center-align">
                    <input type="button" value="Save this Price Certificate" id="btnSavePdf" class="action-btn text_white" onclick="CallToAction();return false;" />
                </div>
            <%--          <div style="display:none">
                 <PQ:pdf runat="server" ID="PQPdfTemplate"/>
                <PQT:Test runat="server" id="Test" />
            </div>--%>
            <div class="margin-top20" style="font-size: 11px !important">
                <i>Disclaimer</i>
                <ul class="light-grey-bullets margin-bottom5 margin-top5">
                    <li><i>The bike prices mentioned here are indicative and are supplied by their authorised dealerships. BikeWale takes utmost care in gathering precise and accurate information, financial quotes and promotional offers. However, it may not reflect the final price you may pay.</i></li>
                    <li><i>The services offered by our associate dealerships or financial partners are independent of BikeWale. BikeWale does not take ownership or offer any guarantee for these services and shall not be held responsible or liable for them.</i></li>
                    <li><i>All promotional offers displayed here are provided by the respective dealers/manufacturers/financiers along with BikeWale.</i></li>
                    <li><i>BikeWale shall not be held liable for any consequences arising either directly or indirectly from your use of the information provided in this price quote.</i></li>
                    <li><i>The prices mentioned here are with respect to cash/cheque payments. The dealership may charge an additional charge for credit / debit card payments.</i></li>
                    <li><i>The accessories shown with the bike may not be part of the standard product and may attract additional charges.</i></li>
                    <li><i>Vehicle availability (if mentioned here) is tentative and to be considered from the date of actual booking with the dealership, which usually involves a booking amount payment.</i></li>
                </ul>
            </div>

        </div>

        <div class="grid_4 right-grid">
            <div class="margin-bottom20">

                <!--Dealer detailes div stsrts here-->
                <div class="inner-content bw-dealer-details">
                    <%if (_objPQ.objDealer != null)
                      { %>
                    <div style="padding: 5px;" class="bw-offer-box">
                        <h2>Assigned Dealer Details</h2>

                        <div class="margin-top10">
                            <h3 class="margin-bottom5"><span class="bw-sprite dealer-search margin-right10"></span><%= _objPQ.objDealer.Organization %></h3>
                            <div class="margin-left20">
                                <p><%= _objPQ.objDealer.Address %></p>
                                <p><%= address  %></p>
                            </div>
                            <div class="padding-left10 margin-top10">
                                <p class="padding-left10"><span class=" bw-sprite call padding-left10 left-float margin-right10"></span><%=contactNo %></p>
                                <div class="clear"></div>
                            </div>


                            <%if (!String.IsNullOrEmpty(_objPQ.objDealer.WorkingTime))
                              {%>
                            <div class="margin-left20  margin-top10"><span><b>Working Hours: </b></span><%=   _objPQ.objDealer.WorkingTime   %></div>
                            <%} %>
                        </div>
                    </div>

                    <div class="margin-top10 margin-right20 map-content hide" id="divMapWindow">
                        <div id="divMap"></div>
                    </div>
                    <%} %>

                    <%if (_objPQ.objFacilities != null && _objPQ.objFacilities.Count > 0)
                      { %>
                    <div class="grey-bullets margin-top20">
                        <h3 class="margin-bottom5 margin-left10">Customers Facilities offered by Dealership</h3>
                        <div class="margin-left10">
                            <asp:repeater id="rptFacility" runat="server">
                                    <HeaderTemplate>
                                        <ul>                                        
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li><%#DataBinder.Eval(Container.DataItem,"Facility") %></li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:repeater>
                        </div>
                    </div>
                    <%} %>
                    <div class="clear"></div>
                </div>
                <!--Dealer detailes div ends here-->

            </div>
            <SB:SimilarBike ID="ctrl_similarBikes" TopCount="2" runat="server" />
        </div>

        <div class="clear"></div>

    </div>
</div>


<!--bw rsa popup starts here-->
<div class="rsa-popup bw-popup hide">
    <!--header starts here-->
    <div class="rsa-header">
        <div class="bw-sprite white-close-btn right-float margin-top5"></div>
        <div class="left-float margin-right10 header-seperator">
            <img class="margin-right10" src="http://img1.carwale.com/bikewaleimg/images/bikebooking/images/rsa-logo.png" border="0">
        </div>
        <div class=" left-float margin-top5">
            <h1>FREE Helmet & Bike RSA Offer</h1>
        </div>
        <div class="clear"></div>
    </div>
    <!--header ends here-->
    <!--inner content starts here-->
    <div class="popup-inner-content">
        <h2>Three simple steps to avail the offers:</h2>
        <div class="steps">
            <h2>Step 1:</h2>
            <p>Purchase your bike and get it registered with RTO.</p>
        </div>
        <div class="seperator"></div>

        <div class="steps">
            <h2>Step 2:</h2>
            <p>Provide your bike purchase details on <a class="blue" href="/pricequote/RSAOfferClaim.aspx">this link.</a></p>
            <p>OR</p>
            <p>Email the following details to <a class="blue" href="#">contact@bikewale.com</a> with subject as <span class="color-text">"Free Helmet and RSA Offer"</span>:</p>

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
            <p>
                On receipt of above details we will verify your purchase from the dealership and dispatch your<strong> FREE Helmet and Bike Roadside Assistance Certificate</strong> on your provided address within 30 days.
            </p>
        </div>
    </div>
    <!--header starts here-->
</div>
<!--bw rsa popup ends here-->
<!--bw color popup starts here-->
<div class="bw-popup bw-popup-sm hide">
    <div class="popup-inner-container">
        <div class="bw-sprite close-btn right-float"></div>
        <h2>Select Bike Color</h2>
        <div>
            <div class="color-box">
                <p class="margin-top10 margin-bottom5">Please select your preferred color for this bike to be shared with the dealership. You can still change the color directly at the dealership:</p>
                <asp:repeater id="rptPopupColors" runat="server">
                            <HeaderTemplate>
                                 <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                    	        <li>
                                    <div class="color-box-outline">
                            	        <div class="tick-box"><span class="bw-sprite" ColorId="<%#DataBinder.Eval(Container.DataItem,"ColorId") %>"></span></div>
                                        <div title="<%#DataBinder.Eval(Container.DataItem,"ColorName") %>" class="bike-colors" style="background-color:#<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>;"></div>
                                    </div>
                                    <span class="color-box-span"><%#DataBinder.Eval(Container.DataItem,"ColorName") %></span>
                                </li>
                            </ItemTemplate>
                             <FooterTemplate>
                                  <div class="clear"></div>
                            </ul>
                             </FooterTemplate>
                            </asp:repeater>
            </div>
            <div class="mid-box margin-bottom10 center-align">
                <input type="submit" class="action-btn" id="btnColorSelection" value="Proceed to Pre-Book" name="btnSavePriceQuote" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","")%>        ', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Proceed _to_prebook' });"></div>
            <p class="font11"><i>*Please note that bike availability, pricing might vary for different color options. Dealership will provide the actual delivery date for your selected color.</i></p>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {       
        <%-- Start 102155010 --%>
        //$("#btnBookBike").click(function() {
        //    $("#blackOut-window").show();
        //    $('.bw-popup-sm').show();
        //});
        
        <%--$("#btnColorSelection").click(function () {
         
            var obj = $(".color-box li").find('.bw-sprite');
            var abc = $(".color-box li .bw-sprite.color-tick");
            var pqId = '<%= PqId%>';
            if (abc.length > 0 && pqId > 0) {
                var colorId = abc.attr('ColorId');
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/Bikewale.Ajax.AjaxBikeBooking,Bikewale.ashx",
                    data: '{"pqId":"' + pqId + '","colorId":"' + colorId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdatePQBikeColor"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        if (resObj == false) {
                            alert("Please select color to proceed.");
                        }
                        else
                            location.href = "/pricequote/BookingSummary.aspx";
                    }
                });
            }
            else {
                alert("Please select color to proceed.");
                return false;
            }
        });--%>
        $("#btnBookBike").click(function () {
            location.href = "/pricequote/BookingSummary.aspx";
        });
        <%-- End 102155010 --%>
        $('.bw-popup, #blackOut-window,').hide();

        $("#rsa").click(function () {
            $('#blackOut-window').show();
            $('.rsa-popup').show();
        });

        $('.white-close-btn').click(function () {
            $("#blackOut-window").hide();
            $('.rsa-popup').hide();
        });

        $('.close-btn').click(function () {
            closePopup();
        });

        $(".color-box li").click(function () {
            $(".color-box li").find('.bw-sprite').removeClass("color-tick");
            $(this).find('.bw-sprite').addClass("color-tick");
        });


        var date = null;
      
        for (i = 0 ; i <= 60; i++) {

            if (i <= 24)
            {
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

        $('#select-hour').change(function () {
            var a = $('#select-hour').find('option:selected').text();
            $('.select-hour-title').text(a);
        });
        $('#select-min').change(function () {
            var a;
            a = $('#select-min').find('option:selected').text();
            $('.select-min-title').text(a);
        });

        // $('.bw-popup, #blackOut-window').hide();
        $('#btnBookDealerAppoinment').click(function () {
            $('#blackOut-window').show();
            $('.bw-popup').show();

        });

        $('#btnSavePriceQuote').click(function () {


            if (date == null) {
                alert("Please Select date.");
                return false;
            }
            else {

                date.setHours($("select#select-hour option").filter(":selected").val());
                date.setMinutes($("select#select-min option").filter(":selected").val());

                var pqid = '<%= PqId%>';

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
                            closePopup();
                        }
                        else {
                            alert("Appointment not sheduled ,Please try again. ");
                        }
                    }
                }
            });
        }


     

        function closePopup()
        {
            $('#blackOut-window').hide();
            $('.bw-popup').hide();
        }

        $("#bw-date").each(function () {
            $(this).datepicker({
                minDate: 0,
                altField: "#" + $(this).attr("id"),
                showOtherMonths: true,
                dateFormat: "dd/mm/yy",
                changeMonth: true, changeYear: true,
                onSelect: function (dateText, inst) {
                    date = $(this).datepicker('getDate');
                }
            });
        });

        $(".inline").colorbox({ inline: true });
    });

    function CallToAction() {
        dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","")%>', act: 'Click Button Get_Dealer_Details' ,lab:'Clicked on Download_PDF'});
        //window.print();

        var myParameters = "versionId="+<%= versionId%>+"&cityId="+<%= cityId%>+"&dealerId="+<%= dealerId%>+"&availability="+<%= numOfDays%>+"&totalPrice="+<%= TotalPrice%>;

        var URL = "/pqcertificate.aspx?"+myParameters;
        var W = window.open(URL);
        //W.window.close(); 
    }

</script>
<!-- Bing Code Starts here -->
<script>(function(w,d,t,r,u){var f,n,i;w[u]=w[u]||[],f=function(){var o={ti:"4051835"};o.q=w[u],w[u]=new UET(o),w[u].push("pageLoad")},n=d.createElement(t),n.src=r,n.async=1,n.onload=n.onreadystatechange=function(){var s=this.readyState;s&&s!=="loaded"&&s!=="complete"||(f(),n.onload=n.onreadystatechange=null)},i=d.getElementsByTagName(t)[0],i.parentNode.insertBefore(n,i)})(window,document,"script","//bat.bing.com/bat.js","uetq");</script>
<noscript>
    <img src="//bat.bing.com/action/0?ti=4051835&Ver=2" height="0" width="0" style="display: none; visibility: hidden;" /></noscript>
<!-- Bing Code Ends here -->
<!-- #include file="/includes/footerInner.aspx" -->


