<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.controls.PopularUsedBikes" %>
<div class="container">
    <div class="grid-12 <%= (FetchedRecordsCount > 0)?"":"hide" %>">
        <h2 class="text-center margin-top40 margin-bottom30"><%= FormatControlHeader() %></h2>
        <div class="jcarousel-wrapper">
            <div class="jcarousel">
                <ul>
                    <asp:Repeater ID="rptPopularUsedBikes" runat="server">
                        <ItemTemplate>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="<%#  FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>">
                                            <img src="" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(), Bikewale.Utility.ImageSize._310x174) %>" title="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>" alt="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom10">
                                            <h3><a href="<%#  FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %>"><%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %></a></h3>
                                        </div>
                                        <div class="margin-bottom10 font24">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font25"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"AvgPrice").ToString()) %></span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span><%# DataBinder.Eval(Container.DataItem,"TotalBikes").ToString() %></span> Bikes Available
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
            <p class="text-center jcarousel-pagination margin-bottom30"></p>
        </div>
        <div class="text-center margin-bottom40">
            <a class="font16" href="<%= FormatCompleteListUrl() %>">View More used Bikes</a>
        </div>
    </div>
    <div class="clear"></div>
</div>
