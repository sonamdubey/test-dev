<%@ Page Language="C#" AutoEventWireup="true" Inherits="Bikewale.Used.BikesByMake" %>
<%
    title           = "Make wise used bikes listing - BikeWale";
    keywords        = "Make wise used bikes listing,used bikes for sale, second hand bikes, buy used bike";
    description     = "bikewale.com Make wise used bikes listing.";
%>
<!-- #include file="/includes/headUsed.aspx" -->
<style type="text/css">
.ul-hrz-makes li { width:150px; float:left; padding:5px 0; }
</style>
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
                <li class="current"><strong>Bikes by Make</strong></li>
            </ul>
        </div>
        <div class="clear"></div>
        <h1 class="grid_8  margin-top10">Make wise used bike listings</h1>
        <div id="divContent" class="grid_8 border-light margin-top10">
        <asp:repeater id="rptMake" runat="server">
            <headerTemplate><ul class="ul-hrz-makes margin-top10 margin-left10" style="font-size:13px;"></HeaderTemplate>
            <itemTemplate><li class="margin-left10 width"><a class="href-grey" href="/used/<%#DataBinder.Eval( Container.DataItem, "MaskingName") %>-bikes-in-india/"><%#DataBinder.Eval( Container.DataItem, "MakeName")%> (<%#DataBinder.Eval( Container.DataItem, "MakeCount")%>) </a></li></itemTemplate>
            <footerTemplate></ul></FooterTemplate>
        </asp:repeater>
            </div>
    </div>
<!-- #include file="/includes/footerInner.aspx" -->
