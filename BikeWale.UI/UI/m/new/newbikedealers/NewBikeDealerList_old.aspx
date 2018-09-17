<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.New.NewBikeDealerList_Old" %>
<% 
    title = make + " Dealers in city | " + make + " New bike Showrooms in "+ city + " - BikeWale";
    keywords = make + " dealers city, Make showrooms "+ city + "," + city +" bike dealers, " + make + " dealers, " + city + " bike showrooms, bike dealers, bike showrooms, dealerships";
    description = make + " bike dealers/showrooms in "+ city + ". Find " + make + " bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
    canonical = canonicalUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "10";
%>
<!-- #include file="/UI/includes/headermobile.aspx" -->
<style>
    .pad5 {padding:5px;}
    .borBot{border-bottom:1px solid #b3b4c6;padding-bottom:10px;}
    .hide {display:none;}
</style>
    <div class="padding5">
        <div id="br-cr" itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a href="/m/new-bikes-in-india/" class="normal" itemprop="url"><span itemprop="title">New Bikes</span></a> &rsaquo; 
            <a href="/m/new/locate-dealers/" class="normal" itemprop="url"><span itemprop="title">Locate Dealers</span></a> &rsaquo; 
            <span class="lightgray"><%=make %> Dealers in <%=city %> </span>
        </div>
        <h1><%=make %> Bikes Dealers/Showrooms in <%=city %></h1>
        <%if(dealerCount > 0) {%>
        <div class="box1 new-line5" style="font-size:13px!important;padding:0px 5px;">
            <asp:Repeater id="rptDealers" runat="server">
                <itemtemplate><%# GetDealerCity( DataBinder.Eval(Container.DataItem, "CityName").ToString())%>
	                <div class="pad5 new-line5"><h2><b><%#DataBinder.Eval(Container.DataItem, "Name").ToString()%></b></h2></div>
                    <div class="pad5"><%#GetFormattedAddress(DataBinder.Eval(Container.DataItem, "Address").ToString(), DataBinder.Eval(Container.DataItem, "CityName").ToString(), DataBinder.Eval(Container.DataItem, "State.StateName").ToString(),DataBinder.Eval(Container.DataItem, "PinCode").ToString())%></div>
                    <div class='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "PhoneNo").ToString()) ? "hide" : "pad5" %>'>Phone : <%#DataBinder.Eval(Container.DataItem, "PhoneNo").ToString()%></div>
                    <div class='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Fax").ToString()) ? "hide" : "pad5" %>'>Fax : <%#DataBinder.Eval(Container.DataItem, "Fax").ToString()%></div>
                    <div class='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Website").ToString()) ? "hide" : "pad5" %>'>Website : <a target="new" href="https://<%#DataBinder.Eval(Container.DataItem, "Website").ToString().Replace("http://", "")%>"><%#DataBinder.Eval(Container.DataItem, "Website").ToString().Replace("http://", "")%></a></div>
                    <div class='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Email").ToString()) ? "hide" : "pad5"%>'>Email : <%#DataBinder.Eval(Container.DataItem, "Email").ToString()%></div>
                    <div class="borBot"></div>
                </itemtemplate>
             </asp:Repeater>
        </div>
        <%} %>
    </div>
<!-- #include file="/UI/includes/footermobile.aspx" -->

