<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewVideosWidget" %>
<h3 class="margin-top20 model-section-subtitle">Videos</h3>
<div class="model-expert-review-container">
                        
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate> 
        <div class="margin-bottom20">
            <div class="review-image-wrapper">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                    <img class="swiper-lazy" data-src="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                        alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                </a>
            </div>
            <div class="review-heading-wrapper">
                <h4>
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" 
                        class="font14 text-black"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
                </h4>
                <p class="font12 text-truncate text-light-grey">Uploaded on <%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"DisplayDate")).ToString("MMMM dd, yyyy")%></p> 
            </div>
        </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div>
    <a href="<%=MoreVideoUrl%>" class="bw-ga" c="Model_Page" a="View_all_videos_link_cliked" v="myBikeName">View all videos<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
