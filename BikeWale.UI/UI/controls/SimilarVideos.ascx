<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SimilarVideos.ascx.cs" Inherits="Bikewale.Controls.SimilarVideos" %>
<asp:Repeater ID="rptSimilarVideos" runat="server">
    <HeaderTemplate>
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top20 margin-bottom20 font28"><%=SectionTitle %></h2>
                <div class="jcarousel-wrapper related-video-jcarousel">
                    <div class="jcarousel">
                        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li class="front">
            <div class="videocarousel-image-wrapper rounded-corner2">
                <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-bold text-default">
                    <img class="lazy" data-original="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                        alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                </a>
            </div>
            <div class="videocarousel-desc-wrapper">
                <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-bold text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"dd MMMM yyyy")  %></p>
                <div class="grid-6 alpha omega border-light-right font14">
                    <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default comma"><%# DataBinder.Eval(Container.DataItem,"Views") %></span></div>
                <div class="grid-6 omega padding-left20">
                    <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default comma"><%# DataBinder.Eval(Container.DataItem,"Views") %></span></div>
                <div class="clear"></div>
            </div>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
        </div>
                    <%--<a href="" class="font16 text-center more-videos-link">View more videos</a>--%>
        </div>
                <div class="clear"></div>
        </div>
    </FooterTemplate>
</asp:Repeater>
