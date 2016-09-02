<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewNewsWidget" %>

 <div id="makeNewsContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14">
    <h2 class="text-bold"><%= WidgetTitle %> News</h2>
    <asp:Repeater ID="rptNews" runat="server">
        <ItemTemplate>
            <div class="margin-bottom20">
                <div class="review-image-wrapper">
                    <a href="/m/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html" >
                        <img class="lazy" data-original="<%#Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem,"HostUrl").ToString() ,Bikewale.Utility.ImageSize._370x208) %>" title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>" alt="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>"  />
                    </a>
                </div>
                <div class="review-heading-wrapper">
                    <a href="/m/news/<%#String.Format("{0}-{1}.html", DataBinder.Eval(Container.DataItem,"BasicId").ToString(),DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString()) %>" class="target-link"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Title").ToString(), 44) %></a>
                    <div class="grid-7 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%#Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(), "MMM dd, yyyy") %></span>
                    </div>
                    <div class="grid-5 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# DataBinder.Eval(Container.DataItem,"AuthorName").ToString()%></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <p class="margin-top10">
                    <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem,"Description").ToString(),180) %>
                </p>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div>
        <a href="/m/news/" class="font14">Read all news<span class="bwmsprite blue-right-arrow-icon"></span></a>
    </div>
</div>

<div class="bw-tabs-data" id="ctrlNews">  
    <script type="text/javascript">
        $(document).ready(function () { $("img.lazy").lazyload(); });
    </script>
</div>
