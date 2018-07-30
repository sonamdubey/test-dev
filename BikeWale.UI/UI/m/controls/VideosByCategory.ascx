<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.VideosByCategory" %>

<section>
    <div class="container">
        <h2 class="text-center margin-top5 margin-bottom15"><%= SectionTitle %></h2>
        <div class="swiper-container">
            <div class="swiper-wrapper">
                <asp:Repeater ID="rptVideosByCat" runat="server">
                    <ItemTemplate>
                        <div class="swiper-slide video-carousel-content rounded-corner2">
                            <div class="video-carousel-image">
                                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                                    <img class="swiper-lazy" data-src="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                                        alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="video-carousel-desc">
                               <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-bold text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                                <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"dd MMMM yyyy")  %></p>
                                <div class="grid-6 alpha omega border-light-right font14">
                                    <span class="bwmsprite video-views-icon margin-right5"></span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                                </div>
                                <div class="grid-6 omega padding-left10 font14">
                                    <span class="bwmsprite video-likes-icon margin-right5"></span><span class="text-default"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="padding-left10 view-all-btn-container margin-top10 padding-bottom20">
    <a href="/m<%= Bikewale.Utility.UrlFormatter.VideoByCategoryPageUrl(SectionTitle,CategoryIdList) %>" title="<%=SectionTitle %> Bike Videos" class="btn view-all-target-btn">View more videos<span class="bwmsprite teal-right"></span></a>
     </div>
         </div>
</section>
