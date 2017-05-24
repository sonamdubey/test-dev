<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.News_new" %>

<div class="bw-tabs-data news-expert-video-content" id="ctrlNews"><!-- News data code starts here-->    
    <asp:Repeater ID="rptNews" runat="server">
        <HeaderTemplate>
            <!-- #include file="/ads/Ad976x204.aspx" -->
        </HeaderTemplate>
        <ItemTemplate>
            <div class="padding-bottom20">
                <div class="grid-4">
                    <div class="img-preview">
                        <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>"><img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src></a>
                    </div>
                </div>
                <div class="grid-8 padding-top5 font14">
                    <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>" class="article-target-link margin-bottom10"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                    <p class="margin-bottom10 text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "dd MMMM yyyy") %>, by <span><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                    <p class="line-height"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString()) %></p>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>    
    </asp:Repeater>    
    <div class="padding-bottom30 text-center">
        <a href="/news/" class="font16">View more news</a>
    </div>
</div><!-- Ends here-->