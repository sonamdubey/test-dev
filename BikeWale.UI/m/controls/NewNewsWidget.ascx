<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewNewsWidget" %>

 <div id="makeNewsContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14">
    <h2><%= WidgetTitle %> News</h2>

       <!-- when one news -->
    <% if (FetchedRecordsCount == 1)
        { %>

    <div class="margin-bottom15">
        <div class="review-image-wrapper">
            <a href="/m/<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(firstPost.BasicId),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>">
                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages( firstPost.OriginalImgUrl, firstPost.HostUrl ,Bikewale.Utility.ImageSize._370x208) %>" title="<%=firstPost.Title %>" alt="<%=firstPost.Title %>"  />
            </a>
        </div>
        <div class="review-heading-wrapper">
            <a href="/m/<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(firstPost.BasicId),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>" class="target-link">
                <%=firstPost.Title %>
            </a>
            <p class="font10 text-truncate text-light-grey"><%= Bikewale.Utility.FormatDate.GetFormatDate(firstPost.DisplayDate.ToString(), "MMMM dd, yyyy") %>, by <%=firstPost.AuthorName %></p>
        </div>
    </div>
      <% }
        else
        { %>
     
    <div class="margin-bottom20">
        <div class="review-image-wrapper">
            <a href="/m<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(firstPost.BasicId),firstPost.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>">
                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages( firstPost.OriginalImgUrl, firstPost.HostUrl ,Bikewale.Utility.ImageSize._370x208) %>" title="<%=firstPost.Title %>" alt="<%=firstPost.Title %>"  />
            </a>
        </div>
        <div class="review-heading-wrapper">
            <a href="/m/news/<%= String.Format("{0}-{1}.html", firstPost.BasicId,firstPost.ArticleUrl) %>" class="target-link"><%= Bikewale.Utility.FormatDescription.TruncateDescription(firstPost.Title.ToString(),44) %></a>
            <div class="grid-7 alpha padding-right5">
                <span class="bwmsprite calender-grey-sm-icon"></span>
                <span class="article-stats-content"><%= Bikewale.Utility.FormatDate.GetFormatDate(firstPost.DisplayDate.ToString(), "MMM dd, yyyy") %></span>
            </div>
            <div class="grid-5 alpha omega">
                <span class="bwmsprite author-grey-sm-icon"></span>
                <span class="article-stats-content"><%=firstPost.AuthorName %></span>
            </div>
            <div class="clear"></div>
        </div>
        <p class="margin-top10">
            <!-- desc -->
        </p>
    </div>

    <ul id="makeNewsList">
      <asp:Repeater ID="rptNews" runat="server">
          <ItemTemplate>
            <li>
                <p class="margin-bottom5 red-bullet-point"><a href="/m<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>"
                     class="target-link"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a></p>
                <p class="margin-left15 font12 text-truncate text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMMM dd, yyyy") %>, by <%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></p>
            </li>
          </ItemTemplate>
      </asp:Repeater>
    </ul>
    <% } %>
    <div>
        <a href="/m/news/" class="font14">Read all news<span class="bwmsprite blue-right-arrow-icon"></span></a>
    </div>
</div>

<div class="bw-tabs-data" id="ctrlNews">  
    <script type="text/javascript">
        $(document).ready(function () { $("img.lazy").lazyload(); });
    </script>
</div>
