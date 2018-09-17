<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.PQCertificate" EnableEventValidation="false"%>

<!doctype html>
<html>
<head>
    <title><%= BikeName %> - Price Certificate</title>
    <meta charset="utf-8">
    <link href="/UI/css/bw-pq.css?14sept2015" rel="stylesheet" />
        <style type="text/css">
            .hide { display:none}
            .breakhere {page-break-before: always}
        </style>
    <script type="text/javascript" src="https://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
</head>

<body>
    <script type="text/javascript">
        jQuery(window).load(function () {
            window.print();

            window.onafterprint = function () {
                window.close();
            }
        });
    </script>
<div style="padding:10px;background:#fff; border:1px solid #e6e6e6; margin:0 auto; font-family:Segoe UI; font-size:14px; color:#737373;width:820px;">
	<div>
    	<div style="margin-top:10px;">
        	<h1 style="font-size:19px; color:#191919; margin-bottom:5px;"><%=Organization %> Dealer Price Quote</h1>
            <!--Get pq code starts here-->
            <div style=" border:1px solid #eaeaea; margin-bottom:20px; padding:10px;">
            	<div id="div_ShowPQ">
                	<h2 style="font-size:16px; color:#191919; margin:0;"><%= BikeName %></h2>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="width:40%;vertical-align:central;border-right:1px solid #E5E5E5">
                                 <div>
                                    <img alt="<%= BikeName %> Images" src="<%= ImgPath %>" title="<%=BikeName %> Images">
                                 </div>
                            </td>
                            
                            <td valign="top" style="padding-left:20px;">
                                <asp:Repeater ID="rptQuote" runat="server">
                                        <HeaderTemplate>
                                        <table>     
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td  style="padding-bottom:5px;" width="370">
                                                        <%# DataBinder.Eval(Container.DataItem,"CategoryName") %>
                                            </td>
                                            <td style="padding-bottom:5px;" width="100" align="right"><span id="exShowroomPrice"><b>  <%#Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></b></span></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr><td colspan="2"><div style="border-bottom:1px dotted #ccc;"></div><td></tr>
                                        <tr>
                                            <td style="padding-bottom:5px;" style="font-weight:bold;">Total On Road Price</td>
                                            <td style="padding-bottom:5px;" width="100" style="font-weight:bold;" align="right"><b>Rs. <%=Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></b></td>
                                        </tr>
                                        <tr>
                                            <%if(noOfDays > 0){%>
                                                <td colspan="2" align="right" style="padding-bottom:5px;"><i>Approximate vehicle waiting period:  <%=noOfDays %> days </i></td>
                                            <%}else{ %>
                                                <td colspan="2" align="right" style="padding-bottom:5px;"><i>Availability: Vehicle in stock </i></td>
                                            <%} %>
                                        </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                                <tr>
                                    <td colspan="3">
                                        <ul class="std-ul-list">
                                            <asp:Repeater id="rptDisclaimer" runat="server">
                                                <ItemTemplate>
                                                    <li><i><%# Container.DataItem %></i></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
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
            <!--bw accordion code starts here-->
            <%if (objCEMI != null) {%>
            <div style="margin-bottom:20px;">
                <div style="border:1px solid #eaeaea; cursor:pointer; padding:10px; border-radius:5px;">
                      <h2  style=" font-size:16px; color:#191919; margin:0;">Bank Loan Details <span style="font-size:14px;font-weight:normal;">(Loan EMI starting from <b>Rs. <%=Bikewale.Common.CommonOpn.FormatPrice( Math.Ceiling(objCEMI.EMI).ToString()) %></b>)</span> </h2>
                </div>
                <div style="padding:10px; margin-bottom:20px; border:1px solid #eaeaea; border-radius:0 0 5px 5px;">
                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                        <tr>
                            <td style="padding:5px 0;">Minimum Down Payment</td>
                            <td style="padding:5px 0;" align="right"><b><%=Bikewale.Common.CommonOpn.FormatPrice((objCEMI.DownPayment.ToString())) %></b></td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Loan Amount (up to <%=objCEMI.objEMI.LoanToValue %>% of Total On-Road Price) </td>
                            <td style="padding:5px 0;" align="right"><b><%= Bikewale.Common.CommonOpn.FormatPrice((objCEMI.LoanAmount.ToString())) %></b></td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Maximum Tenure</td>
                            <td style="padding:5px 0;" align="right"><b><%= objCEMI.objEMI.Tenure %> Months </b></td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Interest Rate</td>
                            <td style="padding:5px 0;" align="right"><b><%=objCEMI.objEMI.RateOfInterest %> % </b></td>
                        </tr>
                        <tr class="<%= !String.IsNullOrEmpty(objCEMI.objEMI.LoanProvider) ? "" : "hide" %>">
                            <td height="30" align="left">Loan Provider</td>
                            <td height="30" align="right"><b><%=objCEMI.objEMI.LoanProvider %></b></td>
                        </tr>
                        <tr><td style="padding:5px 0;" colspan="2"><div style="border-bottom:1px solid #ccc;"></div></td></tr>
                        <tr>
                            <td style="padding:5px 0; font-weight:bold;">EMI<br />
                            </td>
                            <td style="padding:5px 0; font-weight:bold;" align="right"><span>Rs. </span><b><%= Bikewale.Common.CommonOpn.FormatPrice(Math.Ceiling(objCEMI.EMI).ToString()) %></b></td>
                        </tr>
                         <tr>
                            <td colspan="2" style="font-size:11px !important">
                                <i>*This is an indicative EMI value. The final loan amount, interest rate and EMI depend on buyer's credit profile and can be decided by the financing bank only. Moreover, an additional loan processing fee may be charged by the bank.</i>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <%} %>
            <!--bw accordion code ends here-->
            <!--Exciting offers div starts here-->
           <%if (objPQ.objOffers != null && objPQ.objOffers.Count > 0){ %>
            <div  class="bw-offer-box" style="background-color:#fff;border:2px dotted #eaeaea;">
                <h2 style="margin:0; font-size:16px; color:#191919; padding-bottom:10px">Exclusive Offers for BikeWale Customers</h2>
                <div style="margin-top:5px;">
                   <asp:Repeater ID="rptOffers" runat="server">
                        <HeaderTemplate>
                            <ul>                                        
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li><%# DataBinder.Eval(Container.DataItem,"OfferCategoryId").ToString() != "3" ? DataBinder.Eval(Container.DataItem,"OfferText") : ""%> </li>
                        </ItemTemplate>
                        <FooterTemplate>
                                <li>Get one year of Bike Roadside Assistance absolutely FREE.</li>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <%} %>
            <!--Exciting offers div ends here-->
            <!--Dealer detailes div stsrts here-->
            <%if (objPQ.objDealer != null){ %>
            <div style="padding:10px; margin-bottom:20px; border:1px solid #eaeaea;">
                
                <h2 style="margin:0; font-size:16px; color:#191919;">Authorised Dealer Details</h2>
                <div style="width:620px;">
                    <div style="width:300px; float:left;">
                        <h3 style="margin:0 0 5px; font-size:14px; color:#191919;"><%=objPQ.objDealer.Organization %></h3>
                        <div style="margin-left:20px;">
                            <p style="margin:0;"><%= objPQ.objDealer.Address%></p>
                            <p style="margin:0;"><%= address %></p>
                            <p style="margin:0 0 5px;"><%=contactNo %></p>
                        </div>
                    </div>
                    
                    <%if (objPQ.objFacilities != null && objPQ.objFacilities.Count > 0){ %>
                    <div style="width:300px; border-left:1px solid #eaeaea; float:left;margin-left:10px;">
                        <h3 style="margin:0 0 0 10px; font-size:14px; color:#191919;">Customers Facilities offered by Dealership</h3>
                        <div style="margin-left:10px;">
                            <asp:Repeater ID="rptFacility" runat="server">
                                <HeaderTemplate>
                                    <ul style="padding:0 0 0 10px; margin:0;">                                        
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li style="padding:3px 0;"><%#DataBinder.Eval(Container.DataItem,"Facility") %></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <%} %>
                    
                </div>
                <div style="clear:both" ></div>
            </div>
            <%} %>
            <!--Dealer detailes div ends here-->
            <!--bw accordion code starts here-->
            <div style="margin-bottom:20px;">
            <div style="padding:10px; border:1px solid #eaeaea; border-radius:5px;">
                <h2 style="margin:0; font-size:16px; color:#191919;">Required Documents For RTO & Loan</h2>
            </div>
            <div style=" border-radius:0 0 5px 5px; margin-top:-3px; padding:10px; margin-bottom:20px; border:1px solid #eaeaea;">
                <div style="width:620px;">
                    <div style="width:300px; float:left;">
                       
                        <ul style="padding:0 0 0 20px;">
                            <li>Mandatory Documents:
                            	<ul style="margin:5px 0; padding:0 0 0 10px;">
                                    <li>2 Color Photographs</li>
                                    <li>PAN Card</li>
                                </ul>    
                            </li>
                            <li>Identity Proof:
                                <ul style="margin:5px 0; padding:0 0 0 10px;">
                                    <li>Passport / Voter ID / Driving License</li>
                                </ul>
                            </li>   
                            <li>Additional Documents for Loan:
                                <ul style="margin:5px 0; padding:0 0 0 10px;">
                                    <li>Last 6 Months Bank Statement</li>
                                    <li>Salary Slip / Latest I.T. Return</li>
                                </ul>
                            </li> 
                         </ul>
                    </div>
                    <div style="width:300px; float:left; border-left:1px solid #eaeaea;">
                       <ul style="margin-left:10px;">
                            <li>Residential / Address Proof:
                                <ul style="margin:5px 0; padding-left:10px;">
                                    <div style="margin-top:5px">(Self-Owned House)</div>
                                    <li>Light bill / Passport</li>
                                    <li>Ration Card (Relation Proof)</li>
                                    <div style="margin-top:5px">(Rented House)</div>
                                    <li>Registered Rent Agreement + Police N.O.C.</li>
                                    <li>Rent Home Electricity Bill</li>
                                    <li>Permanent Address Proof</li>
                                    <li>Ration Card (Relation Proof)</li>
                                </ul>
                            </li>
                       </ul>
                    </div>
                </div>
                <div style="clear:both;"></div>
            </div>
            </div>
            <!--bw accordion code starts here-->
            <div style="margin-bottom:20px;">
                <i>This price quote is valid only till  <%= Bikewale.Common.CommonOpn.GetValidDate(7).ToString("dd MMM, yyyy") %>.</i><br />
                <%--<p style="margin:0;">Please read other <a style="text-decoration:none; color:#0056cc;" href="/termsconditions.aspx">Important Terms & Conditions</a></p>--%>
            </div>
        </div>
         </div>
           
    </div>
    <div style="padding:10px;  background:#fff; border:1px solid #e6e6e6; margin:0 auto;margin-top:15px !important;  font-family:Segoe UI; font-size:14px; color:#737373;width:820px;">
                    <div style="padding:10px 10px 0;">
        	            <div style="font-size:16px; font-weight:bold; color:#333; margin-bottom:10px; border-bottom:1px solid #c62000; padding-bottom:10px;">Next Steps</div>
                            <div style="padding-top:10px; text-align:center;">
                    
                                <div class="next-step">
                                    <div><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-confirmation.png" border="0"></div>
                                    <div  class="next-step-heading" style="margin:15px 0;">Get in touch with Dealership</div>
                    		            <div class="next-step-content"> <%= objPQ.objDealer.Organization  %> will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convenient time.</div>
                                </div>
                                <div class="next-step">
                                    <div><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/claim-price.png" border="0"></div>
                                    <div class="next-step-heading" style="margin:15px 0 26px;">Claim your Price</div>
                    		            <div class="next-step-content">Please present this price certificate to dealership to claim the price for your purchase.</div>
                                </div>
                                <div class="next-step">
                                    <div><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/documentation.png" border="0"></div>
                                    <div class="next-step-heading" style="margin:15px 0;">Be ready with Documentation</div>
                    		            <div class="next-step-content">Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</div>
                                </div>
                              <div class="next-step">
                                    <div><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/buy-your-bike.png" border="0"></div>
                                    <div class="next-step-heading" style="margin:15px 0 26px;">Buy your Bike!</div>
                    		            <div class="next-step-content">Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased <%=BikeName %>.</div>
                                </div>
                            </div>
                    </div>
        </div>
        <div style="padding:10px;  background:#fff; border:1px solid #e6e6e6;font-family:Segoe UI;border-top:0px; font-size:14px; color:#737373;width:820px; margin:0 auto;">
            <div>
                <div style="font-size:11px !important; margin-top:20px;">
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
        </div>
   
</body>
</html>
