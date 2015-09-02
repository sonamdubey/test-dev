<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ExpertReviewsWidget" %>
<div class="bw-tabs-data hide" id="ctrlExpertReviews">
    <div class="jcarousel-wrapper">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptExpertReviews" runat="server">
                <ItemTemplate>
                    <li>
                        <div class="front">
                            <div class="contentWrapper">
                                <div class="imageWrapper">
                                    <a href="/m/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html">
                                        <img alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._370x208) %>">
                                    </a>
                                </div>
                                <div class="bikeDescWrapper">
                                    <div class="bikeTitle margin-bottom20">
                                        <h3><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></h3>
                                    </div>
                                    <div class="margin-bottom10 text-light-grey"><%# Bikewale.Utility.FormatDate.GetDDMMYYYY(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %></div>                                                    
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
        <a class="font16" href="/m/road-tests/">View more reviews</a>
    </div>
</div>