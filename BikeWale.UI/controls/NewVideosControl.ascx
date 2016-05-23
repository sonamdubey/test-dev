﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewVideosControl" %>
<h3 class="margin-top25 padding-left20 model-section-subtitle">Videos</h3>
<div class="model-updates-videos-container" id="ctrlVideos">
    <!-- Videos data code starts here-->
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate>
            <div class="margin-bottom20">
                <div class="grid-4 alpha">
                    <div class="model-preview-image-container">
                        <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                            <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" alt="" src="" />
                        </a>
                    </div>
                </div>
                <div class="grid-8 alpha omega">
                    <h3 class="margin-top5">
                        <a href="" class="font16 text-black"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a></h3>
                    <p class="text-light-grey margin-bottom15">Updated on <span><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"MMMM dd, yyyy") %></span></p>
                    <div class="grid-3 alpha omega border-solid-right font14">
                        <span class="bwsprite video-views-icon margin-right5"></span>
                        <span class="text-light-grey margin-right5">Views:</span>
                        <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                    </div>
                    <div class="grid-3 omega padding-left20 font14">
                        <span class="bwsprite video-likes-icon margin-right5"></span>
                        <span class="text-light-grey margin-right5">Likes:</span>
                        <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="padding-left20">
        <a href="<%=MoreVideoUrl%>">View all videos<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
</div>
<!-- Ends here-->

