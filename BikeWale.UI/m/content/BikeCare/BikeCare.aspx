<%@ Page Language="C#" AutoEventWireup="false" Async="true" Trace="false" Inherits="Bikewale.Mobile.Content.BikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="/m/controls/LinkPagerControl.ascx" %>
<!-- #include file="/includes/headermobile.aspx" -->
<%
    title = "Bike Care | Maintenance Tips from Bike Experts - BikeWale";
    description = "BikeWale brings you maintenance tips from the bike experts to help you keep your bike in good shape. Read through these maintenance tips to learn more about your bike maintenance";
    keywords = "Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care";
    
    
    
%>
<style type="text/css">
	#divListing .box1 { padding-top:20px; }
	.sponsored-tag-wrapper { width: 92px;height: 24px;background: #4d5057; color: #fff; font-size: 12px; line-height: 25px; padding: 0 9px; top:-8px; left:-10px; }
	.sponsored-left-tag {width: 0;height: 0;border-top: 13px solid transparent;border-bottom: 15px solid transparent;border-right: 10px solid #fff;position: relative;top: -6px;left: 12px;font-size: 0;line-height: 0;z-index: 1; }
	.article-wrapper { display:table; margin-bottom:10px; }
	.article-image-wrapper { width:120px; }
	.article-image-wrapper, .article-desc-wrapper { display:table-cell; vertical-align:top; }
	.article-stats-wrapper { min-width:115px; padding-right:10px; }
	.calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-1px; margin-right:6px; }
	.calender-grey-icon { background-position:-40px -460px; }
	.author-grey-icon { background-position:-64px -460px; }
</style>
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
 <h1>Bike Care</h1>
<h2>BikeWale brings you maintenance tips from experts to rescue you from common problems</h2>
	<div id="divListing">
         <%foreach(var article in objArticleList) {%>
				 <a class="normal" href="/m/<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices.ToString()) %>" title="<%=article.Title%>">
					<div class="box1 new-line15" >
						<%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
						<div class="article-wrapper">
							<div class="article-image-wrapper">
                                <img alt='Bike Care:<%=article.Title%>' title="Expert reviews: <%=article.Title%>" src="<%=Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(article.OriginalImgUrl),Convert.ToString(article.HostUrl),Bikewale.Utility.ImageSize._110x61) %>" width="100%" border="0">
							</div>
							<div class="padding-left10 article-desc-wrapper">
								<h2 class="font14 text-bold text-black">
                                    Bike Care: <%=article.Title%>
								</h2>
							</div>
						</div>
						<div class="article-stats-wrapper font12 leftfloat text-light-grey">
							<span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%=Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(article.DisplayDate), "MMM dd, yyyy") %></span>
						</div>
						<div class="article-stats-wrapper font12 leftfloat text-light-grey">
							<span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%=article.AuthorName %></span>
						</div>
						<div class="clear"></div>
					</div>
				</a>
        <%} %>     
	 <BikeWale:Pager ID="ctrlPager" runat="server" />
    <!-- #include file="/includes/footermobile.aspx" -->
