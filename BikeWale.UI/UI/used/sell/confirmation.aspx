<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.Confirmation" %>
<%
   is300x250BTFShown = false;
    is300x250Shown = false;
%>
<!-- #include file="/includes/headUsed.aspx" -->
    <div class="container_12 margin-top15">
        <div class="grid_8 min-height">
            <div class="grey-bg content-block border-light margin-top10">
		        <h2 id="msg_listing" runat="server" align="left" class="margin-top10">Congratulations!! Your bike is now ready to be sold. Your bike will be live after verification.</h2>
		        <div class="margin-top10 action-btn red-text"  style="font-weight:bold;">Manage your listing in My BikeWale <a href="/mybikewale/mylisting.aspx">S<%= inquiryId %></a></div>
	        </div>
        </div>
    </div>
<!-- #include file="/includes/footerinner.aspx" -->