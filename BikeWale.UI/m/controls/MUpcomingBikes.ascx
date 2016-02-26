<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.controls.MUpcomingBikes" %>

<!-- Mobile Upcoming Bikes Starts here-->
    <asp:Repeater ID="rptUpcomingBikes" runat="server">
        <ItemTemplate>
        <div class="swiper-slide">
            <div class="front">
                <div class="contentWrapper">
                    <!--<div class="position-abt pos-right10 pos-top10 infoBtn bwmsprite alert-circle-icon"></div>-->
                    <div class="imageWrapper">
                            <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>">
                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>"  alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" />
                            <span class="swiper-lazy-preloader"></span>
                            </a>
                    </div>
                    <div class="bikeDescWrapper">
                        <div class="bikeTitle">
                            <h3><a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></a></h3>
                        </div>
                        <div class="margin-bottom5">
                           <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin")) %>  
                        </div>
                        <div class=" <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"hide":""%> margin-bottom10 font14 text-light-grey">Expected Price</div>
                        <%# ShowLaunchDate(DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate")) %>
                    </div>
                </div>
            </div>
        </div> 
        </ItemTemplate>
    </asp:Repeater>
<!-- Ends here-->
