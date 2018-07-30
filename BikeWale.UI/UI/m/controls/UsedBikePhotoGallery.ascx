<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikePhotoGallery" %>
<!-- gallery start -->
<div id="model-gallery-container">
    <p class="font16 text-white"><%=ModelYear %>, <%= BikeName %> Images</p>
    <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

    <div id="bike-gallery-popup">
        <div class="font14 text-white margin-bottom15">
            <span class="leftfloat media-title"></span>
            <span class="rightfloat gallery-count"></span>
            <div class="clear"></div>
        </div>
        <div class="connected-carousels-photos">
            <div class="stage-photos">
                <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                    <div class="swiper-wrapper">
                        <asp:Repeater ID="rptUsedBikePhotos" runat="server">
                            <ItemTemplate>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="" title="" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="bwmsprite swiper-button-next"></div>
                    <div class="bwmsprite swiper-button-prev"></div>
                </div>
            </div>

            <div class="navigation-photos">
                <div class="swiper-container noSwiper carousel-navigation-photos">
                    <div class="swiper-wrapper">
                        <asp:Repeater ID="rptUsedBikeNavPhotos" runat="server">
                            <ItemTemplate>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" alt="<%= BikeName %>" title="<%= BikeName %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="bwmsprite swiper-button-next hide"></div>
                    <div class="bwmsprite swiper-button-prev hide"></div>
                </div>
            </div>
        </div>
    </div>
</div>
<!-- gallery end -->

