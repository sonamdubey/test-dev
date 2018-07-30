<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewMUpcomingBikes" %>
<section id="bikeByMakesUpcoming" class="bg-white margin-bottom20">
    <div class="container box-shadow padding-top20 padding-bottom20">
        <div class="carousel-heading-content">
        <div class="swiper-heading-left-grid inline-block">        
        <h2>Upcoming <%= MakeName %> bikes</h2>
        </div>
            <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m/<%=MakeMaskingName%>-bikes/upcoming/" title="Upcoming Bikes in India" class="btn view-all-target-btn">View all</a>
        </div>
        <div class="clear"></div>
            </div>
        <div class="swiper-container">
            
            <div class="swiper-wrapper upcoming-carousel-content">
                <asp:Repeater ID="rptUpcomingBikes" runat="server">
                    <ItemTemplate>
                        <div class="swiper-slide bike-carousel-swiper">
                            <div class="bike-swiper-image-wrapper">
                                <a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>">
                                    <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._174x98) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="bike-swiper-details-wrapper">
                                <h3 class="bikeTitle margin-bottom5"><a href="/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName"))) %>"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></a></h3>
                                <p class="text-xx-light margin-bottom5">Expected launch</p>
                                <p class="margin-bottom10 text-bold"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ExpectedLaunchDate")) %></p>
                                <p class="text-xx-light margin-bottom5 <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin"))=="0")?"hide":""%>">Expected price</p>
                                <div class="font16">
                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "EstimatedPriceMin").ToString()) %></span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</section>
