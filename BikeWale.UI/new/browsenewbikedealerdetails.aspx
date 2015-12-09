<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.BrowseNewBikeDealerDetails" Trace="false" Debug="false" %>

<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<% 
    keywords = makeName + " dealers city, Make showrooms " + strCity + "," + strCity + " bike dealers, " + makeName + " dealers, " + strCity + " bike showrooms, bike dealers, bike showrooms, dealerships";
    description = makeName + " bike dealers/showrooms in " + strCity + ". Find " + makeName + " bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
    title = makeName + " Dealers in city | " + makeName + " New bike Showrooms in " + strCity + " - BikeWale";
    canonical = "http://www.bikewale.com/new/" + MakeMaskingName + "-dealers/" + cityId + "-" + strCity + ".html";
    alternate = "http://www.bikewale.com/m/new/" + MakeMaskingName + "-dealers/" + cityId + "-" + strCity + ".html";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";    
%>
<!-- #include file="/includes/headNew.aspx" -->


<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/">New Bikes</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/locate-dealers/">New Bike Dealer</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/<%=MakeMaskingName%>-dealers/"><%=makeName%> Dealers</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%=makeName%> Dealers in <%=strCity%></strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8  margin-top10">
        <!--    Left Container starts here -->
        <h1><%=makeName%> Dealers/Showrooms in <%=strCity%></h1>
        <div class="margin-top5 margin-bottom15"><%=makeName%> bike company has <%=dealerCount%> new bike dealers/showrooms in <%=strCity%>.</div>
        <div>
            <asp:label id="lblResults1" runat="server"></asp:label>
            <asp:datalist id="dlDealers" repeatdirection="Vertical" repeatlayout="Table" cellpadding="3" runat="server" class="tbl-std">				
				        <itemtemplate>
					        <div class="grey-bg content-block">
						        <h3 class="margin-bottom5"><%# DataBinder.Eval(Container.DataItem, "DealerName").ToString() %></h3>
						        <%# GetAddress(DataBinder.Eval(Container.DataItem, "Address").ToString(), "", DataBinder.Eval(Container.DataItem, "City").ToString(), DataBinder.Eval(Container.DataItem, "State").ToString(), DataBinder.Eval(Container.DataItem, "PinCode").ToString())%>
						        <%# GetContactNumbers(DataBinder.Eval(Container.DataItem, "ContactNo").ToString(),"",DataBinder.Eval(Container.DataItem, "FaxNo").ToString())%>	
						        <%# GetWebsite(DataBinder.Eval(Container.DataItem, "WebSite").ToString())%>
						        <%# GetEmail(DataBinder.Eval(Container.DataItem, "EMailId").ToString(), "")%>
						        <%# DataBinder.Eval(Container.DataItem, "WorkingHours")%>					
						        <div class="margin-top10"><span class="action-btn"><a href="/pricequote/default.aspx?make=<%=makeId %>">Check On-Road Price</a></span>&nbsp;&nbsp;<a href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString()%>-bikes/"><%=makeName%> Bikes</a>&nbsp; | &nbsp;<a href="/user-reviews/"><%=makeName%> User Reviews</a></div>
					        </div>
				        </itemtemplate>
			        </asp:datalist>
        </div>
    </div>
    <!--    Left Container ends here -->
    <div class="grid_4">
        <!--    Right Container starts here -->
        <div class="margin-top5">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />
            <div class="clear"></div>
        </div>
    </div>
    <!--    Right Container ends here -->
</div>

<PW:PopupWidget runat="server" ID="PopupWidget" />
<!-- #include file="/includes/footerInner.aspx" -->
