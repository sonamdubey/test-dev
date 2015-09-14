<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.Quotation" Trace="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398839030772";
    menu = "3";
%>
<!-- #include file="/includes/headermobile_noad.aspx" -->
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?26june2015" />
    <div class="padding5">
        <h1>On Road Price Quote</h1>
    	<div class="box1 box-top new-line5 bot-red new-line10">
            <h2 class="margin-bottom20"><%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%></h2>
            
                <div class="new-line10 selection-box no-border <%= versionList.Count > 1 ?"":"hide" %>"><b>Variants: </b>
                    <asp:DropDownList id="ddlVersion" runat="server" AutoPostBAck="true"></asp:DropDownList>
                </div>
            
            <div class="bike-img">
            	<%--<img src="<%= ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/"+objVersionDetails.LargePicUrl,objVersionDetails.HostUrl) %>" alt="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" title="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" border="0" />--%>
                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objVersionDetails.OriginalImagePath,objVersionDetails.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" title="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" border="0" />
            </div>
          <%--  <div class="rounded-corner5 new-line10 selection-box">
            	<div class="selected-input-text floatleft">401107 (Mira Road,Thane)</div>
                <div class="bw-sprite select-right-arrow floatright"></div>
                <div class="clear"></div>
            </div>--%>
            <div class="new-line10">
            	<h2 class="f-bold">On Road Price Breakup</h2>

                <% if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                   {%>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td height="30" align="left">Ex-Showroom Price</td>
                        <td height="30" align="right"><%= CommonOpn.FormatPrice(objQuotation.ExShowroomPrice.ToString()) %></td>
                      </tr>
                      <tr>
                        <td height="30" align="left">RTO</td>
                        <td height="30" align="right"><%= CommonOpn.FormatPrice(objQuotation.RTO.ToString()) %></td>
                      </tr>
                      <tr>
                        <td height="30" align="left">Insurance</td>
                        <td height="30" align="right"><%=CommonOpn.FormatPrice(objQuotation.Insurance.ToString()) %></td>
                      </tr>
                      <tr align="left">
                  	    <td height="1" colspan="2" class="break-line"></td>
                      </tr>
                      <tr>
                        <td height="30" align="left">Total On Road Price</td>
                        <td height="30" align="right" class="f-bold"><span class="WebRupee">Rs.</span><%=CommonOpn.FormatPrice(objQuotation.OnRoadPrice.ToString()) %></td>
                      </tr>
                    </table>
                <%} else{%>
                <div class="margin-top-10 padding5" style="background:#fef5e6;">Price for this bike is not available in this city.</div>
                <%} %>
            </div>
        </div>
    </div>
<!-- #include file="/includes/footermobile_noad.aspx" -->
