﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.News.Default"  Trace="false" Async="true"%>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<% 
    title       = "Bike News - Latest Indian Bike News & Views - BikeWale";
    description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
    keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
    canonical   = "http://www.bikewale.com/news/";
    relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
    relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "6";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<form runat="server"> 
    <div class="padding5">
        <div id="br-cr"><a href="/m/" class="normal">Home</a> &rsaquo; <span class="lightgray">News</span></div>
        <h1>Latest Bike News</h1>
        <div id="divListing">
            <asp:Repeater id="rptNews" runat="server">
                <ItemTemplate>
                    <a class="normal" href='/m/news/<%# DataBinder.Eval(Container.DataItem,"BasicId") %>-<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>.html' >
		                <div class="box1 new-line15" >
                            <table cellspacing="0" cellpadding="0" style="width:100%;overflow:visible;">
				                <tr>
					                <%--<td style="width:100px;vertical-align:top;"><img style="width:100%;max-width:100%;height:auto;" alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"LargePicUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>'></td>--%>
                                    <td style="width:100px;vertical-align:top;"><img style="width:100%;max-width:100%;height:auto;" alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>'></td>
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
</form>
<!-- #include file="/includes/footermobile.aspx" -->