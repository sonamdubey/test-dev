<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.BikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="/m/controls/LinkPagerControl.ascx" %>
<%
    title = pgTitle;
    description = pgDescription;
    keywords = pgKeywords;
    relPrevPageUrl = pgPrevUrl;
    relNextPageUrl = pgNextUrl;
    canonical = "http://www.bikewale.com/bike-care/";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_Reviews_";
  
%>
<!-- #include file="/includes/headermobile.aspx" -->

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
    #pagination-list{margin-right:20px;margin-left:20px;overflow:hidden}#pagination-list li{float:left;margin-right:2px;margin-left:2px}#pagination-list .active,#pagination-list a{color:#82888b;font-size:12px;padding-right:5px;padding-left:5px;border:1px solid #f3f3f3;display:block;min-width:25px;text-align:center}#pagination-list a:hover{text-decoration:none}#pagination-list li.active{color:#4d5057;font-weight:700;border:1px solid #a2a2a2;border-radius:1px}.pagination-control-next,.pagination-control-prev{position:absolute;top:0;height:20px}.pagination-control-prev{left:0}.pagination-control-next{right:5px}.next-page-icon,.prev-page-icon{width:18px;height:18px}.prev-page-icon{background-position:-164px -391px}.next-page-icon{background-position:-178px -391px}.pagination-control-next.inactive .next-page-icon,.pagination-control-prev.inactive .prev-page-icon{opacity:.2;pointer-events:none}
</style>
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<div class="padding5">
<h1 class="margin-bottom5">Bike Care</h1>
<h2 class="font14 text-unbold">BikeWale brings you maintenance tips from experts to rescue you from common problems</h2>
   <% if(objArticleList!=null){%>
	<div id="divListing">
         <%foreach(var article in objArticleList.Articles) {%>
				 <a class="normal" href="/m<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices.ToString()) %>" title="<%=article.Title%>">
					<div class="box1 new-line15" >
						<div class="article-wrapper">
							<div class="article-image-wrapper">
                                <img alt='Bike Care:<%=article.Title%>' title="Bike Care: <%=article.Title%>" src="<%=Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(article.OriginalImgUrl),Convert.ToString(article.HostUrl),Bikewale.Utility.ImageSize._110x61) %>" width="100%" border="0">
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
        <%} %>
        <div class="margin-top10">
             <div class="grid-5 omega text-light-grey">
                 <div class="font13"><span class="text-bold"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold"><%=totalArticles %></span> articles</div>
             </div> 
	         <BikeWale:Pager ID="ctrlPager" runat="server" />
            <div class="clear"></div>
        </div>
    </div>
    
</div>
    <!-- #include file="/includes/footermobile.aspx" -->
