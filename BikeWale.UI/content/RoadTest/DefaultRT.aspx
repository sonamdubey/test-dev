<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultRT" Trace="false" Async="true" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%
    // Listing page
    if (string.IsNullOrEmpty(modelName) && string.IsNullOrEmpty(makeName))
    {
        title = "Expert Bike Reviews India - Bike Comparison & Road Tests - BikeWale";
        description = "Latest expert reviews on upcoming and new bikes in India. Read bike comparison tests and road tests exclusively on BikeWale";
        keywords = "Expert bike reviews, bike road tests, bike comparison tests, bike reviews, road tests, expert reviews, bike comparison, comparison tests";
        canonical = "http://www.bikewale.com" + "/expert-reviews/";
        alternate = "http://www.bikewale.com" + "/m/expert-reviews/";
    }
    // Model Name exists
    else if (!string.IsNullOrEmpty(modelName))
    {
        title = string.Format("{0} {1} Expert Reviews India - Bike Comparison & Road Tests - BikeWale",makeName, modelName);
        description = string.Format("Latest expert reviews on {0} {1} in India. Read {0} {1} comparison tests and road tests exclusively on BikeWale", makeName, modelName);
        keywords = string.Format("{0} {1} expert reviews, {0} {1} road tests, {0} {1} comparison tests, {0} {1} reviews, {0}{1} bike comparison", makeName, modelName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/expert-reviews/", makeMaskingName, modelMaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/{1}/expert-reviews/", makeMaskingName, modelMaskingName);
    }
    // Make name exists
    else
    {
        title = string.Format("{0} Bikes Expert Reviews India - Bike Comparison & Road Tests - BikeWale", makeName);
        description = string.Format("Latest expert reviews on upcoming and new {0} bikes in India. Read {0} bike comparison tests and road tests exclusively on BikeWale", makeName);
        keywords = string.Format("{0} bike expert reviews, {0} bike road tests, {0} bike comparison tests, {0} bike reviews, {0} road tests, {0} expert reviews, {0} bike comparison, {0} comparison tests.",makeName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/expert-reviews/", makeMaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/expert-reviews/", makeMaskingName);
    }
    fbTitle = title;
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_Reviews_";
    prevPageUrl = prevUrl;
    nextPageUrl = nextUrl;
    fbImage = Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo;
%>
<!-- #include file="/includes/headNew.aspx" -->
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
<div class="container_12 margin-bottom20 padding-bottom20">
	<div class="grid_12">
		<ul class="breadcrumb">
			<li>You are here: </li>
			<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
				<a href="/" itemprop="url"><span itemprop="title">Home</span></a>
			</li>
			<li class="fwd-arrow">&rsaquo;</li>
			<li class="current"><strong>Expert Reviews</strong></li>
		</ul>
		<div class="clear"></div>
	</div>
	<div id="content" class="grid_8 margin-top10">
        <% if (!string.IsNullOrEmpty(modelName)) 
           {%>
        <h1><%= makeName  %> <%= modelName %> Expert Reviews</h1>
        <% }
           else if(!string.IsNullOrEmpty(makeName)) { %>
		<h1><%= makeName  %> Bikes Expert Reviews</h1>
        <% } else {
         %>
        <h1>Expert Reviews</h1>
        <% } %>
		<div class="clear"></div>
		<div class="alert moz-round" id="alertObj" runat="server" visible="false"></div>
		<asp:repeater id="rptRoadTest" runat="server" enableviewstate="false">
				<Itemtemplate>					
					<div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="margin-bottom15">
						<div class="anchor-title">
							<a href='/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html' rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
								<%#  DataBinder.Eval(Container.DataItem,"Title").ToString()%>
							</a>
						</div>	
						<div class="grid_5 alpha">
							<abbr><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %></abbr> by 
							<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
						</div><div class="clear"></div>
						<div class="margin-top10">
							<div class="grid_5 alpha"><%# DataBinder.Eval(Container.DataItem,"Description") %></div>
							<div class="grid_3 omega"><%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString()) ? "<a class='cbBox cboxElement' href='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._210x118) + "'><img class='alignright size-thumbnail border-light' src='" + Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._144x81) +"' align='right' border='0' style='padding:2px;' /></a>" : "" %></div>
						</div><div class="clear"></div>
						<div class="margin-top10 item-footer">
							<div class="grid_5 alpha">
								<a href="/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html">Read full article &raquo;</a>
							</div>                                
							<div class="clear"></div>
						</div>                           						
					</div>
					<div class="sept-dashed"></div>
			</Itemtemplate>
		</asp:repeater>
		<BikeWale:RepeaterPager ID="linkPager" runat="server" />
	</div>
	<div class="grid_4">
		<!--    Right Container starts here -->
		<%--<div class="margin-top15">
			<!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
			<!-- #include file="/ads/Ad300x250.aspx" -->
		</div>--%>
		<div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
			<uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
		</div>
		<div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
			<CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
			<div class="clear"></div>
		</div>
		<div>
			<!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
			<!-- #include file="/ads/Ad300x250BTF.aspx" -->
		</div>        
	</div>
</div>
<%--<script type="text/javascript" language="javascript">
	$("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
</script>--%>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
<script type="text/javascript">
	$(document).ready(function () {

		$(".cboxElement").colorbox({
			rel: 'cboxElement'
		});
	});
</script>
<style type="text/css">
	#colorbox {
		width: 400px !important;
		height: 400px !important;
	}
</style>
<!-- #include file="/includes/footerInner.aspx" -->
