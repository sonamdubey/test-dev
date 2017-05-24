<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewsWidget" %>
<div class="bw-tabs-data" id="ctrlNews">
    <div class="swiper-container padding-bottom60">
        <div class="swiper-wrapper">
            <asp:Repeater ID="rptNews" runat="server">
                <ItemTemplate>
                    <div class="swiper-slide">
                        <div class="front">
                            <div class="contentWrapper">
                                <div class="imageWrapper">
                                    <a href="/m<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>">
                                        <img class="swiper-lazy" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._144x81) %>" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </a>
                                </div>
                                <div class="bikeDescWrapper">
                                    <div class="bikeTitle margin-bottom20"> 
                                        <a href="/m<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.News.ToString()) %>">
                                            <h3><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></h3>
                                        </a>
                                    </div>
                                    <div class="margin-bottom10 text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "dd MMMM yyyy") %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></div>
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
    <div class="text-center margin-bottom30">
        <a class="font16" href="/m/news/">View more news</a>
    </div>
</div>
