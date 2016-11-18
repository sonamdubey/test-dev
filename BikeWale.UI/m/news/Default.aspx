﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.News.Default"  Trace="false" Async="true"%>
<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/m/controls/LinkPagerControl.ascx" %>
<% 
	title       = "Bike News - Latest Indian Bike News & Views - BikeWale";
	description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
	keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
	canonical   = "http://www.bikewale.com/news/";
	relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
	relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
	AdPath = "/1017752/Bikewale_Mobile_NewBikes";
	AdId = "1398766302464";
	Ad_320x50 = true;
	Ad_Bot_320x50 = true;
	menu = "6";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<style type="text/css">
	#divListing .box1 { padding-top:20px; }
	.sponsored-tag-wrapper { width: 92px;height: 24px;background: #4d5057; color: #fff; font-size: 12px; line-height: 25px; padding: 0 9px; top:-8px; left:-10px; }
	.sponsored-left-tag {width: 0;height: 0;border-top: 13px solid transparent;border-bottom: 15px solid transparent;border-right: 10px solid #fff;position: relative;top: -6px;left: 12px;font-size: 0;line-height: 0;z-index: 1; }
	.article-wrapper { display:table; margin-bottom:10px; }
	.article-image-wrapper { width:120px; }
	.article-image-wrapper, .article-desc-wrapper { display:table-cell; vertical-align:top; }
	.article-desc-wrapper { position:relative;top:-5px; }
	.article-category { color:#c20000; }
	.article-stats-wrapper { min-width:115px; padding-right:10px; }
	.calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-1px; margin-right:6px; }
	.calender-grey-icon { background-position:-40px -460px; }
	.author-grey-icon { background-position:-64px -460px; }
    #pagination-list{margin-right:20px;margin-left:20px;overflow:hidden}#pagination-list li{float:left;margin-right:2px;margin-left:2px}#pagination-list .active,#pagination-list a{color:#82888b;font-size:12px;padding-right:5px;padding-left:5px;border:1px solid #f3f3f3;display:block;min-width:25px;text-align:center}#pagination-list a:hover{text-decoration:none}#pagination-list li.active{color:#4d5057;font-weight:700;border:1px solid #a2a2a2;border-radius:1px}.pagination-control-next,.pagination-control-prev{position:absolute;top:0;height:20px}.pagination-control-prev{left:0}.pagination-control-next{right:5px}.next-page-icon,.prev-page-icon{width:18px;height:18px}.prev-page-icon{background-position:-164px -391px}.next-page-icon{background-position:-178px -391px}.pagination-control-next.inactive .next-page-icon,.pagination-control-prev.inactive .prev-page-icon{opacity:.2;pointer-events:none}
</style>

	<div class="padding5">
		<div id="br-cr"><span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/m/" itemprop="url" class="normal"><span itemprop="title">Home</span></a></span> &rsaquo; <span class="lightgray">News</span></div>
		<h1>Latest Bike News</h1>
		<div id="divListing">
			<asp:Repeater id="rptNews" runat="server">
				<ItemTemplate>
					<a class="normal" href='<%# string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CategoryId")))) %>' >
						  <div class='box1 new-line15 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")).ToLower().Contains("sponsored") ? "sponsored-content" : ""%>'>
						   <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
							<div class="article-wrapper">
								<div class="article-image-wrapper">
									<img alt='<%# DataBinder.Eval(Container.DataItem,"Title") %>' title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0">
								</div>
								<div class="padding-left10 article-desc-wrapper">
									<div class="article-category">
										<span class="text-uppercase font12 text-bold"><%# GetContentCategory(DataBinder.Eval(Container.DataItem,"CategoryId").ToString()) %></span>
									</div>
									<h2 class="font14 text-bold text-black">
										<%# DataBinder.Eval(Container.DataItem,"Title") %>
									</h2>
								</div>
							</div>
							<div class="article-stats-wrapper font12 leftfloat text-light-grey">
								<span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %></span>
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
        <div class="margin-top10">
             <div class="grid-5 omega text-light-grey">
                 <div class="font13"><span class="text-bold"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold"><%=totalrecords %></span> news articles</div>
             </div> 
	         <BikeWale:newPager ID="ctrlPager" runat="server" />
            <div class="clear"></div>
        </div>
</div>
<!-- #include file="/includes/footermobile.aspx" -->
<script type="text/javascript">
	ga_pg_id = "10";
</script>