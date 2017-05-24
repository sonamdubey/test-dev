<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.VideosControl" %>
<div class="bw-tabs-data news-expert-video-content" id="ctrlVideos"><!-- Videos data code starts here-->
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate>
            <div class="padding-bottom20">
                <div class="grid-4">
                    <div class="img-preview rounded-corner2">
                        <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                            <img class="lazy" data-original="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                                alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                        </a>
                    </div>
                </div>
                <div class="grid-8 padding-top5 font14 text-light-grey">
                    <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="article-target-link margin-bottom10"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
                    <p class="margin-bottom15">Updated on <%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"dd MMMM yyyy") %></p>
                    <div class="grid-3 alpha omega border-solid-right font14">
                        <span class="bwsprite video-views-icon margin-right5"></span>
                        <span class="text-light-grey margin-right5">Views: <%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                    </div>
                    <div class="grid-3 omega padding-left20 font14">
                        <span class="bwsprite video-likes-icon margin-right5"></span>
                        <span class="text-light-grey margin-right5">Likes: <%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>                        
    <div id="divViewMoreVideo" class="padding-bottom30 text-center">
        <a href="<%=MoreVideoUrl%>" class="font16">View more videos</a>
    </div>
</div><!-- Ends here-->