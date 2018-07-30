<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultTA" Trace="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="RP" TagName="RepeaterPager" src="/content/RepeaterPager.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%
    title 			= "Bike Tips, Advices, How-To's and Do It Yourself";
	description 	= " Tips, advices, how-to's and DIYs for bike driving, ownership and maintenance. Know what to do and what not to around everyday bike ownership and driving.";
   	keywords		= "bike tips, bike advices, bike how to, do it yourself, DIY, bike DIY";   
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_"; 
%>
<!-- #include file="/includes/headnew.aspx" -->
<script type"text/javascript">
    $(document).ready(function () {
        $("a.cbBox").colorbox({ rel: "nofollow" });
    });
</script>
<div class="container_12">
    <div class="grid_12"><ul class="breadcrumb"><li>You are here: </li><li><a href="/">Home</a></li><li class="fwd-arrow">&rsaquo;</li><li><a href="/new/">New</a></li><li class="fwd-arrow">&rsaquo;</li><li class="current"><strong>Bike Tips And Advices</strong></li></ul><div class="clear"></div></div>    
    <div id="content" class="grid_8 margin-top10">        
        <h1>Tips And Advices</h1>      
		<RP:RepeaterPager id="rpgTipsAdvices" PageSize="10" PagerPageSize="10" runat="server">
			<asp:Repeater ID="rptTipsAdvices" runat="server" EnableViewState="false">
					<itemtemplate>
					<div class="margin-bottom15">	
						<div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>'>
							<div class="anchor-title">
								<a href="/tipsadvices/<%# DataBinder.Eval(Container.DataItem,"Url") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/" rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
									<%# DataBinder.Eval(Container.DataItem,"Title") %>
								</a>
							</div>
                            <div class="grid_5 alpha">
								<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>, <abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:dd-MMM-yyyy}") %></abbr>								
							</div> 
                            <div class="clear"></div>                          
							<div class="margin-top10">
								<%--<%# DataBinder.Eval(Container.DataItem,"ImagePathThumbNail").ToString() != "" ? "<a class='cbBox' href='" + "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval(Container.DataItem,"ImagePathLarge") + "'><img class='alignright size-thumbnail img-border-news' src='" + "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval(Container.DataItem,"ImagePathThumbNail")  +"' align='right' border='0' /></a>" : "" %>--%>
                                <%# DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString() != "" ? "<a class='cbBox' href='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostURL").ToString(),Bikewale.Utility.ImageSize._210x118) + "'><img class='alignright size-thumbnail img-border-news' src='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostURL").ToString(),Bikewale.Utility.ImageSize._144x81)  +"' align='right' border='0' /></a>" : "" %>
								<%# DataBinder.Eval(Container.DataItem,"Description") %>                                    								    
							</div>
                            <div class="margin-top10 item-footer"><a href="/tipsadvices/<%# DataBinder.Eval(Container.DataItem,"Url") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/">Read full article &raquo;</a></div>
						</div>                            
					</div>
                    <div class="sept-dashed"></div> 
				</itemtemplate>
			</asp:Repeater>	
		</RP:RepeaterPager>
    </div>                  
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20">
            <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
            <LD:LocateDealer runat="server" id="LocateDealer" />
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>  
</div>
<script language="javascript">
    $("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
</script>
<!-- #include file="/includes/footerInner.aspx" -->