<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopularUsedBikes" %>
<%if(FetchedRecordsCount > 0) { %>
<div class="container">
    <div class="grid-12 ">
        <h2 class="text-bold text-center margin-top30 margin-bottom20 font22"><%= header %></h2>
        <div class="content-box-shadow padding-top20 padding-bottom20">
            <div class="jcarousel-wrapper inner-content-carousel">
                <div class="jcarousel">
                    <ul>
                        <asp:Repeater ID="rptPopularUsedBikes" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="<%# FormatUsedBikeUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(), DataBinder.Eval(Container.DataItem,"CityMaskingName").ToString()) %>" title="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>" class="jcarousel-card">
                                    <div class="model-jcarousel-image-preview">
                                        <div class="card-image-block">
                                            <img class="lazy" src="" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._310x174) %>" alt="<%# FormatImgAltTitle(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>">
                                        </div>
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle"><%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %></h3>
                                        <div class="text-bold text-default margin-bottom10">
                                            <span class="bwsprite inr-lg"></span>
                                            <span class="font18"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"AvgPrice").ToString()) %></span>
                                            <span class="font14">(Average price)</span>
                                        </div>
                                        <p class="font16">
                                            <%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"TotalBikes").ToString()) %> Bikes Available
                                        </p>
                                    </div>
                                  </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>
            <div class="view-all-btn-container margin-top15">
                <a class="btn view-all-target-btn" title="View all Used Bikes in <%=cityName%>" href="<%= FormatCompleteListUrl() %>">View complete list<span class="bwsprite teal-right"></span></a>
            </div>
        </div>
    </div>
    <div class="clear"></div>
</div>
<% } %>
