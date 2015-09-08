﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewCT" Trace="false"%>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<% 
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<div class="container_12 margin-top15">
    <div class="grid_8">		
		<h1><%= ArticleTitle%></h1>
        <div class="byline" style="padding-bottom:5px;">A comparison road test of <asp:Label ID="lblCarNames" runat="server" /></div>        
		<div class="byline"><asp:Label ID="lblAuthor" runat="server" />, <asp:Label ID="lblDate" runat="server" /></div>        
		
        <div class="clear"></div>
		<div class="navStrip" id="topNav" runat="server">
			<div align="right" style="width:245px;float:right;">
				<asp:DropDownList ID="drpPages" CssClass="drpClass" AutoPostBack="true" runat="server"></asp:DropDownList>
			</div>
			<div style="width:380px; padding:5px 0;">
				<b>Read Page : </b>
				<asp:Repeater ID="rptPages" runat="server">
					<itemtemplate>
						<%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url ) %>
					</itemtemplate>
					<footertemplate>
						<% if ( ShowGallery )  { %>
						<%# CreateNavigationLink( str, Url ) %>
						<% } %>	
					</footertemplate>
				</asp:Repeater>
			</div>	
		</div>
		<div class="readable">
			<asp:Label ID="lblDetails" runat="server" />
			<asp:DataList ID="dlstPhoto" DataKeyField="ID" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
				<itemtemplate>
					<%--<a rel="slidePhoto" target="_blank" href="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathLarge" ).ToString() %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ) %></b>" />
						<img alt="<%# DataBinder.Eval( Container.DataItem, "Name" ) %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ).ToString() %>" title="Click to view larger photo" />
					</a>--%>
                    <a rel="slidePhoto" target="_blank" href="<%# Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString(),DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() ,"310x174") %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ) %></b>" />
						<img alt="<%# DataBinder.Eval( Container.DataItem, "Name" ) %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString(),DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() ,Bikewale.Utility.ImageSize._310x174) %>" title="Click to view larger photo" />
					</a>
				</itemtemplate>
			</asp:DataList>
		</div>
		<div class="navStrip" id="bottomNav" runat="server">
			<div align="right" style="width:245px;float:right;">
				<asp:DropDownList ID="drpPages_footer" AutoPostBack="true" CssClass="drpClass" runat="server"></asp:DropDownList>
			</div>
			<div style="width:380px; padding:5px 0;">
				<b>Read Page : </b>
				<asp:Repeater ID="rptPages_footer" runat="server">
					<itemtemplate>
						<%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url) %>
					</itemtemplate>
					<footertemplate>
						<% if ( ShowGallery )  { %>
						<%# CreateNavigationLink( str, Url ) %>
						<% } %>	
					</footertemplate>
				</asp:Repeater>
			</div>	
		</div>
    </div>
    <div class="grid_4"><!--    Right Container starts here -->
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>                
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
</div>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $("a[rel='slidePhoto']").colorbox();
    });
</script>
<!-- #include file="/includes/footerInner.aspx" -->

