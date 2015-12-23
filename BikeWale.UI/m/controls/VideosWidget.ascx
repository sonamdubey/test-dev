﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.VideosWidget" %>
<div class="bw-tabs-data" id="ctrlVideos">
    <div class="swiper-container padding-bottom60">
         <div class="swiper-wrapper">
                <asp:Repeater ID="rptVideos" runat="server">
                <ItemTemplate>
                <div class="swiper-slide">
                    <div class="front">
                        <div class="contentWrapper">
                            <div class="yt-iframe-preview">
                                <iframe id="video_<%= counter++ %>" frameborder="0" allowtransparency="true" src="<%# DataBinder.Eval(Container.DataItem,"VideoUrl").ToString() %>&enablejsapi=1"></iframe>
                            </div>
                            <div class="bikeDescWrapper">
                                <div class="bikeTitle margin-bottom20">
                                    <h3><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></h3>
                                </div>
                                <div class="margin-bottom15 text-light-grey">
                                    <span class="bwmsprite review-sm-lgt-grey"></span> Views <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                                </div>
                                <div class="text-light-grey">
                                    <span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                    </ItemTemplate>
                </asp:Repeater>
        </div>
        <!-- Add Pagination -->
        <div class="swiper-pagination"></div>
        <!-- Navigation -->
        <div class="bwmsprite swiper-button-next hide"></div>
        <div class="bwmsprite swiper-button-prev hide"></div>
    </div>
    <%--<div class="text-center margin-bottom40 clear">
        <a class="font16" href="javascript:void(0)">View more videos</a>
    </div>--%>
    </div>