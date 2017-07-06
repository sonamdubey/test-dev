<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ModelGallery" %>
<!-- Start of Model Page Gallery -->
<section>
    <div class="model-gallery-container">
        <div class="blackOut-window-model"></div>
        <div class="modelgallery-close-btn position-abt pos-top20 pos-right20 bwsprite cross-lg-white cur-pointer hide"></div>
        <div class="bw-tabs-panel bike-gallery-popup hide" id="bike-gallery-popup">
        <% if(videoCount > 0) { %>
        <div class="text-center photos-videos-tabs margin-bottom20">
            <div class="bw-tabs home-tabs">
                <ul>
                    <li class="active" data-tabs="Photos" id="photos-tab">Images</li>
                    <li data-tabs="Videos" id="videos-tab">Videos</li>
                </ul>
            </div>
        </div>
        <% } %>        
        <div class="bike-gallery-heading margin-bottom20 margin-left30 <%= videoCount == 0 ? "margin-top90" : "" %>">
            <p class="font18 text-bold text-white"><%= bikeName %></p>
        </div>
        <div class="bw-tabs-data" id="Photos">
            <div class="connected-carousels-photos">
                <div class="stage-photos stage-media">
                    <div class="carousel-photos carousel-stage-photos carousel-stage-media">
                        <ul>
                            <asp:Repeater ID="rptModelPhotos" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <div class="gallery-photo-img-container">
                                            <span>
                                                <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._640x348) %>"  src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  title="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %> ">
                                            </span>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div class="bike-gallery-details">
                        <span class="leftfloatbike-gallery-details"></span>
                        <span class="rightfloat bike-gallery-count"></span>
                    </div>
                    <a href="#" class="prev photos-prev-stage bwsprite media-prev-next-stage" rel="nofollow"></a>
                    <a href="#" class="next photos-next-stage bwsprite media-prev-next-stage" rel="nofollow"></a>
                </div>

                <div class="navigation-photos navigation-media">
                    <a href="#" class="prev photos-prev-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                    <a href="#" class="next photos-next-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                    <div class="carousel-photos carousel-navigation-photos carousel-navigation-media">
                        <ul>
                            <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <div class="gallery-photo-nav-img-container">
                                        <span>
                                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._144x81) %>" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" title =" <%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>"/>
                                        </span>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <% if(videoCount > 0) { %>
        <div class="bw-tabs-data hide" id="Videos">
            <div class="connected-carousels-videos">
                <div class="stage-videos stage-media">
                        <div class="carousel-videos carousel-stage-videos carousel-stage-media">
                            <div class="yt-iframe-container">
                                <iframe id="video-iframe" src="" frameborder="0" allowfullscreen></iframe>
                            </div>
                        </div>
                    </div>
                <div class="navigation-videos navigation-media">
                    <a href="#" class="prev videos-prev-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                    <a href="#" class="next videos-next-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                    <div class="carousel-videos carousel-navigation-videos carousel-navigation-media">
                        <ul>
                            <asp:Repeater ID="rptVideoNav" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <div class="yt-iframe-container">
                                            <img iframe-data="<%# DataBinder.Eval(Container.DataItem,"VideoUrl").ToString() %>" src="<%# String.Format("https://img.youtube.com/vi/{0}/1.jpg",DataBinder.Eval(Container.DataItem,"VideoId").ToString()) %>" />
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <% } %>
    </div>
    </div>
</section>
<!-- End of Model Page Gallery -->