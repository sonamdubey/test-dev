<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.BikeCare" %>
<h2 class="section-heading">Bike Care - Maintenance tips</h2>
            <div class="container bg-white box-shadow card-bottom-margin content-inner-block-20">
               <%foreach(var article in objArticleList) {%>
                 <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Convert.ToString(Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices)) %>" title="<%=article.Title%>">
                            <img class="lazy"  alt="<%=article.Title%>" src="<%=Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._144x81) %>">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Convert.ToString(Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices)) %>" title="<%=article.Title%>" class="target-link"><%=Bikewale.Utility.FormatDescription.TruncateDescription( article.Title, 44)%></a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content"><%=Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(article.DisplayDate), "dd MMMM yyyy") %></span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content"><%=article.AuthorName %></span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        <%=Bikewale.Utility.FormatDescription.TruncateDescription(article.Description,180)%>
                    </p>
                </div>
                <%} %>
                <a href="/m/bike-care/" title="Bike Maintenace Tips" class="font14">Read all bike maintenance tips<span class="bwmsprite blue-right-arrow-icon"></span></a>
            </div>