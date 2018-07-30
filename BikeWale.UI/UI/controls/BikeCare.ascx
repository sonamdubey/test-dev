<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.BikeCare" %>
<section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Bike Care - Maintenance tips</h2>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <ul class="article-list">
                              <%foreach(var article in objArticleList) {%>
                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Convert.ToString(Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices)) %>" title="<%=article.Title%>">
                                            <img class="lazy" alt="<%=article.Title%>" src="<%=Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._370x208) %>">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Convert.ToString(Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices)) %>" title="<%=article.Title%>" class="article-target-link line-height"><%= article.Title %></a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content"><%=Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(article.DisplayDate), "dd MMMM yyyy") %></span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content"><%=article.AuthorName %></span>
                                    </div>
                                    <p class="font14 line-height17"><%= Bikewale.Utility.FormatDescription.TruncateDescription(article.Description,280)%></p>
                                </div>
                                <div class="clear"></div>
                            </li>
                            <%} %>
                        </ul>
                        <a href="/bike-care/" title="Bike Maintenace Tips" class="font14">Read all bike maintenance tips<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
</section>