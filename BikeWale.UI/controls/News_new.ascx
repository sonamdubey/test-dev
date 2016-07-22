<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.News_new" %>

<div class="bw-tabs-data news-expert-video-content" id="ctrlNews"><!-- News data code starts here-->    
    <asp:Repeater ID="rptNews" runat="server">
        <HeaderTemplate>
            <!-- #include file="/ads/Ad976x204.aspx" -->
        </HeaderTemplate>
        <ItemTemplate>
            <div class="padding-bottom30">
                <div class="grid-4 alpha">
                    <div class="img-preview">
                        <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html"><img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src></a>
                    </div>
                </div>
                <div class="grid-8 omega padding-top5 font14">
                    <a href="/news/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() + "-" + DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() %>.html" class="article-target-link margin-bottom10"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                    <p class="margin-bottom10 text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMMM dd, yyyy") %>, by <span><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
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