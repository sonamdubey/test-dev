<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.New_Default_old" %>
<%@ Register TagPrefix="NBD" TagName="NewBikeDealers" Src="/m/controls/LocateDealer.ascx" %>
<%
    title = "New Bike Dealers in India - Locate Authorized Showrooms - BikeWale";
    keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships, price quote";
    description = "Locate New bike dealers and authorized bike showrooms in India. Find new bike dealer information for more than 200 cities. Authorized company showroom information includes full address, phone numbers, email address, pin code etc.";
    canonical = "http://www.bikewale.com/new/locate-dealers/";
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "10";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
    <div class="padding5">
        <div id="br-cr"  itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a href="/m/new/" class="normal" itemprop="url"><span itemprop="title">New Bikes</span></a> &rsaquo; 
            <span class="lightgray">New Bike Dealers / Showrooms in India</span>
        </div>
        <h1>New Bike Dealers / Showrooms in India</h1>
        <p class="lightgray f-12 new-line10">Find new bike dealers and authorized showrooms in India. New bike dealer information for more than 200 cities is available. Click on a bike manufacturer name to get the list of its authorized dealers in India.</p>
        <div class="new-line10">
                <NBD:NewBikeDealers ID="NewBikeDealers1" runat="server" HeaderText="Search Dealers by City & Manufacturer"></NBD:NewBikeDealers>
        </div>
    </div>
<!-- #include file="/includes/footermobile.aspx" -->