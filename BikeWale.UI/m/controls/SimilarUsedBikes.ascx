<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SimilarUsedBikes.ascx.cs" Inherits="Bikewale.m.controls.SimilarUsedBikes" %>
<%--<asp:Repeater ID="rptSimilarVideos" runat="server">
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
                    <img class="swiper-lazy" data-src="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>" alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" border="0" />
                    <span class="swiper-lazy-preloader"></span>
                </a>
            </div>
            <div class="video-carousel-desc">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-default text-bold"><%# DataBinder.Eval(Container.DataItem,"VideoTitle") %></a>
                <p class="font12 text-light-grey margin-top10 margin-bottom10"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy")  %></p>
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
        </div>
    </FooterTemplate>
</asp:Repeater>--%>
<div id="modelSimilar" class="bw-model-tabs-data padding-top20 padding-bottom15">
    <h2 class="margin-right20 margin-bottom15 margin-left20">Similar <%= usedBikeViewModel.ModelName %> bikes</h2>
    <div id="similar-bike-swiper" class="swiper-container padding-top5 padding-bottom5">
        <div class="swiper-wrapper">
            <% foreach(var bike in similarBikeList){ %>
            <div class="swiper-slide swiper-shadow">
                <div class="model-swiper-image-preview">
                    <a href="">
                        <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="" alt="" />
                        <span class="swiper-lazy-preloader"></span>
                    </a>
                </div>
                <div class="model-swiper-details font11">
                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart"><%= usedBikeViewModel.BikeName %></a>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite model-date-icon-xs"></span>
                        <span class="model-details-label"><%=bike.ModelYear %> model</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite kms-driven-icon-xs"></span>
                        <span class="model-details-label"><%=bike.KmsDriven %> kms</span>
                    </div>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite author-grey-icon-xs"></span>
                        <span class="model-details-label"><%=bike.OwnerType %> owner</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite model-loc-icon-xs"></span>
                        <span class="model-details-label"><%=bike.RegisteredAt %></span>
                    </div>
                    <div class="clear"></div>
                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold"><%=bike.AskingPrice %></span></p>
                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                </div>
            </div>
            <% } %>
          <%--  <div class="swiper-slide swiper-shadow">
                <div class="model-swiper-image-preview">
                    <a href="">
                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                        <span class="swiper-lazy-preloader"></span>
                    </a>
                </div>
                <div class="model-swiper-details font11">
                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite model-date-icon-xs"></span>
                        <span class="model-details-label">2013 model</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite kms-driven-icon-xs"></span>
                        <span class="model-details-label">1,45,000 kms</span>
                    </div>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite author-grey-icon-xs"></span>
                        <span class="model-details-label">2nd owner</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite model-loc-icon-xs"></span>
                        <span class="model-details-label">Mumbai</span>
                    </div>
                    <div class="clear"></div>
                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                </div>
            </div>

            <div class="swiper-slide swiper-shadow">
                <div class="model-swiper-image-preview">
                    <a href="">
                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                        <span class="swiper-lazy-preloader"></span>
                    </a>
                </div>
                <div class="model-swiper-details font11">
                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite model-date-icon-xs"></span>
                        <span class="model-details-label">2013 model</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite kms-driven-icon-xs"></span>
                        <span class="model-details-label">1,45,000 kms</span>
                    </div>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite author-grey-icon-xs"></span>
                        <span class="model-details-label">2nd owner</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite model-loc-icon-xs"></span>
                        <span class="model-details-label">Mumbai</span>
                    </div>
                    <div class="clear"></div>
                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                </div>
            </div>

            <div class="swiper-slide swiper-shadow">
                <div class="model-swiper-image-preview">
                    <a href="">
                        <img class="swiper-lazy" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-ismart-spoke-755.jpg?20151209181902" title="" alt="" />
                        <span class="swiper-lazy-preloader"></span>
                    </a>
                </div>
                <div class="model-swiper-details font11">
                    <a href="" class="target-link font12 text-truncate margin-bottom5" title="Hero Splendor iSmart">Hero Splendor iSmart</a>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite model-date-icon-xs"></span>
                        <span class="model-details-label">2013 model</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite kms-driven-icon-xs"></span>
                        <span class="model-details-label">1,45,000 kms</span>
                    </div>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite author-grey-icon-xs"></span>
                        <span class="model-details-label">2nd owner</span>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <span class="bwmsprite model-loc-icon-xs"></span>
                        <span class="model-details-label">Mumbai</span>
                    </div>
                    <div class="clear"></div>
                    <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">1,22,000</span></p>
                    <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                </div>
            </div>--%>
        </div>
    </div>
    <div class="margin-top10 margin-right20 margin-left20">
        <a href="" title="" class="font14">View all <%= usedBikeViewModel.ModelName %> in <%= usedBikeViewModel.CityName %><span class="bwmsprite blue-right-arrow-icon"></span></a>
    </div>
</div>

<div class="margin-right20 margin-left20 border-solid-bottom"></div>
