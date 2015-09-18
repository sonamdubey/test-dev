<%@ Page Language="C#" AutoEventWireup="true" Inherits="Bikewale.Used.BikesInCity" %>
<%
    title = "city wise used bikes listing - BikeWale";
    keywords = "city wise used bikes listing,used bikes for sale, second hand bikes, buy used bike";
    description = "bikewale.com city wise used bikes listing.";
%>
<!-- #include file="/includes/headUsed.aspx" -->
    <div  class="container_12">
         <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/used/">Used Bikes</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a href="/used/bikes-in-india/">Search</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Bikes in City</strong></li>
            </ul>           
        </div>
        <div class="clear"></div>
        <h1 class="grid_8  margin-top10">City wise used bikes listings</h1>
        <div id="divContent" class="grid_8 margin-top10  border-light">
        <asp:repeater id="rptCity" runat="server">
            <headerTemplate><ul class="ul-hrz margin-top10 margin-left10" style="font-size:13px;"></HeaderTemplate>
            <itemTemplate><li><a class="href-grey" href="/used/bikes-in-<%#DataBinder.Eval( Container.DataItem, "CityMaskingName") %>/"><%#DataBinder.Eval( Container.DataItem, "City")%> (<%#DataBinder.Eval( Container.DataItem, "BikeCount")%>) </a></li></itemTemplate>
            <footerTemplate></ul></FooterTemplate>
        </asp:repeater>
            </div>
    </div>
<!-- #include file="/includes/footerInner.aspx" -->