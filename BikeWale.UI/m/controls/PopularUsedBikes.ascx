<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.PopularUsedBikes" %>
<%if(FetchedRecordsCount > 0) { %>
<div class="container">
    <div class="grid-12 alpha omega ">
        <h2 class="text-center margin-top20 margin-bottom10"><%= header %></h2>
        <div class="content-box-shadow padding-top15 padding-bottom20">
            <div class="swiper-container card-container used-swiper">
            <div class="swiper-wrapper">
                <asp:Repeater ID="rptPopularUsedBikes" runat="server">
                    <ItemTemplate>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="<%#  FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>" title="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>">
                                    <div class="swiper-image-preview">
                                        <div class="image-thumbnail">
                                            <img class="swiper-lazy" data-src="<%#  Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._210x118) %>" alt="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="target-link font12 margin-bottom5 text-truncate"><%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %></h3>
                                        <div class="margin-bottom5 text-default">
                                            <span class="bwmsprite inr-xsm-icon"></span>
                                            <span class="font16 text-bold">
                                                <%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"AvgPrice").ToString()) %>
                                            </span>
                                            <span class="font12">(Average price)</span>
                                        </div>
                                        <p class="font12"><%# DataBinder.Eval(Container.DataItem,"TotalBikes").ToString() %> Bikes Available
                                        </p>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="view-all-btn-container margin-top10">
            <a class="btn view-all-target-btn" title="Second Hand Bikes in <%=!String.IsNullOrEmpty(cityName)?cityName:"India"%>" href="<%= FormatCompleteListUrl() %>">View all used bikes<span class="bwmsprite teal-right"></span></a>
        </div>
        </div>
    </div>
    <div class="clear"></div>
</div>
<% } %>
