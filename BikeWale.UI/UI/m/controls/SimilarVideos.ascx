<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SimilarVideos.ascx.cs" Inherits="Bikewale.Mobile.Controls.SimilarVideos" %>
<asp:Repeater ID="rptSimilarVideos" runat="server">
    <HeaderTemplate>
        <div class="container">
            <h2 class="text-center margin-top25 margin-bottom15"><%=SectionTitle %></h2>
            <div class="swiper-container">
                <div class="swiper-wrapper">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="swiper-slide video-carousel-content rounded-corner2">
            <div class="video-carousel-image">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-bold text-default">
                    <img class="swiper-lazy" data-src="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>" alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" border="0" />
                    <span class="swiper-lazy-preloader"></span>
                </a>
            </div>
            <div class="video-carousel-desc">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-default text-bold"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"dd MMMM yyyy")  %></p>
                <div class="grid-6 alpha omega border-light-right font14">
                    <span class="bwmsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5"></span><span class="text-default comma"><%# DataBinder.Eval(Container.DataItem,"Views") %></span></div>
                <div class="grid-6 omega padding-left10 font14">
                    <span class="bwmsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5"></span><span class="text-default comma"><%# DataBinder.Eval(Container.DataItem,"Likes") %></span></div>
                <div class="clear"></div>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
        </div>
                </div>
                <%--<a href="" class="font14 text-center more-videos-link">View more videos</a>--%>
        </div>
    </FooterTemplate>
</asp:Repeater>
