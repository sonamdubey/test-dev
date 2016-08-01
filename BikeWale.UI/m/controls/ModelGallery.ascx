<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.controls.ModelGallery" %>
<!-- model-gallery-container starts here -->
<section class="model-gallery-container">
    <h1 class="font16 text-white"><%=bikeName %> Photos</h1>
    <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

    <div class="bw-tabs-panel">
        <ul class="bw-tabs horizontal-tabs-wrapper <%= videoCount == 0 ? "hide" : "" %>">
            <li class="active" data-tabs="photos" id="photos-tab">Photos</li>
            <li data-tabs="videos" id="videos-tab">Videos</li>
        </ul>

        <div id="bike-gallery-popup">
            <div class="bw-tabs-data" id="photos">
                <div class="font14 text-white margin-bottom15">
                    <span class="leftfloat media-title"></span>
                    <span class="rightfloat gallery-count"></span>
                    <div class="clear"></div>
                </div>
                <div class="connected-carousels-photos">
                    <div class="stage-photos">
                        <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                            <div class="swiper-wrapper">
                                <asp:Repeater ID="rptModelPhotos" runat="server">
                                    <ItemTemplate>
                                        <div class="swiper-slide">
                                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" alt="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" />
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
                                <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                    <ItemTemplate>
                                        <div class="swiper-slide">
                                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" title="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" />
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

            <div class="bw-tabs-data" id="videos">
                <div class="connected-carousels-videos">
                    <div class="stage-videos">
                        <div class="carousel-videos carousel-stage-videos">
                            <div class="yt-iframe-preview">
                                <iframe id="video-iframe" src="" frameborder="0" allowfullscreen></iframe>
                            </div>
                        </div>
                    </div>
                    <div class="navigation-videos">
                        <div class="swiper-container noSwiper carousel-navigation-videos">
                            <div class="swiper-wrapper">
                                <asp:Repeater ID="rptVideoNav" runat="server">
                                    <ItemTemplate>
                                        <div class="swiper-slide">
                                            <img iframe-data="<%# DataBinder.Eval(Container.DataItem,"VideoUrl").ToString() %>" src="<%# String.Format("http://img.youtube.com/vi/{0}/1.jpg",DataBinder.Eval(Container.DataItem,"VideoId").ToString()) %>" width="83" height="47" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<!-- model-gallery-container ends here -->
