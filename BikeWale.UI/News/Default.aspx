<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.Default" Trace="false" EnableViewState="false" Async="true" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="RP" TagName="RepeaterPager" Src="/News/RepeaterPagerNews.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/LinkPagerControl.ascx" %>
<%
	title = "Bike News - Latest Indian Bike News & Views | BikeWale";
	description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
	keywords = "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
	canonical = "http://www.bikewale.com/news/";
	prevPageUrl = prevUrl;
	nextPageUrl = nextUrl;
	alternate = "http://www.bikewale.com/m/news/";
	AdId = "1395995626568";
	AdPath = "/1017752/BikeWale_News_";
%>
<!-- #include file="/includes/headnews.aspx" -->
<style type="text/css">
	#content { margin:0; }
	#content h1 { margin-left:10px; margin-right:10px; }
	.sponsored-tag-wrapper { width: 120px;height: 24px;background: #4d5057; color: #fff; font-size: 12px; line-height: 25px; padding: 0 20px; top:-10px; left:-20px; }
	.sponsored-left-tag {width: 0;height: 0;border-top: 13px solid transparent;border-bottom: 15px solid transparent;border-right: 10px solid #fff;position: relative;top: -6px;left: 30px;font-size: 0;line-height: 0;z-index: 1; }
	.sept-dashed { margin:10px 0 15px; }
	.top-breadcrumb { padding-top:20px; margin-right:15px; margin-bottom:10px; margin-left:15px; }
	.article-content { margin-left:10px; padding-top:20px; padding-bottom:20px; border-top:1px solid #e2e2e2; }
	#content > div.article-content:first-of-type { padding-top:0; border-top:0; }
	.article-image-wrapper a { width:100%; height:116px; display:block; }
	.article-category { color:#c20000; margin-top:4px; margin-bottom:6px; }
	.margin-bottom8 { margin-bottom:8px; }
	.calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-2px; margin-right:4px; }
	.calender-grey-icon { background-position:-129px -515px; }
	.author-grey-icon { background-position:-105px -515px; }
</style>
<script type="text/javascript">
	$(document).ready(function () {
		$("a.cbBox").colorbox({ rel: "nofollow" });
	});
</script>
<div class="container margin-bottom30">
	<div class="grid-12">
		<div class="content-box-shadow padding-bottom20">
			<ul class="breadcrumb top-breadcrumb">
				<li>You are here: </li>
				<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url"><span itemprop="title">Home</span></a></li>
				<li class="fwd-arrow">&rsaquo;</li>
				<li class="current"><strong>News</strong></li>
			</ul>
			<div class="clear"></div>
				
			<div id="content" class="grid-8">
				<h1 class="black-text margin-bottom20">Bike News <span>Latest Indian Bikes News and Views</span></h1>
					<asp:repeater id="rptNews" runat="server">
						<itemtemplate>
							<div id='post-<%# Eval("BasicId") %>' class="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "sponsored-content" : "post-content" %> article-content">
								<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
								<div class="margin-bottom10">
									<div class="grid-4 alpha omega article-image-wrapper">
										<%#"<a href='/news/" + Eval("BasicId") + "-" + Eval("ArticleUrl") + ".html'><img src='" + Bikewale.Utility.Image.GetPathToShowImages(Eval("OriginalImgUrl").ToString(), Eval("HostUrl").ToString(),Bikewale.Utility.ImageSize._210x118) + "' alt='"+ Eval("Title") +"' title='"+ Eval("Title") +"' width='100%' border='0' /></a>" %>
									</div>
									<div class="grid-8 padding-left20 omega">
										<div class="article-category">
											<span class="text-uppercase font12 text-bold"><%# GetContentCategory(DataBinder.Eval(Container.DataItem,"CategoryId").ToString()) %></span>
										</div>
										<h2 class="font14 margin-bottom8">
											<a href="/news/<%# Eval("BasicId") %>-<%# Eval("ArticleUrl") %>.html" rel="bookmark" class="text-black text-bold"><%# Eval("Title") %></a>
										</h2>
										<div class="font12 text-light-grey">
											<div class="grid-5 alpha">
												<span class="bwsprite calender-grey-icon inline-block"></span>
												<span class="inline-block">
													<%# Bikewale.Utility.FormatDate.GetFormatDate(Eval("DisplayDate").ToString(),"MMMM dd, yyyy") %>
												</span>
											</div>
											<div class="grid-7 alpha">
												<span class="bwsprite author-grey-icon inline-block"></span>
												<span class="inline-block">
													<%# Eval("AuthorName") %>
												</span>
											</div>
											<div class="clear"></div>
										</div>
									</div>
									<div class="clear"></div>
								</div>
								<div class="font14"><%# Eval("Description") %><a href="/news/<%# Eval("BasicId") %>-<%# Eval("ArticleUrl") %>.html">Read full story</a></div>
								<div class="clear"></div>
							</div>
						</itemtemplate>
					</asp:repeater>
				<BikeWale:RepeaterPager ID="linkPager" runat="server" />
			</div>
			<div class="grid-4">
				<%--<div class="margin-top15">
					<!-- BikeWale_News/BikeWale_News_300x250 -->
					<!-- #include file="/ads/Ad300x250.aspx" -->
				</div>--%>
				<div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
					<BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
				</div>
				<div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top10">
					<CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
				</div>
				<div>
					<%--    <!-- BikeWale_News/BikeWale_News_300x250 -->--%>
					<!-- #include file="/ads/Ad300x250BTF.aspx" -->
				</div>
			</div>
			<div class="clear"></div>
		</div>
	</div>
	<div class="clear"></div>
</div>

<!-- #include file="/includes/footerInner.aspx" -->
