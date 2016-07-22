<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewVideosWidget" %>
<div class="model-expert-review-container">
                        
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate> 
        <div class="margin-bottom20">
            <div class="review-image-wrapper">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                    <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                        alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                </a>
            </div>
            <div class="review-heading-wrapper">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" 
                    class="font14 target-link"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
                <p class="font12 text-truncate text-light-grey">Uploaded on <%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"DisplayDate")).ToString("MMMM dd, yyyy")%></p> 
            </div>
        </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div>
    <a href="<%=MoreVideoUrl%>" class="bw-ga" c="Model_Page" a="View_all_videos_link_cliked" v="myBikeName">View all videos<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
