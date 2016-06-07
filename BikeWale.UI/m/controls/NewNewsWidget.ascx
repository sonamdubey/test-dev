<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewNewsWidget" %>

 <div id="makeNewsContent" class="bw-model-tabs-data padding-right20 padding-left20">
    <h2><%= WidgetTitle %> News</h2>

       <!-- when one news -->
        <% if (FetchedRecordsCount == 1)
           { %>

    <div class="margin-bottom15">
        <div class="news-image-wrapper">
            <a href="<%=firstPost.ArticleUrl %>">
                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages( firstPost.OriginalImgUrl, firstPost.HostUrl ,Bikewale.Utility.ImageSize._370x208) %>" title="<%=firstPost.Title %>" alt="<%=firstPost.Title %>"  />
            </a>
        </div>
        <div class="news-heading-wrapper">
            <h4>
                <a href="/news/<%=firstPost.ArticleUrl %>" class="font12 text-black">
                    <%=firstPost.Title %>
                </a>
            </h4>
            <p class="font10 text-truncate text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(firstPost.DisplayDate.ToString(), "MMMM dd, yyyy") %>, by <%=firstPost.AuthorName %></p>
        </div>
    </div>
      <% }
           else
           { %>
     
    <div class="margin-bottom15">
        <div class="news-image-wrapper">
            <a href="<%=firstPost.ArticleUrl %>">
                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages( firstPost.OriginalImgUrl, firstPost.HostUrl ,Bikewale.Utility.ImageSize._370x208) %>" title="<%=firstPost.Title %>" alt="<%=firstPost.Title %>"  />
            </a>
        </div>
        <div class="news-heading-wrapper">
            <h4>
                <a href="/news/<%=firstPost.ArticleUrl %>" class="font12 text-black">
                    <%=firstPost.Title %>
                </a>
            </h4>
            <p class="font10 text-truncate text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(firstPost.DisplayDate.ToString(), "MMMM dd, yyyy") %>, by <%=firstPost.AuthorName %></p>
        </div>
    </div>

    <ul id="makeNewsList">
      <asp:Repeater ID="rptNews" runat="server">
          <ItemTemplate>
            <li>
                <h4 class="margin-bottom5 red-bullet-point"><a href="/m/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html"
                     class="font12 text-black"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a></h4>
                <p class="margin-left15 font10 text-truncate text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMMM dd, yyyy") %>, by <%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></p>
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
