<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.MaintenanceTips.BikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
 <h1>Bike Care</h1>
<h2>BikeWale brings you maintenance tips from experts to rescue you from common problems</h2>
	<div id="divListing">
         <%foreach(var article in objArticleList) {%>
				 <a class="normal" href="/m/<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices.ToString()) %>" title="<%=article.Title%>">
					<div class="box1 new-line15" >
						<%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
						<div class="article-wrapper">
							<div class="article-image-wrapper">
                                <img alt='Expert Review:<%=article.Title%>' title="Expert reviews: <%=article.Title%>" src="<%=Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(article.OriginalImgUrl),Convert.ToString(article.HostUrl),Bikewale.Utility.ImageSize._110x61) %>" width="100%" border="0">
							</div>
							<div class="padding-left10 article-desc-wrapper">
								<h2 class="font14 text-bold text-black">
                                    Expert Review: <%=article.Title%>
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
	</div>  
	<Pager:Pager ID="listPager" runat="server" />  
