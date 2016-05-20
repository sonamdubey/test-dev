<%@ Page Language="C#" Inherits="Bikewale.Mobile.New.NewBikeLaunch" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<% 
    title = "New Bikes Launches in " + year + " - BikeWale";
    keywords = "new bikes 2014, new bike launches in " + year + ", just launched bikes, new bike arrivals, bikes just got launched";
    description = "List of all new bikes launched in India in " + year + " (last eight months)";
    canonical = "http://www.bikewale.com/new-bikes-launches/";
    relPrevPageUrl = prevPageUrl;
    relNextPageUrl = nextPageUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "5";
    Ad_320x50 = true;
    Ad_Bot_320x50 = true;
%>
<!-- #include file="/includes/headermobile.aspx" -->
<div class="padding5">
    <div id="br-cr"  itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
        <a href="/m/" class="normal" itemprop="url"><span itemprop="title">Home</span></a> &rsaquo; 
        <!--<a href="/m/new/" class="normal">New Bikes</a> &rsaquo; -->
        <span class="lightgray">New Bike Launches in <%=year %></span>
    </div>
    <h1>New Bikes Launches</h1>
    <div id="pagesContainer" class="box1 new-line5" style="padding-top:0px;padding-bottom:0px;margin-top:10px;">
	    <div type="page" id="page1">
	    <asp:Repeater ID="rptLaunchedBikes" runat="server">
            <ItemTemplate>
                <a class="normal" href='/m/<%#DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/' >
		            <div style="text-decoration:none;" class="container">
			            <table cellspacing="0" cellpadding="0" style="width:100%;">
				            <tr>					
                                <%--<td style="width:100px;" valign="top"><img alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + DataBinder.Eval(Container.DataItem, "LargePicUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>' width="100"></td>--%>
                                <td style="width:100px;" valign="top"><img alt="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100"></td>
					            <td valign="top" style="padding-left:10px;">
						            <div><h2><b><%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " +  DataBinder.Eval(Container.DataItem,"ModelName")%></b>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></h2></div>
						            <div class="darkgray new-line5">Launched On</div>
						            <div class="darkgray new-line"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"LaunchDate")).ToString("dd-MMM-yyyy") %></div>
						            <div class="darkgray new-line5">Ex-Showroom Price, <%= Bikewale.Common.Configuration.GetDefaultCityName %></div>
						            <div class="darkgray new-line">Rs. <%#Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) %></div>
                                    <div class="margin-top10 ">
                                    <div class="darkgray new-line <%# DataBinder.Eval(Container.DataItem,"ReviewCount").ToString() == "0" ? "hide" : "" %>"> <%#Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ReviewRate"))) %> &nbsp; <span style="position:relative;top:2px;"><%#DataBinder.Eval(Container.DataItem,"ReviewCount") %> reviews</span></div>
                                </div>
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
<!-- #include file="/includes/footermobile.aspx" -->
