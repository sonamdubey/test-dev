<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Videos.ascx.cs" Inherits="Bikewale.Controls.Videos" %>



<section>
    <div id="videoJumbotron" class="container">
        <div class="grid-12">
            <div class="content-box-shadow">
                <div class="grid-8">
                    <a href="<%= Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(FirstVideoRecord.VideoTitleUrl,FirstVideoRecord.BasicId.ToString()) %>" class="main-video-container">
                        <img class="lazy" data-original="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",FirstVideoRecord.VideoId)  %>" alt="<%= FirstVideoRecord.VideoTitle  %>" title="<%= FirstVideoRecord.VideoTitle  %>" src="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",FirstVideoRecord.VideoId)  %>" border="0" />
                        <span><%= FirstVideoRecord.VideoTitle  %></span>
                    </a>
                </div>
                <div class="grid-4">
                    <ul> 
                        <asp:Repeater ID="rptLandingVideos" runat="server">
                            <ItemTemplate>

                                <li>
                                    <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="sidebar-video-image">
                                        <img class="lazy" data-original="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" border="0" /></a>
                                    <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" class="sidebar-video-title font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString().Substring(0, 25) + "..." %></a>
                                </li>

                            </ItemTemplate>
                        </asp:Repeater> 
                    </ul>
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
</section>


