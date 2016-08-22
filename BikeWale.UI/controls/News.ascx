<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.News_Widget" %>
<div id="ctrlNews">
    <div id="modelNewsContent" class="bw-model-tabs-data margin-right10 margin-left10 content-inner-block-2010 border-solid-bottom font14">
        <h2><%=WidgetTitle %> News</h2>
        <!-- when one news -->
        <%--<% if (FetchedRecordsCount == 1)
           { %>
        <div class="grid-12 alpha omega model-single-news margin-bottom20">
            <div class="model-preview-image-container leftfloat">
                <a href="<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(firstPost.BasicId.ToString(),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>">
                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages( firstPost.OriginalImgUrl, firstPost.HostUrl ,Bikewale.Utility.ImageSize._310x174) %>" title="<%=firstPost.Title %>" alt="<%=firstPost.Title %>" src="">
                </a>
            </div>
            <div class="model-news-title-container leftfloat">
                <a href="<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(firstPost.BasicId.ToString(),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>" class="article-target-link line-height"><%=firstPost.Title %></a>
                <p class="text-light-grey margin-bottom15"><%= Bikewale.Utility.FormatDate.GetFormatDate(firstPost.DisplayDate.ToString(), "MMMM dd, yyyy") %>, by <span class="text-light-grey"><%=firstPost.AuthorName %></span></p>
                <p class="margin-top20 line-height17">
                    <%= Bikewale.Utility.FormatDescription.TruncateDescription(firstPost.Description,300) %>
                </p>
            </div>
            <div class="clear"></div>
        </div>
        <% }
           else
           { %>
        <div class="margin-bottom10">
            <div class="grid-8 alpha border-light-right">
                <div class="padding-bottom5">
                    <div class="model-preview-image-container leftfloat margin-right20">
                        <a href="<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(firstPost.BasicId.ToString(),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>">
                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages( firstPost.OriginalImgUrl, firstPost.HostUrl ,Bikewale.Utility.ImageSize._310x174) %>" title="<%=firstPost.Title %>" alt="<%=firstPost.Title %>" src="">
                        </a>
                    </div>
                    <div class="model-news-title-container model-news-text-wrapper">
                        <a href="<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(firstPost.BasicId.ToString(),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>" class="article-target-link line-height"><%=firstPost.Title %></a>
                        <p class="text-light-grey margin-bottom25"><%= firstPost.DisplayDate.ToString("MMMM dd, yyyy") %>, by <span class="text-light-grey"><%=firstPost.AuthorName%></span></p>
                        <p class="line-height17">
                            <%= Bikewale.Utility.FormatDescription.TruncateDescription(firstPost.Description,190) %>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="grid-4 omega">
                <ul>
                    <asp:Repeater ID="rptNews" runat="server">
                        <ItemTemplate>
                            <li>
                                <p class="red-bullet-point">
                                    <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>" class="article-target-link font14"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                                </p>
                                <p class="text-light-grey margin-left15"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMMM dd, yyyy") %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div class="clear"></div>
        </div>--%>

        <!-- main content -->
               <asp:Repeater ID="rptNews" runat="server">
                        <ItemTemplate>
        <div class="margin-bottom20">
            <div class="grid-4 alpha">
                <div class="model-preview-image-container">
                    <a href="<%#Bikewale.Utility.UrlFormatter.GetArticleUrl(DataBinder.Eval(Container.DataItem,"BasicId").ToString(),DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString(),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>">
                        <img class="lazy" data-original="<%#Bikewale.Utility.Image.GetPathToShowImages( DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem,"HostUrl").ToString() ,Bikewale.Utility.ImageSize._310x174) %>" title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" alt="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" src="">
                </a>
                </div>
            </div>
            <div class="grid-8">
                <a href="<%#Bikewale.Utility.UrlFormatter.GetArticleUrl(DataBinder.Eval(Container.DataItem,"BasicId").ToString(),DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString(),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString())  %>" class="article-target-link line-height">
                    <%#DataBinder.Eval(Container.DataItem,"Title").ToString()%>
                    </a>
                <div class="article-stats-left-grid">
                    <span class="bwsprite calender-grey-sm-icon"></span>
                    <span class="article-stats-content"><%#Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMM dd, yyyy") %></span>
                </div>
                <div class="article-stats-right-grid">
                    <span class="bwsprite author-grey-sm-icon"></span>
                    <span class="article-stats-content"><%#DataBinder.Eval(Container.DataItem,"AuthorName").ToString()%></span>
                </div>
                <p class="line-height17 margin-top10">
                    <%#Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem,"Description").ToString() ,280) %>
                </p>
            </div>
            <div class="clear"></div>
        </div>
        
                             </ItemTemplate>
                    </asp:Repeater>
        <!-- main content -->
       
        <div>
            <a href="/news/">Read all news<span class="bwsprite blue-right-arrow-icon"></span></a>
        </div>
    </div>
</div>
<!-- Ends here-->
