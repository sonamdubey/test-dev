<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.UpcomingbikesList" Trace="false" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<% 
    title = "Upcoming Bikes in India - Expected Launches in 2014";
    keywords = "Find out upcoming new bikes in 2014 in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.";
    description = "upcoming bikes, new upcoming launches, upcoming bike launches, upcoming models, future bikes, future bike launches, 2012 bikes, speculated launches, futuristic models";
    canonical = "http://www.bikewale.com/upcoming-bikes/";
    relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
    relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "4";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<%--<style type="text/css">
    @media all and (min-width:128px) and (max-width:480px)
    {
        table { font-size:12px;}
    }
</style>--%>
<form runat="server">
<div class="padding5">
    <div id="br-cr">
        <a href="/m/" class="normal">Home</a> &rsaquo; 
        <!--<a href="/m/new/" class="normal">New Bikes</a> &rsaquo; -->
        <span class="lightgray">Upcoming Bikes</span>
    </div>
    <h1>Upcoming Bikes</h1>
    <Pager:Pager ID="listPager_top" runat="server" />
    <div id="pagesContainer" class="box1 new-line5" style="padding-top:0px;padding-bottom:0px;margin-top:10px;">
	    <div type="page" id="page1">
	    <asp:Repeater ID="rptUpcomingBikes" runat="server">
            <ItemTemplate>
                <a class="normal" href='/m/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") %>/' >
		            <div style="text-decoration:none;" class="container">
			            <table cellspacing="0" cellpadding="0" style="width:100%;">
				            <tr>					
                                <%--<td style="width:100px;" valign="top"><img alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") %> <%# DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") %> <%# DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %>" src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"LargePicImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>' width="100"></td>--%>
                                <td style="width:100px;" valign="top"><img alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") %> <%# DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") %> <%# DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100"></td>
					            <td valign="top" style="padding-left:10px;">
						            <div><h2><b><%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") %> <%# DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %></b>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></h2></div>
						            <div class="darkgray new-line5">When to expect</div>
						            <div class="darkgray new-line"> <%# DataBinder.Eval(Container.DataItem,"ExpectedLaunchDate") %></div>
						            <div class="darkgray new-line5">Estimated price</div>
						            <div class="darkgray new-line">Rs.<%# Bikewale.Common.CommonOpn.FormatPrice( DataBinder.Eval(Container.DataItem,"EstimatedPriceMin").ToString(), DataBinder.Eval(Container.DataItem,"EstimatedPriceMax").ToString())%></div>
					            </td>
				            </tr>
			            </table>
		            </div>
	            </a>	
            </ItemTemplate>
        </asp:Repeater>
        </div>
    </div>
    <Pager:Pager id="listPager" runat="server"></Pager:Pager>
    </div>
    </form>
<!-- #include file="/includes/footermobile.aspx" -->