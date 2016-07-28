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
	Ad_320x50 = true;
	Ad_Bot_320x50 = true;
%>
<!-- #include file="/includes/headermobile.aspx" -->
<style type="text/css">
	#divListing .box1 { padding-top:20px; }
	.sponsored-tag-wrapper { width: 92px;height: 24px;background: #4d5057; color: #fff; font-size: 12px; line-height: 25px; padding: 0 9px; top:-8px; left:-10px; }
	.sponsored-left-tag {width: 0;height: 0;border-top: 13px solid transparent;border-bottom: 15px solid transparent;border-right: 10px solid #fff;position: relative;top: -6px;left: 12px;font-size: 0;line-height: 0;z-index: 1; }
	.article-wrapper { display:table; margin-bottom:10px; }
	.article-image-wrapper { width:120px; }
	.article-image-wrapper, .article-desc-wrapper { display:table-cell; vertical-align:top; }
	.article-desc-wrapper { position:relative;top:-4px; }
	.article-stats-wrapper { min-width:115px; padding-right:10px; }
	.calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-1px; margin-right:6px; }
	.calender-grey-icon { background-position:-40px -460px; }
	.author-grey-icon { background-position:-64px -460px; }
</style>
<div class="padding5">
		<div id="br-cr">
			<span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
			<a href="/m/" class="normal" itemprop="url"><span itemprop="title">Home</span></a> </span>
			&rsaquo; <span class="lightgray">Features</span></div>
		<h1>Latest Bike Features</h1>
		<div id="divListing">  
			<asp:Repeater id="rptFeatures" runat="server">
				<ItemTemplate>
					<a class="normal" href='/m/features/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>/' >
						<div class='box1 new-line15 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "sponsored-content" : ""%>'>
						   <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
							<div class="article-wrapper">
								<div class="article-image-wrapper">
									<img alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0">
								</div>
								<div class="padding-left10 article-desc-wrapper">
									<h2 class="font14 text-bold text-black">
										<%# DataBinder.Eval(Container.DataItem,"Title") %>
									</h2>
								</div>
							</div>
							<div class="article-stats-wrapper font12 leftfloat text-light-grey">
								<span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%# Bikewale.Utility.FormatDate.GetFormatDate(Eval("DisplayDate").ToString(),"MMMM dd, yyyy") %></span>
							</div>
							<div class="article-stats-wrapper font12 leftfloat text-light-grey">
								<span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%# DataBinder.Eval(Container.DataItem,"AuthorName") %></span>
							</div>
							<div class="clear"></div>
						</div>
					</a>
				</ItemTemplate>
			</asp:Repeater>
		</div>
		<Pager:Pager id="listPager" runat="server"></Pager:Pager>
</div>
<!-- #include file="/includes/footermobile.aspx" -->
