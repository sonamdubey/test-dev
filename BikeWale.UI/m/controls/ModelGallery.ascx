<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.controls.ModelGallery" %>
<!-- model-gallery-container starts here -->
<section class="model-gallery-container">
    <div class="blackOut-window-model"></div>
    <div class="modelgallery-close-btn position-abt pos-top20 pos-right20 bwmsprite cross-lg-white cur-pointer hide"></div>
    <div class="bw-tabs-panel bike-gallery-popup hide" id="bike-gallery-popup">
        <div class="text-center margin-top30 margin-bottom20">
            <div class="bw-tabs home-tabs <%= videoCount == 0 ? "hide" : "" %>">
                <ul>
                    <li class="active" data-tabs="Photos" id="photos-tab">Photos</li>
                    <li data-tabs="Videos" id="videos-tab">Videos</li>
                </ul>
            </div>
        </div>
        <div class="bike-gallery-heading margin-bottom20 margin-left15 <%= videoCount == 0 ? "margin-top90" : "" %>">
            <h3 class="text-white"><%= bikeName %></h3>
        </div>
        <div class="bw-tabs-data" id="Photos">
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
                    <div class="bike-gallery-details">
                        <span class="leftfloatbike-gallery-details"></span>
                        <span class="rightfloat bike-gallery-count"></span>
                    </div>
                </div>

                <div class="navigation-photos">
                    <div class="swiper-container noSwiper carousel-photos carousel-navigation-photos">
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
        <div class="bw-tabs-data hide" id="Videos">
            <div class="connected-carousels-videos">
                <div class="stage-videos">
                    <div class="carousel-videos carousel-stage-videos">
                        <div class="yt-iframe-preview">
                            <iframe id="video-iframe" src="" frameborder="0" allowfullscreen></iframe>
                        </div>
                    </div>
                </div>
                <div class="navigation-videos">
                    <div class="swiper-container noSwiper carousel-videos carousel-navigation-videos">
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
</section>
<!-- model-gallery-container ends here -->
