<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.Quotation" Trace="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398839030772";
    menu = "3";
%>
<script>var quotationPage = true;</script>
<!-- #include file="/includes/headermobile_noad.aspx" -->
<link rel="stylesheet"  href="/m/css/bw-new-style.css?<%= staticFileVersion %>" />
    
    	<div class="box1 box-top bot-red">
            
            
            <div class="bike-img new-line10">
            	<%--<img src="<%= ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/"+objVersionDetails.LargePicUrl,objVersionDetails.HostUrl) %>" alt="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" title="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" border="0" />--%>
                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objVersionDetails.OriginalImagePath,objVersionDetails.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" title="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" border="0" />
            </div>
            <h1 class="margin-top20 font18 padding-left10 padding-right10" style="margin-left:0px;"><%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%></h1>
            
                <div class="<%= versionList.Count>1 ?"":"hide" %> margin-top20">
                    <asp:DropDownList id="ddlVersion" runat="server" AutoPostBAck="true"></asp:DropDownList>
                </div>
          <%--  <div class="rounded-corner5 new-line10 selection-box">
            	<div class="selected-input-text floatleft">401107 (Mira Road,Thane)</div>
                <div class="bw-sprite select-right-arrow floatright"></div>
                <div class="clear"></div>
            </div>--%>
            <div class="new-line15 padding-left10 padding-right10" style="margin-top:20px;">
            	<h2 class="font16" style="font-weight:normal">On-road price in Andheri, Mumbai</h2>

                <% if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                   {%>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
                      <tr>
                        <td class="text-medium-grey" align="left">Ex-Showroom Price</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span> <%= CommonOpn.FormatPrice(objQuotation.ExShowroomPrice.ToString()) %></td>
                      </tr>
                      <tr>
                        <td  class="text-medium-grey" align="left">RTO</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span> <%= CommonOpn.FormatPrice(objQuotation.RTO.ToString()) %></td>
                      </tr>
                      <tr>
                        <td  class="text-medium-grey" align="left">Insurance</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span> <%=CommonOpn.FormatPrice(objQuotation.Insurance.ToString()) %></td>
                      </tr>
                      <tr align="left"><td height="10" colspan="2" style="padding:0;"></td></tr>
                      <tr align="left"><td height="1" colspan="2" class="break-line" style="padding:0 0 10px;"></td></tr>
                      <tr>
                        <td  class="text-grey font16" align="left">Total On Road Price</td>
                        <td class="text-grey text-bold font18" align="right" class="f-bold"><span class="fa fa-rupee"></span> <%=CommonOpn.FormatPrice(objQuotation.OnRoadPrice.ToString()) %></td>
                      </tr>
                    </table>
                <%} else{%>
                <div class="margin-top-10 padding5" style="background:#fef5e6;">Price for this bike is not available in this city.</div>
                <%} %>
            </div>
        </div>
    
<!-- #include file="/includes/footermobile_noad.aspx" -->
