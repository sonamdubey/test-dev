<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.PopularUsedBikes" %>
<div class="container">
    <div class="grid-12 <%= (FetchedRecordsCount > 0)?"":"hide" %>">
        <h2 class="text-bold text-center margin-top50 margin-bottom30 font28"><%= FormatControlHeader() %></h2>
        <div class="jcarousel-wrapper popular-used-bikes-container">
            <div class="jcarousel used-bike-carousel">
                <ul>
                    <asp:Repeater ID="rptPopularUsedBikes" runat="server">
                        <ItemTemplate>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="<%# FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>">
                                            <img class="lazy" src="" data-original="<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem,"HostURL").ToString(),DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString()) %>" title="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>" alt="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom15">
                                            <h3><a href="<%# FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %>"><%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %></a></h3>
                                        </div>
                                        <div class="margin-bottom10 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"AvgPrice").ToString()) %></span>
                                            <span class="font16">(Average price)</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
                                            <a href="<%# FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %>"><span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"TotalBikes").ToString()) %> Bikes Available</span></a>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
            <!--<p class="jcarousel-pagination"></p> -->
        </div>
        <div class="text-center margin-bottom30">
            <a class="font16" href="<%= FormatCompleteListUrl() %>">View complete list</a>
        </div>
    </div>
    <div class="clear"></div>
</div>
