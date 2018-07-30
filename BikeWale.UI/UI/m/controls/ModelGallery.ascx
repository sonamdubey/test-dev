<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ModelGallery" %>
<!-- model-gallery-container starts here -->
<section>
    <div class="model-gallery-container relative-gallery-container">
        <%if(isModelPage){ %>
        <h1 class="font16 text-white padding-top10 padding-left10"><%=bikeName %> Images</h1>
        <%} else{ %>
        <h3 class="font16 text-white"><%=articleName %> Images</h3>
        <%} %>
        <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

        <div class="bw-tabs-panel">
            <% if(videoCount > 0) { %>
            <ul class="bw-tabs horizontal-tabs-wrapper">
                <li class="active" data-tabs="photos" id="photos-tab">Images</li>
                <li data-tabs="videos" id="videos-tab">Videos</li>
            </ul>
            <% } %>
        
            <div id="bike-gallery-popup">
                <div class="bw-tabs-data" id="photos">
                    <div class="font14 text-white margin-bottom15">
                        <span class="leftfloat media-title"></span>
                        <span class="rightfloat gallery-count"></span>
                        <div class="clear"></div>
                    </div>
                        <%if(Photos!=null){ %>
                    <div class="connected-carousels-photos">
                        <div class="stage-photos">
                            <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                                <div class="swiper-wrapper">
                                <%foreach(var PhotoDetails in Photos){ %>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(PhotoDetails.OriginalImgPath,PhotoDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%=PhotoDetails.ImageCategory %>"  title="<%=string.Format("{0} {1}",modelName,PhotoDetails.ImageCategory) %>" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                      <%} %>
                                </div>
                                <div class="bwmsprite swiper-button-next"></div>
                                <div class="bwmsprite swiper-button-prev"></div>
                            </div>
                        </div>

                        <div class="navigation-photos">
                            <div class="swiper-container noSwiper carousel-navigation-photos">
                                <div class="swiper-wrapper">
                                   <%foreach(var PhotoDetails in Photos){ %>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(PhotoDetails.OriginalImgPath,PhotoDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%=PhotoDetails.ImageCategory %>"  title="<%=string.Format("{0} {1}",modelName,PhotoDetails.ImageCategory) %>" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                      <%} %>
                                </div>
                                <div class="bwmsprite swiper-button-next hide"></div>
                                <div class="bwmsprite swiper-button-prev hide"></div>
                            </div>
                        </div>
                    </div>
                    <%} %>
                </div>
                <% if(videoCount > 0) { %>
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
                <% } %>
            </div>
        </div>
    </div>
</section>
<!-- model-gallery-container ends here -->
