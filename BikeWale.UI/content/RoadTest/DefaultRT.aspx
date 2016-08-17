<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultRT" Trace="false" Async="true" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="Mms" TagName="MakeModelSearch" Src="/Controls/MakeModelSearch.ascx" %>
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
    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250Shown = false;
%>
<!-- #include file="/includes/headNew.aspx" -->
<style type="text/css">
	#content { margin:0; }
	#content h1 { margin-left:10px; margin-right:10px; }
	.sponsored-tag-wrapper { width: 120px;height: 24px;background: #4d5057; color: #fff; font-size: 12px; line-height: 25px; padding: 0 20px; top:-10px; left:-20px; }
	.sponsored-left-tag {width: 0;height: 0;border-top: 13px solid transparent;border-bottom: 15px solid transparent;border-right: 10px solid #fff;position: relative;top: -6px;left: 30px;font-size: 0;line-height: 0;z-index: 1; }
	.sept-dashed { margin:10px 0 15px; }
	.top-breadcrumb { padding-top:20px; margin-right:15px; margin-bottom:10px; margin-left:15px; }
	.article-image-wrapper { width: 206px; margin-right: 20px; float: left; }
	.article-desc-wrapper { width: auto; }
	.article-content { margin-left:10px; padding-top:20px; padding-bottom:20px; border-top:1px solid #e2e2e2; }
	#content > div.article-content:first-of-type { padding-top:0; border-top:0; }
	.article-image-wrapper a { width:100%; height:116px; display:block; }
	.article-image-wrapper img { height: 116px; }
	.margin-bottom8 { margin-bottom:8px; }
	.calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-2px; margin-right:4px; }
	.calender-grey-icon { background-position:-129px -515px; }
	.author-grey-icon { background-position:-105px -515px; }
	#colorbox { width: 400px !important;height: 400px !important;}
	.article-date { min-width: 164px; padding-right: 10px; }
	.article-author { min-width: 220px; }
	.article-date , .article-author { display: inline-block; vertical-align: middle; }
</style>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
<div class="container margin-bottom30">
	<div class="grid-12">
		<div class="content-box-shadow padding-bottom20">
			<ul class="breadcrumb top-breadcrumb">
				<li>You are here: </li>
				<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url"><span itemprop="url">Home</span></a></li>
				<li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/<%=makeMaskingName %>-bikes/" itemprop="url"><span itemprop="title"><%=makeName%> Bikes</span></a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                  <% if (!string.IsNullOrEmpty(modelName)) 
		           {%>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/<%=modelMaskingName%>-bikes/" itemprop="url"><span itemprop="title"><%=makeName%> <%=modelName%> Bikes</span></a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                 <% } %>
                <li class="current"><strong> Expert Reviews</strong></li>
			</ul>
			<div class="clear"></div>
	
			<div id="content" class="grid-8">
                <% if (!string.IsNullOrEmpty(modelName)) 
		           {%>
		        <h1 class="black-text margin-bottom15"><%= makeName  %> <%= modelName %> Expert Reviews</h1>
		        <% }
		           else if(!string.IsNullOrEmpty(makeName)) { %>
		        <h1 class="black-text margin-bottom15"><%= makeName  %> Bikes Expert Reviews</h1>
		        <% } else {
		         %>
		        <h1 class="black-text margin-bottom15">Expert Reviews</h1>
		        <% } %>

				<Mms:MakeModelSearch ID="MakeModelSearch" RequestType="RoadTest" runat="server" Visible="false"></Mms:MakeModelSearch>
				<div class="alert moz-round" id="alertObj" runat="server" visible="false"></div>
				<asp:repeater id="rptRoadTest" runat="server" enableviewstate="false">
					<Itemtemplate>					
						<div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>' class="<%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "sponsored-content" : "post-content" %> article-content">
							<%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
							<div class="margin-bottom10">
								<div class="article-image-wrapper">
									<%# string.Format("<a href='/expert-reviews/{0}-{1}.html'><img src='{2}' alt='{3}' title='{3}' width='100%' border='0' /></a>", DataBinder.Eval(Container.DataItem,"ArticleUrl"),DataBinder.Eval(Container.DataItem,"BasicId"),Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._210x118),DataBinder.Eval(Container.DataItem,"Title")) %>
								</div>
								<div class="article-desc-wrapper">
									<h2 class="font14 margin-bottom8">
										<a href='/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html' rel="bookmark" class="text-black text-bold">
											<%# DataBinder.Eval(Container.DataItem,"Title").ToString() %>
										</a>
									</h2>
									<div class="font12 text-light-grey margin-bottom25">
										<div class="article-date">
											<span class="bwsprite calender-grey-icon inline-block"></span>
											<span class="inline-block">
												<%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %>
											</span>
										</div>
										<div class="article-author">
											<span class="bwsprite author-grey-icon inline-block"></span>
											<span class="inline-block">
												<%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
											</span>
										</div>
									</div>
									<div class="font14"><%# DataBinder.Eval(Container.DataItem,"Description") %><a href="/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>.html">Read full review</a></div>
								</div>
								<div class="clear"></div>
							</div>
						</div>
					</Itemtemplate>
				</asp:repeater>
				<BikeWale:RepeaterPager ID="linkPager" runat="server" />
			</div>
			<div class="grid-4">
		<!--    Right Container starts here -->
		<%--<div class="margin-top15">
			<!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
			<!-- #include file="/ads/Ad300x250.aspx" -->
		</div>--%>
		<div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
			<uc:InstantBikePrice runat="server" ID="ucInstantBikePrice" />
		</div>
		<div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top10">
			<CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
			<div class="clear"></div>
		</div>
		<div>
			<!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
			<!-- #include file="/ads/Ad300x250BTF.aspx" -->
		</div>        
	</div>
			<div class="clear"></div>
		</div>
	</div>
	<div class="clear"></div>
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
<!-- #include file="/includes/footerInner.aspx" -->

