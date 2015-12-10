<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultCT" Trace="false" %>

<%@ Register TagPrefix="BW" TagName="RepeaterPager" Src="/content/RepeaterPager.ascx" %>
<%@ Register TagPrefix="Mms" TagName="MakeModelSearch" Src="/Controls/MakeModelSearch.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<% 
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<div class="container_12 margin-top15">
    <div id="Div1" class="grid_8">
        <h1>Comparison Tests</h1>
        <div id="content" class="left-grid">
            <div class="entry">
                <Mms:MakeModelSearch ID="MakeModelSearch" RequestType="ComparisonTest" runat="server"></Mms:MakeModelSearch>
                <div class="clear"></div>
            </div>
            <div class="alert moz-round" id="alertObj" runat="server"></div>
            <BW:RepeaterPager ID="rpgCarCompare" PageSize="10" PagerPageSize="10" runat="server">
                <asp:repeater id="rptCarCompare" runat="server" enableviewstate="false">
					<ItemTemplate>					    
						<div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="margin-bottom15">
							<div class="anchor-title">
								<a href="/comparos/<%# DataBinder.Eval(Container.DataItem,"Url") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html" rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
									<%# DataBinder.Eval(Container.DataItem,"Title") %>
								</a>
							</div>
                            <div class="grid_5 alpha">
								<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>, <abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:dd-MMM-yyyy}") %></abbr>								
							</div><div class="clear"></div>
                            <div class="margin-top10">
								<%--<%# DataBinder.Eval(Container.DataItem,"ImagePathThumbnail").ToString() != "" ? "<a class='cbBox' href='" + "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval(Container.DataItem,"ImagePathLarge") + "'><img class='alignright size-thumbnail img-border-news' src='" + "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathThumbNail").ToString() +"' align='right' border='0' /></a>" : "" %>--%>
                                <%# DataBinder.Eval(Container.DataItem,"ImagePathThumbnail").ToString() != "" ? "<a class='cbBox' href='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostURL").ToString(),Bikewale.Utility.ImageSize._310x174) + "'><img class='alignright size-thumbnail img-border-news' src='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostURL").ToString(),Bikewale.Utility.ImageSize._310x174) +"' align='right' border='0' /></a>" : "" %>
								<%# DataBinder.Eval(Container.DataItem,"Description") %>                                
							</div>
                            <div class="margin-top10 item-footer">						       
                                <div class="float-rt"><a href="/comparos/<%# DataBinder.Eval(Container.DataItem,"Url") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html">Read full comparison &raquo;</a></div>                                
                            </div>							
						</div>
                        <div class="sept-dashed"></div>                           					   
				    </ItemTemplate>
			    </asp:repeater>
            </BW:RepeaterPager>
        </div>
    </div>
    <div class="grid_4">
        <!--    Right Container starts here -->
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->
