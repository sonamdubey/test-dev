﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.controls.ModelGallery" %>
<!-- model-gallery-container starts here -->
<section class="model-gallery-container">
    <div class="blackOut-window-model"></div>
    <div class="modelgallery-close-btn position-abt pos-top20 pos-right20 bwmsprite cross-lg-white cur-pointer hide"></div>
    <div class="bw-tabs-panel bike-gallery-popup hide" id="bike-gallery-popup">
        <div class="text-center margin-top50 margin-bottom20">
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
                    <div class="carousel-photos carousel-stage-photos">
                        <ul>
                            <asp:Repeater ID="rptModelPhotos" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="http://img1.aeplcdn.com/grey.gif" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div class="bike-gallery-details">
                        <span class="leftfloatbike-gallery-details"></span>
                        <span class="rightfloat bike-gallery-count"></span>
                    </div>
                    <a href="#" class="prev photos-prev-stage bwmsprite"></a>
                    <a href="#" class="next photos-next-stage bwmsprite"></a>
                </div>

                <div class="navigation-photos">
                    <a href="#" class="prev photos-prev-navigation bwmsprite hide" style="display: none"></a>
                    <a href="#" class="next photos-next-navigation bwmsprite hide" style="display: none"></a>
                    <div class="carousel-photos carousel-navigation-photos">
                        <ul>
                            <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" src="http://img1.aeplcdn.com/grey.gif" title="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" width="83" height="47" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="bw-tabs-data hide" id="Videos">
            <div class="connected-carousels-videos">
                <div class="stage-videos">
                    <div class="carousel-videos carousel-stage-videos">
                        <div class="yt-iframe-container">
                            <iframe id="video-iframe" src="" frameborder="0" allowfullscreen></iframe>
                        </div>
                    </div>
                </div>
                <div class="navigation-videos">
                    <a href="#" class="prev videos-prev-navigation bwmsprite hide" style="display: none"></a>
                    <a href="#" class="next videos-next-navigation bwmsprite hide" style="display: none"></a>
                    <div class="carousel-videos carousel-navigation-videos">
                        <ul>
                            <asp:Repeater ID="rptVideoNav" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <img iframe-data="<%# DataBinder.Eval(Container.DataItem,"VideoUrl").ToString() %>" src="<%# String.Format("http://img.youtube.com/vi/{0}/1.jpg",DataBinder.Eval(Container.DataItem,"VideoId").ToString()) %>" width="83" height="47" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<!-- model-gallery-container ends here -->
