<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.New.UpcomingbikesList" Trace="false" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/UI/m/controls/ListPagerControl.ascx" %>
<% 
    if (meta != null)
    {
        title = meta.Title;
        keywords = meta.Keywords;
        description = meta.Description;
        canonical = meta.CanonicalUrl;
        relPrevPageUrl = meta.PreviousPageUrl;
        relNextPageUrl = meta.NextPageUrl;
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    }
%>
<!-- #include file="/UI/includes/headermobile.aspx" -->
<div class="padding5">
    <div id="br-cr"  itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
        <a href="/m/" class="normal" itemprop="url"><span itemprop="title">Home</span></a> &rsaquo; 
        <a href="/m/new-bikes-in-india/" class="normal">New Bikes</a> &rsaquo; 
        <span class="lightgray">Upcoming Bikes</span>
    </div>
    <h1><%= pageTitle %></h1>
    <Pager:Pager ID="listPager_top" runat="server" />
    <div id="pagesContainer" class="box1 new-line5" style="padding-top:0px;padding-bottom:0px;margin-top:10px;">
	    <div type="page" id="page1">
            <%foreach(var bike in objList){ %>
            <a class="normal" href='/m/<%= bike.MakeBase.MaskingName  %>-bikes/<%=bike.ModelBase.MaskingName %>/' >
		            <div style="text-decoration:none;" class="container">
			            <table cellspacing="0" cellpadding="0" style="width:100%;">
				            <tr>					
                                <td style="width:100px;" valign="top"><img alt="<%= bike.BikeName %>" title="<%= bike.BikeName %>" src='<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._110x61) %>' width="100"></td>
					            <td valign="top" style="padding-left:10px;">
						            <div><h2><b><%= bike.BikeName %></b>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></h2></div>
						            <div class="darkgray new-line5">When to expect</div>
						            <div class="darkgray new-line"> <%= String.IsNullOrEmpty(bike.ExpectedLaunchDate) ? "N/A" : Convert.ToDateTime(bike.ExpectedLaunchDate).ToString("MMMM yyyy") %></div>
						            <div class="darkgray new-line5">Estimated price</div>
						            <div class="darkgray new-line">Rs.<%= Bikewale.Common.CommonOpn.FormatPrice(bike.EstimatedPriceMin.ToString(), bike.EstimatedPriceMax.ToString())%></div>
					            </td>
				            </tr>
			            </table>
		            </div>
	            </a>
            <% } %>
        </div>
    </div>
    <Pager:Pager id="listPager" runat="server"></Pager:Pager>
    </div>
<!-- #include file="/UI/includes/footermobile.aspx" -->
