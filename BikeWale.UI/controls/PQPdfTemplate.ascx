<%@ Control Language="C#" Inherits="Bikewale.Controls.PQPdfTemplate" AutoEventWireup="false" %>
<div style="display:none;">
    <div style="padding:10px; width:620px; background:#fff; border:1px solid #e6e6e6; margin:0 auto; font-family:Segoe UI;color:#737373;">
	<%--<h1 style="font-size:10px; color:#191919; margin-bottom:5px;"><%=BikeName %>: <%= Oragnization %> Price Quote </h1>--%>
         <h2 style="margin:0; font-size:12px;color:#191919;padding-bottom:10px"><%=BikeName %>: <%= Oragnization %> Price Quote</h2>
	<div>
    	<div style="margin-top:10px;">
            <!--Get pq code starts here-->
            <div style=" border:1px solid #eaeaea; margin-bottom:20px; padding:10px;">
            	<div id="div_ShowPQ">
                    <table cellspacing="0" cellpadding="0" style="font-size:8px;">
                        <tr>
                            <td style="width:100px;padding-left:5px;padding-top:5px;" valign="center">
                                
                                     <%if (!String.IsNullOrEmpty(ImgPath)){ %>
                                        <img alt="<%=BikeName %>" src="<%= ImgPath%>">
                                     <%} %>
                                 
                            </td>
                            
                            <td valign="bottom">
                                      <asp:Repeater ID="rptQuote" runat="server">
                                            <HeaderTemplate>
                                                <table border="1" style="border-color:gray">     
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
                                                    <tr>
                                                        <td style="padding-bottom:5px;" style="font-weight:bold;">Total On Road Price</td>
                                                        <td style="padding-bottom:5px;" width="100" style="font-weight:bold;" align="right"><span>Rs.</span><%=Bikewale.Common.CommonOpn.FormatPrice(TotalPrice.ToString()) %></td>
                                                    </tr>
                                                    <tr>
                                                        <%if(Availability > 0){%>
                                                            <td colspan="2" align="right" class="f-12" border="0"><i>Approximate vehicle waiting period:  <%=Availability %> days </i></td>
                                                        <%}else{ %>
                                                            <td colspan="2" align="right" class="f-12"  border="0"><i>Availability: Vehicle in stock </i></td>
                                                        <%} %>
                                                    </tr>
                                              </table>
                                           </FooterTemplate>
                                    </asp:Repeater>
                            </td>
                        </tr>
                    </table>        
                
            	</div>
               
            </div>
            <!--Get pq code ends here-->
            <!--bw accordion code starts here-->
            <%if (CEmi != null)
              { %>
            <div style="margin-bottom:20px;">

                    <h2 style="font-size:10px; color:#191919; margin:0;padding:0;">Loan EMI Starting From <b><span class="WebRupee">Rs.</span><%=Bikewale.Common.CommonOpn.FormatPrice(CEmi.EMI.ToString()) %></b></h2>
              
                <div style="padding:10px; margin-bottom:20px; border:1px solid black; border-radius:0 0 5px 5px;">
                    <table cellpadding="0" cellspacing="0" width="100%" border="1" style="font-size:8px;">
                        <tr>
                            <td style="padding:5px 0;">Minimum DownPayment</td>
                            <td style="padding:5px 0;" align="right"><b><%=Bikewale.Common.CommonOpn.FormatPrice(CEmi.DownPayment.ToString()) %></b></td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Loan Amount  ( <%=CEmi.objEMI.LoanToValue%> % of Total On-Road Price)</td>
                            <td style="padding:5px 0;" align="right"><b><%= Bikewale.Common.CommonOpn.FormatPrice(CEmi.LoanAmount.ToString()) %></b></td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Maximum Tenure</td>
                            <td style="padding:5px 0;" align="right"><b><%=CEmi.objEMI.Tenure %> Months </b></td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Interest Rate</td>
                            <td style="padding:5px 0;" align="right"><b><%=CEmi.objEMI.RateOfInterest %> % </b></td>
                        </tr>
                       <%-- <tr><td></td><td style="padding:5px 0;">---------------------------------------------------------------------------------------------------------</td></tr>--%>
                        <tr>
                            <td style="font-weight:bold;">EMI<br />
                            </td>
                            <td style="padding:5px 0; font-weight:bold;" align="right"><span>Rs.</span><%=Bikewale.Common.CommonOpn.FormatPrice(CEmi.EMI.ToString()) %></td>
                        </tr>
                    </table>
                </div>
            </div>
            <%} %>
            <!--bw accordion code ends here-->
            <!--Exciting offers div starts here-->
            <h2 style="margin:0; font-size:10px;color:#191919;padding-bottom:10px">Exciting Offers for BikeWale Customers</h2>
            <div style="margin-top:20px;" id="divOffers">
                <table style="margin-top:10px;" border="1">
                    <tr>
                        <td> 
                             <asp:Repeater ID="rptOffers" runat="server">
                                <HeaderTemplate>
                                        <ul style="padding:0; margin:0;font-size:8px;">                                        
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <p style="margin:5px 0 0; display:block; float:left; width:555px;"> <%# DataBinder.Eval(Container.DataItem,"OfferText") %></p>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
            <!--Exciting offers div ends here-->
            <!--Dealer detailes div starts here-->
            <h2 style="margin:0; font-size:10px;color:#191919;padding-bottom:10px">Dealer Details</h2>

            <div style="padding:10px; margin-bottom:20px;margin-top:10px; border:1px solid #eaeaea;">  
                <table border="1">
                    <tr>
                        <th style="padding-left:10px;font-size:8px;">Authorised Dealer Details</th>
                        <th style="padding-left:10px;font-size:8px;">Customers Facilities offered by Dealership</th>
                    </tr>
                    <tr>
                        <td>
                            <p style="margin:0 0 5px; font-size:10px; color:#191919;"><span style="padding-right:10px;"></span><%= Oragnization %></p>
                            <div style="margin-left:20px;">
                                <p style="margin:0;font-size:8px;"><%= address %></p>
                                    <p style="margin:0;font-size:8px;"><%= "" %></p>
                                    <p style="margin:0;font-size:8px;"><b>Working Hours : </b><%=  workingHours   %></p>
                                    <p style="margin:0;font-size:8px"><span style="padding-right:10px;"></span><%=ContactNo%></p>
                   
                            </div>
                        </td>
                        <td>
	                        <asp:Repeater ID="rptFacility" runat="server">
                                <HeaderTemplate>
                                    <ul style="padding-left:10px;font-size:8px;">          
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li style="margin:5px 0; padding-left:10px;"><%#DataBinder.Eval(Container.DataItem,"Facility") %></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
            </table>
               <%-- <h2 style="margin:0; font-size:10px; color:#191919;">Authorised Dealer Details</h2>
                <div style="width:620px;">
                    <div style="width:300px; float:left;">
                        <h3 style="margin:0 0 5px; font-size:10px; color:#191919;"><span style="padding-right:10px;"></span><%= Oragnization %></h3>
                        <div style="margin-left:20px;">
                            <p style="margin:0;font-size:8px;"><%= address %></p>
                             <p style="margin:0;font-size:8px;"><%= "" %></p>
                               <div style="margin:0;font-size:8px;"><b>Working Hours : </b><%=  workingHours   %></div>
                               <p style="margin:0;font-size:8px"><span style="padding-right:10px;"></span><%=ContactNo%></p>
                   
                        </div>
                    </div>
                    <div style="width:300px; border-left:1px solid #eaeaea; float:left;" id="divFacility">
                        <asp:Repeater ID="rptFacility" runat="server">
                            <HeaderTemplate>
                                <h3 style="margin:0 0 0 10px; font-size:10px; color:#191919;">Customers Facilities offered by Dealership</h3>
                                <div style="margin-left:10px;">
                                <ul style="padding:0 0 0 10px; margin:0;font-size:8px;">                                        
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li  style="padding:3px 0;"><%#DataBinder.Eval(Container.DataItem,"Facility") %></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div> 
                </div>
                <div style="clear:both;"></div>--%>
            </div>
            <!--Dealer detailes div ends here-->
            <!--bw accordion code starts here-->
           <h2 style="margin:0; font-size:10px; color:#191919;">Required Documents For RTO & Loan</h2>
            <table border="1">
            <tr>
            <th style="padding-left:10px;font-size:8px;">Documents for Registration</th>
            <th style="padding-left:10px;font-size:8px;">Additional Documents for Loan</th>
            </tr>
            <tr>
            <td>
	            <ul style="padding-left:10px;font-size:8px;">
		            <li><b>Mandatory Documents: </b>
			            <ul style="margin:5px 0; padding-left:10px;">
				            <li>2 Color Photographs</li>
				            <li>PAN Card</li>
			            </ul>
		            </li>
		            <li><b>Identity Proof: </b>
			            <ul style="margin:5px 0; padding-left:10px;">
				          <li>Passport / Voter ID / Driving License</li>
			            </ul>
		            </li>
                    <li><b>Identity Proof: </b>
                        <ul style="margin:5px 0; padding-left:10px;">
                            <li>Last 6 Months Bank Statement</li>  
                            <li>Salary Slip / Latest I.T. Return</li>   
                        </ul>
                    </li>  
	            </ul>
            </td>
            <td style="vertical-align:top;">
	            <ul style="padding-left:10px;font-size:8px;">
                      <li><b>Residential / Address Proof:</b>
                    
			            <ul style="margin:5px 0; padding-left:10px;">
                            <li>(Self-Owned House)</li>
				            <li>Light Bill / Passport</li>
                            <li>Ration Card (Relation Proof)</li>
			            </ul> 
          
                        <ul class="light-grey-bullets margin-bottom5 margin-top5">
                            <li>(Rented House)</li>
                            <li>Registered Rent Agreement + Police N.O.C.</li>
                            <li>Rent Home Electricity Bill</li>
                            <li>Permanent Address Proof</li>
                            <li>Ration Card (Relation Proof)</li>
                        </ul>   
		            </li>
		             
	            </ul>
            </td>
            </tr>
        </table>
        <!--bw accordion code starts here-->
        <div style="margin-bottom:20px;font-size:8px;">
            <i>This price quote is valid only till <%= Bikewale.Common.CommonOpn.GetValidDate(7).ToString("dd MMM, yyyy") %>.</i><br />
        </div>
        <h3 style="font-size:12px; font-weight:bold; color:#333; margin-bottom:10px;">What Happens Next?</h3>
        <table style="padding:10px 10px 0;font-size:8px;" border="1">
            <tr>
                <td>
        	 
                  <table border="0">
                        <tr style="display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px;text-align:center; vertical-align:top; position:relative;">
                            <td  style="font-size:10px;  color:#333;margin:15px 0 26px;"><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-confirmation.png" border="0">Get in touch with Dealership</td>
                      
                    		<td style="text-align:left;"><%= Oragnization %> will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convinient time.</td>
                        </tr>
                        <tr style="display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;">
                            <td  style="font-size:10px; color:#333; margin:15px 0 26px;"><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/claim-price.png" border="0">Claim your Price</td>
                        
                    		<td style="text-align:left;">Please present this price certificate to dealership to claim the price for your purchase.</td>
                        </tr>
                        <tr style="display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;">
                            <td  style="font-size:10px; color:#333;margin:15px 0 26px;"><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/documentation.png" border="0">Be ready with Documentation</td>
                          
                    		<td style="text-align:left;">Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</td>
                        </tr>
                        <tr style="display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;">
                            <td  style="font-size:10px;color:#333;margin:15px 0 26px;"><img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/buy-your-bike.png" border="0">Buy your Bike!</td>
                         
                    		<td style="text-align:left;">Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased <%=BikeName %>.</td>
                        </tr>
                   </table>
                </td>
            </tr>
        </table>

        <div class="margin-top20" style="font-size:8px">
                    <i>Disclaimer</i>
                    <ul class="light-grey-bullets margin-bottom5 margin-top5">
                       <li><i>The bike prices mentioned here are indicative and are supplied by their authorized dealerships. BikeWale takes utmost care in gathering precise and accurate information, financial quotes and promotional offers. However, it may not reflect the final price you may pay.</i></li>
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
</div>
</div>
<!--bw popup code ends here-->
<div style="padding:10px; width:620px; background:#fff; border:1px solid #e6e6e6; margin:0 auto; font-family:Segoe UI; font-size:12px; color:#737373;">
	<div>
    	<div style="margin-top:10px;">
        	<h1 style="font-size:19px; color:#191919; margin-bottom:5px;">Dealer Price Quote</h1>
            <!--Get pq code starts here-->
            <div style=" border:1px solid #eaeaea; margin-bottom:20px; padding:10px;">
            	<div id="div1">
                	<h2 style="font-size:16px; color:#191919; margin:0;">Royal Enfield Classic 350</h2>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="width:100px;vertical-align:top;border-right:1px solid #E5E5E5;">
                                 <div>
                                    <img alt=" Bajaj Avenger 220 DTS- i Photos" src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/548b.jpg" title="Bajaj Avenger 220 DTS- i Photos">
                                 </div>
                            </td>
                            
                            <td valign="top" style="padding-left:20px;">
                                <table>
                                    <h2 style="font-size:16px; color:#191919;">Royal Enfield Classic 350</h2>
                                    <tr>
                                        <td  style="padding-bottom:5px;" width="370">
                                             Ex-Showroom Mumbai
                                        </td>
                                        <td style="padding-bottom:5px;" width="100" align="right"><span id="exShowroomPrice">75,502</span></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:5px;">RTO</td>
                                        <td style="padding-bottom:5px;" width="100" align="right">6,785</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:5px;">Insurance (Comprehensive)</td>
                                        <td style="padding-bottom:5px;" width="100" align="right">1,842</td>
                                    </tr>
                                    <tr><td style="padding-bottom:5px;" colspan="2"><div style="border-bottom:1px dotted #ccc;"></div><td></tr>
                                    <tr>
                                        <td style="padding-bottom:5px;" style="font-weight:bold;">Total On Road Price</td>
                                        <td style="padding-bottom:5px;" width="100" style="font-weight:bold;" align="right"><span>Rs.</span>84,129</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:5px;" colspan="3">Bike available with Zero Dep insurance for <span>Rs.</span>1,33,000</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>        
                
            	</div>
               
            </div>
            <!--Get pq code ends here-->
            <!--bw accordion code starts here-->
            <div style="margin-bottom:20px;">
                <div style="border:1px solid #eaeaea; cursor:pointer; padding:10px; border-radius:5px;">
                    
                    <h2 style=" font-size:16px; color:#191919; margin:0;">Loan EMI Starting From <span></span>10,000</h2>
                </div>
                <div style="padding:10px; margin-bottom:20px; border:1px solid #eaeaea; border-radius:0 0 5px 5px;">
                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                        <tr>
                            <td style="padding:5px 0;">Minimum DownPayment</td>
                            <td style="padding:5px 0;" align="right">21,000</td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Loan Amount</td>
                            <td style="padding:5px 0;" align="right">1,00,000</td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Maximum Tenure</td>
                            <td style="padding:5px 0;" align="right">12 months</td>
                        </tr>
                        <tr>
                            <td style="padding:5px 0;">Interest Rate</td>
                            <td style="padding:5px 0;" align="right">10%</td>
                        </tr>
                        <tr><td style="padding:5px 0;" colspan="2"><div style="border-bottom:1px solid #ccc;"></div></td></tr>
                        <tr>
                            <td style="padding:5px 0; font-weight:bold;">EMI<br />
                            </td>
                            <td style="padding:5px 0; font-weight:bold;" align="right"><span>Rs.</span>10,000</td>
                        </tr>
                        <tr><td><i>*This is an indicative EMI only</i></td></tr>
                    </table>
                </div>
            </div>
            <!--bw accordion code ends here-->
            <!--Exciting offers div starts here-->
            <div style="padding:10px; margin-bottom:20px; border:1px solid #eaeaea;">
                <h2 style="margin:0; font-size:16px; color:#191919; padding-bottom:10px; border-bottom:2px solid #c62000;">Exciting Offers for BikeWale Customers</h2>
                <div style="margin-top:10px;">
                    <ul style="padding:0; margin:0;">
                        <li style=" display:block; margin-bottom:10px;">
                        	<span style="display:block; float:left; width:35px;"></span>
                            <p style="margin:5px 0 0; display:block; float:left; width:555px;">Croma Gift Voucher worth 2,500 </p>
                            <div style="clear:both;"></div>
                        </li>
                        <li style="display:block; margin-bottom:10px;">
                        	<span style="display:block; float:left; width:35px;"></span>
                            <p style="margin:5px 0 0; display:block; float:left; width:555px;">Free accessories worth 3,000</p>
                            <div style="clear:both;"></div>
                        </li>
                        <li style="display:block; margin-bottom:10px;">
                        	<span style="display:block; float:left; width:35px;"></span>
                            <p style="margin:5px 0 0; display:block; float:left; width:555px;">Get a priority delivery within 2 weeks. Normal delivery time ranges from 8 - 12 weeks</p>
                            <div style="clear:both;"></div>
                       	</li>
                    </ul>
                </div>
            </div>
            <!--Exciting offers div ends here-->
            <!--Dealer detailes div stsrts here-->
            <div style="padding:10px; margin-bottom:20px; border:1px solid #eaeaea;">
                
                <h2 style="margin:0; font-size:16px; color:#191919;">Authorised Dealer Details</h2>
                <div style="width:620px;">
                    <div style="width:300px; float:left;">
                        <h3 style="margin:0 0 5px; font-size:14px; color:#191919;"><span style="padding-right:10px;"><img src="images/dealer.png"></span>ABC Dealer</h3>
                        <div style="margin-left:20px;">
                            <p style="margin:0;">6 & 7, Ground Floor, Imperial Plaza, 27 & 30thRoad Junction, Off Linking Road, Bandra (West), Mumbai, Maharashtra-400050</p>
                            <p style="margin:0 0 5px;"><a style="text-decoration:none; color:#0056cc" href="#">Locate on Map</a></p>
                        </div>
                        <p style="margin:0;"><span style="padding-right:10px;"><img src="images/call-icon.png"></span>022 2234 5678</p>
                    </div>
                    
                    <div style="width:300px; border-left:1px solid #eaeaea; float:left;">
                        <h3 style="margin:0 0 0 10px; font-size:14px; color:#191919;">Customers Facilities offered by Dealership</h3>
                        <div style="margin-left:10px;">
                            <ul style="padding:0 0 0 10px; margin:0;">
                                <li style="padding:3px 0;">Free Pick Up & Drop</li>
                                <li style="padding:3px 0;">Cashless Insurance</li>
                                <li style="padding:3px 0;">Extended Waranty</li>
                                <li style="padding:3px 0;">Finance Option</li>
                            </ul>
                        </div>
                    </div>
                    
                </div>
                <div style="clear:both;"></div>
            </div>
            <!--Dealer detailes div ends here-->
            <!--bw accordion code starts here-->
            <div style="margin-bottom:20px;">
            <div style="padding:10px; border:1px solid #eaeaea; border-radius:5px;">
                <h2 style="margin:0; font-size:16px; color:#191919;">Required Documents For RTO & Loan</h2>
            </div>
            <div style=" border-radius:0 0 5px 5px; margin-top:-3px; padding:10px; margin-bottom:20px; border:1px solid #eaeaea;">
                <div style="width:620px;">
                    <div style="width:300px; float:left;">
                        <h3 style="margin:0 0 10px; color:#191919; font-size:14px;">Documents for Registration</h3>
                        <ul style="padding-left:10px;">
                            <li>Income Proof:
                            	<ul style="margin:5px 0; padding-left:10px;">
                                    <li>Salary Slip</li>
                                    <li>Latest I.T Returns</li>
                                </ul>
                            </li>
                            <li>Bank Statement for 6 months</li>
                            <li>Identity Proof:
                            	<ul style="margin:5px 0; padding-left:10px;">
                                	<li>Driving Licence</li>
                                    <li>Voting Card</li>
                                    <li>PAN Card</li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                    
                    <div style="width:300px; float:left; border-left:1px solid #eaeaea;">
                        <h3 style="margin:0 0 10px 10px; color:#191919; font-size:14px;">Documents for Loan</h3>
                        <ul style="padding:0 0 0 20px;">
                            <li>Address Proof:
                            	<ul style="margin:5px 0; padding:0 0 0 10px;">
                                    <li>LIC Policy</li>
                                    <li>Election ID Card</li>
                                    <li>Passport</li>
                                </ul>    
                            </li>
                            <li>Residential Proof
                                <ul style="margin:5px 0; padding:0 0 0 10px;">
                                    <li>Ration Card</li>
                                    <li>Electricity Bill</li>
                                    <li>Regd.Rent Agreement</li>
                                    <li>Policy N.O.C</li>
                                    <li>House Tax Receipt</li>
                                    <li>Telephone Bill</li>
                                    <li>Gas Receipt</li>
                                    <li>Owner Electricity Bill</li>
                                    <li>Permanent Address Proof</li>
                                    
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
                <i>This price quote is valid only till 31 Oct, 2014.</i><br />
                <p style="margin:0;">Please read other <a style="text-decoration:none; color:#0056cc;" href="#">Important Terms & Conditions</a></p>
            </div>
        </div>
        
        
    </div>
</div>
