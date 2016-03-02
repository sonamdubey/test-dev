<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.VideosControl" %>
<div class="bw-tabs-data news-expert-video-content" id="ctrlVideos"><!-- Videos data code starts here-->
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate>
            <div class="padding-bottom30">
                <div class="grid-4 alpha">
                    <div class="img-preview rounded-corner2">
                        <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                            <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                                alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                        </a>
                    </div>
                </div>
                <div class="grid-8 omega">
                    <h2 class="margin-bottom10 font20"><a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="text-black"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a></h2>
                    <p class="margin-bottom10 text-light-grey font14">Updated on <span><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"MMMM dd, yyyy") %></span></p>
                    <div class="margin-bottom15 text-light-grey"><span class="bwsprite video-views-icon"></span> Views <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span></div>
                    <div class="text-light-grey"><span class="bwsprite video-likes-icon text-light-grey margin-right5"></span> Likes <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span></div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>                        
    <div id="divViewMoreVideo" class="padding-bottom30 text-center">
        <a href="<%=MoreVideoUrl%>" class="font16">View more videos</a>
    </div>
</div><!-- Ends here-->