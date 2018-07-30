<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.MUpcomingBikes" %>

<!-- Mobile Upcoming Bikes Starts here-->
<asp:Repeater ID="rptUpcomingBikes" runat="server">
    <ItemTemplate>
        <div class="swiper-slide">
            <div class="swiper-card">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>">
                    <div class="swiper-image-preview">
                        <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._174x98) %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" />
                        <span class="swiper-lazy-preloader"></span>
                    </div>
                    <div class="swiper-details-block">
                        <h3 class="target-link font12 text-truncate margin-bottom5"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></h3>
                        <div class="text-light-grey margin-bottom5"><%# ShowLaunchDate(DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate")) %></div>
                        <div>
                            <div class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"hide":""%> font11 text-light-grey">Expected Price</div>
                            <div class="text-default"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin")) %></div>
                        </div>
                    </div>
                </a>                
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<!-- Ends here-->
