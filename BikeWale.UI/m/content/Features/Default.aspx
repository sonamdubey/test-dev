<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.Features"  Async="true" Trace="false"%>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<% 
    title = "Features - Stories, Specials & Travelogues - BikeWale";
    description = "Features section of BikeWale brings specials, stories, travelogues and much more.";
    keywords = "features, stories, travelogues, specials, drives.";
    canonical = "http://www.bikewale.com/features/";
    relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
    relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "8";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<div class="padding5">
    <form runat="server" id="form1">
        <div id="br-cr"><a href="/m/" class="normal">Home</a> &rsaquo; <span class="lightgray">Features</span></div>
        <h1>Latest Bike Features</h1>
        <div id="divListing">  
            <asp:Repeater id="rptFeatures" runat="server">
                <ItemTemplate>
                    <a class="normal" href='/m/features/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/' >
		                <div class="box1 new-line15" >
                            <table cellspacing="0" cellpadding="0" style="width:100%;overflow:visible;">
				                <tr>
					                <%--<td style="width:100px;vertical-align:top;"><img style="width:100%;max-width:100%;height:auto;" alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title='<%# DataBinder.Eval(Container.DataItem,"Title") %>' src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"LargePicUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>'></td>--%>
                                    <td style="width:100px;vertical-align:top;"><img style="width:100%;max-width:100%;height:auto;" alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title='<%# DataBinder.Eval(Container.DataItem,"Title") %>' src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>'></td>
					                <td valign="top" style="padding-left:10px;">
			                            <div class="sub-heading">
				                            <%# DataBinder.Eval(Container.DataItem,"Title") %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                            </div>
			                            <div class="lightgray new-line" style="font-size:13px;margin-bottom:10px;">
				                            by <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
			                            </div>
                                        <div style="border:1px solid #b3b4c6;background-color:#ffffff;width:100px;position:absolute;right:-1px;bottom:-10px;padding:2px 2px;font-size:13px;" class="lightgray">
                                             <%# Bikewale.Common.CommonOpn.GetDisplayDate( DataBinder.Eval(Container.DataItem,"DisplayDate").ToString()) %>
                                        </div>
                                    </td>
                                </tr>
                            </table>
		                </div>
	                </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <Pager:Pager id="listPager" runat="server"></Pager:Pager>
</div>
<!-- #include file="/includes/footermobile.aspx" -->